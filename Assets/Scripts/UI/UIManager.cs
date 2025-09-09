using UnityEngine;

namespace ProjectGateway.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        public bool IsInUI => CurrentPanel;
        public UIPanel CurrentPanel { get; private set; }

        public UIPanel inventoryUI;
        public UIPanel optionsUI;

        private void Awake()
        {
            Instance = this;
        }

        private void Update()
        {
            HandleUIPanels();
            HandleUIInputs();
        }

        private void HandleUIPanels()
        {
            inventoryUI.canvasGroup.alpha = CurrentPanel == inventoryUI ? 1f : 0f;
        }

        private void HandleUIInputs()
        {
            if (CurrentPanel)
            {
                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    CurrentPanel = null;
                }

                if(Input.GetKeyDown(KeyCode.Tab) && CurrentPanel == inventoryUI)
                {
                    CurrentPanel = null;
                }
            }
            else
            {
                if(Input.GetKeyDown(KeyCode.Tab))
                {
                    CurrentPanel = inventoryUI;
                    inventoryUI.Refresh();
                }
            }
        }
    }
}
