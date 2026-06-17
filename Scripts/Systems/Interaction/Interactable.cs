using Godot;
using System;

[GlobalClass]
public partial class Interactable : Area2D
{
	[Signal] public delegate void InteractedEventHandler(Player player);
	public Interactable()
	{
		CollisionLayer = 0;
		CollisionMask = 0;
		SetCollisionMaskValue(2, true);
	}
	public override void _Ready()
	{
		BodyEntered += OnBodyEntered;
		BodyExited += OnBodyExited;
	}
	public virtual void Interact(Player player)
	{
		GD.Print("Player Interacted with " + Name);
		EmitSignal(SignalName.Interacted, player);
	}
	public void OnBodyEntered(Node2D body)
	{
		var player = body as Player;
		if (player != null)
		{
			player.RegisterInteractable(this);
		}
	}

	public void OnBodyExited(Node2D body)
	{
		var player = body as Player;
		if (player != null)
		{
			player.UnregisterInteractable(this);
		}
	}
}
