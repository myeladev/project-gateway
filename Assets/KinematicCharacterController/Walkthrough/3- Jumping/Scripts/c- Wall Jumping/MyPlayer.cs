using Cinemachine;
using UnityEngine;
using KinematicCharacterController.Examples;

namespace ProjectGateway
{
    public class MyPlayer : MonoBehaviour
    {
        public CharacterCamera OrbitCamera;
        public CinemachineVirtualCamera vehicleCamera;
        public Transform CameraFollowPoint;
        public MyCharacterController Character;

        public bool IsInVehicle => drivingVehicle is not null;
        [HideInInspector]
        public Vehicle drivingVehicle;
        
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
        private void Awake()
        {
            instance = this;
            inventory = GetComponent<Inventory>();
            hunger = 60f;
            sleep = 0f;
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;

            // Tell camera to follow transform
            OrbitCamera.SetFollowTransform(CameraFollowPoint);

            // Ignore the character's collider(s) for camera obstruction checks
            OrbitCamera.IgnoredColliders.Clear();
            OrbitCamera.IgnoredColliders.AddRange(Character.GetComponentsInChildren<Collider>());
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Cursor.lockState = CursorLockMode.Locked;
            }

            // Handle exiting things
            if (Input.GetKeyDown(KeyCode.Q))
            {
                drivingVehicle?.Exit();

                if (isSleeping) Wake();
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
            if (Cursor.lockState != CursorLockMode.Locked || InformationUI.instance.IsViewingInformation)
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

            // Apply inputs to character
            Character.SetInputs(ref characterInputs);
        }

        public void SetVehicle(Vehicle newVehicle)
        {
            drivingVehicle = newVehicle;

            vehicleCamera.gameObject.SetActive(drivingVehicle is not null);
            vehicleCamera.LookAt = drivingVehicle?.transform;
            vehicleCamera.Follow = drivingVehicle?.transform;
            Character.gameObject.SetActive(drivingVehicle is null);
        }

        public void EatFood(Food food)
        {
            hunger += food.hungerRestoration;
            food.gameObject.SetActive(false);
        }

        public void Sleep(Bed bed)
        {
            isSleeping = true;
        }

        private void Wake()
        {
            isSleeping = false;
        }
    }
}