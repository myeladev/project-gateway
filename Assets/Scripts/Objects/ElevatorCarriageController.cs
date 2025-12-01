using KinematicCharacterController;
using ProjectDaydream.KinematicCharacterController.Core;
using UnityEngine;

namespace ProjectDaydream.Objects
{
    public class ElevatorCarriageController : MonoBehaviour, IMoverController
    {
        public PhysicsMover mover;
        public Transform carriageDriver;

        private void Start()
        {
            mover.MoverController = this;
        }

        // This is called every FixedUpdate by our PhysicsMover in order to tell it what pose it should go to
        public void UpdateMovement(out Vector3 goalPosition, out Quaternion goalRotation, float deltaTime)
        {
            // Set our platform's goal pose to the animation's
            goalPosition = carriageDriver.position;
            goalRotation = carriageDriver.rotation;
        }
    }
}
