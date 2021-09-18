using UnityEngine;
using UnityEngine.SceneManagement;

namespace SubiNoOnibus.UI
{
    public class PauseMenu : MonoBehaviour, IMenu
    {
        [SerializeField] private RaceManager raceManager;
        [SerializeField] private int resumeRaceCountdown = 3;
        
        public void Open()
        {
            raceManager.Pause();
            Time.timeScale = 0f;
            gameObject.SetActive(true);
        }
        
        public void Close()
        {
            // raceManager.StartRace();
            raceManager.StartCoroutine(raceManager.Countdown(resumeRaceCountdown));
            gameObject.SetActive(false);
        }

        public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void MainMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}