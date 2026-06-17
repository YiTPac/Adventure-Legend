using Godot;

[GlobalClass]
public partial class Hurtbox : Area2D
{
	[Signal] public delegate void HurtEventHandler(Hitbox hitbox, int DamageAmount);

}
