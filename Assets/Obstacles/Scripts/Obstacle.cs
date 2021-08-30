using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    private ObstaclePropertiesPreset obstaclePropertiesPreset;

    public int BaseDamage => obstaclePropertiesPreset.GetBaseDamage();
    public int VehicleSpeedDecrease => obstaclePropertiesPreset.GetVehicleSpeedDecrease();
    public float DestroyCountdownPlayer => obstaclePropertiesPreset.GetDestroyCountdownPlayer();
    public float DestroyCountdownObstacle => obstaclePropertiesPreset.GetDestroyCountdownObstacle();
    public float FadeSpeed => obstaclePropertiesPreset.GetFadeSpeed();

    public bool HitPlayer { get; set; }
    private bool hitObstacle;
    
    private float collisionTimeCount; // Seconds since collision with player or another obstacle

    private Renderer _renderer;

    void Start()
    {
        _renderer = GetComponentInChildren<Renderer>();
    }

    void FixedUpdate()
    {
        if (HitPlayer || hitObstacle) {
            DestroyCountdown();
        }
    }

    void OnCollisionEnter(Collision other)
    {
        // HitPlayer is set in ObstacleCollision
        if (!HitPlayer)
        {
            // Only the first hit deals damage and decreases player speed
            if (other.gameObject.CompareTag("Player"))
            {
                hitObstacle = false; // When a player hit happens after an obstacle hit, reset obstacle hit. The countdown time in effect is destroyCountdownPlayer
                collisionTimeCount = 0f; // Reset time count. Discard time count since obstacle hit and start counting up to destroyCountdownPlayer
            }
            else if (other.gameObject.CompareTag("Obstacle"))
            {
                // When an obstacle hit happens after a player hit, the obstacle hit is ignored. The countdown time is effect is still destroyCountdownPlayer
                hitObstacle = true;
            }
        }
    }

    // Destroy obstacle after countdown
    private void DestroyCountdown()
    {
        collisionTimeCount += Time.deltaTime;

        if ((HitPlayer && collisionTimeCount >= DestroyCountdownPlayer) || (hitObstacle && collisionTimeCount >= DestroyCountdownObstacle))
        {
            Color c = _renderer.material.color;
            c.a = Mathf.Lerp(c.a, 0f, FadeSpeed * Time.deltaTime);
            _renderer.material.color = c;

            if (_renderer.material.color.a < 0.2f)
            {
                // if (hitPlayer)
                // {
                //     Debug.Log("Destroying obstacle after collision with player (" + collisionTimeCount + " s)");
                // } else {
                //     Debug.Log("Destroying obstacle after collision with another obstacle (" + collisionTimeCount + " s)");
                // }
                Destroy(gameObject);
            }
            
            Destroy(gameObject);
        }
    }
}
