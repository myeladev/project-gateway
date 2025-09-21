using UnityEngine;

namespace ProjectGateway.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField]
        private MenuListUI menuListUI;
        [SerializeField]
        private SaveSelectMenuUI saveSelectMenuUI;
        public void ClickEnter()
        {
            saveSelectMenuUI.gameObject.SetActive(true);
            saveSelectMenuUI.Show();
            menuListUI.Hide();
        }
        
        public void ClickSettings()
        {
            menuListUI.Hide();
            UIManager.Instance.ShowPanel<SettingsUI>();
        }
        
        public void ClickChanges()
        {
            menuListUI.Hide();
        }
        
        public void ClickLeave()
        {
            Application.Quit();
        }

        public void BackToMainMenuFromSelectSave()
        {
            menuListUI.gameObject.SetActive(true);
            menuListUI.Show();
            saveSelectMenuUI.Hide();
        }
    }
}
