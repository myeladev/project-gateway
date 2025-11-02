using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using KinematicCharacterController.Examples;
using ProjectGateway.Core;
using ProjectGateway.DataPersistence;
using ProjectGateway.Objects.Furniture;
using ProjectGateway.Objects.Items;
using ProjectGateway.Objects.Vehicles;
using ProjectGateway.UI;

namespace ProjectGateway
{
    public class MyPlayer : MonoBehaviour, IDataPersistence
    {
        public CharacterCamera OrbitCamera;
        public CinemachineVirtualCamera vehicleCamera;
        public Transform CameraFollowPoint;
        public MyCharacterController Character;

        public bool IsInVehicle => drivingVehicle is not null;
        [HideInInspector]
        public Vehicle drivingVehicle;
        
        public Transform flashlightTransform;

        public Transform currentSeatingAnchor;
        
        private const string MouseXInput = "Mouse X";
        private const string MouseYInput = "Mouse Y";
        private const string MouseScrollInput = "Mouse ScrollWheel";
        private const string HorizontalInput = "Horizontal";
        private const string VerticalInput = "Vertical";
        private const float HungerFallRate = 0.06f;
        private const float SleepFallRate = 0.045f;
        private const float SleepRecoveryRate = 0.2f;

        public static MyPlayer instance;
        [HideInInspector]
        public Inventory inventory;

        [HideInInspector] public float hunger;
        [HideInInspector] public float sleep;
        public bool isSleeping;
        public bool IsSitting => currentSeatingAnchor;

        [SerializeField] 
        private List<Light> flashlight = new();
        
        private void Awake()
        {
            instance = this;
            inventory = GetComponent<Inventory>();
            hunger = 60f;
            sleep = 0f;
        }

        private void Start()
        {
            // Tell camera to follow transform
            OrbitCamera.SetFollowTransform(CameraFollowPoint);

            // Ignore the character's collider(s) for camera obstruction checks
            OrbitCamera.IgnoredColliders.Clear();
            OrbitCamera.IgnoredColliders.AddRange(Character.GetComponentsInChildren<Collider>());
        }

        private void Update()
        {
            // Handle exiting things
            if (Input.GetKeyDown(KeyCode.Q))
            {
                drivingVehicle?.Exit();
                if(currentSeatingAnchor) Sit(null);

                if (isSleeping) Wake();
            }
                
            // Handle flashlight control
            if (Input.GetKeyDown(KeyCode.F) && !GameplayUI.Instance.IsAnyPanelActive())
            {
                foreach (var fLight in flashlight)
                {
                    fLight.enabled = !fLight.enabled;
                }
            }

            if(flashlightTransform != null)
            {
                flashlightTransform.position = Vector3.Lerp(flashlightTransform.position, CameraFollowPoint.position - (CameraFollowPoint.up * 0.25f), Time.deltaTime * 30f);
                flashlightTransform.rotation = Quaternion.Lerp(flashlightTransform.rotation, Camera.main.transform.rotation, Time.deltaTime * 10f);
            }

            hunger -= Time.deltaTime * HungerFallRate;
            hunger = Mathf.Clamp(hunger, 0, 120);

            if (isSleeping)
            {
                Time.timeScale = 20f;
                sleep += Time.deltaTime * SleepRecoveryRate;
            }
            else
            {
                Time.timeScale = 1f;
                sleep -= Time.deltaTime * SleepFallRate;
            }
            sleep = Mathf.Clamp(sleep, 0, 100);

            HandleCharacterInput();
        }

        private void LateUpdate()
        {
            HandleCameraInput();
        }

        private void HandleCameraInput()
        {
            // Create the look input vector for the camera
            float mouseLookAxisUp = Input.GetAxisRaw(MouseYInput);
            float mouseLookAxisRight = Input.GetAxisRaw(MouseXInput);
            Vector3 lookInputVector = new Vector3(mouseLookAxisRight, mouseLookAxisUp, 0f);

            // Prevent moving the camera while the cursor isn't locked
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                lookInputVector = Vector3.zero;
            }

            // Input for zooming the camera (disabled in WebGL because it can cause problems)
            float scrollInput = -Input.GetAxis(MouseScrollInput);
#if UNITY_WEBGL
        scrollInput = 0f;
#endif

            // Apply inputs to the camera
            OrbitCamera.UpdateWithInput(Time.deltaTime, scrollInput, lookInputVector);
        }

        private void HandleCharacterInput()
        {
            PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();

            // Build the CharacterInputs struct
            characterInputs.MoveAxisForward = Input.GetAxisRaw(VerticalInput);
            characterInputs.MoveAxisRight = Input.GetAxisRaw(HorizontalInput);
            characterInputs.CameraRotation = OrbitCamera.Transform.rotation;
            characterInputs.JumpDown = Input.GetKeyDown(KeyCode.Space);
            characterInputs.Sprinting = Input.GetKey(KeyCode.LeftShift);
            characterInputs.Crouching = Input.GetKey(KeyCode.LeftControl);

            // Apply inputs to character
            Character.SetInputs(ref characterInputs);
        }

        public void SetVehicle(Vehicle newVehicle)
        {
            drivingVehicle = newVehicle;

            vehicleCamera.gameObject.SetActive(drivingVehicle is not null);
            vehicleCamera.LookAt = drivingVehicle?.cameraAnchor;
            vehicleCamera.Follow = drivingVehicle?.transform;
            Character.gameObject.SetActive(drivingVehicle is null);
        }

        public void EatFood(Food food)
        {
            hunger += food.hungerRestoration;
        }

        public void Sleep(Bed bed)
        {
            isSleeping = true;
        }

        private void Wake()
        {
            isSleeping = false;
        }
        
        public void Sit(Transform seatingAnchor)
        {
            currentSeatingAnchor = seatingAnchor;
        }


        public void LoadData(GameData data)
        {
            if (SceneManager.Instance.IsInMainMenu) return;
            if(data.player == null) return;
            hunger = data.player.hunger;
            sleep = data.player.sleep;
            
            if (data.player.position.Length == 3 && 
                (data.player.position[0] != 0 || data.player.position[1] != 0 || data.player.position[2] != 0))
            {
                Character.Motor.SetPosition(new Vector3(data.player.position[0], data.player.position[1], data.player.position[2]));
                
                // TODO: This isn't working, fix it
                Quaternion rotation =  Quaternion.Euler(new Vector3(data.player.rotation[0], data.player.rotation[1], data.player.rotation[2]));
                Camera.main.transform.rotation = rotation;
            }
            Sit(null);
        }

        public void SaveData(ref GameData data)
        {
            data.player = new PlayerSaveData
            {
                hunger = hunger,
                sleep = sleep,
                position = new[] { Character.transform.position.x, Character.transform.position.y, Character.transform.position.z },
                rotation = new[] { Character.transform.eulerAngles.x, Character.transform.eulerAngles.y, Character.transform.eulerAngles.z },
            };
        }
    }

    [Serializable]
    public class PlayerSaveData
    {
        public float hunger;
        public float sleep;
        public float[] position;
        public float[] rotation;
    }
}