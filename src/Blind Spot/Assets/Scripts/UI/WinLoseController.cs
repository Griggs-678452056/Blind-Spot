using UnityEngine;
using UnityEngine.SceneManagement;

namespace BlindSpot
{
    public class WinLoseController : MonoBehaviour
    {
        [SerializeField] private GameObject _winPanel;
        [SerializeField] private GameObject _losePanel;

        private bool _isGameOver;

        public void ShowWinPanel()
        {
            if (_isGameOver)
            {
                return;
            }

            _isGameOver = true;
            _winPanel.SetActive(true);
            Time.timeScale = 0f;
        }

        public void ShowLosePanel()
        {
            //Debug.Log($"Lose triggered at frame {Time.frameCount}");
            if (_isGameOver)
            {
                return;
            }

            _isGameOver = true;
            _losePanel.SetActive(true);
            Time.timeScale = 0f;
        }

        public void RestartGame()
        {
            Time.timeScale = 1f;
            //Debug.Log($"Начинаем заново. TimeScale = {Time.timeScale}");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void ContinueGame()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Game");
        }

        public void LoadMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Main Menu");
        }
    }
}
