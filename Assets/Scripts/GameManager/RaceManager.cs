using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SubiNoOnibus.UI;
using SubiNoOnibus.Networking.Requests;

public class RaceManager : MonoBehaviour
{
    [SerializeField] public GameObject player;

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

    [SerializeField] private Button itemButton;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemDurationText;

    private HealthSystem healthSystem;
    
    private Animator burningAnimator;
    
    private float timer;
    private int coins;
    private ScoreManager scoreManager;

    private Image startRaceCountdownCircleFill;
    
    private int itemsUsedCount;
    private float distanceTraveled;

    private VehicleController vehicle;

    public int Distance => Mathf.RoundToInt(distanceTraveled);
    public float Timer => timer;
    public int Coins => coins;
    public int ItemsUsed => itemsUsedCount;
    public long Score { get; private set; }
    
    [SerializeField] private RaceData raceData;
    

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
        
        startRaceCountdownCircleFill = startRaceCountdownSign.GetComponentsInChildren<Image>()[1];

        StartRace();
    }
    
    void Update()
    {
        timer += Time.deltaTime;
        distanceTraveled += vehicle.GetCurrentSpeed() * Time.deltaTime;
        UpdateUI();
    }

    private void UpdateUI()
    {
        // Update the score multiplier text and color
        // scoreMultiplier.sprite = scoreMultiplierSprites[scoreManager.GetRange()];
        scoreMultiplier.color = Color.Lerp(scoreMultiplier.color, scoreMultiplierColors[scoreManager.GetRange()], 1f * Time.deltaTime);
        scoreMultiplierText.text = System.Math.Round(scoreManager.GetMultiplier(), 1).ToString(System.Globalization.CultureInfo.GetCultureInfo("en-US")) + "x";
        
        // Update speedometer
        speedometer.fillAmount = Mathf.Clamp01(vehicle.GetCurrentSpeed() / vehicle.GetMaximumSpeed());

        // Burn animation (when player exceeds maximum speed)
        bool burn = vehicle.GetCurrentSpeed() > vehicle.GetMaximumSpeed();
        burning.enabled = burn;
        burningAnimator.SetBool("Burn", burn);

        if (burn)
        {
            burningAnimator.speed = Mathf.Clamp01(vehicle.GetCurrentSpeed() / vehicle.GetMaximumSpeed() - 0.5f);
        }
        
        // Update the timer
        timerText.text = System.TimeSpan.FromSeconds(timer).ToString("mm\\:ss\\:ff");
        
        // Update the score text
        scoreText.text = scoreManager.GetScore().ToString("000,000", System.Globalization.CultureInfo.GetCultureInfo("pt-BR"));
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
            (raceData) => Debug.Log(raceData),
            (req) => Debug.Log(req.error)
        );
        
        StartCoroutine(startRaceEnumerator);
    }

    public void EndRace()
    {
        Score = scoreManager.GetScore();

        raceData.gold = Coins;
        raceData.score = (int) Score;

        var finishRaceEnumerator = RaceRequestHandler.FinishRace(raceData, 
            () => Debug.Log("Success"), 
            (req) =>
            {
                Debug.Log(req.responseCode);
            }
        );
        
        StartCoroutine(finishRaceEnumerator);
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
}
