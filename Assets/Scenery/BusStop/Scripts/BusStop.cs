using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BusStop : MonoBehaviour
{
    [SerializeField]
    private int healAmount;
    
    void Start() {
        
    }
    
    void Update() {
        
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            other.gameObject.GetComponent<HealthSystem>().Heal(healAmount);
        }
    }
}
