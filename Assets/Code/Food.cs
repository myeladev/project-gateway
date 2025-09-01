using System.Collections.Generic;
using ProjectGateway.Code;
using ProjectGateway.Code.Scripts;

namespace ProjectGateway
{
    public class Food : Item, IInteractable
    {
        public float hungerRestoration;
        
        public new List<string> GetInteractOptions(InteractContext context)
        {
            // Get the base prop interactions
            var interactList = base.GetInteractOptions(context);
            // Add the "pick up" interaction for items
            interactList.Add("Eat");
            // Return the modified list
            return interactList;
        }
        
        public new void Interact(string option, InteractContext context)
        {
            base.Interact(option, context);
            switch (option)
            {
                case "Eat":
                    MyPlayer.instance.EatFood(this);
                    if (context == InteractContext.Inventory)
                    {
                        MyPlayer.instance.inventory.RemoveFromInventory(this);
                        UIManager.instance.inventoryUI.Refresh();
                    }
                    Destroy(gameObject);
                    break;
            }
        }
    }
}
