using System.Collections.Generic;
using ProjectGateway.Code.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace ProjectGateway
{
    public class Furniture : MonoBehaviour, IInteractable
    {
        public bool IsInteractable => MyPlayer.instance.Character.CanInteract;

        private MyCharacterController _player;

        public Transform seatingAnchor;

        protected virtual void Awake()
        {
            _player = GameObject.FindGameObjectWithTag("Player").GetComponent<MyCharacterController>();
        }

        public List<string> GetInteractOptions(InteractContext context)
        {
            var list = new List<string>();
            list.Add("Move");
            if(seatingAnchor) list.Add("Sit");
            return list;
        }

        public void Interact(string option, InteractContext context)
        {
            switch (option)
            {
                case "Move":
                    if (_player.MoveFurniture(this))
                    {
                        gameObject.SetActive(false);
                    }
                    break;
                case "Sit":
                    MyPlayer.instance.Sit(seatingAnchor);
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
