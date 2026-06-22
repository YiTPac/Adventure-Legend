using Godot;

public partial class ShackyCamera : Camera2D
{
	[Export] private float strength = 0.0f;
	[Export] private float recoverySpeed = 16.0f;
	public override void _Ready()
	{
		Game.Instance.CameraShouldShake += (float shakeStrength) =>  strength += shakeStrength;
	}

	public override void _PhysicsProcess(double delta)
	{
		Offset = new Vector2((float)GD.RandRange(-strength, strength), (float)GD.RandRange(-strength, strength));
		strength = Mathf.MoveToward(strength, 0, (float)delta * recoverySpeed);
	}
}
