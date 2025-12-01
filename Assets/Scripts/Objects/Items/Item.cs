using System.Collections.Generic;
using System.Linq;
using ProjectDaydream.Common;
using ProjectDaydream.Logic;
using ProjectDaydream.UI;
using UnityEngine;

namespace ProjectDaydream.Objects.Items
{
    public class Item : Prop, IInteractable
    {
        public string itemName;
        [TextArea]
        public string itemDescription;
        public float weight;
        private List<Collider> _colliders;
        public bool canClean;

        protected new void Awake()
        {
            base.Awake();
            _colliders = GetComponentsInChildren<Collider>().ToList();
        }

        public new List<string> GetInteractOptions(InteractContext context)
        {
            // Get the base prop interactions
            var interactList = base.GetInteractOptions(context);
            // Add the "pick up" interaction for items
            interactList.Add(context == InteractContext.Inventory ? "Drop" : "Pickup");
            // Return the modified list
            return interactList;
        }
        
        public new void Interact(string option, InteractContext context)
        {
            base.Interact(option, context);
            switch (option)
            {
                case "Drop":
                    InventoryController.Instance.RemoveFromInventory(this);
                    InventoryUI.Instance.Refresh();
                    
                    var targetPosition = Camera.main.transform.position + Camera.main.transform.forward * 2f;
                    transform.position = targetPosition;
                    _colliders.ForEach(m => m.enabled = true);
                    Rigidbody.isKinematic = false;
                    Rigidbody.linearVelocity = Vector3.zero;
                    break;
                case "Pickup":
                    var success = InventoryController.Instance.AttemptToAddItem(this);

                    if (!success)
                    {
                        FeedbackMessageUIManager.instance.ShowMessage("Not enough inventory space");
                    }
                    else
                    {
                        Rigidbody.isKinematic = true;
                        _colliders.ForEach(m => m.enabled = false);
                        transform.position = Utilities.InventoryPoolPosition;
                    }
                    break;
            }
        }
    }
}
