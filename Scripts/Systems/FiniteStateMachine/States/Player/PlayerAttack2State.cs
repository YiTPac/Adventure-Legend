using Godot;
using System;

public partial class PlayerAttack2State : State
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
		player.AnimationPlayer.Play("Attack 2");
		SoundManager.Instance.PlaySfx("Attack2");
		player.IsComboRequested = false; // Reset combo request when entering Attack 2 state
		player.AnimationPlayer.AnimationFinished += OnAnimationFinished;
	}
	public override void OnExit()
	{
		player.AnimationPlayer.AnimationFinished -= OnAnimationFinished;
	}
	public override void OnPhysicsUpdate(double delta)
	{
		if (stateMachine.StateTime < 0.1)
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
		if (animName == "Attack 2")
		{
			if (player.IsComboRequested)
			{
				stateMachine.SwitchState<PlayerAttack3State>();
				return;
			}
			else
			{
				stateMachine.SwitchState<PlayerIdleState>();
				return;
			}
		}
	}
}
