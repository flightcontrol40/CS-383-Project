using Godot;
using System;
using System.Linq;

public partial class LevelManager : Node
{
	[Export]
	private Level currentLevel;
	[Export]
	private Map currentMap;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("load_map")) {
			GD.Print("Ahhhhh");
			loadMap();
		}
	}

	public void loadMap() { 
		var newMap = currentLevel.mapScene.Instantiate();
		AddChild(newMap);
	}

	public void addTower(Node2D tower) {
		currentLevel.towers.Append(tower);
	}

	public void removeTower(Node2D tower) {
		currentLevel.towers = currentLevel.towers.Where(n => n != tower).ToArray();
		tower.QueueFree();
	}


}
