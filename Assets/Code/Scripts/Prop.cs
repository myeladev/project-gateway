using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace ProjectGateway
{
    public class Prop : MonoBehaviour, IInteractable
    {
        public bool IsInteractable => MyPlayer.instance.Character.CanInteract;

        public Dictionary<InteractType, string> GetInteractText(InteractContext context)
        {
            return context == InteractContext.Default ? 
                new Dictionary<InteractType, string>
                {
                    { InteractType.Grab, "Grab" },
                }
                : new Dictionary<InteractType, string>();
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

        private void Awake()
        {
            _camera = Camera.main;
            Rigidbody = GetComponent<Rigidbody>();
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<MyCharacterController>();
            _originalDragValue = Rigidbody.drag;
            _originalAngularDragValue = Rigidbody.angularDrag;
            _originalPosition = transform.position;
            _originalRotation = transform.rotation;
        }

        private void Update()
        {
            if (targetPosition is not null)
            {
                var distance = Vector3.Distance(transform.position, targetPosition.Value);
                if (distance > 0.1f)
                {
                    Rigidbody.AddForce((targetPosition.Value - transform.position) * GrabForce);
                }
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
                Rigidbody.velocity = Vector3.zero;
            }
        }

        public void Interact(InteractType interactType, InteractContext context)
        {
            switch (interactType)
            {
                case InteractType.Grab:
                    _player.GrabProp(this);
                    gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
                    Rigidbody.useGravity = false;
                    Rigidbody.drag = 10f;
                    Rigidbody.angularDrag = 10f;
                    break;
            }
        }


        public void Release(bool launch)
        {
            gameObject.layer = LayerMask.NameToLayer("Prop");
            Rigidbody.useGravity = true;
            Rigidbody.drag = _originalDragValue;
            Rigidbody.angularDrag = _originalAngularDragValue;
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