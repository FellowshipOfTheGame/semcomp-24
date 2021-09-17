using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class PowerUp : MonoBehaviour
{
    public Sprite icon;
    public FMODUnity.StudioEventEmitter eventEmitter;
    
    public virtual void OnActivate(VehicleController controller, VehicleRenderer renderer)
    {
        eventEmitter.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
#if UNITY_EDITOR
        Debug.Log($"Collected {this}");
#endif
        ItemSystem itemSystem = other.gameObject.GetComponent<ItemSystem>();
        itemSystem.ReplaceIfNull(this);
        Destroy(gameObject);
    }
}
