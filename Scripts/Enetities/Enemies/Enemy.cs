using Godot;
using System.Collections.Generic;

public struct EnemyData
{
	public Enemy.Direction direction;
	public float PosX;
	public float PosY;
	public StatsData enemyStatsData;
}
public partial class Enemy : CharacterBody2D, ISaveable<EnemyData>
{
	public enum Direction
	{
		Right = 1,
		Left = -1
	}
	[Signal] public delegate void EnemyDiedEventHandler();
	[Export] public Stats Stats { get; private set; }
	[Export] public Node2D Graphics { get; private set; }
	[Export] public AnimationPlayer AnimationPlayer { get; private set; }
	[Export]
	public Direction FacingDirection
	{
		get => direction;
		set
		{
			if (Mathf.IsZeroApprox((float)value))
			{
				return;
			}
			direction = value;
			if (Graphics != null && IsInstanceValid(this))
			{
				//GD.Print(Name + ":" + Mathf.Abs(Graphics.Scale.X).ToString() + "and" + (-(float)value).ToString());
				Graphics.Scale = new Vector2(-(float)value * Mathf.Abs(Graphics.Scale.X), Graphics.Scale.Y);
			}

		}
	}
	[Export] private float maxSpeed = 180;
	[Export] private float acceleration = 2000;
	public Queue<Damage> PendingDamage { get; private set; } = new Queue<Damage>();
	private Vector2 gravity;
	private Direction direction;
	// Called when the node enters the scene tree for the first time.
	public string SaveKey { get => GetPathTo(this); }

	// public override Dictionary<string, EnemyData> SaveState()
	// {
	// 	return new Dictionary<>
	// }

	public void GravityControl(double delta, float gravityScale)
	{
		gravity = GetGravity();
		Velocity += gravity * (float)delta * gravityScale;
	}
	public void Move(double delta, float speedScale)
	{
		Velocity = new
		Vector2(Mathf.MoveToward(Velocity.X, (float)FacingDirection * maxSpeed * speedScale,
		acceleration * (float)delta), Velocity.Y);
		MoveAndSlide();
	}

	public EnemyData OnSaveState()
	{
		return new EnemyData
		{
			direction = FacingDirection,
			PosX = GlobalPosition.X,
			PosY = GlobalPosition.Y,
			enemyStatsData = Stats.Data
		};
	}

	public void OnLoadState(EnemyData data)
	{
		FacingDirection = data.direction;
		GlobalPosition = new Vector2(data.PosX, data.PosY);
		Stats.Data = data.enemyStatsData;
	}

}
