using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class OnTimeTravelEventArgs : System.EventArgs
{
    public TimeTravel.Period Period { get; set; }
}

public class TimeTravel : MonoBehaviour
{
    [Space(10)]
    [Header("Gameplay")]
    [Space(10)]
    
    [SerializeField] private float duration = 30f;
    [SerializeField] [Range(0f, 100f)] private float increaseRate = 1f;
    [SerializeField] private int stages = 5;

    [Space(10)]
    [Header("Bonuses")]
    [Space(10)]
    
    [SerializeField] private int scoreBonus = 300;
    // [SerializeField] private int coinsBonus = 100;

    [FormerlySerializedAs("pastGlobalVolume")]
    [Space(10)]
    [Header("Graphics (post-processing)")]
    [Space(10)]
    
    [SerializeField] private GameObject globalVolumePast;
    [SerializeField] private GameObject portalPrefab;

    private GameObject player;
    private HealthSystem healthSystem;
    private ScoreManager scoreManager;

    public static bool InThePast { get; private set; }
    public int CurrentStage { get; private set; }
    
    private float counter;
    private TimeTravelPortal portal;

    public static event System.EventHandler<OnTimeTravelEventArgs> OnTimeTravel;
    
    public enum Period { Past, Present }

    void Start()
    {
        player = GetComponent<RaceManager>().player;
        healthSystem = player.GetComponent<HealthSystem>();
        
        scoreManager = GetComponent<ScoreManager>();
    }
    
    void Update()
    {
        if (!InThePast && scoreManager.GetRange() == scoreManager.GetMaxRange())
        {
            counter += Time.deltaTime * increaseRate;
            CurrentStage = Mathf.RoundToInt(counter / (100f / (stages - 2)));
        }

        if (counter >= 100)
        {
            StartCoroutine(Travel());
        }

        /* DEBUG --------
        
        Debug.Log($"InThePast: {InThePast}");
        Debug.Log($"counter: {counter}");
        Debug.Log($"CurrentStage: {CurrentStage}");
        
        --------------- */
    }

    private IEnumerator Travel()
    {
        InThePast = true;
        healthSystem.SetInvulnerable(true);
        CurrentStage = stages - 1;

        float decreaseRate = (100f / duration);

        GeneratePortal();

        if (OnTimeTravel != null)
        {
            OnTimeTravelEventArgs e = new OnTimeTravelEventArgs() 
            {
                Period = Period.Past
            };
            
            OnTimeTravel(this, e);
        }

        scoreManager.GrantBonus(scoreBonus);

        while (counter > 0)
        {
            counter -= Time.deltaTime * decreaseRate;
            
            if (counter <= 100 - (decreaseRate * 1f))
            {
                CurrentStage = Mathf.RoundToInt(counter / (100f / (stages - 2))) + 1;
            }

            yield return null;
        }

        GeneratePortal();

        yield return new WaitUntil(() => portal.Out);

        if (OnTimeTravel != null)
        {
            OnTimeTravelEventArgs e = new OnTimeTravelEventArgs()
            {
                Period = Period.Present
            };
            
            OnTimeTravel(this, e);
        }

        InThePast = false;
        healthSystem.SetInvulnerable(false);
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    private void GeneratePortal()
    {
        Vector3 portalPosition = Vector3.forward * (player.transform.position.z + 4f);
        
        if (portal == null)
        {
            portal = Instantiate(portalPrefab, portalPosition, Quaternion.identity).GetComponent<TimeTravelPortal>();
            portal.GlobalVolumePast = globalVolumePast;
        }
        else
        {
            portal.transform.position = portalPosition;
        }
    }
}
