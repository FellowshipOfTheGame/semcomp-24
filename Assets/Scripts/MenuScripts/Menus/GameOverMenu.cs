using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace SubiNoOnibus.UI
{
    public class GameOverMenu : MonoBehaviour, IMenu
    {
        [SerializeField] private RaceManager raceManager;
        [SerializeField] private TextMeshProUGUI distanceText;
        [SerializeField] private TextMeshProUGUI timeText;
        [SerializeField] private TextMeshProUGUI coinsText;
        [SerializeField] private TextMeshProUGUI itemsUsedText;
        [SerializeField] private TextMeshProUGUI finalScoreText;
        [SerializeField] private TextMeshProUGUI finalScoreTextUnderlay;

        public void Open(bool isPersonalRecord)
        {
            if (isPersonalRecord)
                finalScoreText.text = $"NOVO RECORDE: {raceManager.Score} pts";
            else
                finalScoreText.text = $"PONTUAÇÃO FINAL: {raceManager.Score} pts";

            Open();
        }

        public void Open()
        {
            distanceText.text = $"{raceManager.Distance} m";
            timeText.text = System.TimeSpan.FromSeconds(raceManager.Timer).ToString("mm\\:ss\\:ff");
            coinsText.text = $"x{raceManager.Coins}";
            finalScoreTextUnderlay.text = finalScoreText.text;
            
            Time.timeScale = 0f;
            
            gameObject.SetActive(true);
        }

        public void Close()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene("StartupMenu");
        }

        public void Continue()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}