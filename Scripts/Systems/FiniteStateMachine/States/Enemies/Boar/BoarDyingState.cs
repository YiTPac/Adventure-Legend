using Godot;

public partial class BoarDyingState : State
{
	Boar boar;
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
		boar.AnimationPlayer.Play("Death");
		boar.AnimationPlayer.AnimationFinished += OnAnimationFinished;
	}
	public override void OnExit()
	{
		boar.AnimationPlayer.AnimationFinished -= OnAnimationFinished;
	}
	public override void OnPhysicsUpdate(double delta)
	{

	}
	public override void OnProcessUpdate(double delta)
	{

	}
	private void OnAnimationFinished(StringName animName)
	{
		if (animName == "Death")
		{
			boar.Die();
			return;
		}
	}
}
