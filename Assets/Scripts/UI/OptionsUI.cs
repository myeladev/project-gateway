using System.Collections.Generic;
using ProjectGateway.Logic;
using UnityEngine;

namespace ProjectGateway.UI
{
    public class OptionsUI : MonoBehaviour
    {
        public static OptionsUI Instance;
        public bool IsViewingOptions => Mathf.Approximately(canvasGroup.alpha, 1);
        private CanvasGroup canvasGroup;

        protected void Awake()
        {
            Instance = this;
            canvasGroup = GetComponent<CanvasGroup>();
            Close();
        }

        [SerializeField]
        private InteractOption interactOptionPrefab;
        private List<InteractOption> _options = new List<InteractOption>();
        private IInteractable _interactable;
        public void Refresh()
        {
            if (_interactable is not null)
            {
                var interactOptions = _interactable.GetInteractOptions(InteractContext.Default);

                foreach (var option in interactOptions)
                {
                    var newButton = Instantiate(interactOptionPrefab.gameObject, transform).GetComponent<InteractOption>();
                    newButton.SetOption(option);
                    _options.Add(newButton);
                }
            }
            else
            {
                Close();
            }
        }

        private void ClearOptions()
        {
            foreach (var option in _options)
            {
                Destroy(option.gameObject);
            }

            _options.Clear();
        }
        
        public void Open(IInteractable interactable)
        {
            _interactable = interactable;
            canvasGroup.alpha = 1;
            canvasGroup.interactable = true;
            Refresh();
        }

        public void ChooseOption(string option)
        {
            _interactable.Interact(option, InteractContext.Default);
            Close();
        }

        private void Close()
        {
            _interactable = null;
            canvasGroup.alpha = 0;
            canvasGroup.interactable = false;
            ClearOptions();
        }
    }
}
