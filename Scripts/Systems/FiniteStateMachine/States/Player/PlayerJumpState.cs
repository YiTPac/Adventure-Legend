using Godot;
using System;

public partial class PlayerJumpState : State
{
	private Player player;
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
		player.AnimationPlayer.Play("Jump");
		SoundManager.Instance.PlaySfx("Jump");
		player.Jump();
	}
	public override void OnExit()
	{

	}
	public override void OnPhysicsUpdate(double delta)
	{
		player.Move(delta, 1);
		player.GravityControl(delta, 1);
	}
	public override void OnProcessUpdate(double delta)
	{
		if (player.PendingDamage.Count > 0)
		{
			stateMachine.SwitchState<PlayerHurtState>();
			return;
		}
		if (player.Velocity.Y >= 0)
		{
			stateMachine.SwitchState<PlayerFallState>();
			return;
		}
		if (player.IsOnWall() && player.CanWallSlide)
		{
			stateMachine.SwitchState<PlayerWallSlidingState>();
			return;
		}
	}
}

