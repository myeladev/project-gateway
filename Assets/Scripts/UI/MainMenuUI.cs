using UnityEngine;

namespace ProjectGateway.UI
{
    public class MainMenuUI : UIMenu
    {
        public static MainMenuUI Instance;

        protected override void Awake()
        {
            Instance = this;
        }
        
        public void ClickLeave()
        {
            Application.Quit();
        }
    }
}
