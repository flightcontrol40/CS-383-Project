using Godot;
using System;
using System.Linq;

public partial class LevelManager : Node
{
	[Export]
	private Level level;
	[Export]
	public Map currentMap;

	[Export]
	private bool LevelLoaded = false;

	public override void _Process(double delta)
	{
		// this was used to test if the map would load
		if (Input.IsActionJustPressed("load_map")) {
			loadMap();
		} else
		if (Input.IsActionJustPressed("unload_map")) {
			unloadMap();
		}
	}

	// Loads a map and add's its node as a child of the LevelManager
	// Note: it won't load a map if on is already loaded
	public void loadMap() { 
		if (!IsInstanceValid(currentMap)) {
			currentMap = (Map)level.mapScene.Instantiate();
			AddChild(currentMap);
		}
	}
	
	// Unloads a map if one has been loaded
	public void unloadMap() {
		if (IsInstanceValid(currentMap)) {
			currentMap.QueueFree();
		}
	}

	void OnLoadLevel(RoundManager.RoundManager roundManager) {
	}

	// Probably adds a tower to the tower record
	public void addTower(Node2D tower) {
		level.towers.Append(tower);
	}

	// Probably removes a tower from tower records
	public void removeTower(Node2D tower) {
		level.towers = level.towers.Where(n => n != tower).ToArray();
		tower.QueueFree();
	}

	Path2D getPath() {
		return currentMap.GetNode<Path>("Path").getPath();
	}
	
}