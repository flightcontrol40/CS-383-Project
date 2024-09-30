using Godot;
using System;
using System.Linq;

public partial class LevelManager : Node
{
	[Export]
	private Level currentLevel;
	[Export]
	private Map currentMap;

	public override void _Ready()
	{
	}

	public override void _Process(double delta)
	{
		// this was used to test if the map would load
		if (Input.IsActionJustPressed("load_map")) {
			loadMap();
		}
	}

	// Loads a map and add's its node as a child of the LevelManager
	// Note: it won't load a map if on is already loaded
	public void loadMap() { 
		if (!IsInstanceValid(currentMap)) {
			currentMap = (Map)currentLevel.mapScene.Instantiate();
			AddChild(currentMap);
		}
	}
	
	// Unloads a map if one has been loaded
	public void unloadMap() {
		if (IsInstanceValid(currentMap)) {
			currentMap.QueueFree();
		}
	}

	// Probably adds a tower to the tower record
	public void addTower(Node2D tower) {
		currentLevel.towers.Append(tower);
	}

	// Probably removes a tower from tower records
	public void removeTower(Node2D tower) {
		currentLevel.towers = currentLevel.towers.Where(n => n != tower).ToArray();
		tower.QueueFree();
	}


}
