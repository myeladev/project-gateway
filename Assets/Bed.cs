using System.Collections.Generic;

namespace ProjectGateway
{
    public class Bed : Furniture, IInteractable
    {
        public new Dictionary<InteractType, string> GetInteractText()
        {
            // Get the base prop interactions
            var interactList = base.GetInteractText();
            // Add the "pick up" interaction for items
            interactList.Add(InteractType.Use, "Sleep");
            // Return the modified list
            return interactList;
        }
        
        public new void Interact(InteractType interactType)
        {
            base.Interact(interactType);
            switch (interactType)
            {
                case InteractType.Use:
                    MyPlayer.instance.Sleep(this);
                    break;
            }
        }
    }
}
