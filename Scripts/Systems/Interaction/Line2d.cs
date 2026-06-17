using Godot;
using System;

public partial class Line2d : Line2D
{
	[Export] private Node2D form;
	[Export] private Node2D to;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AddPoint(Vector2.Zero);
		AddPoint(Vector2.Zero);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		SetPointPosition(0, form.Position);
		SetPointPosition(1, to.Position);
	}
}
