using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProjectGateway
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
            
            var interactOptions = _interactable.GetInteractOptions(InteractContext.Default);

            foreach (var option in interactOptions)
            {
                var newButton = Instantiate(interactOptionPrefab.gameObject, transform).GetComponent<InteractOption>();
                newButton.SetOption(option);
                _options.Add(newButton);
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
