using System;
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
        private TextMeshProUGUI itemContextActions;
        [SerializeField]
        private ItemViewerModel itemViewerModel;

        private Item selectedItem;

        void Update()
        {
            if (selectedItem && UIManager.instance.CurrentPanel == this)
            {
                if (Input.GetKeyDown(KeyCode.E)) selectedItem.Interact(InteractType.Use, InteractContext.Inventory);
                if (Input.GetKeyDown(KeyCode.F)) selectedItem.Interact(InteractType.Pickup, InteractContext.Inventory);
            }
        }
        
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
            var interactStrings = item.GetInteractText(InteractContext.Inventory).OrderBy(m => m.Key).Select(m => $"[{ Utilities.GetInputTextForInteractType(m.Key) }] {m.Value}");
            itemContextActions.text = string.Join(Environment.NewLine, interactStrings);
            
            itemViewerModel.SetItem(item.gameObject);
        }
    }
}
