using System;
using UnityEngine;

namespace ProjectGateway
{
    public class Prop : MonoBehaviour, IInteractable
    {
        public bool IsInteractable => true;
        public string InteractText => "Interact";

        private Vector3? targetPosition;
        private MyCharacterController _player;
        private Rigidbody _rigidbody;
        private float _originalDragValue;
        private Camera _camera;
        private const float GrabForce = 20f;

        private void Awake()
        {
            _camera = Camera.main;
            _rigidbody = GetComponent<Rigidbody>();
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<MyCharacterController>();
            _originalDragValue = _rigidbody.drag;
        }

        private void Update()
        {
            if (targetPosition is not null && Vector3.Distance(transform.position, targetPosition.Value) > 0.1f)
            {
                _rigidbody.AddForce((targetPosition.Value - transform.position) * GrabForce);
            }
        }

        public void Interact(InteractType interactType)
        {
            switch (interactType)
            {
                case InteractType.Grab:
                    _player.GrabProp(this);
                    _rigidbody.useGravity = false;
                    _rigidbody.drag = 10f;
                    break;
            }
        }


        public void Release(bool launch)
        {
            _rigidbody.useGravity = true;
            _rigidbody.drag = _originalDragValue;
            SetTarget(null);
            if (launch)
            {
                _rigidbody.AddForce(_camera.transform.TransformDirection(Vector3.forward) * (GrabForce * 25f));
            }
        }

        public void SetTarget(Vector3? newTargetPosition)
        {
            targetPosition = newTargetPosition;
        }
    }
}