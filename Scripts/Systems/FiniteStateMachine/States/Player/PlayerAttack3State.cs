using Godot;
using System;

public partial class PlayerAttack3State : State
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
		player.AnimationPlayer.Play("Attack 3");
		player.IsComboRequested = false; // Reset combo request when entering Attack 3 state
		player.AnimationPlayer.AnimationFinished += OnAnimationFinished;
	}
	public override void OnExit()
	{
		player.AnimationPlayer.AnimationFinished -= OnAnimationFinished;
	}
	public override void OnPhysicsUpdate(double delta)
	{
		if (stateMachine.StateTime < 0.2)
		{
			player.Move(delta, 1f);
		}
		else
		{
			player.Stand(delta);
		}
		//player.Stand(delta);
		//player.Move(delta, 0.3f);
		player.GravityControl(delta, 1);
	}
	public override void OnProcessUpdate(double delta)
	{
		if (player.PendingDamage.Count > 0)
		{
			stateMachine.SwitchState<PlayerHurtState>();
			return;
		}
	}
	private void OnAnimationFinished(StringName animName)
	{
		if (animName == "Attack 3")
		{
			stateMachine.SwitchState<PlayerIdleState>();
			return;
		}
	}
}
