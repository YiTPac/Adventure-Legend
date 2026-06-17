using Godot;
using System;

public partial class BoarWalkState : State
{
	private Boar boar;
	public override void StateReady()
	{
		if (owner is not Boar)
		{
			GD.PrintErr("require Boar as owner");
			return;
		}
		boar = (Boar)owner;
	}
	public override void OnEnter()
	{
		boar.AnimationPlayer.Play("Walk");
		//boar.WallChecker.ForceRaycastUpdate();
		if (!boar.FloorChecker.IsColliding())
		{
			boar.direction = boar.direction ==
			Enemy.Direction.Left ? Enemy.Direction.Right : Enemy.Direction.Left;

		}
	}
	public override void OnExit()
	{

	}
	public override void OnPhysicsUpdate(double delta)
	{
		boar.GravityControl(delta, 1);
		boar.Move(delta, 0.33f);
	}
	public override void OnProcessUpdate(double delta)
	{
		if (boar.PendingDamage.Count > 0)
		{
			stateMachine.SwitchState<BoarHurtState>();
			return;
		}
		if (boar.CanSeePlayer)
		{
			stateMachine.SwitchState<BoarRunState>();
			return;
		}
		if (boar.WallChecker.IsColliding() || !boar.FloorChecker.IsColliding())
		{
			stateMachine.SwitchState<BoarIdleState>();
			return;
		}
	}
}
