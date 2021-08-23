using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RaceManager : MonoBehaviour
{
    [SerializeField] public GameObject player;

    [Space(10)]
    [Header("Gameplay")]
    [SerializeField] private int startRaceCountdownTime = 3;
    [SerializeField] private Image startRaceCountdownPanel;
    [SerializeField] private GameObject startRaceCountdownCircle;
    [SerializeField] private TextMeshProUGUI startRaceCountdownText;

    [Space(10)]
    [Header("UI Elements")]
    
    [SerializeField] private GameObject HUDPanel;
    [SerializeField] private GameObject UIControls;
    
    [SerializeField] private Image speedometer;
    [SerializeField] private Image turbo;
    [SerializeField] private Sprite[] turboSprites;
    [SerializeField] private Image burning;

    [SerializeField] private Image scoreMultiplier;
    [SerializeField] private Sprite[] scoreMultiplierSprites;

    [SerializeField] private Color[] scoreMultiplierColors;

    [SerializeField] private TextMeshProUGUI scoreMultiplierText;
    
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI scoreText;

    private HealthSystem healthSystem;
    
    private Animator burningAnimator;
    
    private float timer;
    private bool timerActive;
    private int coins;
    private ScoreManager scoreManager;

    private Image startRaceCountdownCircleFill;
    
    private int usedItemsCount;
    private int distanceTraveled;

    private VehicleController vehicle;

    private void OnEnable()
    {
        healthSystem = player.GetComponent<HealthSystem>();
        healthSystem.OnDie += GameOver;
    }

    private void OnDisable()
    {
        healthSystem.OnDie -= GameOver;
    }

    void Start()
    {
        vehicle = player.GetComponent<VehicleController>();
        burningAnimator = burning.GetComponent<Animator>();
        // scoreMultiplier.sprite = scoreMultiplierSprites[0];
        scoreMultiplier.color = scoreMultiplierColors[0];
        scoreManager = GetComponent<ScoreManager>();
        
        startRaceCountdownCircleFill = startRaceCountdownCircle.GetComponentsInChildren<Image>()[1];
        Debug.Log(startRaceCountdownCircleFill.sprite.name);
        
        StartRace();
    }
    
    void Update()
    {
        // scoreMultiplier.sprite = scoreMultiplierSprites[scoreManager.GetRange()];
        scoreMultiplier.color = Color.Lerp(scoreMultiplier.color, scoreMultiplierColors[scoreManager.GetRange()], 1f * Time.deltaTime);
        
        scoreMultiplierText.text = System.Math.Round(scoreManager.GetMultiplier(), 1).ToString(System.Globalization.CultureInfo.GetCultureInfo("en-US")) + "x";
        
        speedometer.fillAmount = Mathf.Clamp01(vehicle.GetCurrentSpeed() / vehicle.GetMaximumSpeed());

        bool burn = vehicle.GetCurrentSpeed() > vehicle.GetMaximumSpeed();
        burning.enabled = burn;
        burningAnimator.SetBool("Burn", burn);

        if (burn)
        {
            burningAnimator.speed = Mathf.Clamp01(vehicle.GetCurrentSpeed() / vehicle.GetMaximumSpeed() - 0.5f);
        }

        if (timerActive)
            timer += Time.deltaTime;

        scoreText.text = scoreManager.GetScore().ToString("000,000", System.Globalization.CultureInfo.GetCultureInfo("pt-BR"));
        
        timerText.text = System.TimeSpan.FromSeconds(timer).ToString("mm\\:ss\\:ff");
    }

    private void GameOver(object sender, System.EventArgs e)
    {
        EndRace();
        // Call game over menu
    }

    private void StartRace()
    {
        Time.timeScale = 0f;
        HUDPanel.SetActive(false);
        UIControls.SetActive(false);
        startRaceCountdownPanel.gameObject.SetActive(true);
        startRaceCountdownPanel.enabled = true;
        startRaceCountdownCircle.SetActive(true);
        startRaceCountdownText.enabled = true;
        StartCoroutine(Countdown());
    }

    private void EndRace()
    {
        timerActive = false;
    }

    private IEnumerator Countdown()
    {
        float wait = 0.5f;
        float count = startRaceCountdownTime + wait;
        startRaceCountdownCircleFill.fillAmount = 0f;

        while (count > 0)
        {
            if (count <= startRaceCountdownTime && count >= 1)
            {
                startRaceCountdownText.text = Mathf.Round(count).ToString();
            }
            
            startRaceCountdownCircleFill.fillAmount += Time.unscaledDeltaTime / (startRaceCountdownTime + wait);

            if (startRaceCountdownCircleFill.fillAmount == 1f)
            {
                startRaceCountdownCircleFill.fillAmount = 0f;
            }

            count -= Time.unscaledDeltaTime;
            
            yield return null;
        }

        
        Time.timeScale = 1f;
        timerActive = true;
        startRaceCountdownPanel.enabled = false;
        startRaceCountdownCircle.SetActive(false);
        HUDPanel.SetActive(true);
        UIControls.SetActive(true);
        
        startRaceCountdownText.text = "GO!";

        yield return new WaitForSeconds(3);
        
        startRaceCountdownText.enabled = false;
        
        yield return null;
    }
}
