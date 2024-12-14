using UnityEngine;
using KinematicCharacterController;
using ProjectGateway.Code.Scripts;

namespace ProjectGateway
{
    public struct PlayerCharacterInputs
    {
        public float MoveAxisForward;
        public float MoveAxisRight;
        public Quaternion CameraRotation;
        public bool JumpDown;
        public bool Sprinting;
    }

    public class MyCharacterController : MonoBehaviour, ICharacterController
    {
        public KinematicCharacterMotor Motor;
        public bool IsHoldingProp => _holdingProp is not null;
        public bool CanInteract => !_holdingProp && !myPlayer.drivingVehicle && !movingFurniture && !InformationUI.instance.IsViewingInformation;

        [Header("Stable Movement")]
        public float MaxStableMoveSpeed = 10f;
        public float MaxStableSprintSpeed = 15f;
        public float StableMovementSharpness = 15;
        public float OrientationSharpness = 10;

        [Header("Air Movement")]
        public float MaxAirMoveSpeed = 10f;
        public float AirAccelerationSpeed = 5f;
        public float Drag = 0.1f;

        [Header("Jumping")]
        public bool AllowJumpingWhenSliding = false;
        public bool AllowDoubleJump = false;
        public bool AllowWallJump = false;
        public float JumpSpeed = 10f;
        public float JumpPreGroundingGraceTime = 0f;
        public float JumpPostGroundingGraceTime = 0f;

        [Header("Misc")]
        public Vector3 Gravity = new Vector3(0, -30f, 0);
        public Transform MeshRoot;
        [SerializeField]
        private MeshFilter furnitureHolderMeshFilter;
        [SerializeField]
        private MeshRenderer furnitureHolderMeshRenderer;
        [SerializeField]
        private FurniturePlacementMarker furniturePlacementMarker;
        [SerializeField]
        private MyPlayer myPlayer;
        
        [HideInInspector]
        public Furniture movingFurniture;

        private const float FurniturePlacementRange = 4f;
        private Collider _furniturePlacementMarkerCollider;
        private Vector3 _moveInputVector;
        private Vector3 _lookInputVector;
        private bool _jumpRequested = false;
        private bool _jumpConsumed = false;
        private bool _jumpedThisFrame = false;
        private bool _sprinting = false;
        private float _timeSinceJumpRequested = Mathf.Infinity;
        private float _timeSinceLastAbleToJump = 0f;
        private bool _doubleJumpConsumed = false;
        private bool _canWallJump = false;
        private Vector3 _wallJumpNormal;
        private Prop _holdingProp;
        private Camera _camera;

        private void Awake()
        {
            _camera = Camera.main;
            _furniturePlacementMarkerCollider = furniturePlacementMarker.GetComponent<Collider>();
        }

        private void Start()
        {
            // Assign to motor
            Motor.CharacterController = this;
        }

        void Update()
        {
            HandlePropGrabbing();
            HandleFurnitureMoving();
        }

        private void HandlePropGrabbing()
        {
            if (_holdingProp is not null)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    ReleaseProp(_holdingProp, false);
                }
                if (Input.GetMouseButtonUp(1))
                {
                    ReleaseProp(_holdingProp, true);
                }
            }

            _holdingProp?.SetTarget(_camera.transform.position + (_camera.transform.TransformDirection(Vector3.forward) * 1.2f));
        }
        
        private void HandleFurnitureMoving()
        {
            if (movingFurniture is not null)
            {
                if (Physics.Raycast(_camera.transform.position, _camera.transform.TransformDirection(Vector3.forward),
                        out var hit, FurniturePlacementRange))
                {
                    var blocked = false;
                    furniturePlacementMarker.transform.position = hit.point;
                    furniturePlacementMarker.gameObject.SetActive(true);
                    if (hit.normal != Vector3.up) blocked = true;
                    SnapRotatedObjectToGround(_furniturePlacementMarkerCollider);
                    furniturePlacementMarker.isBlocked = blocked;
                }
                else
                {
                    furniturePlacementMarker.FlushCollisions();
                    furniturePlacementMarker.gameObject.SetActive(false);
                }
                if (Input.GetMouseButtonUp(0))
                {
                    PlaceFurniture(movingFurniture);
                }
                if (Input.mouseScrollDelta.y != 0)
                {
                    furniturePlacementMarker.transform.rotation = 
                        Quaternion.Euler(
                            furniturePlacementMarker.transform.eulerAngles.x, 
                            furniturePlacementMarker.transform.eulerAngles.y + (Time.deltaTime * 1000f * Input.mouseScrollDelta.y), 
                            furniturePlacementMarker.transform.eulerAngles.z);
                }
            }
        }
        
        private void SnapRotatedObjectToGround(Collider objectToSnap)
        {
            Collider collider = objectToSnap;
            Vector3 highestPoint = collider.bounds.max;
            RaycastHit hit;
            var leeway = 0.01f;
            if (Physics.Raycast(highestPoint + (Vector3.up * leeway), Vector3.down, out hit, Mathf.Infinity, ~LayerMask.NameToLayer("Ignore Raycast")))
            {
                float newY = hit.point.y + (collider.bounds.size.y/2);
                objectToSnap.transform.position = new Vector3(objectToSnap.transform.position.x, newY + leeway, objectToSnap.transform.position.z);
            }
        }

        /// <summary>
        /// This is called every frame by MyPlayer in order to tell the character what its inputs are
        /// </summary>
        public void SetInputs(ref PlayerCharacterInputs inputs)
        {
            if (myPlayer.drivingVehicle 
                || myPlayer.isSleeping 
                || InformationUI.instance.IsViewingInformation 
                || UIManager.instance.IsInUI)
            {
                _moveInputVector = Vector3.zero;
                return;
            }
            
            // Clamp input
            Vector3 moveInputVector = Vector3.ClampMagnitude(new Vector3(inputs.MoveAxisRight, 0f, inputs.MoveAxisForward), 1f);

            // Calculate camera direction and rotation on the character plane
            Vector3 cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.forward, Motor.CharacterUp).normalized;
            if (cameraPlanarDirection.sqrMagnitude == 0f)
            {
                cameraPlanarDirection = Vector3.ProjectOnPlane(inputs.CameraRotation * Vector3.up, Motor.CharacterUp).normalized;
            }
            Quaternion cameraPlanarRotation = Quaternion.LookRotation(cameraPlanarDirection, Motor.CharacterUp);

            // Move and look inputs
            _moveInputVector = cameraPlanarRotation * moveInputVector;
            _lookInputVector = cameraPlanarDirection;
            _sprinting = inputs.Sprinting;

            // Jumping input
            if (inputs.JumpDown)
            {
                _timeSinceJumpRequested = 0f;
                _jumpRequested = true;
            }
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is called before the character begins its movement update
        /// </summary>
        public void BeforeCharacterUpdate(float deltaTime)
        {
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is where you tell your character what its rotation should be right now. 
        /// This is the ONLY place where you should set the character's rotation
        /// </summary>
        public void UpdateRotation(ref Quaternion currentRotation, float deltaTime)
        {
            if (_lookInputVector != Vector3.zero && OrientationSharpness > 0f)
            {
                // Smoothly interpolate from current to target look direction
                Vector3 smoothedLookInputDirection = Vector3.Slerp(Motor.CharacterForward, _lookInputVector, 1 - Mathf.Exp(-OrientationSharpness * deltaTime)).normalized;

                // Set the current rotation (which will be used by the KinematicCharacterMotor)
                currentRotation = Quaternion.LookRotation(smoothedLookInputDirection, Motor.CharacterUp);
            }
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is where you tell your character what its velocity should be right now. 
        /// This is the ONLY place where you can set the character's velocity
        /// </summary>
        public void UpdateVelocity(ref Vector3 currentVelocity, float deltaTime)
        {
            Vector3 targetMovementVelocity = Vector3.zero;
            if (Motor.GroundingStatus.IsStableOnGround)
            {
                // Reorient velocity on slope
                currentVelocity = Motor.GetDirectionTangentToSurface(currentVelocity, Motor.GroundingStatus.GroundNormal) * currentVelocity.magnitude;

                // Calculate target velocity
                Vector3 inputRight = Vector3.Cross(_moveInputVector, Motor.CharacterUp);
                Vector3 reorientedInput = Vector3.Cross(Motor.GroundingStatus.GroundNormal, inputRight).normalized * _moveInputVector.magnitude;
                targetMovementVelocity = reorientedInput * (_sprinting ? MaxStableSprintSpeed : MaxStableMoveSpeed);

                // Smooth movement Velocity
                currentVelocity = Vector3.Lerp(currentVelocity, targetMovementVelocity, 1 - Mathf.Exp(-StableMovementSharpness * deltaTime));
            }
            else
            {
                // Add move input
                if (_moveInputVector.sqrMagnitude > 0f)
                {
                    targetMovementVelocity = _moveInputVector * MaxAirMoveSpeed;

                    // Prevent climbing on unstable slopes with air movement
                    if (Motor.GroundingStatus.FoundAnyGround)
                    {
                        Vector3 perpendicularObstructionNormal = Vector3.Cross(Vector3.Cross(Motor.CharacterUp, Motor.GroundingStatus.GroundNormal), Motor.CharacterUp).normalized;
                        targetMovementVelocity = Vector3.ProjectOnPlane(targetMovementVelocity, perpendicularObstructionNormal);
                    }

                    Vector3 velocityDiff = Vector3.ProjectOnPlane(targetMovementVelocity - currentVelocity, Gravity);
                    currentVelocity += velocityDiff * (AirAccelerationSpeed * deltaTime);
                }

                // Gravity
                currentVelocity += Gravity * deltaTime;

                // Drag
                currentVelocity *= (1f / (1f + (Drag * deltaTime)));
            }

            // Handle jumping
            {
                _jumpedThisFrame = false;
                _timeSinceJumpRequested += deltaTime;
                if (_jumpRequested)
                {
                    // Handle double jump
                    if (AllowDoubleJump)
                    {
                        if (_jumpConsumed && !_doubleJumpConsumed && (AllowJumpingWhenSliding ? !Motor.GroundingStatus.FoundAnyGround : !Motor.GroundingStatus.IsStableOnGround))
                        {
                            Motor.ForceUnground(0.1f);

                            // Add to the return velocity and reset jump state
                            currentVelocity += (Motor.CharacterUp * JumpSpeed) - Vector3.Project(currentVelocity, Motor.CharacterUp);
                            _jumpRequested = false;
                            _doubleJumpConsumed = true;
                            _jumpedThisFrame = true;
                        }
                    }

                    // See if we actually are allowed to jump
                    if (_canWallJump ||
                        (!_jumpConsumed && ((AllowJumpingWhenSliding ? Motor.GroundingStatus.FoundAnyGround : Motor.GroundingStatus.IsStableOnGround) || _timeSinceLastAbleToJump <= JumpPostGroundingGraceTime)))
                    {
                        // Calculate jump direction before ungrounding
                        Vector3 jumpDirection = Motor.CharacterUp;
                        if (_canWallJump)
                        {
                            jumpDirection = _wallJumpNormal;
                        }
                        else if (Motor.GroundingStatus.FoundAnyGround && !Motor.GroundingStatus.IsStableOnGround)
                        {
                            jumpDirection = Motor.GroundingStatus.GroundNormal;
                        }

                        // Makes the character skip ground probing/snapping on its next update. 
                        // If this line weren't here, the character would remain snapped to the ground when trying to jump. Try commenting this line out and see.
                        Motor.ForceUnground(0.1f);

                        // Add to the return velocity and reset jump state
                        currentVelocity += (jumpDirection * JumpSpeed) - Vector3.Project(currentVelocity, Motor.CharacterUp);
                        _jumpRequested = false;
                        _jumpConsumed = true;
                        _jumpedThisFrame = true;
                    }
                }

                // Reset wall jump
                _canWallJump = false;
            }
        }

        /// <summary>
        /// (Called by KinematicCharacterMotor during its update cycle)
        /// This is called after the character has finished its movement update
        /// </summary>
        public void AfterCharacterUpdate(float deltaTime)
        {
            // Handle jump-related values
            {
                // Handle jumping pre-ground grace period
                if (_jumpRequested && _timeSinceJumpRequested > JumpPreGroundingGraceTime)
                {
                    _jumpRequested = false;
                }

                if (AllowJumpingWhenSliding ? Motor.GroundingStatus.FoundAnyGround : Motor.GroundingStatus.IsStableOnGround)
                {
                    // If we're on a ground surface, reset jumping values
                    if (!_jumpedThisFrame)
                    {
                        _doubleJumpConsumed = false;
                        _jumpConsumed = false;
                    }
                    _timeSinceLastAbleToJump = 0f;
                }
                else
                {
                    // Keep track of time since we were last able to jump (for grace period)
                    _timeSinceLastAbleToJump += deltaTime;
                }
            }
        }

        public bool IsColliderValidForCollisions(Collider coll)
        {
            return true;
        }

        public void OnGroundHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
        }

        public void OnMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, ref HitStabilityReport hitStabilityReport)
        {
            // We can wall jump only if we are not stable on ground and are moving against an obstruction
            if (AllowWallJump && !Motor.GroundingStatus.IsStableOnGround && !hitStabilityReport.IsStable)
            {
                _canWallJump = true;
                _wallJumpNormal = hitNormal;
            }
        }

        public void PostGroundingUpdate(float deltaTime)
        {
        }

        public void AddVelocity(Vector3 velocity)
        {
        }

        public void ProcessHitStabilityReport(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 atCharacterPosition, Quaternion atCharacterRotation, ref HitStabilityReport hitStabilityReport)
        {
        }

        public void OnDiscreteCollisionDetected(Collider hitCollider)
        {
        }

        public void GrabProp(Prop propToHold)
        {
            _holdingProp = propToHold;
        }

        public void ReleaseProp(Prop propToRelease, bool launch)
        {
            if (_holdingProp == propToRelease)
            {
                _holdingProp?.Release(launch);
                _holdingProp = null;
            }
        }

        public bool MoveFurniture(Furniture furnitureToMove)
        {
            if (movingFurniture) return false;
            movingFurniture = furnitureToMove;
            furnitureHolderMeshRenderer.gameObject.SetActive(true);
            var mesh = furnitureToMove.GetComponent<MeshFilter>().mesh;
            furnitureHolderMeshFilter.mesh = mesh;
            furnitureHolderMeshRenderer.materials = furnitureToMove.GetComponent<MeshRenderer>().materials;
            furniturePlacementMarker.GetComponent<MeshFilter>().mesh = mesh;
            furniturePlacementMarker.GetComponent<MeshCollider>().sharedMesh = mesh;
            return true;

        }

        private void PlaceFurniture(Furniture furnitureToPlace)
        {
            if (movingFurniture == furnitureToPlace)
            {
                if (furniturePlacementMarker.IsFree)
                {
                    if (Physics.Raycast(_camera.transform.position,
                            _camera.transform.TransformDirection(Vector3.forward),
                            out var hit, FurniturePlacementRange))
                    {
                        movingFurniture.Place(furniturePlacementMarker.transform.position, furniturePlacementMarker.transform.rotation);
                        ReleaseHeldFurniture();
                    }
                }
                else
                {
                    FeedbackMessageUIManager.instance.ShowMessage("Object is blocked by something");
                }
            }
        }

        public void ReleaseHeldFurniture()
        {
            movingFurniture = null;
            furnitureHolderMeshRenderer.gameObject.SetActive(false);
            furniturePlacementMarker.gameObject.SetActive(false);
        }
    }
}