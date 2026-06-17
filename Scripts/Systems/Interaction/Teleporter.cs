using Godot;
using System;

[GlobalClass]
public partial class Teleporter : Interactable
{
	[Export(PropertyHint.File)] private string path;
	[Export] private string entryPointName;
	public override void Interact(Player player)
	{
		base.Interact(player);
		Game.Instance.ChangeScene(path, entryPointName);
	}
}
