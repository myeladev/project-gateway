using System.Collections.Generic;
using UnityEngine;

namespace ProjectGateway
{
    public class InformationItem : Item, IInteractable
    {
        [SerializeField]
        [TextArea]
        private string informationText;
        
        public new Dictionary<InteractType, string> GetInteractText(InteractContext context)
        {
            // Get the base prop interactions
            var interactList = base.GetInteractText(context);
            // Add the "read" interaction
            interactList.Add(InteractType.Use, "Read");
            // Return the modified list
            return interactList;
        }
        
        public new void Interact(InteractType interactType, InteractContext context)
        {
            base.Interact(interactType, context);
            switch (interactType)
            {
                case InteractType.Use:
                    InformationUI.instance.ShowMessage(informationText);
                    break;
            }
        }
    }
}
