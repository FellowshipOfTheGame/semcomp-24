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

        public void Open()
        {
            distanceText.text = $"{raceManager.Distance} m";
            timeText.text = System.TimeSpan.FromSeconds(raceManager.Timer).ToString("mm\\:ss\\:ff");
            coinsText.text = $"x{raceManager.Coins}";
            itemsUsedText.text = raceManager.ItemsUsed.ToString();
            finalScoreText.text = $"PONTUAÇÃO FINAL: {raceManager.Score} pts";
            finalScoreTextUnderlay.text = finalScoreText.text;
            
            Time.timeScale = 0f;
            
            gameObject.SetActive(true);
        }

        public void Close()
        {
            SceneManager.LoadScene(0);
        }

        public void Continue()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}