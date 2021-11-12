using UnityEngine;
using UnityEngine.SceneManagement;

#if SUBI_NO_ONIBUS_ONLINE
using Backend = SubiNoOnibus.Backend.Online.Requests;
#else
using Backend = SubiNoOnibus.Backend.Offline.Requests;
#endif

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
            var finishRaceEnumerator = Backend::RaceRequestHandler.FinishRace
            (
                data, 
                (data) => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex),
                raceManager.HandleEndRaceError
            );
            StartCoroutine(finishRaceEnumerator);
        }

        public void MainMenu()
        {
            RaceData data = raceManager.GetEndRaceData();
            var finishRaceEnumerator = Backend::RaceRequestHandler.FinishRace
            (
                data, 
                (data) => SceneManager.LoadScene("StartupMenu"),
                raceManager.HandleEndRaceError
            );
            StartCoroutine(finishRaceEnumerator);
        }
    }
}