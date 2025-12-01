using System;
using System.Collections.Generic;
using System.Linq;
using ProjectDaydream.Logic;
using ProjectDaydream.Objects.Items;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectDaydream.UI
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

        private Item _selectedItem;
        private InputAction _interactAction;
        private InputAction _dropAction;

        public static InventoryUI Instance;
        
        protected override void Awake()
        {
            base.Awake();
            Instance = this;
        }
        
        private void Start()
        {
            _interactAction = InputSystem.actions.FindAction("Interact");
            _dropAction = InputSystem.actions.FindAction("Drop");
        }
        
        void Update()
        {
            if (_selectedItem && GameplayUI.Instance.IsPanelActive(this))
            {
                IInteractable interactable = _selectedItem;
                if (_interactAction.WasPressedThisFrame()) interactable.Interact("Use", InteractContext.Inventory);
                if (_dropAction.WasPressedThisFrame()) interactable.Interact("Drop", InteractContext.Inventory);
            }
        }

        public override void OnShow()
        {
            Refresh();
        }
        
        public void Refresh()
        {
            SelectItem(null); 
            _selectedItem = null;
            foreach(Transform child in itemListContentWindow.transform)
            {
                Destroy(child.gameObject);
            }
            
            var items = InventoryController.Instance.GetItems();
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
            _selectedItem = item;
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
