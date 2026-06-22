using Godot;

public partial class TitleScreen : Control
{
	[Export] private AudioStream backgroundMusic;
	[Export] private Button newGameButton;
	[Export] private Button loadGameButton;
	[Export] private Button exitGameButton;
	[Export] private VBoxContainer boxContainer;
	[Export] private Camera2D camera;
	[Export] private TileMapLayer tileMapLayer;
	public override void _Ready()
	{
		newGameButton.GrabFocus();
		loadGameButton.Disabled = !Game.Instance.HasSave;
		newGameButton.Pressed += OnNewGamePressed;
		loadGameButton.Pressed += OnLoadGamePressed;
		exitGameButton.Pressed += OnExitGamePressed;
		SoundManager.Instance.PlayMusic(backgroundMusic);
		SoundManager.Instance.SetupUiSounds(this);
		//InitializeCamera();
	}
	
	private void InitializeCamera()
	{
		var used = tileMapLayer.GetUsedRect().Grow(-1);
		var tileSize = tileMapLayer.TileSet.TileSize;

		camera.LimitTop = used.Position.Y * tileSize.Y;
		camera.LimitLeft = used.Position.X * tileSize.X;
		camera.LimitBottom = used.End.Y * tileSize.Y;
		camera.LimitRight = used.End.X * tileSize.X;
		camera.ResetSmoothing();
		//camera.ForceUpdateScroll();
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
