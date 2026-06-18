using Godot;
using System;

public partial class PlayerWallJumpState : State
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
		player.WallJump();
		//player.Graphics.Scale = new Vector2(player.GetWallNormal().X, player.Graphics.Scale.Y);
		player.FacingDirection = Player.ToDrection(player.GetWallNormal().X);
	}
	public override void OnExit()
	{

	}
	public override void OnPhysicsUpdate(double delta)
	{
		//GD.Print($"[WallJumpState] StateTime={stateMachine.StateTime}, before Gravity Velocity={player.Velocity}");
		if (stateMachine.StateTime < 0.2)
		{
			player.GravityControl(delta, 0);
		}
		else
		{
			//player.Move(delta);
			player.GravityControl(delta, 1);
		}
		//GD.Print($"[WallJumpState] after Gravity Velocity={player.Velocity}");
		player.MoveAndSlide();
		//GD.Print($"[WallJumpState] after MoveAndSlide Velocity={player.Velocity}");
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
		if (player.IsOnWall() && player.CanWallSlide && stateMachine.StateTime > 0.1)
		{
			stateMachine.SwitchState<PlayerWallSlidingState>();
			return;
		}
	}
}

