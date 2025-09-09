using ProjectGateway.Common;
using ProjectGateway.Logic;
using TMPro;
using UnityEngine;

namespace ProjectGateway.UI
{
    public class SleepUIManager : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI timeText;
        [SerializeField]
        private WeatherManager weatherManager;
        [SerializeField]
        private RectTransform sleepBackgroundBar;
        [SerializeField]
        private RectTransform sleepFillBar;

        private CanvasGroup _canvasGroup;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        private void Update()
        {
            _canvasGroup.alpha = MyPlayer.instance.isSleeping ? 1f : 0f;
            timeText.text = $"[{weatherManager.GetFriendlyTimeString()}]";
            sleepFillBar.UpdateBar(MyPlayer.instance.sleep, 100, sleepBackgroundBar);
        }
    }
}
