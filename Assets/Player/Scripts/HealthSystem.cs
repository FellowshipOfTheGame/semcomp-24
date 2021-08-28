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
	private int health;
	
	[SerializeField]
	private int healthMax;

	public event System.EventHandler<OnHealthChangeEventArgs> OnHealthChange;
	public event System.EventHandler OnDie;

	public HealthSystem(int healthMax)
	{
		this.healthMax = healthMax;
		health = healthMax;
	}

	public void SetHealth(int health)
	{
		this.health = Mathf.Clamp(health, 0, healthMax);
	}
	
	public void SetHealthMax(int healthMax)
	{
		this.healthMax = healthMax;
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

	public void Damage(int damageAmount)
	{
		if (health <= 0)
		{
			return;
		}

		damageAmount = Mathf.Clamp(damageAmount, 0, health);
		SetHealth(health - damageAmount);

		OnHealthChangeEventArgs onHealthChangeEventArgs = new OnHealthChangeEventArgs();
		onHealthChangeEventArgs.Damaged = (damageAmount > 0);
		onHealthChangeEventArgs.DamageAmount = damageAmount;
		
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
}