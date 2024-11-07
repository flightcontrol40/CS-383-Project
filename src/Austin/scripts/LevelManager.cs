using Chicken;
using Godot;
using RoundManager;
using System;

public partial class LevelManager : Node
{
    private const string difficultyTablePath = "res://src/Nathan/CustomResources/DifficultyTable.cs";

	[Export]
    public Difficulty baseDifficulty = Difficulty.Easy;
    [Export]
    public Level level;
    private bool levelLoaded = false;
    public bool mapLoaded { get { return IsInstanceValid(level.MapInstance); } }

    [Signal]
    public delegate void LoadRoundEventHandler(Level level, int difficulty);

    public override void _Ready()
    {
        level.difficultyTable = loadDifficultyTable(baseDifficulty);
    }

    public override void _Process(double delta)
    {
        return;
    }

    public void OnLoadLevel() {
        GD.Print("Loading level...");
        // load the map
        level.loadMap();
        //book keeping
        if (level.MapInstance != null) {
            AddChild(level.MapInstance); //makes map visible on the screen
            level.MapInstance.SetOwner(this); // makes visible in scene tree, and able to be serialized into a PackedScene
        }

        // load round
        GD.Print("Level loading, attempting to load round...");
        EmitSignal(SignalName.LoadRound, level, (int)baseDifficulty);
    }

    public void setDifficulty(Difficulty difficulty) {
        if (!levelLoaded) {
            // create new difficulty table
            DifficultyTable newDifficultyTable = loadDifficultyTable(difficulty);

            // book keeping
            baseDifficulty = difficulty;
            level.difficultyTable = newDifficultyTable;
        }
    }

    public void setMap(PackedScene map) {
        if (map != null) {
            level.mapScene = map;
        }
    }

    private DifficultyTable loadDifficultyTable(Difficulty difficulty) {
        int initialRoundDifficulty; //need to swap this to some kind of exponential equation
        int incrementDifficutly;
        level.maxRound = 100;
        DifficultyTable difficultyTable = new DifficultyTable();

        //init EnemyRanks
        switch (difficulty) {
            case Difficulty.Hard:
                GD.Print("Set Difficulty to Hard");
                difficultyTable.EnemyRanks = new Godot.Collections.Array<int>{
                    (int)Chicken.Cost.ChickenR1, 
                    (int)Chicken.Cost.ChickenR2, 
                    (int)Chicken.Cost.ChickenR3, 
                    (int)Chicken.Cost.ChickenR4};
                initialRoundDifficulty = 15;
                incrementDifficutly = 3;
                break;
            case Difficulty.Medium:
                GD.Print("Set Difficulty to Medium");
                difficultyTable.EnemyRanks = new Godot.Collections.Array<int>{
                    (int)Chicken.Cost.ChickenR1, 
                    (int)Chicken.Cost.ChickenR2, 
                    (int)Chicken.Cost.ChickenR3, 
                    (int)Chicken.Cost.ChickenR4};
                initialRoundDifficulty = 8;
                incrementDifficutly = 2;
                break;
            default:
                GD.Print("Set Difficulty to Easy");
                difficultyTable.EnemyRanks = new Godot.Collections.Array<int>{
                    (int)Chicken.Cost.ChickenR1, 
                    (int)Chicken.Cost.ChickenR2, 
                    (int)Chicken.Cost.ChickenR3};
                initialRoundDifficulty = 6;
                incrementDifficutly = 1;
                break;
        }

        //init RoundDifficulty
        difficultyTable.RoundDifficultyValue = new int[level.maxRound];
        difficultyTable.RoundDifficultyValue[0] = initialRoundDifficulty;
        for (int i = 1; i < difficultyTable.RoundDifficultyValue.Length; i++) {
            difficultyTable.RoundDifficultyValue[i] = difficultyTable.RoundDifficultyValue[i - 1] + incrementDifficutly;
        }

        return difficultyTable;
    }

}
