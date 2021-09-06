using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class PowerUp : MonoBehaviour
{
    public Sprite icon;
    
    public abstract void OnActivate(VehicleController controller, VehicleRenderer renderer);

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
