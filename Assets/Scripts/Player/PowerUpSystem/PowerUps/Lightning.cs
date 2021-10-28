using System.Collections.Generic;
using UnityEngine;
//TODO: call OnActivate from base

public class Lightning : PowerUp
{
    public Vector3 size;
    public int maxTargets;
    public LayerMask collisionLayer;
    
    public override void OnActivate(VehicleController controller, VehicleRenderer renderer)
    {
        base.OnActivate(controller, renderer);

        RaycastHit[] results = new RaycastHit[32];
        int num = Physics.BoxCastNonAlloc(controller.transform.position, size, controller.transform.forward, results, controller.transform.rotation, size.z, collisionLayer);
        Debug.Log("Raio acertou " + num);
        
        List<RaycastHit> hitList = new List<RaycastHit>();
        
        for (int i = 0; i < num; i++)
        {
            if (Vector3.Distance(controller.transform.position, results[i].transform.position) >= 0f)
            {
                hitList.Add(results[i]);
            }
        }
        
        // Debug.Log("--- Unsorted ---");
        // foreach (RaycastHit hit in hitList)
        // {
        //     Debug.Log($"Hit {hit.transform.name} ({hit.transform.position}). Raycast distance: {hit.distance}. Vector3Distance: {Vector3.Distance(controller.transform.position, hit.transform.position)}");
        // }
        
        hitList.Sort((a, b) => (controller.transform.position - a.transform.position).sqrMagnitude.CompareTo((controller.transform.position - b.transform.position).sqrMagnitude));
        
        // Debug.Log("--- Sorted ---");
        //
        // foreach (RaycastHit hit in hitList)
        // {
        //     Debug.Log($"Hit {hit.transform.name} ({hit.transform.position}). Raycast distance: {hit.distance}. Vector3Distance: {Vector3.Distance(controller.transform.position, hit.transform.position)}");
        // }
        
        // Debug.Log("--- Destroy ---");
        
        for (int i = 0; i < hitList.Count && i < maxTargets; i++)
        {
            if (hitList[i].transform.gameObject.TryGetComponent(out Obstacle obstacle))
            {
                obstacle.FadeOut(controller.VehicleCollider, renderer.ShieldCollider);
                Debug.Log($"Destroying {hitList[i].transform.name}");
            }
            else
            {
                Debug.Log($"Failed to destroy {hitList[i].transform.name}. Object doesn't have an obstacle component");
            }
        }
        
        renderer.ActivateLightning(size);
    }
}
