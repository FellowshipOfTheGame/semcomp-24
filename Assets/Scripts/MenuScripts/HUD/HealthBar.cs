using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	[SerializeField]
	private HealthSystem healthSystem;

	[SerializeField]
	private Slider damageTaken;
	
	[SerializeField]
	private GameObject indicatorsParent;
	
	private Slider healthBarSlider;
	private Image[] indicators;
	private float indicatorPercentage;

	void Awake()
	{
		healthSystem.OnHealthChange += UpdateHealthBar;
		healthSystem.OnSetHealthMax += Setup;
	}
	
	void OnDestroy()
	{
		healthSystem.OnHealthChange -= UpdateHealthBar;
		healthSystem.OnSetHealthMax -= Setup;
	}

	private void Setup(object sender, System.EventArgs e)
	{
		healthBarSlider = GetComponent<Slider>();
		healthBarSlider.maxValue = healthSystem.GetHealthMax();
		SetHealthBarValue(healthSystem.GetHealth());
		
		damageTaken.maxValue = healthBarSlider.maxValue;
		damageTaken.value = healthBarSlider.value;

		indicators = indicatorsParent.GetComponentsInChildren<Image>();
		indicatorPercentage = 100f * (1f / indicators.Length);
	}

	private void SetHealthBarValue(int healthBarValue)
	{
		healthBarSlider.value = Mathf.Clamp(healthBarValue, healthBarSlider.minValue, healthBarSlider.maxValue);
	}
	
	// Update only when health value changes (damage or heal)
	private void UpdateHealthBar(object sender, OnHealthChangeEventArgs eventArgs)
	{
		SetHealthBarValue(healthSystem.GetHealth());

		// Gets current and next health values (before and after the healing/damage)
		float currentHealthPercentage = 100f * ((float) (healthSystem.GetHealth() - eventArgs.Amount) / healthSystem.GetHealthMax());
		float nextHealthPercentage = 100f * ((float) healthSystem.GetHealth() / healthSystem.GetHealthMax());

		// Calculates current and next index based on indicators array lenght
		int currentIndex = Mathf.FloorToInt((100 - currentHealthPercentage) / indicatorPercentage); 
		int nextIndex = Mathf.FloorToInt((100 - nextHealthPercentage) / indicatorPercentage);
	
		// Avoid array index out of bounds exception. As soon as the health value gets to zero, the race should end anyway, so this is "just to be extra sure"
		if (currentHealthPercentage <= 0)
		{
			currentIndex = indicators.Length - 1;
		}

		if (nextHealthPercentage <= 0)
		{
			nextIndex = indicators.Length - 1;
		}
		
		/* DEBUG
		
		Debug.Log($"current health percentage: {currentHealthPercentage} = 100 * ({healthSystem.GetHealth() - eventArgs.Amount} / {healthSystem.GetHealthMax()})");
		Debug.Log($"next health percentage: {nextHealthPercentage} = 100 * ({healthSystem.GetHealth()} / {healthSystem.GetHealthMax()})");
		Debug.Log($"indicator percentage: {indicatorPercentage} = 100 * (1 / {indicators.Length})");
		Debug.Log($"current index: {currentIndex} = floor[(100 - {currentHealthPercentage}) / {indicatorPercentage}] ");
		Debug.Log($"next index: {nextIndex} = floor[(100 - {nextHealthPercentage}) / {indicatorPercentage}] ");
		
		*/

		// Enable or disable the indicator based on whether the player was healed or damaged
		
		if (nextIndex > currentIndex) // Damaged
		{
			for (int i = currentIndex; i <= nextIndex; i++)
			{
				indicators[i].gameObject.SetActive(false);
			}
		}
		else if (nextIndex < currentIndex) // Healed
		{
			for (int i = currentIndex; i >= nextIndex; i--)
			{
				indicators[i].gameObject.SetActive(true);
			}
		}
	}
	
	public void UpdateDamageTakenBar()
	{
		if (damageTaken.value > healthBarSlider.value)
		{
			StartCoroutine(UpdateDamageTakenBarEnumerator());
		}
	}

	private IEnumerator UpdateDamageTakenBarEnumerator()
	{
		while ((damageTaken.value - healthBarSlider.value) > 0.01f)
		{
			damageTaken.value = Mathf.Lerp(damageTaken.value, healthBarSlider.value, 1.5f * Time.deltaTime);
			yield return null;
		}
	}
}