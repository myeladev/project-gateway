using System.Linq;
using UnityEngine;

namespace ProjectGateway.UI
{
    public class GameplayUI : UIMenu
    {
        public static GameplayUI Instance;
        
        [SerializeField]
        private PauseUI pauseUI;

        protected override void Awake()
        {
            Instance = this;
        }

        protected override void OnCancel()
        {
            Debug.Log($"Pressing Cancel, there's currently {PanelStack.Count} panels active. Top panel: {(PanelStack.Any() ? PanelStack.Peek() : null )}");
            if (IsAnyPanelActive())
            {
                base.OnCancel();
            }
            else
            {
                PushPanel(pauseUI);
            }
            Debug.Log($"Done with Cancel, there's currently {PanelStack.Count} panels active. Top panel: {(PanelStack.Any() ? PanelStack.Peek() : null )}");
        }
    }
}
