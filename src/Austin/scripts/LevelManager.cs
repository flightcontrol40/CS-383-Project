using Godot;
using RoundManager;
using DifficultyCalculator;
using LevelConverter;

public partial class LevelManager : Node
{
    private const string difficultyTablePath = "res://src/Nathan/CustomResources/DifficultyTable.cs";

	[Export]
    public Difficulty baseDifficulty = Difficulty.Easy;
    [Export]
    public Level level;
    private bool levelLoaded = false;
    public bool mapLoaded { get { return IsInstanceValid(level.MapInstance); } }

    //public ResourceInstanceConverter levelConverter; //This won't be used I just need it for my pattern... (Yes I could use it, but eveything already works so I am not going to break it)

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

    /// <summary>
    /// Initializes the map and emits a signal that tells the round manager to load the first round.
    /// Gets called by the level selection menu once the player has finished selecting a level.
    /// </summary>
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

    /// <summary>
    /// Used to set the difficulty of a level and creates a new difficulty table object.
    /// </summary>
    /// <param name="difficulty">Some difficutly from the RoundManager.Difficulty enum to set the level difficulty to</param>
    public void setDifficulty(Difficulty difficulty) {
        if (!levelLoaded) {
            // create new difficulty table
            DifficultyTable newDifficultyTable = loadDifficultyTable(difficulty);

            // book keeping
            baseDifficulty = difficulty;
            level.difficultyTable = newDifficultyTable;
        }
    }

    /// <summary>
    /// Used to set the map to another map scene.
    /// </summary>
    /// <param name="map">A PackedScene that refers to a map</param>
    public void setMap(PackedScene map) {
        if (map != null) {
            level.mapScene = map;
        }
    }

    /// <summary>
    /// Creates a new difficulty table and sets it based on the level's difficulty
    /// </summary>
    /// <param name="difficulty">Difficutly of the level</param>
    /// <returns>The newly created DifficultyTable object</returns>
    private DifficultyTable loadDifficultyTable(Difficulty difficulty) {
        int initialRoundDifficulty; //need to swap this to some kind of exponential equation
        int incrementDifficutly;
        level.maxRound = 100;
        DifficultyTable difficultyTable = new DifficultyTable();

        //init EnemyRanks
        switch (difficulty) {
            case Difficulty.Hard:
                difficultyTable.EnemyRanks = new Godot.Collections.Array<int>{
                    (int)Chicken.Cost.ChickenR1, 
                    (int)Chicken.Cost.ChickenR2, 
                    (int)Chicken.Cost.ChickenR3, 
                    (int)Chicken.Cost.ChickenR4};
                initialRoundDifficulty = 15;
                incrementDifficutly = 3;
                break;
            case Difficulty.Medium:
                difficultyTable.EnemyRanks = new Godot.Collections.Array<int>{
                    (int)Chicken.Cost.ChickenR1, 
                    (int)Chicken.Cost.ChickenR2, 
                    (int)Chicken.Cost.ChickenR3, 
                    (int)Chicken.Cost.ChickenR4};
                initialRoundDifficulty = 8;
                incrementDifficutly = 2;
                break;
            default:
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
