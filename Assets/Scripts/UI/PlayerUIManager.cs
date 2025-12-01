using ProjectDaydream.Objects.Items;
using ProjectDaydream.Common;
using ProjectDaydream.Logic;
using TMPro;
using UnityEngine;

namespace ProjectDaydream.UI
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
            hungerFillBar.UpdateBar(Mathf.Clamp(100 /*TODO:PlayerController.Instance.hunger*/, 0, 100), 100, hungerBackgroundBar);
            sleepFillBar.UpdateBar(100 /*TODO:PlayerController.Instance.sleep*/, 100, sleepBackgroundBar);
            weightFillBar.UpdateBar(InventoryController.Instance.CurrentWeight, InventoryController.CarryLimit, weightBackgroundBar);
            weightCapacityText.text = $"{InventoryController.Instance.CurrentWeight:N1} / {InventoryController.CarryLimit:N0}";
        }
    }
}
