using Godot;
using System;

[GlobalClass]
public partial class Level : Resource
{
	[Export]
	public RoundManager.Difficulty baseDifficutly = RoundManager.Difficulty.Easy;
	[Export]
	public int playerHealth = 100;
	[Export]
	public int playerMoney = 100;
	[Export]
	public int currentRoundNum = 0;
	[Export]
	public PackedScene mapScene;
	public Node2D[] towers;
}
