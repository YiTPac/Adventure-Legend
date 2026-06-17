using Godot;

[GlobalClass]
public partial class Hitbox : Area2D
{
	[Export] public int DamageAmount { get; set; }
	[Signal] public delegate void HitEventHandler(Hurtbox hurtbox, int DamageAmount);
	public Hitbox()
	{
		AreaEntered += OnAreaEntered;
	}

	private void OnAreaEntered(Area2D area)
	{
		Hurtbox hurtbox = area as Hurtbox;
		if (hurtbox != null)
		{
			GD.Print($"{Owner.Name} => {hurtbox.Owner.Name}");
			EmitSignal(SignalName.Hit, hurtbox, DamageAmount);
			hurtbox.EmitSignal(Hurtbox.SignalName.Hurt, this, DamageAmount);
		}
	}
}
