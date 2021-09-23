using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class OnTimeTravelEventArgs : System.EventArgs
{
    public TimeTravel.Period Period { get; set; }
}

public class TimeTravel : MonoBehaviour
{
    [Space(10)]
    [Header("Gameplay")]

    [SerializeField] private float duration = 30f;
    [SerializeField] [Range(0f, 100f)] private float increaseRate = 1f;
    [SerializeField] private int stages = 5;
    [SerializeField] private float cooldownTime = 15f;
    [SerializeField] private int scoreBonus = 300;
    
    [Space(10)]
    [Header("Graphics (post-processing)")]

    [SerializeField] private GameObject globalVolumePast;
    [SerializeField] private Volume globalVolumeTransition;
    [SerializeField] private GameObject portalPrefab;
    
    [Header("Skybox")]
    [SerializeField] private Material pastSkybox;
    [SerializeField] private Material presentSkybox;

    [Header("Player Effect")]
    [SerializeField] private Material playerRegular;
    [SerializeField] private Material playerTimeTravel;
    [SerializeField] private List<Renderer> playerRenderers;

    [Space(10)]
    [Header("Music")]
    
    [SerializeField] private DynamicMusic musicPlayer;
    
    private GameObject player;
    private HealthSystem healthSystem;
    private ScoreManager scoreManager;
    private VehicleRenderer vehicleRenderer;

    public static bool InThePast { get; private set; }
    public int CurrentStage { get; private set; }
    
    private float counter;
    private bool cooldown;
    private TimeTravelPortal portal;

    public static event System.EventHandler<OnTimeTravelEventArgs> OnTimeTravel;
    
    public enum Period { Past, Present }

    void Start()
    {
        player = GetComponent<RaceManager>().player;
        healthSystem = player.GetComponent<HealthSystem>();
        vehicleRenderer = player.GetComponent<VehicleRenderer>();
        
        scoreManager = GetComponent<ScoreManager>();
    }
    
    void Update()
    {
        if (!cooldown && !InThePast && scoreManager.GetRange() == scoreManager.GetMaxRange())
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
        Debug.Log($"cooldown: {cooldown}");
        
        --------------- */
    }

    private IEnumerator Travel()
    {
        InThePast = true;
        healthSystem.SetInvulnerable(true);
        CurrentStage = stages - 1;

        const float transitionDuration = 3.5f;
        float decreaseRate = (100f / transitionDuration);
        
        musicPlayer.BeginTransition();
        globalVolumeTransition.gameObject.SetActive(true);
        globalVolumeTransition.weight = 0f;
        
        while (counter > 0)
        {
            counter -= Time.deltaTime * decreaseRate;
            globalVolumeTransition.weight = Mathf.MoveTowards(globalVolumeTransition.weight, 1f, decreaseRate/100f * Time.deltaTime);
            yield return null;
        }
        
        globalVolumeTransition.gameObject.SetActive(false);
        
        musicPlayer.EndTransition();
        RenderSettings.skybox = pastSkybox;
        foreach (Renderer renderer in playerRenderers)
        {
            renderer.sharedMaterial = playerTimeTravel;
        }
        
        counter = 100f;
        decreaseRate = (100f / duration);

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

        while (counter > Time.deltaTime * decreaseRate)
        {
            counter -= Time.deltaTime * decreaseRate;
            CurrentStage = Mathf.RoundToInt(counter / (100f / (stages - 2))) + 1;
            yield return null;
        }

        counter = 0f;
        CurrentStage = 0;

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

        if (!healthSystem.HasShield())
        {
            healthSystem.ActivateShield(1, 5f);
            vehicleRenderer.ActivateShieldeEffect(5f);
        }
        
        InThePast = false;
        RenderSettings.skybox = presentSkybox;
        foreach (Renderer renderer in playerRenderers)
        {
            renderer.sharedMaterial = playerRegular;
        }
        healthSystem.SetInvulnerable(false);

        cooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        cooldown = false;
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
