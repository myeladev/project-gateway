using System;
using UnityEngine;

namespace ProjectGateway.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        public static MainMenuUI Instance;

        private void Awake()
        {
            Instance = this;
        }

        [SerializeField]
        private MenuListUI menuListUI;
        [SerializeField]
        private SaveSelectMenuUI saveSelectMenuUI;
        
        public void ClickLeave()
        {
            Application.Quit();
        }
    }
}
