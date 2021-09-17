using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SubiNoOnibus.UI;
using SubiNoOnibus.Networking.Requests;

public class RaceManager : MonoBehaviour
{
    [SerializeField] public GameObject player;
    [SerializeField] private RaceData raceData;

    [Space(10)]
    [Header(("Menu"))]
    
    [SerializeField] private GameOverMenu gameOverMenu;
    
    [Space(10)]
    [Header("Start Race Countdown")]
    [SerializeField] private int startRaceCountdownTime = 3;
    [SerializeField] private Image startRaceCountdownPanel;
    [SerializeField] private GameObject startRaceCountdownSign;
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
    [SerializeField] private TextMeshProUGUI scoreBonusText;
    [SerializeField] private TextMeshProUGUI coinsBonusText;

    [SerializeField] private Button itemButton;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemDurationText;

    [Header("Game progression")]
    [SerializeField] private LevelGenerator levelGenerator;
    [SerializeField] private float progressionTimeScale;
    [SerializeField] private int easyIndex;
    [SerializeField] private int mediumIndex;
    [SerializeField] private int hardIndex;
    [Tooltip("Weight on easy set moves towards this value")]
    [SerializeField] private float easyTarget;
    [Tooltip("Weight on medium set moves towards this value")]
    [SerializeField] private float mediumTarget;
    [Tooltip("Weight on hard set moves towards this value")]
    [SerializeField] private float hardTarget;

    private HealthSystem healthSystem;

    private TimeTravel timeTravel; 
        
    private Animator burningAnimator;
    
    private float timer;
    private int coins;
    private ScoreManager scoreManager;

    private Image startRaceCountdownCircleFill;
    
    private int itemsUsedCount;
    private float distanceTraveled;

    private VehicleController vehicle;
    private NitrousSystem nitrous;

    public int Distance => Mathf.RoundToInt(distanceTraveled);
    public float Timer => timer;
    public int Coins => coins;
    public int ItemsUsed => itemsUsedCount;
    public int Score { get; private set; }

    private Coroutine scoreBonusTextCoroutine;

    private void OnEnable()
    {
        healthSystem = player.GetComponent<HealthSystem>();
        scoreManager = GetComponent<ScoreManager>();
        
        healthSystem.OnDie += GameOver;
        scoreManager.OnScoreBonusGrant += ScoreBonusFeedback;
    }

    private void OnDisable()
    {
        healthSystem.OnDie -= GameOver;
        scoreManager.OnScoreBonusGrant -= ScoreBonusFeedback;
    }

    void Start()
    {
        vehicle = player.GetComponent<VehicleController>();
        nitrous = player.GetComponent<NitrousSystem>();
        timeTravel = GetComponent<TimeTravel>();
        burningAnimator = burning.GetComponent<Animator>();
        // scoreMultiplier.sprite = scoreMultiplierSprites[0];
        scoreMultiplier.color = scoreMultiplierColors[0];
        startRaceCountdownCircleFill = startRaceCountdownSign.GetComponentsInChildren<Image>()[1];

        StartRace();
    }
    
    void FixedUpdate()
    {
        timer += Time.deltaTime;
        distanceTraveled += vehicle.GetCurrentSpeed() * Time.deltaTime;

        if (easyIndex != -1) levelGenerator.sets[easyIndex].weight = Mathf.MoveTowards(levelGenerator.sets[easyIndex].weight, easyTarget, Time.deltaTime * progressionTimeScale);
        if (mediumIndex != -1) levelGenerator.sets[mediumIndex].weight = Mathf.MoveTowards(levelGenerator.sets[mediumIndex].weight, mediumTarget, Time.deltaTime * progressionTimeScale);
        if (hardIndex != -1) levelGenerator.sets[hardIndex].weight = Mathf.MoveTowards(levelGenerator.sets[hardIndex].weight, hardTarget, Time.deltaTime * progressionTimeScale);
        
        UpdateUI();
    }

    // Called when player dies
    private void GameOver(object sender, System.EventArgs e)
    {
        EndRace();
        gameOverMenu.Open();
    }
    
    public void StartRace()
    {
        HUDPanel.SetActive(false);
        UIControls.SetActive(false);
        StartCoroutine(Countdown(startRaceCountdownTime));
        
        // TODO: get player active item from ItemManager
        
        var startRaceEnumerator = RaceRequestHandler.StartRace(
            (raceData) => this.raceData = raceData,
            (req) => Debug.Log(req.error)
        );
        
        StartCoroutine(startRaceEnumerator);
    }

    public void EndRace()
    {
        Score = scoreManager.GetScore();

        raceData.gold = Coins;
        raceData.score = Score;

        var finishRaceEnumerator = RaceRequestHandler.FinishRace(raceData, 
            () => Debug.Log("Success"), 
            (req) =>
            {
                string errorMsg = (string) JsonUtility.FromJson<ErrorMessageData>(req.downloadHandler.text);
                Debug.Log($"{req.responseCode}: {req.error}");
                Debug.Log(errorMsg);
            }
        );
        
        StartCoroutine(finishRaceEnumerator);
    }

    public void AddCoin(int amount)
    {
        this.coins += amount;
    }
    
    public IEnumerator Countdown(int countdown)
    {
        Time.timeScale = 0f;
        
        float wait = 0.5f;
        float count = countdown + wait;

        startRaceCountdownPanel.gameObject.SetActive(true);
        startRaceCountdownPanel.enabled = true;
        startRaceCountdownSign.SetActive(true);
        startRaceCountdownCircleFill.fillAmount = 1f;
        startRaceCountdownText.enabled = true;
        startRaceCountdownText.text = countdown.ToString();

        while (count >= wait)
        {
            if (count <= countdown)
            {
                startRaceCountdownText.text = Mathf.Round(count).ToString();
            }
            
            if (startRaceCountdownCircleFill.fillAmount == 0f)
            {
                startRaceCountdownCircleFill.fillAmount = 1f;
            }
            
            startRaceCountdownCircleFill.fillAmount -= Time.unscaledDeltaTime / countdown;
            
            count -= Time.unscaledDeltaTime;

            yield return null;
        }

        Time.timeScale = 1f;
        startRaceCountdownPanel.enabled = false;
        startRaceCountdownSign.SetActive(false);
        HUDPanel.SetActive(true);
        UIControls.SetActive(true);
        
        startRaceCountdownText.text = "GO!";

        yield return new WaitForSeconds(3);
        
        startRaceCountdownText.enabled = false;
    }
    
    // TODO: ObtainItem and UseItem methods

    // // Subscribe to obtain item event
    // private void ObtainItem()
    // {
    //     itemIcon = itemManager.Icon;
    //     itemButton.interactable = true;
    // }

    // // Subscribe to use item event
    // private void UseItem()
    // {
    //     itemsUsedCount++;
    //     itemIcon = itemIconDefault;
    //     itemDurationText.enabled = true;
    //     itemDurationText.text = System.TimeSpan.FromSeconds(itemManager.Time).ToString("mm\\:ss\\:ff");
    //     itemButton.interactable = false;
    //
    //     StartCoroutine(DisableItemDurationText());
    // }

    // private IEnumerator DisableItemDurationText()
    // {
    //     yield return new WaitUntil(() => !itemManager.HasItem);
    //     itemDurationText.enabled = false;
    // }
    
    private void UpdateUI()
    {
        // Update the score multiplier text and color
        // scoreMultiplier.sprite = scoreMultiplierSprites[scoreManager.GetRange()];
        scoreMultiplier.color = Color.Lerp(scoreMultiplier.color, scoreMultiplierColors[scoreManager.GetRange()], 1f * Time.deltaTime);
        scoreMultiplierText.text = System.Math.Round(scoreManager.GetMultiplier(), 1).ToString(System.Globalization.CultureInfo.GetCultureInfo("en-US")) + "x";
        
        // Update speedometer
        speedometer.fillAmount = Mathf.Clamp01(vehicle.GetCurrentSpeed() / vehicle.GetMaximumSpeed());
        
        // Update time travel bar
        turbo.sprite = turboSprites[timeTravel.CurrentStage];

        // Burn animation (when player exceeds maximum speed)
        bool burn = nitrous.IsActive();
        burning.enabled = burn;
        burningAnimator.SetBool("Burn", burn);

        if (burn)
        {
            burningAnimator.speed = Mathf.Clamp01(vehicle.GetCurrentSpeed() / vehicle.GetActualMaximumSpeed());
        }
        
        // Update the timer
        timerText.text = System.TimeSpan.FromSeconds(timer).ToString("mm\\:ss\\:ff");
        
        // Update the score text
        scoreText.text = scoreManager.GetScore().ToString("000,000 PTS", System.Globalization.CultureInfo.GetCultureInfo("pt-BR"));
        coinsText.text = coins.ToString("D6", System.Globalization.CultureInfo.GetCultureInfo("pt-BR"));
    }
    
    private void ScoreBonusFeedback(object sender, OnScoreBonusGrantEventArgs eventArgs)
    {
        scoreBonusText.text = $"+{eventArgs.BonusAmount} PTS";

        if (scoreBonusTextCoroutine != null)
        {
            StopCoroutine(scoreBonusTextCoroutine);
        }

        scoreBonusTextCoroutine = StartCoroutine(FadeBonusText(scoreBonusText));
    }

    private IEnumerator FadeBonusText(TextMeshProUGUI bonusText)
    {
        bonusText.gameObject.SetActive(true);
        
        Color c = bonusText.color;
        
        if (bonusText.color.a < 1f)
        {
            c.a = 1f;
            bonusText.color = c;
        }
        
        c.a = 0f;

        float fadeInSpeed = 3f;
        float fadeOutSpeed = 0.5f;

        while (c.a < 1f)
        {
            c.a += Time.deltaTime * fadeInSpeed;
            bonusText.color = c;
            yield return null;
        }
        
        while (c.a > 0f)
        {
            c.a -= Time.deltaTime * fadeOutSpeed;
            bonusText.color = c;
            yield return null;
        }
        
        bonusText.gameObject.SetActive(false);
    }
}
