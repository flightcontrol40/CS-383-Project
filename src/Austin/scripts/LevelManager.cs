using Godot;
using RoundManager;
using System;
using System.Linq;

public partial class LevelManager : Node
{
    [Export]
    private Level level;
    [Export]
    public Map currentMap;

    private RoundManager.RoundManager roundManager;

	[Export]
	private bool LevelLoaded = false;

    public override void _Ready()
    {
        this.roundManager = this.GetNode<RoundManager.RoundManager>("RoundManager");
        base._Ready();
    }


    public override void _Process(double delta)
    {
        // this was used to test if the map would load
        if (Input.IsActionJustPressed("load_map")) {
            loadMap();
        } else
        if (Input.IsActionJustPressed("unload_map")) {
            unloadMap();
        }
        if (Input.IsActionJustPressed("start_round")) {
            startRound();
        }
    }

    private void startRound(){
        /*
        if (!IsInstanceValid(this.roundManager)){
            this.loadMap();
        }
        this.roundManager.startRound();
        */
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

    public int Health { 
        get { return this.level.playerHealth; }
        set { this.level.playerHealth = Math.Max(value, 0); }
    }

    public int RoundNumber {
        get { return this.level.currentRoundNum; }
        set { this.level.currentRoundNum = Math.Clamp(value, 0, level.MaxRound); }
    }

    public Path2D LevelPath {
        get { return currentMap.GetNode<Path>("Path").GetNode<Path2D>("Path2D"); }
    }

    public void setLevel(Level newLevel) {
        level = newLevel;
    }

    public bool isMapLoaded() {
        return IsInstanceValid(currentMap);
    }

}