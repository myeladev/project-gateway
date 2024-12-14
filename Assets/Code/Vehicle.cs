using System;
using System.Collections.Generic;
using ProjectGateway.Code.Scripts;
using UnityEngine;

namespace ProjectGateway
{
    public class Vehicle : Prop, IInteractable
    {
        public enum ControlMode
        {
            Keyboard,
            Buttons
        };

        public enum Axel
        {
            Front,
            Rear
        }

        [Serializable]
        public struct Wheel
        {
            public GameObject wheelModel;
            public WheelCollider wheelCollider;
            public GameObject wheelEffectObj;
            public ParticleSystem smokeParticle;
            public Axel axel;
        }

        public ControlMode control;
        public bool hasControl;

        public float maxAcceleration = 30.0f;
        public float brakeAcceleration = 50.0f;

        public float turnSensitivity = 1.0f;
        public float maxSteerAngle = 30.0f;

        public Vector3 centerOfMass;
        public Transform cameraAnchor;

        public List<Wheel> wheels;
        public List<ExitLocation> exitLocations;
        
        private float moveInput;
        private float steerInput;

        private Rigidbody carRb;

        private CarLights carLights;

        private void Start()
        {
            carRb = GetComponent<Rigidbody>();
            carRb.centerOfMass = centerOfMass;

            carLights = GetComponent<CarLights>();
        }

        private void Update()
        {
            GetInputs();
            AnimateWheels();
            //WheelEffects();
            Debug.DrawRay(transform.position, transform.forward);
        }

        private void LateUpdate()
        {
            Move();
            Steer();
            Brake();
        }

        public void MoveInput(float input)
        {
            moveInput = input;
        }

        public void SteerInput(float input)
        {
            steerInput = input;
        }

        private void GetInputs()
        {
            if (control == ControlMode.Keyboard)
            {
                moveInput = hasControl ? Input.GetAxis("Vertical") : 0f;
                steerInput = hasControl ? Input.GetAxis("Horizontal") : 0f;
            }
        }

        private void Move()
        {
            foreach(var wheel in wheels)
            {
                wheel.wheelCollider.motorTorque = moveInput * 600 * maxAcceleration * Time.deltaTime;
            }
        }

        private void Steer()
        {
            foreach(var wheel in wheels)
            {
                if (wheel.axel == Axel.Front)
                {
                    var steerAngle = steerInput * turnSensitivity * maxSteerAngle;
                    wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, steerAngle, 0.6f);
                }
            }
        }

        private void Brake()
        {
            // Handbrake
            var handbrake = hasControl && Input.GetKey(KeyCode.Space);
            if (handbrake)
            {
                foreach (var wheel in wheels)
                {
                    wheel.wheelCollider.brakeTorque = 1000 * brakeAcceleration * Time.deltaTime;
                }

                carLights.OperateBackLights(true);
            }
            // Reversing
            else if(moveInput < 0)
            {
                foreach (var wheel in wheels)
                {
                    wheel.wheelCollider.brakeTorque = 0;
                }
                
                carLights.OperateBackLights(true);
            }
            // Default behaviour
            else
            {
                foreach (var wheel in wheels)
                {
                    wheel.wheelCollider.brakeTorque = 0;
                }
                carLights.OperateBackLights(false);
            }
        }

        private void AnimateWheels()
        {
            foreach(var wheel in wheels)
            {
                wheel.wheelCollider.GetWorldPose(out var pos, out var rot);
                wheel.wheelModel.transform.position = pos;
                wheel.wheelModel.transform.rotation = rot;
            }
        }

        private void WheelEffects()
        {
            foreach (var wheel in wheels)
            {
                //var dirtParticleMainSettings = wheel.smokeParticle.main;

                if (Input.GetKey(KeyCode.Space) && wheel.axel == Axel.Rear && wheel.wheelCollider.isGrounded && carRb.velocity.magnitude >= 10.0f)
                {
                    wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = true;
                    wheel.smokeParticle.Emit(1);
                }
                else
                {
                    wheel.wheelEffectObj.GetComponentInChildren<TrailRenderer>().emitting = false;
                }
            }
        }

        public new bool IsInteractable => MyPlayer.instance.Character.CanInteract;
        public new Dictionary<InteractType, string> GetInteractText(InteractContext context)
        {
            // Get the base prop interactions
            var interactList = base.GetInteractText(context);
            // Add the "drive" interaction for items
            interactList.Add(InteractType.Use, "Drive");
            // Return the modified list
            return interactList;
        }
        public new void Interact(InteractType interactType, InteractContext context)
        {
            base.Interact(interactType, context);
            switch (interactType)
            {
                case InteractType.Use:
                    Drive();
                    break;
            }
        }

        private void Drive()
        {
            hasControl = true;
            MyPlayer.instance.SetVehicle(this);
            carLights.OperateFrontLights(true);
        }
        
        public void Exit()
        {
            var exitLocation = GetFreeExitLocation();

            if (exitLocation is null)
            {
                FeedbackMessageUIManager.instance.ShowMessage("Not enough space to exit vehicle");
                return;
            }
            hasControl = false;
            MyPlayer.instance.SetVehicle(null);
            carLights.OperateFrontLights(false);
            MyPlayer.instance.Character.Motor.SetPosition(exitLocation.Value);
        }

        private Vector3? GetFreeExitLocation()
        {
            foreach (var exitLocation in exitLocations)
            {
                if (exitLocation.IsFree) return exitLocation.transform.position;
            }

            return null;
        }
    }
}
