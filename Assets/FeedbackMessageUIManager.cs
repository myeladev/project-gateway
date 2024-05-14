using TMPro;
using UnityEngine;

namespace ProjectGateway
{
    public class FeedbackMessageUIManager : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshProUGUI messageText;
        private CanvasGroup _canvasGroup;

        public static FeedbackMessageUIManager instance;
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            instance = this;
        }

        private float messageTimer;

        private void Update()
        {
            messageTimer -= Time.unscaledDeltaTime;
            _canvasGroup.alpha = messageTimer > 0f ? 1f : Mathf.Lerp(_canvasGroup.alpha, 0f, 4f * Time.unscaledDeltaTime);
        }

        public void ShowMessage(string message)
        {
            messageText.text = message;
            messageTimer = 4f;
        }
    }
}
