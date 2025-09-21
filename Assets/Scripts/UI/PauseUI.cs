using ProjectGateway.Core;
using ProjectGateway.DataPersistence;
using UnityEngine;

namespace ProjectGateway.UI
{
    public class PauseUI : UIPanel
    {
        [SerializeField]
        private SettingsUI settingsUI;
        public void ClickResume()
        {
            UIManager.Instance.HidePanel<PauseUI>();
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
            SceneLoader.Instance.LoadMainMenu();
        }

        public override void OnShow()
        {
            
        }

        protected override void OnHide()
        {
            
        }

        protected override void OnHidden()
        {
            
        }
    }
}
