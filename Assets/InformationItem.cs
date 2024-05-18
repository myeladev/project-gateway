using System.Collections.Generic;
using UnityEngine;

namespace ProjectGateway
{
    public class InformationItem : Item, IInteractable
    {
        [SerializeField]
        [TextArea]
        private string informationText;
        
        public new Dictionary<InteractType, string> GetInteractText()
        {
            // Get the base prop interactions
            var interactList = base.GetInteractText();
            // Add the "pick up" interaction for items
            interactList.Add(InteractType.Use, "Read");
            // Return the modified list
            return interactList;
        }
        
        public new void Interact(InteractType interactType)
        {
            base.Interact(interactType);
            switch (interactType)
            {
                case InteractType.Use:
                    InformationUI.instance.ShowMessage(informationText);
                    break;
            }
        }
    }
}
