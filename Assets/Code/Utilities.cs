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
    }
}