using System;
using System.Collections.Generic;
using System.Linq;
using ProjectGateway.Code;
using ProjectGateway.Code.Scripts;
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
        private TextMeshProUGUI itemDetailDescription;
        [SerializeField]
        private GameObject noItemsText;
        [SerializeField]
        private TextMeshProUGUI itemContextActions;
        [SerializeField]
        private ItemViewerModel itemViewerModel;

        private Item selectedItem;

        void Update()
        {
            if (selectedItem && UIManager.instance.CurrentPanel == this)
            {
                IInteractable interactable = selectedItem;
                if (Input.GetKeyDown(KeyCode.E)) interactable.Interact("Use", InteractContext.Inventory);
                if (Input.GetKeyDown(KeyCode.F)) interactable.Interact("Drop", InteractContext.Inventory);
            }
        }
        
        public override void Refresh()
        {
            SelectItem(null); 
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
            noItemsText.SetActive(!items.Any());
        }

        public void SelectItem(Item item)
        {
            selectedItem = item;
            itemDetailName.text = item?.itemName;
            itemDetailDescription.text = item?.itemDescription;
            var interactStrings = (item as IInteractable)?.GetInteractOptions(InteractContext.Inventory).ToList();
            itemContextActions.text = interactStrings?.Any() ?? false ? string.Join(Environment.NewLine, interactStrings) : "";

            if (item)
            {
                itemViewerModel.SetItem(item.gameObject);
            }
            else
            {
                itemViewerModel.ClearItem();
            }
        }
    }
}
