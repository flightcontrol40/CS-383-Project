using Godot;
using RoundManager.Interfaces;
using System;
using System.Linq;

public partial class LevelManager : Node
{
	[Export]
	private Level level;
	[Export]
	public Map currentMap;
	private LevelData levelInterface;

	[Export]
	private bool LevelLoaded = false;

	public override void _Ready()
	{
		levelInterface = new LevelData(level);
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
			currentMap = (Map)level.mapScene.Instantiate();
			AddChild(currentMap);

			levelInterface.currentMap = currentMap;
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

	
}
public class LevelData : RoundManager.Interfaces.ILevelData {
	private Level level;
	public Map currentMap;

	public LevelData(Level l) {
		level = l;
	}

	int ILevelData.Health { 
		get { return level.playerHealth; }
		set { level.playerHealth = value; }
	}
	
	int ILevelData.RoundNumber {
		get { return level.currentRoundNum; }
		set { level.currentRoundNum = value; }
	}

	Path2D ILevelData.LevelPath {
		get { return currentMap.GetNode<Path>("Path").getPath(); }
	}

	public IDifficultyTable DifficultyTable { get; }
}