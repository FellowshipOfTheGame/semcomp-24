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
    
    private Coroutine destroyObstacleCoroutine;

    void Start()
    {
        renderers = GetComponentsInChildren<Renderer>();
        _collider = GetComponent<Collider>();

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
            // If countdown was already triggered by an obstacle hit, cancel it
            if (destroyObstacleCoroutine != null)
            {
                StopCoroutine(destroyObstacleCoroutine);
            }
            
            StartCoroutine(DestroyCountdown(DestroyCountdownPlayer));

            playerCollider = other.collider;
        }
        else if (other.gameObject.CompareTag("Obstacle"))
        {
            // If countdown was already triggered by a player hit, do nothing
            if (destroyObstacleCoroutine == null)
            {
                destroyObstacleCoroutine = StartCoroutine(DestroyCountdown(DestroyCountdownObstacle));
            }
        }
    }
    
    private IEnumerator DestroyCountdown(float countdownTime)
    {
        yield return new WaitForSeconds(countdownTime);
        
        foreach (Renderer _renderer in renderers)
        {
            StartCoroutine(FadeOut(_renderer));
        }
    }

    private IEnumerator FadeOut(Renderer _renderer)
    {
        Color c = _renderer.material.color;

        while (_renderer.material.color.a >= 0.2f)
        {
            if (_renderer.material.color.a >= 0.7f && _renderer.material.color.a <= 0.8f)
            {
                if (playerCollider != null)
                {
                    Physics.IgnoreCollision(_collider, playerCollider);
                }
            }
            
            c.a = Mathf.Lerp(c.a, 0f, FadeSpeed * Time.deltaTime);
            _renderer.material.color = c;
            yield return null;
        }
        
        Destroy(gameObject);
    }
}