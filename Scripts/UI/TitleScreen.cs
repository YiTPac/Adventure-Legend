using Godot;
using System;

public partial class TitleScreen : Control
{
	[Export] private Button newGameButton;
	[Export] private Button loadGameButton;
	[Export] private Button exitGameButton;
	[Export] private VBoxContainer boxContainer;
	public override void _Ready()
	{
		newGameButton.GrabFocus();
		loadGameButton.Disabled = !Game.Instance.HasSave;
		foreach (Button button in boxContainer.GetChildren())
		{
			button.MouseEntered += button.GrabFocus;
		}
		newGameButton.Pressed += OnNewGamePressed;
		loadGameButton.Pressed += OnLoadGamePressed;
		exitGameButton.Pressed += OnExitGamePressed;
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
