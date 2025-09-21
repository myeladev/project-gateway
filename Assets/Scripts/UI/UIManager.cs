using System;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectGateway.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        public bool IsInUI => CurrentPanel;
        public UIPanel CurrentPanel { get; private set; }

        [SerializeField] private List<UIPanel> panels = new();
        public InventoryUI inventoryUI;
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            foreach (var uiPanel in panels)
            {
                uiPanel.Hide();
            }
        }

        private void Update()
        {
            HandleUIPanels();
            HandleUIInputs();
        }

        private void HandleUIPanels()
        {
            foreach (var panel in panels)
            {
                panel.canvasGroup.alpha = CurrentPanel == panel ? 1f : 0f;
            }
        }

        private void HandleUIInputs()
        {
            if (CurrentPanel)
            {
                foreach (var panel in panels)
                {
                    if(Input.GetKeyDown(panel.hotKey) && CurrentPanel == panel)
                    {
                        CurrentPanel.Hide();
                        CurrentPanel = null;
                    }
                }
                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    CurrentPanel = null;
                }
            }
            else
            {
                foreach (var panel in panels)
                {
                    if(Input.GetKeyDown(panel.hotKey))
                    {
                        panel.gameObject.SetActive(true);
                        panel.Show();
                        CurrentPanel = panel;
                    }
                }
            }
        }

        public UIPanel GetPanel<T>()
        {
            return panels.Find(p => p is T);
        }
        
        public void ShowPanel<T>()
        {
            var panel = GetPanel<T>();
            if (panel)
            {
                panel.gameObject.SetActive(true);
                panel.Show();
                CurrentPanel = panel;
            }
        }

        public void HidePanel<T>()
        {
            var panel = GetPanel<T>();
            if (panel)
            {
                panel.Hide();
                CurrentPanel = null;
            }
        }
    }
}
