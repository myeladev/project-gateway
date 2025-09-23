using System;
using System.Collections.Generic;
using System.Linq;
using ProjectGateway.Logic;
using ProjectGateway.Objects.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectGateway.UI
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

        public static InventoryUI Instance;
        protected override void Awake()
        {
            base.Awake();
            Instance = this;
        }
        void Update()
        {
            if (selectedItem && UIManager.Instance.CurrentPanel == this)
            {
                IInteractable interactable = selectedItem;
                if (Input.GetKeyDown(KeyCode.E)) interactable.Interact("Use", InteractContext.Inventory);
                if (Input.GetKeyDown(KeyCode.F)) interactable.Interact("Drop", InteractContext.Inventory);
            }
        }

        public override void OnShow()
        {
            Refresh();
        }
        
        public void Refresh()
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
                // TODO: Change this to a local index variable, so that we keep position in the list when dropping items
                // We can reset the index to 0 when the UI is reopened
                SelectItem(items.First());
            }
            noItemsText.SetActive(!items.Any());
        }

        protected override void OnHide()
        {
            
        }
        protected override void OnHidden()
        {
            
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
