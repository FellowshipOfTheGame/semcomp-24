using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class PowerUp : MonoBehaviour
{
    public Sprite icon;
    public FMODUnity.StudioEventEmitter useEventEmitter;
    public FMODUnity.StudioEventEmitter pickupEventEmitter;
    
    public virtual void OnActivate(VehicleController controller, VehicleRenderer renderer)
    {
        useEventEmitter.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
#if UNITY_EDITOR
        Debug.Log($"Collected {this}");
#endif
        pickupEventEmitter.Play();
        ItemSystem itemSystem = other.gameObject.GetComponent<ItemSystem>();
        itemSystem.ReplaceIfNull(this);
        Destroy(gameObject);
    }
}
