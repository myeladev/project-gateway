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

        private MyCharacterController _player;
        protected Rigidbody Rigidbody;
        private Vector3 _originalPosition;
        private Quaternion _originalRotation;

        protected void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<MyCharacterController>();
            _originalPosition = transform.position;
            _originalRotation = transform.rotation;
        }

        private void Update()
        {
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
                    
                    break;
            }
        }
    }
}