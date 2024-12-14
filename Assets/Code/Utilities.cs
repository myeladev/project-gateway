using UnityEngine;

namespace ProjectGateway.Code
{
    public static class Utilities
    {
        public static void UpdateBar(this RectTransform fillBar, float numerator, float denominator, RectTransform backgroundBar)
        {
            var value = numerator / denominator;
            var barSize = Mathf.Abs(backgroundBar.rect.width);
            fillBar.offsetMax = new Vector2(-(barSize - (barSize * value)), 0);
        }
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