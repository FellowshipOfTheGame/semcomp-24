using System;
using UnityEngine;

public class OnHealthChangeEventArgs : System.EventArgs
{
	private bool _healed;
	private int _healAmount;
	private int _damageAmount;

	public bool Healed
	{
		get => _healed;
		set => _healed = true;
	}
	
	public bool Damaged
	{
		get => (_healed == false);
		set =>  _healed = false;
	}

	public int HealAmount
	{
		get => _healAmount;
		set => _healAmount = value >= 0 ? value : -value;
	}

	public int DamageAmount
	{
		get => _damageAmount;
		set => _damageAmount = value <= 0 ? value : -value;
	}

	public int Amount
	{
		get => _healAmount > 0 ? _healAmount : _damageAmount;
	}
}

public class HealthSystem : MonoBehaviour
{
	[SerializeField]
	private int healthMax;
	
	private int health;
	private bool invulnerable;
	
	private bool hasShield;
	private int shieldLeft;
	private float shieldTimer;

	public event System.EventHandler<OnHealthChangeEventArgs> OnHealthChange;
	public event System.EventHandler OnDie;

	public void SetHealth(int health)
	{
		this.health = Mathf.Clamp(health, 0, healthMax);
	}
	
	public void SetHealthMax(int healthMax)
	{
		this.healthMax = healthMax;
		ResetHealth();
	}

	public void ResetHealth()
	{
		SetHealth(healthMax);
	}

	public int GetHealth()
	{
		return health;
	}

	public int GetHealthMax()
	{
		return healthMax;
	}

	public bool IsHealthFull()
	{
		return (health == healthMax);
	}

	public bool IsDead()
	{
		return (health <= 0);
	}
	
	public void SetInvulnerable(bool invulnerable)
	{
		this.invulnerable = invulnerable;
	}
	
	public bool IsInvulnerable()
	{
		return invulnerable;
	}

	public void ActivateShield(int protectionTimes, float duration)
	{
		this.hasShield = true;
		this.shieldLeft = protectionTimes;
		this.shieldTimer = duration;
	}

	public void EndShield()
	{
		this.hasShield = false;
#if UNITY_EDITOR
		Debug.Log("Shield ended");
#endif
	}

	public bool HasShield()
	{
		return this.hasShield;
	}

	public void DecreaseShield()
	{
		this.shieldLeft--;
		if (shieldLeft == 0)
			EndShield();
	}

	public void Damage(int damageAmount)
	{
		if (health <= 0)
		{
			return;
		}

		damageAmount = Mathf.Clamp(damageAmount, 0, health);
		SetHealth(health - damageAmount);

		OnHealthChangeEventArgs onHealthChangeEventArgs = new OnHealthChangeEventArgs
		{
			Damaged = (damageAmount > 0),
			DamageAmount = damageAmount
		};

		if (OnHealthChange != null)
			OnHealthChange(this, onHealthChangeEventArgs);

		if (OnDie != null && health <= 0)
		{
			OnDie(this, System.EventArgs.Empty);
		}
		
		// Debug.Log("Damage: " + damageAmount);
	}

	public void Heal(int healAmount)
	{
		if (health <= 0)
		{
			return;
		}
		
		healAmount = Mathf.Clamp(healAmount, 0, healthMax - health);
		SetHealth(health + healAmount);

		OnHealthChangeEventArgs onHealthChangeEventArgs = new OnHealthChangeEventArgs();
		onHealthChangeEventArgs.Healed = true;
		onHealthChangeEventArgs.HealAmount = healAmount;
		
		if (OnHealthChange != null)
			OnHealthChange(this, onHealthChangeEventArgs);
		// Debug.Log("Heal: " + healAmount);
	}

	void Awake() {
		ResetHealth();
	}

	private void Update()
	{
		if (hasShield)
		{
			if (shieldTimer <= 0f)
			{
#if UNITY_EDITOR
				Debug.Log("Shield time ended");
#endif
				hasShield = false;
			}
			else
			{
				shieldTimer -= Time.deltaTime;
			}
		}
	}
}