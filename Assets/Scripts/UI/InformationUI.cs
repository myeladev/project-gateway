using TMPro;
using UnityEngine;

namespace ProjectGateway.UI
{
    public class InformationUI : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshProUGUI messageText;
        private CanvasGroup _canvasGroup;
        private bool shown;
        [HideInInspector]
        public bool IsViewingInformation => Mathf.Approximately(_canvasGroup.alpha, 1);

        public static InformationUI instance;
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            instance = this;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q)) shown = false;
            _canvasGroup.alpha = shown ? 1f : Mathf.Lerp(_canvasGroup.alpha, 0f, 50f * Time.unscaledDeltaTime);
        }

        public void ShowMessage(string message)
        {
            messageText.text = message;
            shown = true;
        }
    }
}
