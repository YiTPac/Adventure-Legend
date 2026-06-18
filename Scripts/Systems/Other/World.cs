using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public struct WorldStateData
{
	public List<string> enemiesAlive = [];
	public WorldStateData() { }
}

public partial class World : Node2D
{
	public WorldStateData Data
	{
		get
		{
			WorldStateData worldStateData = new WorldStateData();
			foreach (Node node in GetTree().GetNodesInGroup("Enemies"))
			{
				var path = GetPathTo(node);
				worldStateData.enemiesAlive.Add(path);
			}
			return worldStateData;
		}
		set
		{
			GD.Print(value.enemiesAlive.ToString());
			foreach (Node node in GetTree().GetNodesInGroup("Enemies"))
			{
				GetPathTo(node).ToString();
				var path = GetPathTo(node).ToString();
				if (!value.enemiesAlive.Contains(path))
				{
					node.QueueFree();
				}
			}
		}
	}
	[Export] private TileMapLayer tileMapLayer;
	[Export] private Camera2D camera;
	[Export] public Player player;
	public override void _Ready()
	{
		InitializeCamera();
		//GD.Print(System.Environment.Version);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
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

	public void UpdatePlayer(Vector2 position, Player.Direction direction)
	{
		player.GlobalPosition = position;
		player.FacingDirection = direction;
		camera.ResetSmoothing();
		//camera.ForceUpdateScroll();
	}

	public void UpdatePlayer(PlayerData playerData)
	{
		player.Data = playerData;
		camera.ResetSmoothing();
	}
}
