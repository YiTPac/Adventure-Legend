using Godot;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

public partial class Game : Node
{
	public const string savePath = "user://data.sav";
	public static Game Instance { get; private set; }
	public Dictionary<string, WorldStateData> worldStates = [];
	[Export] public Stats playerStats;
	public override void _Ready()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else
		{
			QueueFree();
		}
	}
	public async void ChangeScene(string path, string entryPointName)
	{
		var tree = GetTree();
		tree.Paused = true;

		var tween = CreateTween();
		tween.SetPauseMode(Tween.TweenPauseMode.Process);
		//tween.TweenProperty(colorRect, );

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
	}

	public async void ChangeScene(string path, PlayerData playerData)
	{
		var tree = GetTree();
		var originalStateName = tree.CurrentScene.SceneFilePath.GetFile().GetBaseName();
		World world = tree.CurrentScene as World;
		worldStates[originalStateName] = world.Data;


		tree.ChangeSceneToFile(path);
		await ToSignal(tree, SceneTree.SignalName.TreeChanged);

		var newStateName = tree.CurrentScene.SceneFilePath.GetFile().GetBaseName();
		if (tree.CurrentScene is World newWorld && worldStates.ContainsKey(newStateName))
		{
			newWorld.Data = worldStates[newStateName];
			newWorld.UpdatePlayer(playerData);
		}
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
		string fullPath = ProjectSettings.GlobalizePath(Game.savePath);
		GD.Print("Save path: " + fullPath);
		using var file = FileAccess.Open(Game.savePath, FileAccess.ModeFlags.Write);
		if (file != null)
		{
			file.StoreString(jsonString);
			GD.Print("File saved successfully!");
		}
		else
		{
			GD.Print("Failed to open file for writing");
		}
	}

	public void LoadGame()
	{
		using var file = FileAccess.Open(Game.savePath, FileAccess.ModeFlags.Read);
		if (file != null)
		{
			string jsonText = file.GetAsText();
			var options = new JsonSerializerOptions
			{
				IncludeFields = true,
				WriteIndented = true,
			};
			GameData gameData = JsonSerializer.Deserialize<GameData>(jsonText, options);

			worldStates = gameData.worldStates;
			playerStats.Data = gameData.playerStatsData;
			ChangeScene(gameData.scene, gameData.playerData);


			GD.Print("File loaded successfully!");
		}
	}
}
