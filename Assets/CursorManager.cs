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
            interactText.text = interactable?.InteractText ?? "";
            cursorImage.enabled = interactable is not null;

            if (interactable is not null)
            {
                if (Input.GetMouseButtonDown(0)) interactable.Interact(InteractType.Grab);
                
                if(Input.GetKeyDown(KeyCode.E)) interactable.Interact(InteractType.Use);
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
