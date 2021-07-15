using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	
	[SerializeField]
	private Slider _slider;

	[SerializeField]
	private HealthSystem healthSystem;

    public void SetHealthBarMax(int healthBarMax) {
		_slider.maxValue = healthBarMax;
    }

	public void SetHealthBarValue(int healthBarValue) {
		_slider.value = Mathf.Clamp(healthBarValue, _slider.minValue, _slider.maxValue);
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
	}
}