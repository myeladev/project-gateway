using System;
using ProjectGateway.DataPersistence;
using UnityEngine;

namespace ProjectGateway.UI
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

        public void ClickCreateNewSave()
        {
            DataPersistenceManager.Instance.CreateNewProfile("New Profile " + DateTime.Now.ToString("HH mm ss dd MM "));
            
            OnShow();
        }
    }
}
