using Godot;
using RoundManager;
using System;
using System.Linq;

public partial class LevelManager : Node
{
    [Export]
    public Level level;

    private Map currentMap;

    [Export]
    private bool levelLoaded = false;

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

    Path2D getPath() {
        return currentMap.GetNode<Path>("Path").getPath();
    }

    public int playerHealth { 
        get { return this.level.playerHealth; }
        set { this.level.playerHealth = Math.Max(value, 0); }
    }

    public int RoundNumber {
        get { return this.level.currentRoundNum; }
        set { this.level.currentRoundNum = Math.Clamp(value, 0, level.MaxRound); }
    }

    public void setMap(PackedScene mapScene) {
        if (!levelLoaded) {
            level.mapScene = mapScene;
        }
    }

    public void setDifficutly(Difficulty difficulty) {
        if (!levelLoaded) {
            level.baseDifficulty = difficulty;

            DifficultyTable newDifficultyTable = (DifficultyTable)GD.Load<Resource>("res://src/Nathan/DifficultyTable.cs");
            switch (difficulty) {
                case Difficulty.Easy:
                    loadEasyDifficultyTable(newDifficultyTable);
                    break;
                case Difficulty.Medium:
                    loadMediumDifficultyTable(newDifficultyTable);
                    break;
                case Difficulty.Hard:
                    loadHardDifficultyTable(newDifficultyTable);
                    break;
                default:
                    loadEasyDifficultyTable(newDifficultyTable);
                    break;
            }

            level.difficultyTable = newDifficultyTable;
        }
    }

    public bool mapLoaded {
        get { return IsInstanceValid(currentMap); }
    }

    private void loadEasyDifficultyTable(DifficultyTable difficultyTable) {
        // figure out EnemyRanks, should they be defined by chickens?

        difficultyTable.RoundDifficultyValue = new int[level.MaxRound];


        for (int i = 0; i < level.MaxRound; i++) {
            difficultyTable.RoundDifficultyValue[i] = 100; // set this to an exponential equation to gradually bring up the difficulty
        }
    }


    private void loadMediumDifficultyTable(DifficultyTable difficultyTable) {
        
    }

    private void loadHardDifficultyTable(DifficultyTable difficultyTable) {
        
    }

}