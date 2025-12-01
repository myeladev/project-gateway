using ProjectDaydream.Core;
using ProjectDaydream.DataPersistence;
using UnityEngine;

namespace ProjectDaydream.UI
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
