using Godot;
using System;

public partial class PlayerAttack1State : State
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
		player.AnimationPlayer.Play("Attack 1");
		player.AttackPlayer.Play();
		player.AnimationPlayer.AnimationFinished += OnAnimationFinished;
	}
	public override void OnExit()
	{
		player.AnimationPlayer.AnimationFinished -= OnAnimationFinished;
	}
	public override void OnPhysicsUpdate(double delta)
	{
		player.Stand(delta);
		//player.Move(delta, 0.3f);
		player.GravityControl(delta, 1);
	}
	public override void OnProcessUpdate(double delta)
	{

	}
	private void OnAnimationFinished(StringName animName)
	{
		if (animName == "Attack 1")
		{
			if (player.IsComboRequested)
			{
				stateMachine.SwitchState<PlayerAttack2State>();
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
