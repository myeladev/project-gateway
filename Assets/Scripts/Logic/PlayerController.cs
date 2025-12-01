using ProjectDaydream.Core;
using ProjectDaydream.DataPersistence;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectDaydream.Logic
{
    public class PlayerController : MonoBehaviour, IDataPersistence
    {
        public CharacterCamera orbitCamera;
        public Transform cameraFollowPoint;
        public CharacterController character;
        public static PlayerController Instance;
        
        private InputAction _moveAction;
        private InputAction _lookAction;
        private InputAction _jumpAction;
        private InputAction _sprintAction;
        private InputAction _crouchAction;
        private InputAction _interactAction;
        
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _moveAction = InputSystem.actions.FindAction("Move");
            _lookAction = InputSystem.actions.FindAction("Look");
            _jumpAction = InputSystem.actions.FindAction("Jump");
            _sprintAction = InputSystem.actions.FindAction("Sprint");
            _crouchAction = InputSystem.actions.FindAction("Crouch");
            _interactAction = InputSystem.actions.FindAction("Interact");
            
            Cursor.lockState = CursorLockMode.Locked;

            // Tell camera to follow transform
            orbitCamera.SetFollowTransform(cameraFollowPoint);

            // Ignore the character's collider(s) for camera obstruction checks
            orbitCamera.IgnoredColliders.Clear();
            orbitCamera.IgnoredColliders.AddRange(character.GetComponentsInChildren<Collider>());
        }

        private void Update()
        {
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                //Cursor.lockState = CursorLockMode.Locked;
            }

            HandleCharacterInput();
        }

        private void LateUpdate()
        {
            HandleCameraInput();
        }

        private void HandleCameraInput()
        {
            // Create the look input vector for the camera
            float mouseLookAxisUp = _lookAction.ReadValue<Vector2>().y;
            float mouseLookAxisRight = _lookAction.ReadValue<Vector2>().x;
            Vector3 lookInputVector = new Vector3(mouseLookAxisRight, mouseLookAxisUp, 0f);

            // Prevent moving the camera while the cursor isn't locked
            if (Cursor.lockState != CursorLockMode.Locked)
            {
                lookInputVector = Vector3.zero;
            }

            // Apply inputs to the camera
            orbitCamera.UpdateWithInput(Time.deltaTime, lookInputVector);
        }
        
        private void HandleCharacterInput()
        {
            PlayerCharacterInputs characterInputs = new PlayerCharacterInputs();

            // Build the CharacterInputs struct
            characterInputs.MoveAxisForward = _moveAction.ReadValue<Vector2>().y;
            characterInputs.MoveAxisRight = _moveAction.ReadValue<Vector2>().x;
            characterInputs.CameraRotation = orbitCamera.Transform.rotation;
            characterInputs.JumpDown = _jumpAction.WasPressedThisFrame();
            characterInputs.JumpHeld = _jumpAction.ReadValue<float>() > 0.5f;
            characterInputs.SprintHeld = _sprintAction.ReadValue<float>() > 0.5f;
            characterInputs.CrouchDown = _crouchAction.WasPressedThisFrame();
            characterInputs.CrouchUp = _crouchAction.WasReleasedThisFrame();
            characterInputs.CrouchHeld = _crouchAction.ReadValue<float>() > 0.5f;
            characterInputs.ClimbLadder = _interactAction.ReadValue<float>() > 0.5f;

            // Apply inputs to character
            character.SetInputs(ref characterInputs);
        }
        
        public void LoadData(GameData data)
        {
            if (SceneManager.Instance.IsInMainMenu) return;
            if(data.player == null) return;
            
            if (data.player.position?.Length == 3 && 
                (data.player.position[0] != 0 || data.player.position[1] != 0 || data.player.position[2] != 0))
            {
                character.Motor.SetPosition(new Vector3(data.player.position[0], data.player.position[1], data.player.position[2]));
                
                // TODO: This isn't working, fix it
                Quaternion rotation =  Quaternion.Euler(new Vector3(data.player.rotation[0], data.player.rotation[1], data.player.rotation[2]));
                Camera.main.transform.rotation = rotation;
            }
            else
            {
                var playerSpawn = GameObject.FindGameObjectWithTag("PlayerSpawn");
                character.Motor.SetPosition(playerSpawn.transform.position);
            }
        }

        public void SaveData(ref GameData data)
        {
            data.player = new PlayerSaveData
            {
                position = new[] { character.transform.position.x, character.transform.position.y, character.transform.position.z },
                rotation = new[] { character.transform.eulerAngles.x, character.transform.eulerAngles.y, character.transform.eulerAngles.z },
            };
        }
    }
}
