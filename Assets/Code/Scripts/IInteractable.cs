namespace ProjectGateway
{
    public interface IInteractable
    {
        bool IsInteractable { get; }

        string InteractText { get; }
        
        void Interact(InteractType interactType);
    }

    public enum InteractType
    {
        Use,
        Grab,
    }
}