using Godot;
using System;
using System.Linq;

public partial class LevelManager : Node
{
    [Export]
    public Level level;

    private Map currentMap;

    [Export]
    private bool LevelLoaded = false;

    public override void _Ready()
    {
    }

    public override void _Process(double delta)
    {
        // this was used to test if the map would load
        if (Input.IsActionJustPressed("load_map")) {
            //load the map
            currentMap = level.loadMap();
            //book keeping
            if (currentMap != null) {
                AddChild(currentMap);      //makes visible on screen
                currentMap.SetOwner(this); //makes visible in scene tree, and able to be packed into PackedScene
            }
        } else
        if (Input.IsActionJustPressed("unload_map")) {
            level.unloadMap();
        }
    }

    void OnLoadLevel(RoundManager.RoundManager roundManager) {
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

    public bool isMapLoaded() {
        return IsInstanceValid(currentMap);
    }

    public void loadLevel() {

    }

}