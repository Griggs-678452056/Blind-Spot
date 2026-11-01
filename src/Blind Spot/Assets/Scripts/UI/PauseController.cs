using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace BlindSpot
{
    public class PauseController : MonoBehaviour
    {
        [SerializeField] private GameObject _pauseMenu;
        [SerializeField] private InputActionReference _pauseAction;

        private bool _isPaused;

        private void OnEnable()
        {
            _pauseAction.action.Enable();
            _pauseAction.action.performed += OnPause;
        }

        private void OnDisable()
        {
            _pauseAction.action.performed -= OnPause;
            _pauseAction.action.Disable();
        }

        private void OnPause(InputAction.CallbackContext context)
        {
            if (_isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        public void ResumeGame()
        {
            _isPaused = false;
            _pauseMenu.SetActive(false);
            Time.timeScale = 1f;
        }

        public void PauseGame()
        {
            _isPaused = true;
            _pauseMenu.SetActive(true);
            Time.timeScale = 0f;
        }

        public void MainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Main Menu");
        }
    }
}