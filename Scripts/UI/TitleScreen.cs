using Godot;
using System;

public partial class TitleScreen : Control
{
	[Export] private AudioStream backgroundMusic;
	[Export] private Button newGameButton;
	[Export] private Button loadGameButton;
	[Export] private Button exitGameButton;
	[Export] private VBoxContainer boxContainer;
	public override void _Ready()
	{
		newGameButton.GrabFocus();
		loadGameButton.Disabled = !Game.Instance.HasSave;
		newGameButton.Pressed += OnNewGamePressed;
		loadGameButton.Pressed += OnLoadGamePressed;
		exitGameButton.Pressed += OnExitGamePressed;
		SoundManager.Instance.PlayMusic(backgroundMusic);
		SoundManager.Instance.SetupUiSounds(this);
	}
	
	public void OnNewGamePressed()
	{
		Game.Instance.NewGame();
	}

	public void OnLoadGamePressed()
	{
		Game.Instance.LoadGame();
	}

	public void OnExitGamePressed()
	{
		GetTree().Quit();
	}
}
