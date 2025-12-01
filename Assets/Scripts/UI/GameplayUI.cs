using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectDaydream.UI
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

        public override void OnCancel(BaseEventData _)
        {
            Debug.Log($"Pressing Cancel, there's currently {PanelStack.Count} panels active. Top panel: {(PanelStack.Any() ? PanelStack.Peek() : null )}");
            if (IsAnyPanelActive())
            {
                base.OnCancel(_);
            }
            else
            {
                PushPanel(pauseUI);
            }
            Debug.Log($"Done with Cancel, there's currently {PanelStack.Count} panels active. Top panel: {(PanelStack.Any() ? PanelStack.Peek() : null )}");
        }
    }
}
