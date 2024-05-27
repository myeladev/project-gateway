using UnityEngine;

namespace ProjectGateway
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;
        public bool IsInUI => _currentPanel;
        private UIPanel _currentPanel;

        [SerializeField] private UIPanel inventoryUI;

        private void Awake()
        {
            instance = this;
        }

        private void Update()
        {
            HandleUIPanels();
            HandleUIInputs();
        }

        private void HandleUIPanels()
        {
            inventoryUI.canvasGroup.alpha = _currentPanel == inventoryUI ? 1f : 0f;
        }

        private void HandleUIInputs()
        {
            if (_currentPanel)
            {
                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    _currentPanel = null;
                }

                if(Input.GetKeyDown(KeyCode.Tab) && _currentPanel == inventoryUI)
                {
                    _currentPanel = null;
                }
            }
            else
            {
                if(Input.GetKeyDown(KeyCode.Tab))
                {
                    _currentPanel = inventoryUI;
                    inventoryUI.Refresh();
                }
            }
        }
    }
}
