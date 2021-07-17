using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	
	[SerializeField]
	private Slider slider;

	[SerializeField]
	private HealthSystem healthSystem;

    public void SetHealthBarMax(int healthBarMax) {
		slider.maxValue = healthBarMax;
    }

	public void SetHealthBarValue(int healthBarValue) {
		slider.value = Mathf.Clamp(healthBarValue, slider.minValue, slider.maxValue);
	}

	void OnEnable() {
		healthSystem.OnHealthChanged += UpdateHealthBar;
	}
	
	void OnDisable() {
		healthSystem.OnHealthChanged -= UpdateHealthBar;
	}

	void Start() {
		SetHealthBarMax(healthSystem.GetHealthMax());
		SetHealthBarValue(healthSystem.GetHealth());
	}
	
	// Update only when health value changes (damage or heal)
	void UpdateHealthBar(object sender, System.EventArgs e) {
		SetHealthBarValue(healthSystem.GetHealth());
		Debug.Log("Health: " + healthSystem.GetHealth());
	}
}