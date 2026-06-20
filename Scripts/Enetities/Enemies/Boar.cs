using System.Collections.Generic;
using Godot;

public partial class Boar : Enemy
{
	[Export] public RayCast2D PlayerChecker { get; private set; }
	[Export] public RayCast2D FloorChecker { get; private set; }
	[Export] public RayCast2D WallChecker { get; private set; }
	[Export] public Timer CalmDownTimer { get; private set; }

	public bool CanSeePlayer
	{
		get
		{
			if (!PlayerChecker.IsColliding())
			{
				return false;
			}
			return PlayerChecker.GetCollider() is Player;
		}
	}

	public override void _Ready()
	{
		GetNode<Hurtbox>("Graphics/Hurtbox").Hurt += OnHurt;
	}

	public override void _Process(double delta)
	{
	}

	public void OnHurt(Area2D area, int damageAmount)
	{
		if (area is Hitbox)
		{
			var damage = new Damage
			{
				Amount = damageAmount,
				Source = area
			};
			PendingDamage.Enqueue(damage);
		}
	}

	public void Die()
	{
		QueueFree();
	}
}
