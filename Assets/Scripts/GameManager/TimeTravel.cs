using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

public class TimeTravel : MonoBehaviour
{
    [Space(10)]
    [Header("Gameplay")]
    [Space(10)]
    
    [SerializeField] private float duration = 30f;
    [SerializeField] [Range(0f, 100f)] private float increaseRate = 1f;
    [SerializeField] private int stages = 5;

    [Space(10)]
    [Header("Graphics (post-processing)")]
    [Space(10)]
    
    [SerializeField] private Volume globalVolume;
    [SerializeField] private GameObject portalPrefab;

    private GameObject player;
    private VehicleController vehicle;
    private HealthSystem healthSystem;
    private ScoreManager scoreManager;

    public bool InThePast { get; private set; }
    public int CurrentStage { get; private set; }
    
    private float counter;
    private TimeTravelPortal portal;

    void Start()
    {
        player = GetComponent<RaceManager>().player;
        vehicle = player.GetComponent<VehicleController>();
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
            portal.GlobalVolume = globalVolume;
        }
        else
        {
            portal.transform.position = portalPosition;
        }
    }
}
