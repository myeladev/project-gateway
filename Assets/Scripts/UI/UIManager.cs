using UnityEngine;

namespace ProjectGateway.UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;
        public bool IsInUI => CurrentPanel;
        public UIPanel CurrentPanel { get; private set; }
        private void Awake()
        {
            Instance = this;
        }
    }
}
