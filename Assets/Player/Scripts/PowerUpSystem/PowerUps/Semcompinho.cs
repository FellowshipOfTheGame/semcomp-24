using System;
using UnityEngine;

public class Semcompinho : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        other.gameObject.GetComponent<SemcompinhoCounter>().Increment();
        Destroy(gameObject);
    }
}
