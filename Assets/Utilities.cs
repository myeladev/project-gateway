namespace ProjectGateway
{
    public static class Utilities
    {
        public static string GetInputTextForInteractType(InteractType interactType)
        {
            return interactType switch
            {
                InteractType.Use => "E",
                InteractType.Grab => "L Click",
                InteractType.Pickup => "F",
                _ => ""
            };
        }
    }
}