using System;
using System.Linq;
using TMPro;
using UnityEngine;
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

        private const float InteractRange = 2f;

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            var interactable = CheckInteractables();

            interactText.enabled = interactable is not null;
            interactText.text = "";
            cursorImage.enabled = interactable is not null;

            if (interactable is not null)
            {
                var interactStrings = interactable.GetInteractText().Select(m => $"[{ GetInputTextForInteractType(m.Key) }] {m.Value}");
                interactText.text = string.Join(Environment.NewLine, interactStrings);

                if (Input.GetMouseButtonDown(0)) interactable.Interact(InteractType.Grab);
                if (Input.GetKeyDown(KeyCode.E)) interactable.Interact(InteractType.Use);
                if (Input.GetKeyDown(KeyCode.F)) interactable.Interact(InteractType.Pickup);
            }
        }

        private string GetInputTextForInteractType(InteractType interactType)
        {
            return interactType switch
            {
                InteractType.Use => "E",
                InteractType.Grab => "L Click",
                InteractType.Pickup => "F",
                _ => ""
            };
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
