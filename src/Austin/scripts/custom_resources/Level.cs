/////////////////////////////////////////////////////////////
/// This is an implementation of the level data.
/// It contains all data the level needs when it is saved
/// 
/// author: Austin Walker
/// 
using System;
using DifficultyCalculator;
using Godot;
using RoundManager;

[GlobalClass]
public partial class Level : Resource
{
    private const string defaultMapScenePath = "res://src/Austin/scenes/map.tscn";

    [Export]
    public DifficultyTable difficultyTable;
    [Export]
    private int PlayerHealth = 100;
    [Export]
    private int playerMoney = 100;
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
        set { playerMoney = Math.Max(value, 0); }
    }
    public int playerHealth {
        get { return PlayerHealth; }
        set { PlayerHealth = Math.Max(value, 0); }
    }
    public int CurrentRoundNum {
        get { return currentRoundNum; }
        set { currentRoundNum = Math.Clamp(value, 0, maxRound); }
    }
    public Map MapInstance {
        get { return mapInstance; }
    }

    public Level() {
        loadDifficultyTable(baseDifficulty);
    }

    public Map loadMap() {
        if (!IsInstanceValid(mapInstance)) {
            mapInstance = mapScene.Instantiate<Map>();
            return mapInstance;
        } else {
            return null;
        }
    }

    public void unloadMap() {
        if (IsInstanceValid(mapInstance)) {
            mapInstance.Free();
        }
    }

    public Path2D getPath(){
        return mapInstance.GetNode<Path>("Path").getPath();
    }

    public void setDifficulty(Difficulty difficulty) {
        baseDifficulty = difficulty;
        loadDifficultyTable(difficulty);
    }

    private DifficultyTable loadDifficultyTable(Difficulty difficulty) {
        int initialRoundDifficulty; //need to swap this to some kind of exponential equation
        int incrementDifficutly;
        maxRound = 100;
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
        difficultyTable.RoundDifficultyValue = new int[maxRound];
        difficultyTable.RoundDifficultyValue[0] = initialRoundDifficulty;
        for (int i = 1; i < difficultyTable.RoundDifficultyValue.Length; i++) {
            difficultyTable.RoundDifficultyValue[i] = difficultyTable.RoundDifficultyValue[i - 1] + incrementDifficutly;
        }

        return difficultyTable;
    }
}
