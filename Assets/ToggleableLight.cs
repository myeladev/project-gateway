using System.Collections.Generic;
using UnityEngine;

namespace ProjectGateway
{
    public class ToggleableLight : Furniture, IInteractable
    {
        [SerializeField]
        private List<Light> lights = new();
        
        public new Dictionary<InteractType, string> GetInteractText()
        {
            var interactText = base.GetInteractText();
            interactText.Add(InteractType.Use, "Toggle");
            return interactText;
        }

        public new void Interact(InteractType interactType)
        {
            base.Interact(interactType);
            switch (interactType)
            {
                case InteractType.Use:
                    foreach (var toggleLight in lights)
                    {
                        toggleLight.enabled = !toggleLight.enabled;
                    }
                    break;
            }
        }
        
    }
}
