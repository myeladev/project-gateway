using System;
using ProjectDaydream.DataPersistence;
using UnityEngine;

namespace ProjectDaydream.UI
{
    public class SaveSelectMenuUI : UIPanel
    {
        [SerializeField] private SaveSelectItemUI menuItemPrefab;
        [SerializeField] private Transform menuItemsParent;

        private void ClearMenuItems()
        {
            foreach (Transform child in menuItemsParent)
            {
                Destroy(child.gameObject);
            }
        }

        private void CreateMenuItem(GameMetaData metadata, Texture thumbnail)
        {
            var menuItem = Instantiate(menuItemPrefab.gameObject, menuItemsParent);
            menuItem.GetComponent<SaveSelectItemUI>().SetSave(metadata, thumbnail);
        }
        
        public override void OnShow()
        {
            var saves = DataPersistenceManager.Instance.LoadProfileList();
            ClearMenuItems();

            foreach (var (metadata, thumbnail) in saves)
            {
                CreateMenuItem(metadata, thumbnail);
            }
        }

        protected override void OnHide()
        {
            
        }
        protected override void OnHidden()
        {
            
        }
    }
}
