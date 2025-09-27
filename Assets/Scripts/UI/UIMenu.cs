using System;
using System.Collections.Generic;
using System.Linq;
using ProjectGateway.Core;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ProjectGateway.UI
{
    [RequireComponent(typeof(Canvas))]
    [DisallowMultipleComponent]
    public class UIMenu : MonoBehaviour
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

        protected virtual void OnCancel()
        {
            if (rootCanvas.enabled && rootCanvas.gameObject.activeInHierarchy)
            {
                if (PanelStack.Any())
                {
                    PopPanel();
                }
            }
        }

        private void Start()
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