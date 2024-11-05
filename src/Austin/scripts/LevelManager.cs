using Chicken;
using Godot;
using RoundManager;
using System;

public partial class LevelManager : Node
{
    private const string difficultyTablePath = "res://src/Nathan/CustomResources/DifficultyTable.cs";

	[Export]
    public Difficulty baseDifficutly = Difficulty.Easy;
    [Export]
    public Level level;
    private bool levelLoaded = false;
    public bool mapLoaded { get { return IsInstanceValid(level.MapInstance); } }

    [Signal]
    public delegate void LoadRoundEventHandler(int difficulty, Level level);

    public override void _Ready()
    {
        return;
    }

    public override void _Process(double delta)
    {
    }

    public void OnLoadLevel() {
        // load the map
        level.loadMap();
        //book keeping
        if (level.MapInstance != null) {
            AddChild(level.MapInstance); //makes map visible on the screen
            level.MapInstance.SetOwner(this); // makes visible in scene tree, and able to be serialized into a PackedScene
        }
        // load round
        GD.Print("Level loading, attempting to load round...");
        EmitSignal(SignalName.LoadRound, level, (int)baseDifficutly);
    }

    public void setDifficutly(Difficulty difficulty) {
        if (!levelLoaded) {
            // create new difficulty table
            DifficultyTable newDifficultyTable = loadDifficultyTable(difficulty);

            // book keeping
            baseDifficutly = difficulty;
            level.difficultyTable = newDifficultyTable;
        }
    }

    private DifficultyTable loadDifficultyTable(Difficulty difficulty) {
        int roundDifficulty; //need to swap this to some kind of exponential equation
        DifficultyTable difficultyTable = new DifficultyTable();

        //init EnemyRanks
        switch (difficulty) {
            case Difficulty.Hard:
                GD.Print("Set Difficulty to Hard");
                difficultyTable.EnemyRanks = new Godot.Collections.Array<int>{5, 50, 250};
                roundDifficulty = 10000;
                break;
            case Difficulty.Medium:
                GD.Print("Set Difficulty to Medium");
                difficultyTable.EnemyRanks = new Godot.Collections.Array<int>{5, 50};
                roundDifficulty = 5000;
                break;
            default:
                GD.Print("Set Difficulty to Easy");
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
