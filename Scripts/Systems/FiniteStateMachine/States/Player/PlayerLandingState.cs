using Godot;

public partial class PlayerLandingState : State
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
		player.AnimationPlayer.Play("Landing");
		player.InvincibleTimer.Start();
		player.AnimationPlayer.AnimationFinished += OnAnimationFinished;
	}

	public override void OnExit()
	{
		player.AnimationPlayer.AnimationFinished -= OnAnimationFinished;
	}
	public override void OnPhysicsUpdate(double delta)
	{
		player.Stand(delta);
		player.GravityControl(delta, 1);
	}
	public override void OnProcessUpdate(double delta)
	{
		while (player.PendingDamage.TryDequeue(out var damage))
		{
			player.Stats.CurrentHealth -= damage.Amount;
		}
		if (player.Stats.CurrentHealth <= 0)
		{
			stateMachine.SwitchState<PlayerDyingState>();
			return;
		}
	}

	private void OnAnimationFinished(StringName animName)
	{
		if (animName == "Landing")
		{
			if (player.IsStill)
			{
				stateMachine.SwitchState<PlayerIdleState>();
				return;
			}
			else
			{
				stateMachine.SwitchState<PlayerRunningState>();
				return;
			}
		}
	}
}

