using System.Collections.Generic;
using UnityEngine;

namespace ProjectGateway
{
    public class Item : Prop, IInteractable
    {
        public string itemName;
        public float weight;
        
        public new Dictionary<InteractType, string> GetInteractText(InteractContext context)
        {
            // Get the base prop interactions
            var interactList = base.GetInteractText(context);
            // Add the "pick up" interaction for items
            interactList.Add(InteractType.Pickup, context == InteractContext.Inventory ? "Drop" : "Pick up");
            // Return the modified list
            return interactList;
        }
        
        public new void Interact(InteractType interactType, InteractContext context)
        {
            base.Interact(interactType, context);
            switch (interactType)
            {
                case InteractType.Pickup:
                    if (context == InteractContext.Inventory)
                    {
                        gameObject.SetActive(true);
                        transform.position = MyPlayer.instance.transform.position + (Vector3.up * 2f) + MyPlayer.instance.transform.forward;
                        Rigidbody.velocity = Vector3.zero;
                        
                        MyPlayer.instance.inventory.RemoveFromInventory(this);
                        UIManager.instance.inventoryUI.Refresh();
                    }
                    else
                    {
                        var success = MyPlayer.instance.inventory.AttemptToAddItem(this);

                        if (!success)
                        {
                            FeedbackMessageUIManager.instance.ShowMessage("Not enough inventory space");
                        }
                    }
                    break;
            }
        }
    }
}
