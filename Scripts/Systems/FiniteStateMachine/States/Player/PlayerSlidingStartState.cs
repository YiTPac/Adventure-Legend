using Godot;
using System;

public partial class PlayerSlidingStartState : State
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
		player.Stats.CurrentEnergy -= player.SlideEnergyCost;
		player.SlideRequestTimer.Stop();
		player.AnimationPlayer.Play("SlidingStart");
		player.AnimationPlayer.AnimationFinished += OnAnimationFinished;
	}
	public override void OnExit()
	{
		player.AnimationPlayer.AnimationFinished -= OnAnimationFinished;
	}

	public override void OnPhysicsUpdate(double delta)
	{

	}
	public override void OnProcessUpdate(double delta)
	{
		player.Slide(delta);
		player.GravityControl(delta, 1);
	}

	private void OnAnimationFinished(StringName animName)
	{
		if (animName == "SlidingStart")
		{
			stateMachine.SwitchState<PlayerSlidingLoopState>();
			return;
		}
	}
}
