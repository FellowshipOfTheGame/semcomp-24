using System;
using UnityEngine;

public class Semcompinho : MonoBehaviour
{
    [SerializeField] private FMODUnity.StudioEventEmitter eventEmitter;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;

        eventEmitter.Play();
        other.gameObject.GetComponent<SemcompinhoCounter>().Increment();
        Destroy(gameObject);
    }
}
