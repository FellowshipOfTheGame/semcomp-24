using System;
using UnityEngine;

public class KillOnOutOfBounds : MonoBehaviour
{
    public float maxX = 20;
    public float maxY = -10;
    private HealthSystem healthSystem;

    private void Start()
    {
        healthSystem = GetComponent<HealthSystem>();
    }

    private void Update()
    {
        if (transform.position.x > maxX || transform.position.x < -maxX || transform.position.y < maxY)
        {
            healthSystem.Damage(1000000);
        }
    }
}
