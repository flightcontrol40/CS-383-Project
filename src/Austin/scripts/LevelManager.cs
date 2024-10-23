using Godot;
using RoundManager;
using System;

public partial class LevelManager : Node
{
    private const string difficultyTablePath_m = "res://src/Nathan/DifficultyTable.cs";

    [Export]
    public Level level { get; set; }
    [Export]
    private Map currentMap { get; set; }
    [Export]
    public int playerHealth { 
        get { return level.playerHealth; }
        set { level.playerHealth = Math.Max(value, 0); }
    }
    [Export]
    public int roundNumber {
        get { return level.currentRoundNum; }
        set { level.currentRoundNum = Math.Clamp(value, 0, level.maxRound); }
    }
    [Export]
    public int maxRound {
        get { return level.maxRound; }
        set {
            if (!levelLoaded) {
                level.maxRound = Math.Max(value, 0);
            }
        }
    }
    private bool levelLoaded { get; } = false;
    public bool mapLoaded { get { return IsInstanceValid(currentMap); } }
    public Path2D path { get { return currentMap.GetNode<Path>("Path").getPath(); } }
    public PackedScene mapScene { 
        set {
            if (!levelLoaded) {
                level.mapScene = value;
            }
        }
    }

    [Signal]
    public delegate void LoadRoundEventHandler(DifficultyTable difficulty);

    public override void _Ready()
    {
        return;
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

    void OnLoadLevel() {
        // load the map
        currentMap = level.loadMap();
        //book keeping
        if (currentMap != null) {
            AddChild(currentMap); //makes map visible on the screen
            currentMap.SetOwner(this); // makes visible in scene tree, and able to be serialized into a PackedScene
        }
        // load round
        EmitSignal(SignalName.LoadRound, level.difficultyTable);
    }

    public void setDifficutly(Difficulty difficulty) {
        if (!levelLoaded) {
            // create new difficulty table
            DifficultyTable newDifficultyTable = loadDifficultyTable(difficulty);

            // book keeping
            level.baseDifficulty = difficulty;
            level.difficultyTable = newDifficultyTable;
        }
    }

    private DifficultyTable loadDifficultyTable(Difficulty difficulty) {
        int roundDifficulty; //need to swap this to some kind of exponential equation
        DifficultyTable difficultyTable = (DifficultyTable)GD.Load<Resource>(difficultyTablePath_m);

        //init EnemyRanks
        switch (difficulty) {
            case Difficulty.Hard:
                difficultyTable.EnemyRanks = new Godot.Collections.Array<int>{5, 50, 250};
                roundDifficulty = 10000;
                break;
            case Difficulty.Medium:
                difficultyTable.EnemyRanks = new Godot.Collections.Array<int>{5, 50};
                roundDifficulty = 5000;
                break;
            default:
                difficultyTable.EnemyRanks = new Godot.Collections.Array<int>{5, 50};
                roundDifficulty = 1000;
                break;
        }

        //init RoundDifficulty
        difficultyTable.RoundDifficultyValue = new int[level.maxRound];
        for (int i = 0; i < difficultyTable.RoundDifficultyValue.Length; i++) {
            difficultyTable.RoundDifficultyValue[i] = roundDifficulty;
        }

        return difficultyTable;
    }

}