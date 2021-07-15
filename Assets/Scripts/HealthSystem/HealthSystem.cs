using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
	[SerializeField]
	private int health;
	
	[SerializeField]
	private int healthMax;

	public event System.EventHandler OnHealthChanged;

	public HealthSystem(int healthMax) {
		this.healthMax = healthMax;
		health = healthMax;
	}

	public void SetHealth(int health) {
		this.health = Mathf.Clamp(health, 0, healthMax);
	}
	
	public void SetHealthMax(int healthMax) {
		this.healthMax = healthMax;
	}

	public void ResetHealth() {
		SetHealth(healthMax);
	}

	public int GetHealth() {
		return health;
	}

	public int GetHealthMax() {
		return healthMax;
	}

	public bool IsHealthFull() {
		return health == healthMax;
	}

	public bool IsDead() {
		return health == 0;
	}

	public void Damage(int damageAmount) {
		SetHealth(health - damageAmount);
		if (OnHealthChanged != null)
			OnHealthChanged(this, System.EventArgs.Empty);
		// Debug.Log("Damage: " + damageAmount);
	}

	public void Heal(int healAmount) {
		SetHealth(health + healAmount);
		if (OnHealthChanged != null)
			OnHealthChanged(this, System.EventArgs.Empty);
		// Debug.Log("Heal: " + healAmount);
	}

	void Awake() {
		ResetHealth();
	}
}
