/////////////////////////////////////////////////////////////
/// This is an implementation of the level data.
/// It contains all data the level needs when it is saved
/// 
/// author: Austin Walker
/// 

using Godot;
using RoundManager.Interfaces;
using System;
using System.Collections.Generic;

[GlobalClass]
public partial class Level : Resource
{
    [Export]
    public RoundManager.Difficulty baseDifficulty;
    [Export]
    public int playerHealth;
    [Export]
    public int playerMoney;
    [Export]
    public int currentRoundNum;
    [Export]
    public int enemyBudget;
    [Export]
    public int maxRound;

    [Export]
    public PackedScene mapScene;
    public Node2D[] towers;

    public Level(RoundManager.Difficulty difficulty = RoundManager.Difficulty.Easy,
                 int health = 100,
                 int money = 100,
                 int enemyBudget = 100,
                 int maxRound = 1) {
        this.baseDifficulty = difficulty;
        this.playerHealth = health;
        this.currentRoundNum = 0;
        this.enemyBudget = enemyBudget;
        this.maxRound = maxRound;
    }
}
