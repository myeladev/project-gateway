using System.Collections.Generic;
using ProjectDaydream.UI;
using UnityEngine;

namespace ProjectDaydream.Logic
{
    public class ContainerObject : MonoBehaviour, IInteractable
    {
        public int width = 4;
        public int height = 2;
        private void Awake()
        {
            ContainerGrid = new ContainerGrid(width, height);
        }
        
        public ContainerGrid ContainerGrid;
        public bool IsInteractable => InteractController.Instance.CanInteract;
        public new List<string> GetInteractOptions(InteractContext context)
        {
            return context == InteractContext.Default ? 
                new List<string>
                {
                    "Open",
                }
                : new List<string>();
        }
        
        public new void Interact(string option, InteractContext context)
        {
            switch (option)
            {
                case "Open":
                    GameplayUI.Instance.OpenContainer(this);
                    break;
            }
        }
    }
}