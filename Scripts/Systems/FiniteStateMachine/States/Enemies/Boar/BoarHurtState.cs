using Godot;
using System;

public partial class BoarHurtState : State
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

		boar.AnimationPlayer.Play("Hit");
		boar.AnimationPlayer.AnimationFinished += OnAnimationFinished;
	}
	public override void OnExit()
	{
		boar.AnimationPlayer.AnimationFinished -= OnAnimationFinished;
	}
	public override void OnPhysicsUpdate(double delta)
	{
		boar.GravityControl(delta, 1);
		boar.Move(delta, 0f);
	}
	public override void OnProcessUpdate(double delta)
	{
		while (boar.PendingDamage.TryDequeue(out var damage))
		{
			boar.Stats.CurrentHealth -= damage.Amount;
			if (damage.Source == null)
			{
				continue;
			}
			var direaction = damage.Source.GlobalPosition.DirectionTo(boar.GlobalPosition);
			boar.Velocity = direaction * 256;
			if (direaction.X < 0)
			{
				boar.direction = Enemy.Direction.Right;
			}
			else
			{
				boar.direction = Enemy.Direction.Left;
			}
		}
		if (boar.Stats.CurrentHealth <= 0)
		{
			stateMachine.SwitchState<BoarDyingState>();
			return;
		}
	}
	private void OnAnimationFinished(StringName animName)
	{
		if (animName == "Hit")
		{
			if (boar.Stats.CurrentHealth > 0)
			{
				stateMachine.SwitchState<BoarRunState>();
				return;
			}
		}
	}
}
