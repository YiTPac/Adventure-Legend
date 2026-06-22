using Godot;

public partial class SaveStone : Interactable, ISaveable<bool>
{
	[Export] private AnimationPlayer animationPlayer;

	public bool Activated
	{
		get => animationPlayer.CurrentAnimation == "Activated";
		set
		{
			if (value)
			{
				animationPlayer.Play("Activated");
			}
			else
			{
				animationPlayer.Play("Ready");
			}
		}
	}

	public override void Interact(Player player)
	{
		base.Interact(player);
		
		player.Stats.CurrentHealth = player.Stats.MaxHealth;
		Activated = true;
		Game.Instance.SaveGame();

	}

	public bool OnSaveState()
	{
		return Activated;
	}

	public void OnLoadState(bool data)
	{
		Activated = data;
	}

}
