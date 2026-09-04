using UnityEngine;

namespace DeccanHeat.UI
{
    public class UIManager : MonoBehaviour
    {
        public GameObject pauseMenuPanel;
        public GameObject hudPanel;

        private bool isPaused = false;

        public void OnPause(UnityEngine.InputSystem.InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                TogglePause();
            }
        }

        public void TogglePause()
        {
            isPaused = !isPaused;
            pauseMenuPanel.SetActive(isPaused);
            hudPanel.SetActive(!isPaused);

            Time.timeScale = isPaused ? 0 : 1;
        }

        public void ResumeGame()
        {
            if (isPaused) TogglePause();
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}