using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectDaydream.UI
{
    [RequireComponent(typeof(Canvas))]
    [DisallowMultipleComponent]
    public class UIMenu : MonoBehaviour, ICancelHandler
    {
        [SerializeField]
        private UIPanel initialPanel;
        [SerializeField]
        private GameObject firstFocusItem;
        [SerializeField]
        private Canvas rootCanvas;
        
        protected readonly Stack<UIPanel> PanelStack = new ();

        protected virtual void Awake()
        {
            
        }

        public virtual void OnCancel(BaseEventData _)
        {
            Debug.Log(rootCanvas.enabled + " - " + rootCanvas.gameObject.activeInHierarchy, gameObject);
            if (rootCanvas.enabled && rootCanvas.gameObject.activeInHierarchy)
            {
                if (PanelStack.Any() && PanelStack.Peek() != initialPanel)
                {
                    PopPanel();
                }
            }
        }

        private void Start()
        {
            Initialise();
        }
        
        private void OnEnable() => Initialise();

        private void Initialise()
        {
            if (initialPanel)
            {
                PushPanel(initialPanel);
            }
            
            if (firstFocusItem)
            {
                EventSystem.current.SetSelectedGameObject(firstFocusItem);
            }
        }

        public void PushPanel(UIPanel panel)
        {
            if (PanelStack.Any())
            {
                UIPanel currentPanel = PanelStack.Peek();

                if (currentPanel.exitOnNewPanelPush)
                {
                    PopPanel();
                }
            }
            
            panel.gameObject.SetActive(true);
            panel.Show();
            PanelStack.Push(panel);
        }
        
        public void PopPanel()
        {
            if (PanelStack.Any())
            {
                UIPanel currentPanel = PanelStack.Pop();
                currentPanel.Hide();
            }
        }

        public void PopAllPanels()
        {
            while (PanelStack.Any())
            {
                PopPanel();
            }
        }

        public bool IsAnyPanelActive()
        {
            return PanelStack.Any();
        }
        
        public bool IsPanelActive(UIPanel panel)
        {
            return PanelStack.Any(p => p == panel);
        }
    }
}