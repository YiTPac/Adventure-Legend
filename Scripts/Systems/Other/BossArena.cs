using Godot;

public partial class BossArena : World
{
	[Export] public Enemy boss;
	public override void _Ready()
	{
		base._Ready();
		boss.EnemyDied += () =>
		{
			GD.Print("sss");
			Game.Instance.BackToTitle();
		};
	}
}
