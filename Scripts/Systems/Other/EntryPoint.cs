using Godot;
using System;

public partial class EntryPoint : Marker2D
{
	[Export] public Player.Direction direction = Player.Direction.Right;
	public override void _Ready() 
	{
		AddToGroup("EntryPoints");
	}
}
