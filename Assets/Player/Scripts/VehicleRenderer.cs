using System;
using System.Collections;
using UnityEngine;

public class VehicleRenderer : MonoBehaviour
{
    // Shield Effect
    [SerializeField] private GameObject shieldEffect;
    private GameObject shieldEffectInstance;
    private Coroutine shieldCoroutine;

    private void Start()
    {
        shieldEffectInstance = Instantiate(shieldEffect, transform);
        shieldEffectInstance.SetActive(false);
    }

    public void ActivateShieldeEffect(float duration)
    {
        shieldCoroutine = StartCoroutine(Shield(duration));
    }
    
    public void DeactivateShieldeEffect()
    {
        StopCoroutine(shieldCoroutine);
        shieldEffectInstance.SetActive(false);
    }

    private IEnumerator Shield(float duration)
    {
        shieldEffectInstance.SetActive(true);
        yield return new WaitForSeconds(duration);
        shieldEffectInstance.SetActive(false);
    }
    
}
