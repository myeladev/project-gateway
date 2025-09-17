using System;
using DG.Tweening;
using ProjectGateway.Logic;
using ProjectGateway.Objects.Furniture;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ProjectGateway.UI
{
    public class CursorManager : MonoBehaviour
    {
        private Camera _camera;
        
        [Header("UI Elements")]
        [SerializeField]
        private TextMeshProUGUI interactText;
        [SerializeField]
        private Image cursorImage;

        [Header("Misc")] 
        [SerializeField] 
        private MyCharacterController characterController;

        private const float InteractRange = 3f;

        private void Awake()
        {
            _camera = Camera.main;
        }


        private IInteractable _interactable;
        private void Update()
        {
            //Cursor.lockState = UIManager.Instance.IsInUI ? CursorLockMode.None : optionsPanel.IsViewingOptions ? CursorLockMode.None : CursorLockMode.Locked;
            /*
            if (!characterController.CanInteract)
            {
                interactText.enabled = false;
                cursorImage.enabled = false;
                return;
            }*/
            var oldInteractable = _interactable;
            _interactable = CheckInteractables();

            var cursorTweenDuration = 0.1f;
            // If the cursor has moved off of an interactable
            if (oldInteractable is not null && _interactable is null)
            {
                cursorImage.transform.DOScale(new Vector3(1f, 1f, 1f), cursorTweenDuration)
                    .SetEase(Ease.Linear);
                cursorImage.DOFade(0.05f, cursorTweenDuration)
                    .SetEase(Ease.Linear);
            }
            // If the cursor has moved over an interactable
            else if (oldInteractable is null && _interactable is not null)
            {
                cursorImage.transform.DOScale(new Vector3(1.75f, 1.75f, 1f), cursorTweenDuration)
                    .SetEase(Ease.Linear);
                cursorImage.DOFade(0.6f, cursorTweenDuration)
                    .SetEase(Ease.Linear);
            }

            interactText.enabled = _interactable is not null;
            interactText.text = "";

            if (_interactable is Furniture furniture && furniture.seatingAnchor == MyPlayer.instance.currentSeatingAnchor)
            {
                return;
            }

            if (_interactable is not null)
            {
                var interactStrings = _interactable.GetInteractOptions(InteractContext.Default);

                if (!optionsPanel.IsViewingOptions)
                {
                    interactText.text = $"[L Click] {interactStrings[0]}";
                    if (interactStrings.Count > 1)
                    {
                        interactText.text += Environment.NewLine + $"[R Click] Options...";
                    }
                }
                else
                {
                    interactText.text = "";
                }

                if (Input.GetMouseButtonDown(0) && !optionsPanel.IsViewingOptions) _interactable.Interact(interactStrings[0], InteractContext.Default);
                if (Input.GetMouseButtonDown(1))
                {
                    ShowInteractOptions(optionsPanel.IsViewingOptions ? null : _interactable);
                }
            }
        }

        private IInteractable CheckInteractables()
        {
            if (Physics.Raycast(_camera.transform.position, _camera.transform.TransformDirection(Vector3.forward),
                    out var hit, InteractRange))
            {
                var interactable = hit.transform.GetComponent<IInteractable>();
                
                if (interactable?.IsInteractable ?? false) return interactable;
            }

            return null;
        }

        [SerializeField] private OptionsUI optionsPanel;
        private void ShowInteractOptions(IInteractable interactable)
        {
            optionsPanel.Open(interactable);
        }
    }

    public enum CursorSprite
    {
        None,
        Grab
    }
}
