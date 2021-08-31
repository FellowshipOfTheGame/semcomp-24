using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

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

    [Space(10)]
    [Header("Graphics (post-processing)")]
    [Space(10)]
    
    [SerializeField] private Volume globalVolume;
    [SerializeField] private GameObject portalPrefab;

    private GameObject player;
    private HealthSystem healthSystem;
    private ScoreManager scoreManager;

    public bool InThePast { get; private set; }
    public int CurrentStage { get; private set; }
    
    private float counter;
    private TimeTravelPortal portal;

    private enum Period { Past, Present }

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
        
        scoreManager.GrantBonus(scoreBonus);

        while (counter > 0)
        {
            counter -= Time.deltaTime * decreaseRate;
            
            if (counter <= 100 - (decreaseRate * 1f))
            {
                CurrentStage = Mathf.RoundToInt(counter / (100f / (stages - 2))) + 1;
            }
            
            ChangeMaterials(Period.Past);

            yield return null;
        }

        GeneratePortal();

        yield return new WaitUntil(() => portal.Out);
        
        ChangeMaterials(Period.Present);
        
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

    // ReSharper disable Unity.PerformanceAnalysis
    private void ChangeMaterials(Period period)
    {
        foreach (GameObject _gameObject in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            Obstacle obstacle = _gameObject.GetComponent<Obstacle>();
            foreach (Renderer _renderer in _gameObject.GetComponentsInChildren<Renderer>())
            {
                _renderer.material = period switch
                {
                    Period.Present => obstacle.PresentMaterial,
                    Period.Past => obstacle.PastMaterial,
                    _ => throw new ArgumentOutOfRangeException(nameof(period), period, null)
                };
            }
        }
    }
}
