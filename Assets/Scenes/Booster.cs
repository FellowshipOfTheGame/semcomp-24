using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booster : MonoBehaviour
{
    [SerializeField] [Range(0f, 1f)]
    private float boost;
    
    [SerializeField]
    private float effectTime = 3f;

    private bool active;
    private float timer;

    private GameObject player;
    private float groundDragDefault;
    
    void Update()
    {
        if (active)
        {
            timer += Time.deltaTime;

            if (timer >= effectTime)
            {
                player.GetComponent<VehicleController>().ResetGroundDrag();
                active = false;
                Debug.Log("Booster time is up! (" + timer + " s)");
            }
        }
    }
   
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) {
            player = other.gameObject;

            if (!active)
            {
                groundDragDefault = player.GetComponent<VehicleController>().groundDrag;
                active = true;
                Debug.Log("Boost!");
            }
            
            player.GetComponent<VehicleController>().groundDrag = groundDragDefault * (1 - boost);
        }
    }
}
