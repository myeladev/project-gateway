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
                    if (MyPlayer.instance.sleep <= 90f)
                    {
                        MyPlayer.instance.Sleep(this);
                    }
                    else
                    {
                        FeedbackMessageUIManager.instance.ShowMessage("You're not tired enough to sleep");
                    }
                    break;
            }
        }
    }
}
