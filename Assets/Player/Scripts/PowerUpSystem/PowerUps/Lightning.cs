using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : PowerUp
{
    public Vector3 size;
    public LayerMask collisionLayer;
    
    public override void OnActivate(VehicleController controller, VehicleRenderer renderer)
    {
        RaycastHit[] results = new RaycastHit[32];
        int num = Physics.BoxCastNonAlloc(controller.transform.position, size, controller.transform.forward, results, controller.transform.rotation, size.z, collisionLayer);
        Debug.Log("Raio acertou " + num);
        for (int i = 0; i < num; i++)
        {
            results[i].transform.gameObject.GetComponent<Obstacle>().FadeOut();
        }
    }
}
