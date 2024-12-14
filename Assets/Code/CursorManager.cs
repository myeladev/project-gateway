using System;
using System.Linq;
using DG.Tweening;
using ProjectGateway.Code;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace ProjectGateway
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

        private const float InteractRange = 2f;

        private void Awake()
        {
            _camera = Camera.main;
        }


        private IInteractable _interactable;
        private void Update()
        {
            Cursor.lockState = UIManager.instance.IsInUI ? CursorLockMode.None : CursorLockMode.Locked;
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

            if (_interactable is not null)
            {
                var interactStrings = _interactable.GetInteractText(InteractContext.Default).OrderBy(m => m.Key).Select(m => $"[{ Utilities.GetInputTextForInteractType(m.Key) }] {m.Value}");
                interactText.text = string.Join(Environment.NewLine, interactStrings);

                if (Input.GetMouseButtonDown(0)) _interactable.Interact(InteractType.Grab, InteractContext.Default);
                if (Input.GetKeyDown(KeyCode.E)) _interactable.Interact(InteractType.Use, InteractContext.Default);
                if (Input.GetKeyDown(KeyCode.F)) _interactable.Interact(InteractType.Pickup, InteractContext.Default);
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
    }

    public enum CursorSprite
    {
        None,
        Grab
    }
}
