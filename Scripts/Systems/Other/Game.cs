using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

public partial class Game : Node
{
	public const string SavePath = "C:\\Users\\YiTPac\\AppData\\Roaming\\Godot\\app_userdata\\Adventure Legend\\data.sav";
	public const string ConfigPath = "user://config.ini";
	public static Game Instance { get; private set; }
	public Dictionary<string, Dictionary<string, string>> worldStates = [];
	public StatsData defaultPlayerStatsData = new StatsData();
	[Export] public ColorRect colorRect;
	[Export] public Stats playerStats;
	[Export(PropertyHint.File)] private string initialWorldPath;
	[Export(PropertyHint.File)] private string titleScreenPath;

	[Signal]
	public delegate void CameraShouldShakeEventHandler(float strength);
	public bool HasSave
	{
		get
		{
			return File.Exists(SavePath);
		}
	}
	public override async void _Ready()
	{
		defaultPlayerStatsData = playerStats.Data;
		
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			QueueFree();
		}
		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		LoadConfig();
	}
	public async void ChangeScene(string path, string entryPointName)
	{
		var tree = GetTree();
		tree.Paused = true;

		var tween = CreateTween();
		tween.SetPauseMode(Tween.TweenPauseMode.Process);
		tween.TweenProperty(colorRect, "color:a", 1, 0.2d);
		await ToSignal(tween, Tween.SignalName.Finished);


		var originalStateName = tree.CurrentScene.SceneFilePath.GetFile().GetBaseName();
		World world = tree.CurrentScene as World;
		worldStates[originalStateName] = world.Data;

		tree.ChangeSceneToFile(path);
		await ToSignal(tree, SceneTree.SignalName.TreeChanged);

		var newStateName = tree.CurrentScene.SceneFilePath.GetFile().GetBaseName();
		if (tree.CurrentScene is World newWorld && worldStates.ContainsKey(newStateName))
		{
			newWorld.Data = worldStates[newStateName];
		}

		foreach (EntryPoint entryPoint in tree.GetNodesInGroup("EntryPoints").Cast<Node2D>().Cast<EntryPoint>())
		{
			if (entryPoint.Name == entryPointName)
			{
				newWorld = tree.CurrentScene as World;
				newWorld?.UpdatePlayer(entryPoint.GlobalPosition, entryPoint.direction);
				break;
			}
		}

		tree.Paused = false;
		tween = CreateTween();
		tween.TweenProperty(colorRect, "color:a", 0, 0.2d);
		await ToSignal(tween, Tween.SignalName.Finished);
	}

	public async void ChangeScene(string path, PlayerData playerData)
	{
		var tree = GetTree();
		tree.Paused = true;

		var tween = CreateTween();
		tween.SetPauseMode(Tween.TweenPauseMode.Process);
		tween.TweenProperty(colorRect, "color:a", 1, 0.2d);
		await ToSignal(tween, Tween.SignalName.Finished);

		tree.ChangeSceneToFile(path);
		await ToSignal(tree, SceneTree.SignalName.TreeChanged);

		var newStateName = tree.CurrentScene.SceneFilePath.GetFile().GetBaseName();
		if (tree.CurrentScene is World newWorld && worldStates.ContainsKey(newStateName))
		{
			newWorld.Data = worldStates[newStateName];
			newWorld.UpdatePlayer(playerData);
		}

		tree.Paused = false;
		tween = CreateTween();
		tween.TweenProperty(colorRect, "color:a", 0, 0.2d);
		await ToSignal(tween, Tween.SignalName.Finished);

	}
	public void SaveGame()
	{
		World world = GetTree().CurrentScene as World;
		string sceneName = world.SceneFilePath.GetFile().GetBaseName();
		worldStates[sceneName] = world.Data;
		GameData gameData = new()
		{
			worldStates = worldStates,
			scene = world.SceneFilePath,
			playerData = world.player.Data,
			playerStatsData = world.player.Stats.Data
		};
		var options = new JsonSerializerOptions
		{
			IncludeFields = true,
			WriteIndented = true
		};
		string jsonString = JsonSerializer.Serialize(gameData, options);
		GD.Print("JSON: " + jsonString);
		try
		{
			File.WriteAllText(SavePath, jsonString);
			GD.Print("File saved successfully!");
		}
		catch (Exception exception)
		{
			GD.Print("Failed to open file for writing, Message:" + exception.Message);
		}

		// string fullPath = ProjectSettings.GlobalizePath(Game.savePath);
		// GD.Print("Save path: " + fullPath);
		// using var file = FileAccess.Open(Game.savePath, FileAccess.ModeFlags.Write);
		// if (file != null)
		// {
		// 	file.StoreString(jsonString);

		// }
		// else
		// {

		// }
	}

	public void LoadGame()
	{
		if (File.Exists(SavePath))
		{
			string jsonText = File.ReadAllText(SavePath);
			var options = new JsonSerializerOptions
			{
				IncludeFields = true,
				WriteIndented = true,
			};
			GameData gameData = JsonSerializer.Deserialize<GameData>(jsonText, options);

			ChangeScene(gameData.scene, gameData.playerData);

			worldStates = gameData.worldStates;
			playerStats.Data = gameData.playerStatsData;



			GD.Print("File loaded successfully!");
		}
	}

	public async void NewGame()
	{
		File.Delete(SavePath);
		worldStates = [];
		var tree = GetTree();
		tree.Paused = true;

		var tween = CreateTween();
		tween.SetPauseMode(Tween.TweenPauseMode.Process);
		tween.TweenProperty(colorRect, "color:a", 1, 0.2d);
		await ToSignal(tween, Tween.SignalName.Finished);

		tree.ChangeSceneToFile(initialWorldPath);
		playerStats.Data = defaultPlayerStatsData;
		await ToSignal(tree, SceneTree.SignalName.TreeChanged);

		tree.Paused = false;
		tween = CreateTween();
		tween.TweenProperty(colorRect, "color:a", 0, 0.2d);
		await ToSignal(tween, Tween.SignalName.Finished);
	}

	public async void BackToTitle()
	{
		var tree = GetTree();
		tree.Paused = true;

		var tween = CreateTween();
		tween.SetPauseMode(Tween.TweenPauseMode.Process);
		tween.TweenProperty(colorRect, "color:a", 1, 0.2d);
		await ToSignal(tween, Tween.SignalName.Finished);

		tree.ChangeSceneToFile(titleScreenPath);

		tree.Paused = false;
		tween = CreateTween();
		tween.TweenProperty(colorRect, "color:a", 0, 0.2d);
		await ToSignal(tween, Tween.SignalName.Finished);
	}
	
	public void SaveConfig()
	{
		var config = new ConfigFile();
		
		config.SetValue("Audio", "Master", SoundManager.Instance.GetVolume((int)SoundBus.Master));
		config.SetValue("Audio", "SFX", SoundManager.Instance.GetVolume((int)SoundBus.SFX));
		config.SetValue("Audio", "BackgroundMusic", SoundManager.Instance.GetVolume((int)SoundBus.BackgroundMusic));
		
		config.Save(ConfigPath);
	}

	public void LoadConfig()
	{
		var config = new ConfigFile();
		config.Load(ConfigPath);
		
		SoundManager.Instance.SetVolume((int)SoundBus.Master, config.GetValue("Audio", "Master", 0.5f).AsSingle());
		SoundManager.Instance.SetVolume((int)SoundBus.SFX, config.GetValue("Audio", "SFX", 1.0f).AsSingle());
		SoundManager.Instance.SetVolume((int)SoundBus.BackgroundMusic, config.GetValue("Audio", "BackgroundMusic", 1.0f).AsSingle());
	}

	public void ShakeCamera(float strength)
	{
		EmitSignal(SignalName.CameraShouldShake, strength);
	}
}
