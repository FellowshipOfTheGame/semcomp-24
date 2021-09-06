using UnityEngine;

public class VehicleRenderer : MonoBehaviour
{
    [SerializeField] private GameObject boostParticle;
    
    public void ActivateBoostEffect()
    {
        GameObject boostEffect = Instantiate(boostParticle, transform);
        Destroy(boostEffect, 2f);
    }
    
}
