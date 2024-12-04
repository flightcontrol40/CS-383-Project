/////////////////////////////////////////////////////////////
/// This is an implementation of the level data.
/// It contains all data the level needs when it is saved
/// 
/// author: Austin Walker
/// 
using System;
using DifficultyCalculator;
using Godot;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RoundManager;

[GlobalClass]
public partial class Level : Resource
{
    private const string defaultMapScenePath = "res://src/Austin/scenes/map.tscn";

    [Export]
    public DifficultyTable difficultyTable;
    [Export]
    private int PlayerHealth = 250;
    [Export]
    private int playerMoney = 500;
    [Export]
    private int currentRoundNum = 0;
    [Export]
    public int maxRound = 100;
    [Export]
    public PackedScene mapScene = GD.Load<PackedScene>(defaultMapScenePath);
    [Export]
    public Difficulty baseDifficulty = Difficulty.Easy;
    private Map mapInstance = null;

    public int PlayerMoney {
        get { return playerMoney; }
        set { EmitSignal(SignalName.MoneyChanged, value - playerMoney); playerMoney = Math.Max(value, 0); }
    }
    public int playerHealth {
        get { return PlayerHealth; }
        set { EmitSignal(SignalName.HealthChanged, PlayerHealth - value); PlayerHealth = Math.Max(value, 0);  }
    }
    public int CurrentRoundNum {
        get { return currentRoundNum; }
        set { currentRoundNum = Math.Clamp(value, 0, maxRound); }
    }
    public Map MapInstance {
        get { return mapInstance; }
    }

    public Level() {
        GD.Print("Hello there I am an instance of level");
    }


    /// <summary>
    /// If there isn't already a map instance, creates a new one.
    /// </summary>
    /// <returns>The newly created map instance or null if non was created.</returns>
    public Map loadMap() {
        if (!IsInstanceValid(mapInstance)) {
            mapInstance = mapScene.Instantiate<Map>();
            return mapInstance;
        } else
        {
            return null;
        }
    }

    /// <summary>
    /// Frees the current map instance, if there is one
    /// </summary>
    public void unloadMap() {
        if (IsInstanceValid(mapInstance)) {
            mapInstance.Free();
        }
    }

    /// <summary>
    /// Gets a path from the map.
    /// </summary>
    /// <returns>The path returned by calling getPath on the map's path</returns>
    public Path2D getPath(){
        return mapInstance.GetNode<Path>("Path").getPath();
    }

    // Pretend this doesn't exist, I needed things for my pattern...
    // (If the oral exam is done it can probably be deleted at this point)
    /*
    public void setDifficulty(Difficulty difficulty) {
        baseDifficulty = difficulty;
        loadDifficultyTable(difficulty);
    }
    */

    // Pretend this doesn't exist, I needed things for my pattern...
    // (If the oral exam is done it can probably be deleted at this point)
    /*
    private DifficultyTable loadDifficultyTable(Difficulty difficulty) {
        int initialRoundDifficulty; //need to swap this to some kind of exponential equation
        int incrementDifficutly;
        maxRound = 100;
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
        difficultyTable.RoundDifficultyValue = new int[maxRound];
        difficultyTable.RoundDifficultyValue[0] = initialRoundDifficulty;
        for (int i = 1; i < difficultyTable.RoundDifficultyValue.Length; i++) {
            difficultyTable.RoundDifficultyValue[i] = difficultyTable.RoundDifficultyValue[i - 1] + incrementDifficutly;
        }

        return difficultyTable;
    }
    */

    //This is to reset the level
    //I am Sohan I added this here.
    public void ResetLevel()
    {
        GD.Print("Resetting level...");
        PlayerHealth = 100;
        PlayerMoney = 500;
        CurrentRoundNum = 0;
        unloadMap();
        loadMap();
    }

    [Signal]
    public delegate void MoneyChangedEventHandler(int delta);
    [Signal]
    public delegate void HealthChangedEventHandler(int delta);
}
