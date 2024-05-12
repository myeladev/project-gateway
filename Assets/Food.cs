using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace ProjectGateway
{
    public class Food : Item, IInteractable
    {
        public float hungerRestoration;
        
        public new Dictionary<InteractType, string> GetInteractText()
        {
            // Get the base prop interactions
            var interactList = base.GetInteractText();
            // Add the "pick up" interaction for items
            interactList.Add(InteractType.Use, "Eat");
            // Return the modified list
            return interactList;
        }
        
        public new void Interact(InteractType interactType)
        {
            base.Interact(interactType);
            switch (interactType)
            {
                case InteractType.Use:
                    MyPlayer.instance.EatFood(this);
                    break;
            }
        }
    }
}
