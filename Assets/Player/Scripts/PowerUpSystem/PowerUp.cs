using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class PowerUp : MonoBehaviour
{
    protected abstract void OnActivate(VehicleController controller, VehicleRenderer renderer);

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
#if UNITY_EDITOR
        Debug.Log($"Collected {this}");
#endif
            
        VehicleController otherController = other.gameObject.GetComponent<VehicleController>();
        VehicleRenderer otherRenderer = other.gameObject.GetComponent<VehicleRenderer>();
            
        OnActivate(otherController, otherRenderer);
    }
}
