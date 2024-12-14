using System.Collections.Generic;

namespace ProjectGateway
{
    public class Bed : Furniture, IInteractable
    {
        public new Dictionary<InteractType, string> GetInteractText(InteractContext context)
        {
            // Get the base prop interactions
            var interactList = base.GetInteractText(context);
            // Add the "pick up" interaction for items
            interactList.Add(InteractType.Use, "Sleep");
            // Return the modified list
            return interactList;
        }
        
        public new void Interact(InteractType interactType, InteractContext context)
        {
            base.Interact(interactType, context);
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
