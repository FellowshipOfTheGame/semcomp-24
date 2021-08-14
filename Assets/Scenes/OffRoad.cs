using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffRoad : MonoBehaviour
{
    [SerializeField] [Range(0f, 1f)]
    private float decreaseFactor;
    
    private float groundDragDefault;

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Player"))
        {
            if (groundDragDefault == 0)
            {
                groundDragDefault = other.gameObject.GetComponent<VehicleController>().groundDrag;
            }

            other.gameObject.GetComponent<VehicleController>().groundDrag = groundDragDefault * (1 + decreaseFactor);
        }
    }

    void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<VehicleController>().ResetGroundDrag();
        }
    }
}
