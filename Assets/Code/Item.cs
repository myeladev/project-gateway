using System.Collections.Generic;
using ProjectGateway.Code;
using ProjectGateway.Code.Scripts;
using UnityEngine;

namespace ProjectGateway
{
    public class Item : Prop, IInteractable
    {
        public string itemName;
        [TextArea]
        public string itemDescription;
        public float weight;
        private Collider _collider;

        protected new void Awake()
        {
            base.Awake();
            _collider = GetComponent<Collider>();
        }

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
                        MyPlayer.instance.inventory.RemoveFromInventory(this);
                        UIManager.instance.inventoryUI.Refresh();
                        
                        Rigidbody.isKinematic = false;
                        _collider.enabled = true;
                        var targetPosition = Camera.main.transform.position + Camera.main.transform.forward * 2f;
                        Debug.Log(targetPosition);
                        transform.position = targetPosition;
                        Rigidbody.velocity = Vector3.zero;
                    }
                    else
                    {
                        var success = MyPlayer.instance.inventory.AttemptToAddItem(this);

                        if (!success)
                        {
                            FeedbackMessageUIManager.instance.ShowMessage("Not enough inventory space");
                        }
                        else
                        {
                            Rigidbody.isKinematic = true;
                            _collider.enabled = false;
                            transform.position = Utilities.InventoryPoolPosition;
                        }
                    }
                    break;
            }
        }
    }
}
