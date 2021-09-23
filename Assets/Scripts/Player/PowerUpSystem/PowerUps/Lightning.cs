using System;
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
        for (int i = 0; i < num && i < maxTargets; i++)
        {
            hitList.Add(results[i]);
        }
        hitList.Sort(((a, b) => a.distance.CompareTo(b.distance)));
        for (int i = 0; i < hitList.Count; i++)
        {
            hitList[i].transform.gameObject.GetComponent<Obstacle>().FadeOut(controller.VehicleCollider, renderer.ShieldCollider);
        }
        
        renderer.ActivateLightning(size);
    }
}
