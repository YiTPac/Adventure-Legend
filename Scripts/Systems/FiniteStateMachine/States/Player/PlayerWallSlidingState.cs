using Godot;

public partial class PlayerWallSlidingState : State
{
	private Player player;
	private float leaveWallTime = 0f;
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
		player.AnimationPlayer.Play("WallSliding");
		//player.Graphics.Scale = new Vector2(-player.GetWallNormal().X, player.Graphics.Scale.Y);
		if (player.Velocity.Y < 0)
		{
			player.Velocity = new Vector2(player.Velocity.X, player.Velocity.Y * 0.75f);
		}
		player.FacingDirection = Player.ToDrection(-player.GetWallNormal().X);
		leaveWallTime = 0f;
	}
	public override void OnExit()
	{

	}
	public override void OnPhysicsUpdate(double delta)
	{
		player.Stand(delta);
		player.GravityControl(delta, 0.3f);
	}
	public override void OnProcessUpdate(double delta)
	{
		if (player.PendingDamage.Count > 0)
		{
			stateMachine.SwitchState<PlayerHurtState>();
			return;
		}
		if (player.IsOnFloor())
		{
			if (player.IsStill)
			{
				stateMachine.SwitchState<PlayerLandingState>();
				return;
			}
			else
			{
				stateMachine.SwitchState<PlayerRunningState>();
				return;
			}
		}
		// if (!player.IsOnWall())
		// {
		// 	leaveWallTime += (float)delta;
		// 	if (leaveWallTime > 0.1f)
		// 	{
		// 		stateMachine.SwitchState<PlayerFallState>();
		// 		return;
		// 	}
		// }
		// else
		// {
		// 	leaveWallTime = 0f;
		// }
		if (!player.CanWallSlide && !player.IsOnWall())
		{
			stateMachine.SwitchState<PlayerFallState>();
			return;
		}
		if (player.JumpRequestTimer.TimeLeft > 0)
		{
			stateMachine.SwitchState<PlayerWallJumpState>();
			return;
		}
	}
}

