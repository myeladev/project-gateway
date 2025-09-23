using ProjectGateway.Core;
using ProjectGateway.DataPersistence;
using UnityEngine;

namespace ProjectGateway.UI
{
    public class PauseUI : UIPanel
    {
        [SerializeField]
        private SettingsUI settingsUI;
        public void ClickSave()
        {
            DataPersistenceManager.Instance.SaveProfile();
        }
        
        public void ClickLeave()
        {
            SceneManager.Instance.LoadMainMenu();
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
