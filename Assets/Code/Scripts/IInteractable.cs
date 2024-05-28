using System.Collections.Generic;

namespace ProjectGateway
{
    public interface IInteractable
    {
        bool IsInteractable { get; }

        Dictionary<InteractType, string> GetInteractText(InteractContext context);
        
        void Interact(InteractType interactType, InteractContext context);
    }

    public enum InteractType
    {
        /// <summary>
        /// For activating objects
        /// </summary>
        Use,
        
        /// <summary>
        /// For physics-based moving of objects
        /// </summary>
        Grab,
        
        /// <summary>
        /// For adding items to the inventory
        /// </summary>
        Pickup,
    }

    public enum InteractContext
    {
        Default,
        Inventory
    }
}