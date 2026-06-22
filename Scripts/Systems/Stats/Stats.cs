using Godot;

public struct StatsData
{
	public int maxHealth;
	public float maxEnergy;
	public int currentHealth;
}

[GlobalClass]
public partial class Stats : Node
{
	public StatsData Data
	{
		get
		{
			return new StatsData
			{
				maxEnergy = MaxEnergy,
				maxHealth = MaxHealth,
				currentHealth = _currentHealth
			};
		}
		set
		{
			MaxHealth = value.maxHealth;
			MaxEnergy = value.maxEnergy;
			_currentHealth = value.currentHealth;
		}
	}
	[Signal] public delegate void HealthChangedEventHandler();
	[Signal] public delegate void EnergyChangedEventHandler();
	[Export] public int MaxHealth { get; private set; }
	[Export] public float MaxEnergy { get; private set; }
	private int _currentHealth;
	[Export]
	public int CurrentHealth
	{
		get
		{
			return _currentHealth;
		}
		set
		{
			value = Mathf.Clamp(value, 0, MaxHealth);
			if (value != _currentHealth)
			{
				_currentHealth = value;
				EmitSignal(SignalName.HealthChanged);
			}
		}
	}
	[Export] private float energyRegen;
	private float _currentEnergy;
	[Export]
	public float CurrentEnergy
	{
		get
		{
			return _currentEnergy;
		}
		set
		{
			value = Mathf.Clamp(value, 0, MaxEnergy);
			if (value != _currentEnergy)
			{
				_currentEnergy = value;
				EmitSignal(SignalName.EnergyChanged);
			}
		}
	}
	public override void _Ready()
	{
		_currentHealth = MaxHealth;
		_currentEnergy = MaxEnergy;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		CurrentEnergy += energyRegen * (float)delta;
	}

}
