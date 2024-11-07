/////////////////////////////////////////////////////////////
/// This is an implementation of the level data.
/// It contains all data the level needs when it is saved
/// 
/// author: Austin Walker
/// 
using System;
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
    public int maxRound = 1;
    [Export]
    public PackedScene mapScene = GD.Load<PackedScene>(defaultMapScenePath);
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
}
