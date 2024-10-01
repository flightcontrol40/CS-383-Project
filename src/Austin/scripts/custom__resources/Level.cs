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
	public RoundManager.Difficulty baseDifficulty = RoundManager.Difficulty.Easy;
	[Export]
	public int playerHealth = 100;
	[Export]
	public int playerMoney = 100;
	[Export]
	public int currentRoundNum = 0;
	[Export]
	public int enemyBudget = 100;
	[Export]
	public int MaxRound = 1;

	[Export]
	public PackedScene mapScene;
	public Node2D[] towers;
}
