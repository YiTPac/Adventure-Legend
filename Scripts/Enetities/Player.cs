using Godot;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

public struct PlayerData
{
	public Player.Direction direction;
	public float PosX;
	public float PosY;
}

public partial class Player : CharacterBody2D
{
	public enum Direction
	{
		Left = -1,
		Right = 1
	}
	public PlayerData Data
	{
		get
		{
			return new PlayerData
			{
				direction = FacingDirection,
				PosX = GlobalPosition.X,
				PosY = GlobalPosition.Y
			};
		}
		set
		{
			FacingDirection = value.direction;
			GlobalPosition = new Vector2(value.PosX, value.PosY);
		}
	}
	private Direction _facingDirection;
	[Export]
	public Direction FacingDirection
	{
		set
		{
			_facingDirection = value;
			Graphics.Scale = new Vector2((float)_facingDirection, Graphics.Scale.Y);
		}
		get
		{
			return _facingDirection;
		}
	}
	[Export] private bool canCombo;
	[Export] private float jumpVelocity;
	[Export] private Vector2 wallJumpVelocity;
	[Export] private float runSpeed;
	[Export] private float slideSpeed;
	[Export] public float SlideEnergyCost { get; private set; }
	[Export] public float SlidingDuration { get; private set; }
	[Export] public AnimatedSprite2D interationIcon;
	[Export] public Node2D Graphics { get; private set; }
	[Export] public AnimationPlayer AnimationPlayer { get; private set; }
	[Export] public Timer CoyoteTimer { get; private set; }
	[Export] public Timer JumpRequestTimer { get; private set; }
	[Export] public Timer SlideRequestTimer { get; private set; }
	[Export] public Timer InvincibleTimer { get; private set; }
	[Export] public RayCast2D HandChecker { get; private set; }
	[Export] public RayCast2D FootChecker { get; private set; }
	[Export] public Stats Stats { get; private set; }
	public List<Interactable> CurrentInteractable { get; set; } = new List<Interactable>();
	public bool IsComboRequested { get; set; }
	public Queue<Damage> PendingDamage { get; set; } = new Queue<Damage>();
	private float floorAcceleration;
	private float airAcceleration;
	private Vector2 gravity;
	private static float InputDirection
	{
		get
		{
			return Input.GetAxis("MoveLeft", "MoveRight");
		}
	}
	public float Acceleration
	{
		get
		{
			return IsOnFloor() ? floorAcceleration : airAcceleration;
		}
	}
	public bool IsStill
	{
		get
		{
			return Mathf.IsZeroApprox(InputDirection) && Mathf.IsZeroApprox(Velocity.X);
		}
	}
	public bool CanJump
	{
		get
		{
			return IsOnFloor() || CoyoteTimer.TimeLeft > 0;
		}
	}
	public bool IsReadyToJump
	{
		get
		{
			return CanJump && JumpRequestTimer.TimeLeft > 0;
		}
	}
	public bool IsReadyToSlide
	{
		get
		{
			return SlideRequestTimer.TimeLeft > 0 && Stats.CurrentEnergy >= SlideEnergyCost;
		}
	}
	public bool CanWallSlide
	{
		get
		{
			return HandChecker.IsColliding() &&
			FootChecker.IsColliding();
		}
	}
	public override void _Ready()
	{
		Stats = Game.Instance.playerStats;
		floorAcceleration = (float)(runSpeed / 0.1);
		airAcceleration = (float)(runSpeed / 0.02);
		GetNode<Hurtbox>("Graphics/Hurtbox").Hurt += OnHurt;
		FacingDirection = Direction.Right;
	}
	public override void _EnterTree()
	{
	}

	public override void _PhysicsProcess(double delta)
	{

	}

	public override void _Process(double delta)
	{
		SetInteractionIconVisibility();
		SetInvincibleFlash();
		//bool  = IsOnFloor() || CoyoteTimer.TimeLeft > 0;
		//IsReadyToJump = canJump && JumpRequestTimer.TimeLeft > 0;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event.IsActionPressed("Jump"))
		{
			JumpRequestTimer.Start();
		}
		if (@event.IsActionReleased("Jump"))
		{
			JumpRequestTimer.Stop();
			if (Velocity.Y < jumpVelocity * 0.5f)
			{
				Velocity = new Vector2(Velocity.X, jumpVelocity * 0.5f);
			}
		}
		if (@event.IsActionPressed("Attack"))
		{
			if (canCombo)
			{
				IsComboRequested = true;
			}
		}
		if (@event.IsActionPressed("Slide"))
		{
			SlideRequestTimer.Start();
			return;
		}
		if (@event.IsActionPressed("Interact") && CurrentInteractable.Count > 0)
		{
			CurrentInteractable.Last().Interact(this);
		}
		if (@event.IsActionPressed("Save"))
		{
			// var options = new JsonSerializerOptions { IncludeFields = true };
			// string jsonString = JsonSerializer.Serialize(Data, options);
			// GD.Print("JSON: " + jsonString);
			// string fullPath = ProjectSettings.GlobalizePath(Game.savePath);
			// GD.Print("Save path: " + fullPath);
			// using var file = FileAccess.Open(Game.savePath, FileAccess.ModeFlags.Write);
			// if (file != null)
			// {
			// 	file.StoreString(jsonString);
			// 	GD.Print("File saved successfully!");
			// }
			// else
			// {
			// 	GD.Print("Failed to open file for writing");
			// }
			Game.Instance.SaveGame();
		}
		if (@event.IsActionPressed("Load"))
		{
			// using var file = FileAccess.Open(Game.savePath, FileAccess.ModeFlags.Read);
			// if (file != null)
			// {
			// 	string jsonText = file.GetAsText();
			// 	var options = new JsonSerializerOptions { IncludeFields = true };
			// 	Data = JsonSerializer.Deserialize<PlayerData>(jsonText, options);
			// 	GD.Print("Yes!");
			// }
			Game.Instance.LoadGame();
		}
	}
	public void RegisterInteractable(Interactable interactable)
	{
		if (!CurrentInteractable.Contains(interactable))
		{
			CurrentInteractable.Add(interactable);
		}
	}
	public void UnregisterInteractable(Interactable interactable)
	{
		if (CurrentInteractable.Contains(interactable))
		{
			CurrentInteractable.Remove(interactable);
		}
	}
	public void SetInteractionIconVisibility()
	{
		interationIcon.Visible = CurrentInteractable.Count > 0;
	}
	public void Move(double delta, float speedScale)
	{
		Velocity = new
		Vector2(Mathf.MoveToward(Velocity.X, InputDirection * runSpeed * speedScale,
		Acceleration * (float)delta), Velocity.Y);
		MoveAndSlide();
		if (!Mathf.IsZeroApprox(InputDirection))
		{
			//Graphics.Scale = new Vector2(InputDirection < 0 ? -1 : 1, Graphics.Scale.Y);
			FacingDirection = ToDrection(InputDirection);
		}
	}
	public void Stand(double delta)
	{
		Velocity = new
		Vector2(Mathf.MoveToward(Velocity.X, 0,
		Acceleration * (float)delta), Velocity.Y);
		MoveAndSlide();
	}
	public void Slide(double delta)
	{
		Velocity = new
		Vector2(Graphics.Scale.X * slideSpeed, Velocity.Y);
		MoveAndSlide();
	}
	public void Jump()
	{
		Velocity = new Vector2(Velocity.X, jumpVelocity);
		CoyoteTimer.Stop();
		JumpRequestTimer.Stop();
	}

	public void WallJump()
	{
		Velocity = wallJumpVelocity;
		Velocity = new Vector2(Velocity.X * GetWallNormal().X, Velocity.Y);
		JumpRequestTimer.Stop();
	}

	public void GravityControl(double delta, float gravityScale)
	{
		gravity = GetGravity();
		Velocity += gravity * (float)delta * gravityScale;
	}

	public void SetInvincibleFlash()
	{
		if (InvincibleTimer.IsStopped())
		{
			Graphics.Modulate = new Color(1, 1, 1, 1);
		}
		else
		{
			Graphics.Modulate = new Color(1, 1, 1, Mathf.Sin(Time.GetTicksMsec() / 50) * 0.5f + 0.5f);
		}
	}
	public void OnHurt(Area2D area, int damageAmount)
	{
		if (area is Hitbox)
		{
			if (!InvincibleTimer.IsStopped()) return;
			var damage = new Damage
			{
				Amount = damageAmount,
				Source = area
			};
			PendingDamage.Enqueue(damage);
		}
	}

	public static Direction ToDrection(float value)
	{
		if (value < 0)
		{
			return Direction.Left;
		}
		else
		{
			return Direction.Right;
		}
	}
}
