using FMODUnity;
using System.Collections;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private ObstaclePropertiesPreset obstaclePropertiesPreset;

    public int BaseDamage => obstaclePropertiesPreset.GetBaseDamage();
    public int VehicleSpeedDecrease => obstaclePropertiesPreset.GetVehicleSpeedDecrease();
    public float DestroyCountdownPlayer => obstaclePropertiesPreset.GetDestroyCountdownPlayer();
    public float DestroyCountdownObstacle => obstaclePropertiesPreset.GetDestroyCountdownObstacle();
    public float FadeSpeed => obstaclePropertiesPreset.GetFadeSpeed();

    public bool HitPlayer { get; set; }

    private Renderer[] renderers;
    private Collider _collider;
    private Collider playerCollider;
    
    public Coroutine DestroyObstacleCoroutine;

    private bool fadeOutReady;
    private StudioEventEmitter _eventEmitter;

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        _collider = GetComponent<Collider>();
        _eventEmitter = gameObject.GetComponent<StudioEventEmitter>();

        if (_collider == null)
        {
            _collider = GetComponentInChildren<Collider>();
        }
    }

    void OnCollisionEnter(Collision other)
    {
        // HitPlayer is set in ObstacleCollision
        // Only the first hit deals damage and decreases player speed
        if (other.gameObject.CompareTag("Player") && !HitPlayer)
        {
            _eventEmitter?.Play();
            // If countdown was already triggered by an obstacle hit, cancel it
            if (DestroyObstacleCoroutine != null)
            {
                StopCoroutine(DestroyObstacleCoroutine);
            }
            
            DestroyObstacleCoroutine = StartCoroutine(DestroyCountdown(DestroyCountdownPlayer));

            playerCollider = other.collider;
        }
        else if (other.gameObject.CompareTag("Obstacle"))
        {
            // If countdown was already triggered by a player hit, do nothing
            if (DestroyObstacleCoroutine == null)
            {
                DestroyObstacleCoroutine = StartCoroutine(DestroyCountdown(DestroyCountdownObstacle));
            }
        }
    }

    private IEnumerator DestroyCountdown(float countdown)
    {
        yield return new WaitForSeconds(countdown);
        FadeOut();
    }

    public void FadeOut()
    {
        foreach (Renderer _renderer in renderers)
        {
            StartCoroutine(FadeOutEnumerator(_renderer));
        }

        fadeOutReady = true;
    }

    public void FadeOut(Collider playerCollider)
    {
        this.playerCollider = playerCollider;
        FadeOut();
    }
    
    private IEnumerator FadeOutEnumerator(Renderer _renderer)
    {
        if (playerCollider != null)
        {
            Physics.IgnoreCollision(_collider, playerCollider);
        }
        
        if (!_renderer.material.HasProperty("_Color"))
        {
            yield break;
        }
        
        yield return new WaitUntil(() => fadeOutReady);
        
        Color c = _renderer.material.color;

        while (_renderer.material.color.a >= 0.2f)
        {
            c.a = Mathf.Lerp(c.a, 0f, FadeSpeed * Time.deltaTime);
            _renderer.material.color = c;
            yield return null;
        }
        
        Destroy(gameObject);
    }
}