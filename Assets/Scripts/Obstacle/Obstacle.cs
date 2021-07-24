using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField]
    private int baseDamageAmount;

    private bool hit;
    private float collisionTime = 0f;

    void Start() {
        
    }
    
    void Update() {
        if (collisionTime > 1.5f) {
            Debug.Log("Destroying obstacle after " + collisionTime + " seconds");
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.CompareTag("Player") && !hit) {
            GameObject player = other.gameObject;
            
            float damage1 = other.rigidbody.velocity.magnitude;
            Debug.Log("Damage #1: " + damage1 + " (velocity)");

            float damage2 = other.relativeVelocity.magnitude;
            Debug.Log("Damage #2: " + damage2 + " (relative velocity)");

            float damage3 = other.relativeVelocity.magnitude * Mathf.Abs(Vector3.Dot(other.relativeVelocity.normalized, other.GetContact(0).normal));
            Debug.Log("Damage #3: " + damage3 + " (relative velocity * vector dot [equation 1]) : " + Vector3.Dot(other.relativeVelocity.normalized, other.GetContact(0).normal));
            
            float damage3_2 = other.relativeVelocity.magnitude * Mathf.Abs(Mathf.Cos(Vector3.Angle(other.relativeVelocity.normalized, other.GetContact(0).normal) * Mathf.Deg2Rad));
            Debug.Log("Damage #3.2: " + damage3_2 + " (relative velocity * vector cos [equation 1]) : cos(" + Vector3.Angle(other.relativeVelocity.normalized, other.GetContact(0).normal) + ") = " + Mathf.Cos(Vector3.Angle(other.relativeVelocity.normalized, other.GetContact(0).normal)  * Mathf.Deg2Rad));
            Debug.Log("Distance: " + Vector3.Distance(other.relativeVelocity.normalized, other.GetContact(0).normal) + " [equation 1]");

            float damage4 = other.relativeVelocity.magnitude * Mathf.Abs(Vector3.Dot((player.transform.position - other.GetContact(0).point).normalized, other.GetContact(0).normal));
            Debug.Log("Damage #4: " + damage4 + " (relative velocity * vector dot [equation 2] : " + Vector3.Dot((player.transform.position - other.GetContact(0).point).normalized, other.GetContact(0).normal));

            float damage4_2 = other.relativeVelocity.magnitude * Mathf.Abs(Mathf.Cos(Vector3.Angle((player.transform.position - other.GetContact(0).point).normalized, other.GetContact(0).normal)  * Mathf.Deg2Rad));
            Debug.Log("Damage #4.2: " + damage4_2 + " (relative velocity * vector cos [equation 2]) : cos(" + Vector3.Angle((player.transform.position - other.GetContact(0).point).normalized, other.GetContact(0).normal) + ") = " + Mathf.Cos(Vector3.Angle((player.transform.position - other.GetContact(0).point).normalized, other.GetContact(0).normal)  * Mathf.Deg2Rad));
            Debug.Log("Distance: " + Vector3.Distance((player.transform.position - other.GetContact(0).point).normalized, other.GetContact(0).normal) + " [equation 2]");
            
            // DEBUG ---------------
            
            Debug.DrawLine(player.transform.position, other.GetContact(0).point, Color.magenta);
            Debug.DrawRay(player.transform.position, other.relativeVelocity.normalized, Color.cyan);
            Debug.DrawLine(player.transform.position, (player.transform.position - other.GetContact(0).normal).normalized, Color.yellow);

            Debug.Log("other.relativeVelocity: " + other.relativeVelocity.normalized);
            Debug.Log("player.transform.position - other.GetContact(0).point: " + (player.transform.position - other.GetContact(0).point).normalized);
            
            Time.timeScale = 0;
            
            // ------------------
            
            player.GetComponent<HealthSystem>().Damage(baseDamageAmount + (int) damage4 / 10);
            
            // Reduces vehicle speed according to damage dealt by the obstacle
            other.rigidbody.velocity *= 1 - (damage4 / 100);
            
            hit = true;
        }
    }

    private void OnCollisionStay(Collision other) {
        if (other.gameObject.CompareTag("Player")) {
            collisionTime += Time.deltaTime;
        }
    }

    public void Resume() {
        Time.timeScale = 1f;
    }
}
