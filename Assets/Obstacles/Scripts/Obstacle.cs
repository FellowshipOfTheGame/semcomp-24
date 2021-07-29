using System.Collections;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    private ObstaclePropertiesPreset obstaclePropertiesPreset;

    private int baseDamage;
    private int vehicleSpeedDecrease;
    private float destroyCountdownPlayer;
    private float destroyCountdownObstacle;

    private float collisionTimeCount; // Seconds since collision with player or another obstacle
    private bool hitPlayer;
    private bool hitObstacle;
    private float fadeSpeed;

    private Renderer renderer;

    void Start()
    {
        baseDamage = obstaclePropertiesPreset.GetBaseDamage();
        vehicleSpeedDecrease = obstaclePropertiesPreset.GetVehicleSpeedDecrease();
        destroyCountdownPlayer = obstaclePropertiesPreset.GetDestroyCountdownPlayer();
        destroyCountdownObstacle = obstaclePropertiesPreset.GetDestroyCountdownObstacle();
        fadeSpeed = obstaclePropertiesPreset.GetFadeSpeed();

        renderer = GetComponent<Renderer>();
    }

    private void FixedUpdate()
    {
        if (hitPlayer || hitObstacle) {
            DestroyCountdown();
        }
    }

    void OnCollisionEnter(Collision other)
    {
        if (!hitPlayer)
        {
            // Only the first hit deals damage and decreases player speed
            if (other.gameObject.CompareTag("Player"))
            {
                GameObject player = other.gameObject;

                // Simple damage calculation based only on the vehicle relative speed
                float damage1 = other.relativeVelocity.magnitude;

                // Damage calculation based on the dot product of the two vectors obtained from the collision

                // Equation 1
                float collisionDot1 = Vector3.Dot(other.relativeVelocity.normalized, other.GetContact(0).normal);
                float damage2 = other.relativeVelocity.magnitude * Mathf.Abs(collisionDot1);

                // Equation 2
                float collisionDot2 = Vector3.Dot((player.transform.position - other.GetContact(0).point).normalized, other.GetContact(0).normal);
                float damage3 = other.relativeVelocity.magnitude * Mathf.Abs(collisionDot2);

                /*// DEBUG ---------------
                
                Debug.DrawRay(player.transform.position, other.GetContact(0).normal, Color.magenta);
                Debug.DrawRay(player.transform.position, other.relativeVelocity + other.transform.rotation.eulerAngles, Color.cyan); // [equation 1]
                Debug.DrawRay(player.transform.position, player.transform.position - other.GetContact(0).point, Color.green); // [equation 2]

                Debug.Log("other.relativeVelocity: " + other.relativeVelocity);
                Debug.Log("player.transform.position - other.GetContact(0).point: " + (player.transform.position - other.GetContact(0).point).normalized);

                float angle1 = Vector3.Angle(other.relativeVelocity, other.GetContact(0).normal);
                float angle2 = Vector3.Angle(player.transform.position - other.GetContact(0).point, other.GetContact(0).normal);
                
                Debug.Log("Angle between relativeVelocity and contact point normal (cyan/magenta) [equation 1]: " + angle1);
                Debug.Log("Angle between 'that other vector' and contact point normal (green/magenta) [equation 2]: " + angle2);
                
                Debug.Log("Damage #1: " + damage1 + " (relative velocity)");
                Debug.Log("Damage #2: " + damage2 + " (relative velocity * vector dot [equation 1]) : " + collisionDot1);
                Debug.Log("Damage #3: " + damage3 + " (relative velocity * vector dot [equation 2] : " + collisionDot2);
                
                Time.timeScale = 0;
                
                // ---------------------*/

                int finalDamage = baseDamage + (int) Mathf.Floor(damage2 / 4f);
                player.GetComponent<HealthSystem>().Damage(finalDamage);
                Debug.Log("Damage: " + finalDamage);

                // Decrease vehicle speed according to the damage dealt by the obstacle
                other.rigidbody.AddRelativeForce(new Vector3(0, 0, -finalDamage * vehicleSpeedDecrease));

                hitPlayer = true;

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
    
    // If it's not possible to use rigidbody colliders due to performance issues, remove the component and use the collider as a trigger instead
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !hitPlayer)
        {
            GameObject player = other.gameObject;
            
            float damage = other.attachedRigidbody.velocity.magnitude;
            int finalDamage = baseDamage + (int) damage / 10;
            Debug.Log("Damage: " + finalDamage);
            
            player.GetComponent<HealthSystem>().Damage(finalDamage);

            // Decrease vehicle speed according to the damage dealt by the obstacle
            other.attachedRigidbody.AddRelativeForce(new Vector3(0, 0, -finalDamage * vehicleSpeedDecrease));

            Destroy(gameObject);
        }
    }

    // Destroy obstacle after countdown
    private void DestroyCountdown()
    {
        collisionTimeCount += Time.deltaTime;

        if ((hitPlayer && collisionTimeCount >= destroyCountdownPlayer) || (hitObstacle && collisionTimeCount >= destroyCountdownObstacle))
        {
            Color c = renderer.material.color;
            // c.a = Mathf.Lerp(1f, 0f, 15f * Time.deltaTime);
            c.a = Mathf.Clamp(c.a - (fadeSpeed * Time.deltaTime), 0f, 1f);
            renderer.material.color = c;

            if (c.a == 0)
            {
                if (hitPlayer)
                {
                    Debug.Log("Destroying obstacle after collision with player (" + collisionTimeCount + " s)");
                } else {
                    Debug.Log("Destroying obstacle after collision with another obstacle (" + collisionTimeCount + " s)");
                }
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator Fade()
    {
        // Color c = renderer.materials[0].color;
        // for (float ft = 1f; ft >= 0; ft -= 0.1f) 
        // {
        //     c.a = ft;
        //     renderer.material.color = c;
        // }

        while (renderer.material.color.a > 0f)
        {
            Color c = renderer.material.color;
            c.a = Mathf.Lerp(1f, 0f, 0.01f * Time.deltaTime);
            // c.a -= 0.1f * Time.deltaTime;
            renderer.material.color = c;
            yield return null;
        }
    }
}
