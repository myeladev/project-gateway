using UnityEngine;

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

        public static Vector3 InventoryPoolPosition => new Vector3(50, -100, 50);
    }
}