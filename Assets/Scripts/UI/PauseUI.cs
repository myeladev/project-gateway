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
            FeedbackMessageUIManager.instance.ShowMessage("Game saved");
        }
        
        public void ClickLeave()
        {
            PauseManager.Instance.Unpause();
            SceneManager.Instance.LoadMainMenu();
        }

        public override void OnShow()
        {
            PauseManager.Instance.Pause();
        }

        protected override void OnHide()
        {
            PauseManager.Instance.Unpause();
            //Cursor.lockState = CursorLockMode.Locked;
        }

        protected override void OnHidden()
        {
            
        }
    }
}
