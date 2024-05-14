using System.Collections.Generic;

namespace ProjectGateway
{
    public class Item : Prop, IInteractable
    {
        public string itemName;
        public float weight;
        
        public new Dictionary<InteractType, string> GetInteractText()
        {
            // Get the base prop interactions
            var interactList = base.GetInteractText();
            // Add the "pick up" interaction for items
            interactList.Add(InteractType.Pickup, "Pick up");
            // Return the modified list
            return interactList;
        }
        
        public new void Interact(InteractType interactType)
        {
            base.Interact(interactType);
            switch (interactType)
            {
                case InteractType.Pickup:
                    var success = MyPlayer.instance.inventory.AttemptToAddItem(this);

                    if (!success)
                    {
                        FeedbackMessageUIManager.instance.ShowMessage("Not enough inventory space");
                    }
                    break;
            }
        }
    }
}
