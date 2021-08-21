using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleInRoad : MonoBehaviour
{
    [SerializeField]
    private int SlowAmount;

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Debug.Log("Hole Collide");

            other.gameObject.GetComponent<VehicleController>().SlowFowardForce(SlowAmount);
        }
    }
}
