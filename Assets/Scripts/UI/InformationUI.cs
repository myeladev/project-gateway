using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ProjectDaydream.UI
{
    public class InformationUI : MonoBehaviour
    {
        [SerializeField] 
        private TextMeshProUGUI messageText;
        private CanvasGroup _canvasGroup;
        private bool _shown;
        private InputAction _cancelAction;
        [HideInInspector]
        public bool IsViewingInformation => Mathf.Approximately(_canvasGroup.alpha, 1);

        public static InformationUI Instance;
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            Instance = this;
        }

        private void Start()
        {
            _cancelAction = InputSystem.actions.FindAction("Cancel");
        }

        private void Update()
        {
            if (_cancelAction.WasPressedThisFrame()) _shown = false;
            _canvasGroup.alpha = _shown ? 1f : Mathf.Lerp(_canvasGroup.alpha, 0f, 50f * Time.unscaledDeltaTime);
        }

        public void ShowMessage(string message)
        {
            messageText.text = message;
            _shown = true;
        }
    }
}
