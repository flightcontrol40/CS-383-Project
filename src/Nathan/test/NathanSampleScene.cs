using Godot;
using System;
using System.Collections.Generic;
using RoundManager;


public partial class NathanSampleScene : Node2D
{
    DifficultyTable difficultyTable = GD.Load<DifficultyTable>("res://src/Nathan/test/SampleHardTable.tres");
    RoundManager.RoundManager roundManager;
    RoundManager.DifficultyCalculator difficultyCalculator;
    List<SpawnOrder> spawnOrders;
    Level level;


    // Called when the node enters the scene tree for the first time.
    public override void _Ready(){
        this.roundManager = this.GetNode<RoundManager.RoundManager>("RoundManager");
        this.difficultyTable.RoundDifficultyValue = new int[100];
        for (int i = 0; i < 100; i++){
            this.difficultyTable.RoundDifficultyValue[i] = 500;
        }
        level = GD.Load<Level>("res://src/Nathan/test/TestLevel.tres");
        this.difficultyCalculator = DifficultyCalculatorFactory.CreateCalculator(
            this.difficultyTable,
            Difficulty.Easy
        );
        this.AddChild(difficultyCalculator);
        this.spawnOrders = new();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) {
    }

    public int spawn_round(int roundNumber){
        List<SpawnOrder> orders = this.difficultyCalculator.CalculateSpawnOrder(roundNumber);
        foreach (SpawnOrder order in orders){
            this.spawnOrders.Add(order);
            order.Enemy.Position = new Vector2(500, 500+spawnOrders.Count);
            // Prevent Memory Leak
            if (order.GetParent() == null){
                this.AddChild(order);
            }
        }
        return this.spawnOrders.Count;
    }
}
