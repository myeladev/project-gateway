using System.Collections.Generic;
using ProjectGateway.Code;
using ProjectGateway.Code.Scripts;
using UnityEngine;
using UnityEngine.Serialization;

namespace ProjectGateway
{
    public class Item : Prop, IInteractable
    {
        public string itemName;
        [TextArea]
        public string itemDescription;
        public float weight;
        private Collider _collider;
        public bool canClean;

        protected new void Awake()
        {
            base.Awake();
            _collider = GetComponent<Collider>();
        }

        public new List<string> GetInteractOptions(InteractContext context)
        {
            // Get the base prop interactions
            var interactList = base.GetInteractOptions(context);
            // Add the "pick up" interaction for items
            interactList.Add(context == InteractContext.Inventory ? "Drop" : "Pick up");
            // Return the modified list
            return interactList;
        }
        
        public new void Interact(string option, InteractContext context)
        {
            base.Interact(option, context);
            switch (option)
            {
                case "Pickup":
                    MyPlayer.instance.inventory.RemoveFromInventory(this);
                    UIManager.instance.inventoryUI.Refresh();
                    
                    Rigidbody.isKinematic = false;
                    _collider.enabled = true;
                    var targetPosition = Camera.main.transform.position + Camera.main.transform.forward * 2f;
                    Debug.Log(targetPosition);
                    transform.position = targetPosition;
                    Rigidbody.linearVelocity = Vector3.zero;
                        break;
                case "Drop":
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
                    break;
            }
        }
    }
}
