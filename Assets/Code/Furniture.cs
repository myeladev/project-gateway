using System.Collections.Generic;
using UnityEngine;

namespace ProjectGateway
{
    public class Furniture : MonoBehaviour, IInteractable
    {
        public bool IsInteractable => MyPlayer.instance.Character.CanInteract;

        private MyCharacterController _player;

        protected virtual void Awake()
        {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<MyCharacterController>();
        }

        public Dictionary<InteractType, string> GetInteractText(InteractContext context)
        {
            return 
                new Dictionary<InteractType, string>
                {
                    { InteractType.Pickup, "Move" },
                };
        }

        public void Interact(InteractType interactType, InteractContext context)
        {
            switch (interactType)
            {
                case InteractType.Pickup:
                    if (_player.MoveFurniture(this))
                    {
                        gameObject.SetActive(false);
                    }
                    break;
            }
        }

        public void Place(Vector3 position, Quaternion rotation)
        {
            gameObject.SetActive(true);
            transform.position = position;
            transform.rotation = rotation;
            GetComponent<Collider>().enabled = true;
        }
    }
}
