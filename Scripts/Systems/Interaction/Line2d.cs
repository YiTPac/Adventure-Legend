using Godot;

public partial class Line2d : Line2D
{
	[Export] private Node2D from;
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
		if (IsInstanceValid(from))
		{
			SetPointPosition(0, from.Position);
		}
		if (IsInstanceValid(to))
		{
			SetPointPosition(1, to.Position);
		}

		if (!IsInstanceValid(to) || !IsInstanceValid(from))
		{
			QueueFree();
		}
	}
}
