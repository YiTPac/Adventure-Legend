using Godot;
using System.Collections.Generic;
using System.Linq;

public partial class Game : Node
{
	public static Game Instance { get; private set; }
	public Dictionary<StringName, List<NodePath>> worldStates = [];
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
		var originalStateName = tree.CurrentScene.SceneFilePath.GetFile().GetBaseName();
		World world = tree.CurrentScene as World;
		worldStates[originalStateName] = world.ToDictionary();
		

		tree.ChangeSceneToFile(path);
		await ToSignal(tree, SceneTree.SignalName.TreeChanged);

		var newStateName = tree.CurrentScene.SceneFilePath.GetFile().GetBaseName();
        if (tree.CurrentScene is World newWorld && worldStates.ContainsKey(newStateName))
        {
            newWorld.FromDictonary(worldStates[newStateName]);
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
}
