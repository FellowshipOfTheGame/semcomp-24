using SubiNoOnibus.Networking.Requests;
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
            raceManager.StartCoroutine(raceManager.Countdown(resumeRaceCountdown));
            gameObject.SetActive(false);
        }

        public void Restart()
        {
            RaceData data = raceManager.GetEndRaceData();
            var finishRaceEnumerator = RaceRequestHandler.FinishRace
            (
                data, 
                (data) => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex)
            );
            StartCoroutine(finishRaceEnumerator);
        }

        public void MainMenu()
        {
            RaceData data = raceManager.GetEndRaceData();
            var finishRaceEnumerator = RaceRequestHandler.FinishRace(data, (data) => SceneManager.LoadScene("StartupMenu"));
            StartCoroutine(finishRaceEnumerator);
        }
    }
}