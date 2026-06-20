using Godot;

public partial class PlayerDyingState : State
{
	Player player;
	public override void StateReady()
	{
		if (owner is not Player)
		{
			GD.PrintErr("require Player as owner");
			return;
		}
		player = (Player)owner;
	}
	public override async void OnEnter()
	{
		player.CurrentInteractable.Clear();
		player.AnimationPlayer.Play("Death");
		player.InvincibleTimer.Stop();
		await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
		player.OnDie();
	}
	public override void OnExit()
	{

	}
	public override void OnPhysicsUpdate(double delta)
	{
		player.GravityControl(delta, 1);
		player.Stand(delta);
	}
	public override void OnProcessUpdate(double delta)
	{

	}
}
