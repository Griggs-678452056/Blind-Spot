using UnityEngine;
using UnityEngine.SceneManagement;

namespace BlindSpot
{
    public class MainMenu : MonoBehaviour
    {
        public void StartGame()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene("Level");
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
