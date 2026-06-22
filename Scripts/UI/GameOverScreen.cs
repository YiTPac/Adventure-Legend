using Godot;

public partial class GameOverScreen : Control
{
	[Export] private AudioStream backgroundMusic;
	[Export] private AnimationPlayer animationPlayer;
	public override void _Ready()
	{
		Hide();
		SetProcessInput(false);
	}

	public override void _Input(InputEvent @event)
	{
		GetWindow().SetInputAsHandled();
		if (@event is InputEventKey or InputEventJoypadButton or InputEventMouseButton)
		{
			if (@event.IsPressed() && !@event.IsEcho())
			{
				if (Game.Instance.HasSave)
				{
					Game.Instance.LoadGame();
				}
				else
				{
					Game.Instance.NewGame();
				}
			}
		}
	}

	public void ShowGameOver()
	{
		Show();
		SoundManager.Instance.PlayMusic(backgroundMusic);
		animationPlayer.Play("Enter");
		animationPlayer.AnimationFinished += OnAnimationFinished;
		
	}

	private void OnAnimationFinished(StringName animName)
	{
		SetProcessInput(true);
	}
}
