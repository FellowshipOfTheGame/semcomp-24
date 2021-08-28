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
	
	private Slider slider;
	
	private Image[] indicators;
	private float indicatorPercentage;

	private bool takenDamage;

	void OnEnable()
	{
		healthSystem.OnHealthChange += UpdateHealthBar;
	}
	
	void OnDisable()
	{
		healthSystem.OnHealthChange -= UpdateHealthBar;
	}

	void Start()
	{
		slider = GetComponent<Slider>();
		
		SetHealthBarMax(healthSystem.GetHealthMax());
		SetHealthBarValue(healthSystem.GetHealth());
		
		damageTaken.maxValue = slider.maxValue;
		damageTaken.value = slider.value;

		indicators = indicatorsParent.GetComponentsInChildren<Image>();
		indicatorPercentage = 100f * (1f / indicators.Length);
	}

	void Update()
	{
		if (damageTaken.value > slider.value)
			damageTaken.value = Mathf.Lerp(damageTaken.value, slider.value, 1.5f * Time.deltaTime);
	}

	private void SetHealthBarMax(int healthBarMax)
	{
		slider.maxValue = healthBarMax;
	}

	private void SetHealthBarValue(int healthBarValue)
	{
		slider.value = Mathf.Clamp(healthBarValue, slider.minValue, slider.maxValue);
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

		// Debug.Log("Health: " + healthSystem.GetHealth());
	}
}