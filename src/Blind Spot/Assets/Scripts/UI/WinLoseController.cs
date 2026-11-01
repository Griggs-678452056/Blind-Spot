using UnityEngine;
using UnityEngine.SceneManagement;

namespace BlindSpot
{
    public class WinLoseController : MonoBehaviour
    {
        [SerializeField] private GameObject _winPanel;
        [SerializeField] private GameObject _losePanel;

        [SerializeField] private MusicPlayer _musicSource;
        [SerializeField] private AudioSource _effectsSource;

        [SerializeField] private AudioClip _winMusic;
        [SerializeField] private AudioClip _loseMusic;

        private bool _isGameOver;

        public void ShowWinPanel()
        {
            if (_isGameOver)
            {
                return;
            }

            _isGameOver = true;

            _musicSource.StopMusic();
            _effectsSource.PlayOneShot(_winMusic);

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

            _musicSource.StopMusic();
            _effectsSource.PlayOneShot(_loseMusic);

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
            SceneManager.LoadScene("Level");
        }

        public void LoadMainMenu()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Main Menu");
        }
    }
}
