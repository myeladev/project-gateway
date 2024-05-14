using ProjectGateway.Code;
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
            hungerFillBar.UpdateBar(MyPlayer.instance.hunger, 100, hungerBackgroundBar);
            sleepFillBar.UpdateBar(MyPlayer.instance.sleep, 100, sleepBackgroundBar);
            weightFillBar.UpdateBar(MyPlayer.instance.inventory.CurrentWeight, Inventory.CarryLimit, weightBackgroundBar);
            weightCapacityText.text = $"{MyPlayer.instance.inventory.CurrentWeight:N1} / {Inventory.CarryLimit:N0}";
        }
    }
}
