using System.Collections.Generic;

namespace ProjectGateway.Logic
{
    public interface IInteractable
    {
        bool IsInteractable { get; }

        List<string> GetInteractOptions(InteractContext context);
        
        void Interact(string option, InteractContext context);
    }

    public enum InteractContext
    {
        Default,
        Inventory
    }
}