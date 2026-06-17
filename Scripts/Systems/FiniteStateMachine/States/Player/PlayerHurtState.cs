using Godot;
using System;

public partial class PlayerHurtState : State
{
	Player player;
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
		player.AnimationPlayer.Play("Hurt");
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
			if (damage.Source == null)
			{
				continue;
			}
			var direaction = damage.Source.GlobalPosition.DirectionTo(player.GlobalPosition);
			player.Velocity = direaction * 256;
		}
		if (player.Stats.CurrentHealth <= 0)
		{
			stateMachine.SwitchState<PlayerDyingState>();
			return;
		}
	}
	private void OnAnimationFinished(StringName animName)
	{
		if (animName == "Hurt")
		{
			if (player.Stats.CurrentHealth > 0)
			{
				stateMachine.SwitchState<PlayerIdleState>();
				return;
			}

		}
	}
}
