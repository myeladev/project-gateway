using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectGateway
{
    public class InventoryUI : UIPanel
    {
        [SerializeField]
        private List<InventoryListItemUI> activeItems = new();
        [SerializeField]
        private GameObject listItemPrefab;
        [SerializeField]
        private Transform itemListContentWindow;
        [SerializeField]
        private TextMeshProUGUI itemDetailName;
        [SerializeField]
        private ItemViewerModel itemViewerModel;

        private Item selectedItem;
        
        public override void Refresh()
        {
            selectedItem = null;
            foreach(Transform child in itemListContentWindow.transform)
            {
                Destroy(child.gameObject);
            }
            
            var items = MyPlayer.instance.inventory.GetItems();
            foreach (var item in items)
            {
                var newListItem = Instantiate(listItemPrefab, itemListContentWindow);
                newListItem.GetComponent<InventoryListItemUI>().Init(item);
            }
            
            if (items.Any())
            {
                itemListContentWindow.GetChild(0).GetComponent<Button>().onClick.Invoke();
            }
        }

        public void SelectItem(Item item)
        {
            selectedItem = item;
            itemDetailName.text = item.itemName;
            
            itemViewerModel.SetItem(item.gameObject);
        }
    }
}
