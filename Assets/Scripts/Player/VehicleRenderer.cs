using System;
using System.Collections;
using UnityEngine;

public class VehicleRenderer : MonoBehaviour
{
    // Shield Effect
    [SerializeField] private GameObject shieldEffect;
    [SerializeField] private ParticleSystem speedLines;
    [SerializeField] private GameObject lightning;
    private GameObject shieldEffectInstance;
    private Animator shieldAnimator;
    private Animator lightningAnimator;
    private Coroutine shieldCoroutine;

    private void Start()
    {
        shieldEffectInstance = Instantiate(shieldEffect, transform);
        shieldEffectInstance.SetActive(false);
        shieldAnimator = shieldEffectInstance.GetComponent<Animator>();
        lightningAnimator = lightning.GetComponent<Animator>();
    }

    private void Update()
    {
    }

    public void ActivateShieldeEffect(float duration)
    {
        shieldCoroutine = StartCoroutine(Shield(duration));
    }
    
    public void DeactivateShieldeEffect()
    {
        StopCoroutine(shieldCoroutine);
        shieldAnimator.Play("ShieldDeactivate");
    }

    public void ActivateLightning()
    {
        lightningAnimator.Play("FadeInOut", 0, 0);
    }

    public void ActivateSpeedLines(float time)
    {
        speedLines.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        var main = speedLines.main;
        main.duration = time;
        speedLines.Play();
    }

    private IEnumerator Shield(float duration)
    {
        shieldEffectInstance.SetActive(true);
        shieldAnimator.Play("ShieldActivate");
        yield return new WaitForSeconds(duration);
        shieldAnimator.Play("ShieldDeactivate");
        yield return new WaitForSeconds(0.5f);
        shieldEffectInstance.SetActive(false);
    }
    
}