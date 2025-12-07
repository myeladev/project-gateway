using ProjectDaydream.Common;
using UnityEngine;

namespace ProjectDaydream.UI
{
    public class PlayerUIManager : MonoBehaviour
    {
        [Header("Hunger")] 
        [SerializeField]
        private RectTransform hungerBackgroundBar;
        [SerializeField]
        private RectTransform hungerFillBar;
        
        [Header("Sleep")] 
        [SerializeField]
        private RectTransform sleepBackgroundBar;
        [SerializeField]
        private RectTransform sleepFillBar;
        
        private void Update()
        {
            hungerFillBar.UpdateBar(Mathf.Clamp(100 /*TODO:PlayerController.Instance.hunger*/, 0, 100), 100, hungerBackgroundBar);
            sleepFillBar.UpdateBar(100 /*TODO:PlayerController.Instance.sleep*/, 100, sleepBackgroundBar);
        }
    }
}
