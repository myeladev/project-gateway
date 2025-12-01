using ProjectDaydream.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace ProjectDaydream.UI
{
    public class PauseManager : MonoBehaviour
    {
        [SerializeField] private InputAction pauseAction;
        [SerializeField] private UIPanel pauseMenu;

        private bool isPaused;

        void Start()
        {
            pauseAction = new InputAction(
                name: "Pause",
                type: InputActionType.Button,
                binding: "<Keyboard>/escape"
            );

            pauseAction.performed += OnPausePerformed;
            pauseAction.Enable();
        }

        private void OnPausePerformed(InputAction.CallbackContext ctx)
        {
            if (!ctx.performed) return;

            if (!isPaused)
                OpenPause();
            else
                ClosePause();
        }

        private void OpenPause()
        {
            isPaused = true;
            pauseMenu.Show();
            Time.timeScale = 0f;
        }

        private void ClosePause()
        {
            isPaused = false;
            pauseMenu.Hide();
            Time.timeScale = 1f;

            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}