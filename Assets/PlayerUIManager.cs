using TMPro;
using UnityEngine;

namespace ProjectGateway
{
    public class PlayerUIManager : MonoBehaviour
    {
        [Header("Hunger")] 
        [SerializeField]
        private RectTransform hungerBackgroundBar;
        [SerializeField]
        private RectTransform hungerFillBar;
        
        [Header("Sleep")] 
        [SerializeField]
        private RectTransform sleepBackgroundBar;
        [SerializeField]
        private RectTransform sleepFillBar;
        
        [Header("Weight")] 
        [SerializeField]
        private RectTransform weightBackgroundBar;
        [SerializeField]
        private RectTransform weightFillBar;
        [SerializeField]
        private TextMeshProUGUI weightCapacityText;
        
        private void Update()
        {
            UpdateBar(MyPlayer.instance.hunger, 100, hungerBackgroundBar, hungerFillBar);
            UpdateBar(MyPlayer.instance.sleep, 100, sleepBackgroundBar, sleepFillBar);
            UpdateBar(MyPlayer.instance.inventory.CurrentWeight, Inventory.CarryLimit, weightBackgroundBar, weightFillBar);
            weightCapacityText.text = $"{MyPlayer.instance.inventory.CurrentWeight:N1} / {Inventory.CarryLimit:N0}";
        }

        private void UpdateBar(float numerator, float denominator, RectTransform backgroundBar, RectTransform fillBar)
        {
            var value = numerator / denominator;
            var barSize = Mathf.Abs(backgroundBar.rect.width);
            fillBar.offsetMax = new Vector2(-(barSize - (barSize * value)), 0);
        }
    }
}
