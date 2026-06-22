using Godot;

public partial class PlayerRunningState : State
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
		player.AnimationPlayer.Play("Running");
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
		if (Input.IsActionJustPressed("Attack"))
		{
			stateMachine.SwitchState<PlayerAttack1State>();
			return;
		}
		if (player.IsReadyToSlide)
		{
			stateMachine.SwitchState<PlayerSlidingStartState>();
			return;
		}
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
			if (player.IsStill)
			{
				stateMachine.SwitchState<PlayerIdleState>();
				return;
			}
		}
		else
		{
			stateMachine.SwitchState<PlayerFallState>();
			player.CoyoteTimer.Start();
			return;
		}
	}
}

