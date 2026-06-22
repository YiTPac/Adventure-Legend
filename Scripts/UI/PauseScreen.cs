using Godot;

public partial class PauseScreen : Control
{
	[Export] private Button resumeButton;
	[Export] private Button quitButton;
	public override void _Ready()
	{
		Hide();
		SoundManager.Instance.SetupUiSounds(this);
		resumeButton.Pressed += OnResumePressed;
		quitButton.Pressed += OnQuitPressed;
		VisibilityChanged += () => GetTree().Paused = Visible;
	}

	public void ShowPauseScreen()
	{
		Show();
		GrabFocus();
	}
	
	private void OnQuitPressed()
	{
		Game.Instance.BackToTitle();
	}

	private void OnResumePressed()
	{
		Hide();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("Pause"))
		{
			Hide();
			GetWindow().SetInputAsHandled();
		}
	}
}
