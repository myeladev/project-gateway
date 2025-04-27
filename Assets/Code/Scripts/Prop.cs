using System.Collections.Generic;
using UnityEngine;

namespace ProjectGateway.Code.Scripts
{
    public class Prop : MonoBehaviour, IInteractable
    {
        public bool IsInteractable => MyPlayer.instance.Character.CanInteract;

        public List<string> GetInteractOptions(InteractContext context)
        {
            return context == InteractContext.Default ? 
                new List<string>
                {
                    "Grab",
                }
                : new List<string>();
        }

        private Vector3? targetPosition;
        private MyCharacterController _player;
        protected Rigidbody Rigidbody;
        private float _originalDragValue;
        private float _originalAngularDragValue;
        private Vector3 _originalPosition;
        private Quaternion _originalRotation;
        private Camera _camera;
        private const float GrabForce = 50f;
        private readonly Vector3 _grabForceScale = new (1.3f, 0.5f, 1.3f);

        protected void Awake()
        {
            _camera = Camera.main;
            Rigidbody = GetComponent<Rigidbody>();
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<MyCharacterController>();
            _originalDragValue = Rigidbody.linearDamping;
            _originalAngularDragValue = Rigidbody.angularDamping;
            _originalPosition = transform.position;
            _originalRotation = transform.rotation;
        }

        private void Update()
        {
            if (targetPosition is not null)
            {
                var distance = Vector3.Distance(transform.position, targetPosition.Value);
                // If item is at the desired held position
                if (distance > 0.1f)
                {
                    // Scale the grab force to the set scale
                    // This is to make grabbing horizontally easier and vertically harder
                    // (Easier to slide an object than lift it)
                    var forceToGrabWith = (targetPosition.Value - transform.position) * GrabForce;
                    forceToGrabWith.x *= _grabForceScale.x;
                    forceToGrabWith.y *= _grabForceScale.y;
                    forceToGrabWith.z *= _grabForceScale.z;
                    Rigidbody.AddForce(forceToGrabWith);
                }
                // Item is too far to be considered held
                if(distance > 4f)
                {
                    _player.ReleaseProp(this, false);
                }
            }
            
            // Item has fallen out of the world for some reason
            if (transform.position.y < -1000f)
            {
                // Reset it
                transform.position = _originalPosition;
                transform.rotation = _originalRotation;
                Rigidbody.linearVelocity = Vector3.zero;
            }
        }

        public void Interact(string option, InteractContext context)
        {
            switch (option)
            {
                case "Grab":
                    _player.GrabProp(this);
                    gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                    Rigidbody.useGravity = false;
                    Rigidbody.linearDamping = 10f;
                    Rigidbody.angularDamping = 10f;
                    break;
            }
        }


        public void Release(bool launch)
        {
            gameObject.layer = LayerMask.NameToLayer("Prop");
            Rigidbody.useGravity = true;
            Rigidbody.linearDamping = _originalDragValue;
            Rigidbody.angularDamping = _originalAngularDragValue;
            SetTarget(null);
            if (launch)
            {
                Rigidbody.AddForce(_camera.transform.TransformDirection(Vector3.forward) * (GrabForce * 8f));
            }
        }

        public void SetTarget(Vector3? newTargetPosition)
        {
            targetPosition = newTargetPosition;
        }
    }
}