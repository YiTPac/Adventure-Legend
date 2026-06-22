using Godot;

public partial class BoarIdleState : State
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
		boar.FacingDirection = Enemy.Direction.Left;
	}
	public override void OnEnter()
	{
		boar.AnimationPlayer.Play("Idle");
		//boar.FloorChecker.ForceRaycastUpdate();
		if (boar.WallChecker.IsColliding())
		{
			boar.FacingDirection = boar.FacingDirection ==
			Enemy.Direction.Left ? Enemy.Direction.Right : Enemy.Direction.Left;
		}
	}
	public override void OnExit()
	{

	}
	public override void OnPhysicsUpdate(double delta)
	{
		boar.GravityControl(delta, 1);
		boar.Move(delta, 0);
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
		if (stateMachine.StateTime > 2)
		{
			stateMachine.SwitchState<BoarWalkState>();
			return;
		}
	}
}