using ProjectDaydream.Objects.Items;
using TMPro;
using UnityEngine;

namespace ProjectDaydream.UI
{
    public class InventoryListItemUI : MonoBehaviour
    {
        private Item _currentItem;

        [SerializeField]
        private TextMeshProUGUI itemName;
        
        public void Init(Item item)
        {
            _currentItem = item;
            itemName.text = item.itemName;
        }

        public void Clicked()
        {
            GetComponentInParent<InventoryUI>().SelectItem(_currentItem);
        }
    }
}
