using System.Collections.Generic;
using ProjectGateway.Logic;
using UnityEngine;

namespace ProjectGateway.UI
{
    public class OptionsUI : UIPanel
    {
        public static OptionsUI Instance;
        public bool IsViewingOptions => Mathf.Approximately(canvasGroup.alpha, 1);

        protected override void Awake()
        {
            base.Awake();
            Instance = this;
        }
        
        [SerializeField]
        private InteractOption interactOptionPrefab;
        private List<InteractOption> _options = new List<InteractOption>();
        private IInteractable _interactable;
        public override void Refresh()
        {
            ClearOptions();

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
            canvasGroup.alpha = interactable is not null ? 1 : 0;
            Refresh();
        }

        public void ChooseOption(string option)
        {
            _interactable.Interact(option, InteractContext.Default);
            Close();
        }

        public void Close()
        {
            ClearOptions();
            _interactable = null;
            canvasGroup.alpha = 0;
        }
    }
}
