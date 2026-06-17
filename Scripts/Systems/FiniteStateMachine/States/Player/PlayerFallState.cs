using Godot;
using System;

public partial class PlayerFallState : State
{
	private Player player;
	private float maxFallSpeed;
	public override void StateReady()
	{
		if (owner is not Player)
		{
			GD.PrintErr("require Player as owner");
			return;
		}
		player = (Player)owner;
	}
	public override void OnEnter()
	{
		maxFallSpeed = 0f;
		player.AnimationPlayer.Play("Fall");
	}
	public override void OnExit()
	{

	}
	public override void OnPhysicsUpdate(double delta)
	{
		player.Move(delta, 1);
		player.GravityControl(delta, 1);
		if (player.Velocity.Y > maxFallSpeed)
		{
			maxFallSpeed = player.Velocity.Y;
		}
	}
	public override void OnProcessUpdate(double delta)
	{
		if (player.PendingDamage.Count > 0)
		{
			stateMachine.SwitchState<PlayerHurtState>();
			return;
		}
		if (player.IsReadyToJump)
		{
			stateMachine.SwitchState<PlayerJumpState>();
			return;
		}
		if (player.IsOnFloor())
		{
			if (maxFallSpeed > 500)
			{
				player.PendingDamage.Enqueue(new Damage
				{
					Amount = (int)((maxFallSpeed - 500) / 5),
					Source = null
				});
				stateMachine.SwitchState<PlayerLandingState>();
				return;
			}
			else
			{
				stateMachine.SwitchState<PlayerRunningState>();
				return;
			}
		}
		if (player.IsOnWall() && player.CanWallSlide)
		{
			stateMachine.SwitchState<PlayerWallSlidingState>();
			return;
		}
	}
}

