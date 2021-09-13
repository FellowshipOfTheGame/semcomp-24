using System;
using UnityEngine;

public class ObstacleCollision : MonoBehaviour
{
    [SerializeField] private RaceManager raceManager;
    [SerializeField] private float scoreBonusMultiplier = 10f;

    private ScoreManager scoreManager;

    private HealthSystem healthSystem;
    private VehicleController controller;
    private VehicleRenderer renderer;

    private Rigidbody _rigidbody;

    void Start()
    {
        healthSystem = GetComponent<HealthSystem>();
        scoreManager = raceManager.GetComponent<ScoreManager>();
        controller = GetComponent<VehicleController>();
        renderer = GetComponent<VehicleRenderer>();
        
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            if (!other.gameObject.TryGetComponent(out Obstacle obstacle))
            {
                throw new NullReferenceException($"{other.gameObject.name} has the obstacle tag, but doesn't have an obstacle component!");
            }

            if (obstacle.HitPlayer)
            {
                return;
            }
            
            obstacle.HitPlayer = true;

            // Simple damage calculation based only on the vehicle relative speed
            // float damage1 = other.relativeVelocity.magnitude;

            // Damage calculation based on the dot product of the two vectors obtained from the collision

            // Equation 1
            float collisionDot1 = Vector3.Dot(other.relativeVelocity.normalized, other.GetContact(0).normal);
            float damage2 = (other.relativeVelocity.magnitude * Mathf.Abs(collisionDot1)) / 4f;

            // Equation 2
            // float collisionDot2 = Vector3.Dot((obstacle.transform.position - other.GetContact(0).point).normalized, other.GetContact(0).normal);
            // float damage3 = other.relativeVelocity.magnitude * Mathf.Abs(collisionDot2);

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

            int finalDamage = obstacle.BaseDamage + Mathf.RoundToInt(damage2);

            if (!healthSystem.IsInvulnerable())
            {
                if (healthSystem.HasShield())
                {
                    healthSystem.DecreaseShield();
                    if (!healthSystem.HasShield())
                        renderer.DeactivateShieldeEffect();
                }
                else
                {
                    healthSystem.Damage(finalDamage);
                }
            }

            if (TimeTravel.InThePast)
            {
                scoreManager.GrantBonus(Mathf.RoundToInt(finalDamage * scoreBonusMultiplier));
            }
            else
            {
                // Decrease vehicle speed according to the damage dealt by the obstacle
                //_rigidbody.AddRelativeForce(other.relativeVelocity.normalized * (finalDamage * obstacle.VehicleSpeedDecrease));
                controller.forwardForce -= Mathf.Clamp(controller.forwardForce, 0, damage2 * obstacle.VehicleSpeedDecrease);
            }
            
            // Debug.Log(finalDamage);
        }
    }
}
