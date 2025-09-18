using ProjectGateway.DataPersistence;
using ProjectGateway.UI;
using UnityEngine;

namespace ProjectGateway
{
    public class PauseUI : UIPanel
    {
        [SerializeField]
        private SettingsUI settingsUI;
        public void ClickResume()
        {
            Hide();
        }
        
        public void ClickSave()
        {
            DataPersistenceManager.Instance.SaveProfile();
        }
        
        public void ClickSettings()
        {
            Hide();
            settingsUI.gameObject.SetActive(true);
            settingsUI.Show();
        }
        
        public void ClickLeave()
        {
            
        }

        public override void OnShow()
        {
            
        }

        protected override void OnHide()
        {
            
        }
    }
}
