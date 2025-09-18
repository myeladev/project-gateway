using UnityEngine;

namespace ProjectGateway.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField]
        private MenuListUI menuListUI;
        [SerializeField]
        private SelectSaveUI selectSaveUI;
        public void ClickEnter()
        {
            selectSaveUI.gameObject.SetActive(true);
            selectSaveUI.Show();
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
    }
}
