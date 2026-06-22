using Godot;

public partial class BoarRunState : State
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
		boar.AnimationPlayer.Play("Run");
		boar.CalmDownTimer.Start();
	}
	public override void OnExit()
	{

	}
	public override void OnPhysicsUpdate(double delta)
	{
		boar.Move(delta, 1);
		boar.GravityControl(delta, 1);
		if (boar.WallChecker.IsColliding() || !boar.FloorChecker.IsColliding())
		{
			boar.FacingDirection = boar.FacingDirection ==
			Enemy.Direction.Left ? Enemy.Direction.Right : Enemy.Direction.Left;
		}
		if (boar.CanSeePlayer)
		{
			boar.CalmDownTimer.Start();
		}
	}
	public override void OnProcessUpdate(double delta)
	{
		if (boar.PendingDamage.Count > 0)
		{
			stateMachine.SwitchState<BoarHurtState>();
			return;
		}
		if (boar.CalmDownTimer.IsStopped())
		{
			stateMachine.SwitchState<BoarWalkState>();
			return;
		}
	}
}
