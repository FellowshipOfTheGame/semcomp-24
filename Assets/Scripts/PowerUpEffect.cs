using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpEffect : MonoBehaviour
{
    [SerializeField] private float floatingMagnitude;
    [SerializeField] private float floatingSpeed;
    [SerializeField] private float rotationSpeed;

    private Vector3 starterPosition;
    private int direction = 1;
    private float maxHeight;
    private float minHeight;

    void Start()
    {
        starterPosition = transform.position;
    }

    void Update()
    {
        Vector3 newPosition = transform.position;
        newPosition.y = Mathf.MoveTowards(newPosition.y, starterPosition.y + (direction * floatingMagnitude), Time.deltaTime * floatingSpeed);
        if (Mathf.Approximately(newPosition.y, starterPosition.y + (direction * floatingMagnitude)))
            direction *= -1;
        transform.position = newPosition;
        transform.Rotate(Vector3.up, Time.deltaTime * rotationSpeed);
    }
}
