using System.Collections.Generic;
using ProjectGateway.Logic;
using ProjectGateway.UI;
using UnityEngine;

namespace ProjectGateway.Objects.Items
{
    public class InformationItem : Item, IInteractable
    {
        [SerializeField]
        [TextArea]
        private string informationText;
        
        public new List<string> GetInteractOptions(InteractContext context)
        {
            // Get the base prop interactions
            var interactList = base.GetInteractOptions(context);
            // Add the "read" interaction
            interactList.Add("Read");
            // Return the modified list
            return interactList;
        }
        
        public new void Interact(string option, InteractContext context)
        {
            base.Interact(option, context);
            switch (option)
            {
                case "Read":
                    InformationUI.instance.ShowMessage(informationText);
                    break;
            }
        }
    }
}
