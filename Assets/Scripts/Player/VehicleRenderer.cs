using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class VehicleRenderer : MonoBehaviour
{
    // Shield Effect
    [SerializeField] private GameObject shieldEffect;
    [SerializeField] private ParticleSystem speedLines;
    [SerializeField] private GameObject lightning;
    [SerializeField] private Image HUDShieldImage;
    private HealthSystem healthSystem;
    private GameObject shieldEffectInstance;
    private Animator shieldAnimator;
    private Collider shieldCollider;
    private Animator lightningAnimator;
    private Coroutine shieldCoroutine;

    public Collider ShieldCollider => shieldCollider;

    private void Start()
    {
        shieldEffectInstance = Instantiate(shieldEffect, transform);
        shieldEffectInstance.SetActive(false);
        shieldCollider = shieldEffectInstance.GetComponent<Collider>();
        shieldAnimator = shieldEffectInstance.GetComponent<Animator>();
        lightningAnimator = lightning.GetComponent<Animator>();
        healthSystem = GetComponent<HealthSystem>();
    }

    private void Update()
    {
        if (healthSystem.HasShield())
        {
            HUDShieldImage.enabled = true;
            HUDShieldImage.material.SetFloat("Fill", healthSystem.shieldPercentage);
        }
        else
        {
            HUDShieldImage.enabled = false;
        }
    }

    public void ActivateShieldeEffect(float duration)
    {
        shieldCoroutine = StartCoroutine(Shield(duration));
    }
    
    public void DeactivateShieldeEffect()
    {
        StopCoroutine(shieldCoroutine);
        shieldCollider.enabled = false;
        shieldAnimator.Play("ShieldDeactivate");
    }

    public void ActivateLightning(Vector3 size)
    {
        lightning.transform.localScale = new Vector3(lightning.transform.localScale.x, lightning.transform.localScale.y, size.z);
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
        shieldCollider.enabled = true;
        shieldAnimator.Play("ShieldActivate");
        while (healthSystem.HasShield())
        {
            yield return null;
        }
        shieldCollider.enabled = false;
        shieldAnimator.Play("ShieldDeactivate");
        yield return new WaitForSeconds(0.1f);
        shieldEffectInstance.SetActive(false);
    }
    
}
