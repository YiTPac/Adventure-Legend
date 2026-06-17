using Godot;
using System;

public partial class PlayerSlidingLoopState : State
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
		player.AnimationPlayer.Play("SlidingLoop");
	}
	public override void OnExit()
	{

	}
	public override void OnPhysicsUpdate(double delta)
	{
		player.Slide(delta);
		player.GravityControl(delta, 1);
	}
	public override void OnProcessUpdate(double delta)
	{
		if (stateMachine.StateTime > player.SlidingDuration || player.IsOnWall())
		{
			stateMachine.SwitchState<PlayerSlidingEndState>();
			return;
		}
	}
}
