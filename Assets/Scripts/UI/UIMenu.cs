using System;
using System.Collections.Generic;
using System.Linq;
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
        private Canvas _rootCanvas;
        
        private Stack<UIPanel> _panelStack = new ();

        private void Awake()
        {
            _rootCanvas = GetComponent<Canvas>();
        }

        private void OnCancel()
        {
            if (_rootCanvas.enabled && _rootCanvas.gameObject.activeInHierarchy)
            {
                if (_panelStack.Any())
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
            if (_panelStack.Any())
            {
                UIPanel currentPanel = _panelStack.Peek();

                if (currentPanel.exitOnNewPanelPush)
                {
                    PopPanel();
                }
            }
            
            panel.gameObject.SetActive(true);
            panel.Show();
            _panelStack.Push(panel);
            
            _panelStack.Push(panel);
        }
        
        public void PopPanel()
        {
            if (_panelStack.Any())
            {
                UIPanel currentPanel = _panelStack.Pop();
                currentPanel.Hide();
            }
        }

        public void PopAllPanels()
        {
            while (_panelStack.Any())
            {
                PopPanel();
            }
        }
        
        public bool IsPanelActive(UIPanel panel)
        {
            return _panelStack.Any(p => p == panel);
        }
    }
}