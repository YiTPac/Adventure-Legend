using Godot;
using System;

public partial class BoarSpawner : Interactable
{
	[Export] private PackedScene boarScene;
	public override void Interact(Player player)
	{
		base.Interact(player);
		player.PendingDamage.Enqueue(new Damage
		{
			Amount = 100000,
			Source = this
		});
	}
}
