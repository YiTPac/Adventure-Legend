using Godot;
using System;
using System.Threading.Tasks;

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
		await ToSignal(GetTree().CreateTimer(2.5f), "timeout");
		Game.Instance.LoadGame();
	}
	public override void OnExit()
	{

	}
	public override void OnPhysicsUpdate(double delta)
	{

	}
	public override void OnProcessUpdate(double delta)
	{

	}
}
