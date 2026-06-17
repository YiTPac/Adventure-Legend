using Godot;
using System.Collections.Generic;


public partial class Enemy : CharacterBody2D
{
	public enum Direction
	{
		Right = 1,
		Left = -1
	}
	[Export] public Node2D Graphics { get; private set; }
	[Export] public AnimationPlayer AnimationPlayer { get; private set; }
	[Export]
	public Direction direction
	{
		get => _direction;
		set
		{
			_direction = value;
			if (Graphics != null)
			{
				Graphics.Scale = new Vector2(-(float)value, Graphics.Scale.Y);
			}
		}
	}
	[Export] private float maxSpeed = 180;
	[Export] private float acceleration = 2000;
	public Queue<Damage> PendingDamage { get; private set; } = new Queue<Damage>();
	private Vector2 gravity;
	private Direction _direction;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public void GravityControl(double delta, float gravityScale)
	{
		gravity = GetGravity();
		Velocity += gravity * (float)delta * gravityScale;
	}
	public void Move(double delta, float speedScale)
	{
		Velocity = new
		Vector2(Mathf.MoveToward(Velocity.X, (float)direction * maxSpeed * speedScale,
		acceleration * (float)delta), Velocity.Y);
		MoveAndSlide();
	}
}
