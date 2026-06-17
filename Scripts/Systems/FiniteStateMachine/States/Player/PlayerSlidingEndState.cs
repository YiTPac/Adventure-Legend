using Godot;
using System;

public partial class PlayerSlidingEndState : State
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
		player.AnimationPlayer.Play("SlidingEnd");
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

	}

	private void OnAnimationFinished(StringName animName)
	{
		if (animName == "SlidingEnd")
		{
			stateMachine.SwitchState<PlayerIdleState>();
			return;
		}
	}
}
