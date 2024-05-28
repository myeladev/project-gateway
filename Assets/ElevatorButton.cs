using System.Collections.Generic;
using UnityEngine;

namespace ProjectGateway
{
    public class ElevatorButton : MonoBehaviour, IInteractable
    {
        public ElevatorStation targetStation;
        private Light _buttonLight;
        private Elevator _elevator;
        
        private void Awake()
        {
            _buttonLight = GetComponentInChildren<Light>();
            _elevator = targetStation.GetComponentInParent<Elevator>();
            
        }

        public void OnPressed()
        {
            //_buttonLight.enabled = true;
            // TODO: Also play a sound
        }

        public void Reset()
        {
            //_buttonLight.enabled = false;
        }

        public bool IsInteractable => MyPlayer.instance.Character.CanInteract || MyPlayer.instance.Character.movingFurniture;
        
        public Dictionary<InteractType, string> GetInteractText(InteractContext context)
        {
            return new Dictionary<InteractType, string>(){{InteractType.Use, "Press"}};
        }
        
        public void Interact(InteractType interactType, InteractContext context)
        {
            switch (interactType)
            {
                case InteractType.Use:
                    _elevator.PressElevatorButton(this);
                    break;
            }
        }
    }
}
