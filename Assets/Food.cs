using System.Collections.Generic;

namespace ProjectGateway
{
    public class Food : Item, IInteractable
    {
        public float hungerRestoration;
        
        public new Dictionary<InteractType, string> GetInteractText(InteractContext context)
        {
            // Get the base prop interactions
            var interactList = base.GetInteractText(context);
            // Add the "pick up" interaction for items
            interactList.Add(InteractType.Use, "Eat");
            // Return the modified list
            return interactList;
        }
        
        public new void Interact(InteractType interactType, InteractContext context)
        {
            base.Interact(interactType, context);
            switch (interactType)
            {
                case InteractType.Use:
                    MyPlayer.instance.EatFood(this);
                    if (context == InteractContext.Inventory)
                    {
                        MyPlayer.instance.inventory.RemoveFromInventory(this);
                        UIManager.instance.inventoryUI.Refresh();
                    }
                    break;
            }
        }
    }
}
