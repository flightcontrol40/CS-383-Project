namespace RoundManager;
using Godot;


using GdUnit4;
using static GdUnit4.Assertions;

using System.Collections.Generic;
// using System.Diagnostics;
using System.Diagnostics;
using System;
using Castle.Core.Logging;

[TestSuite]
public class DifficultyCalculatorTests
{

    /// <summary>
    /// Testing the Easy unit difficulty calculator
    /// Should spawn 581 enemies
    /// </summary>
    [TestCase]
    public void UnitCalculateEasyDifficulty()
    {
        var difficultyTable = GD.Load<DifficultyTable>("res://src/Nathan/Tests/SampleEasyTable.tres");
        DifficultyCalculator calculator = DifficultyCalculatorFactory.CreateCalculator(
            difficultyTable,
            Difficulty.Easy
        );

        List<SpawnOrder> spawnOrders = calculator.CalculateSpawnOrder(1);
        
        AssertArray(spawnOrders).HasSize(581);
    }


    /// <summary>
    /// Testing the Medium unit difficulty calculator
    /// Should spawn 726 enemies
    /// </summary>
    [TestCase]
    public void UnitCalculateMediumDifficulty()
    {
        var difficultyTable = GD.Load<DifficultyTable>("res://src/Nathan/Tests/SampleMediumTable.tres");
        DifficultyCalculator calculator = DifficultyCalculatorFactory.CreateCalculator(
            difficultyTable,
            Difficulty.Medium
        );
        List<SpawnOrder> spawnOrders = calculator.CalculateSpawnOrder(1);
        AssertArray(spawnOrders).HasSize(726);
    }

    /// <summary>
    /// Testing the Hard unit difficulty calculator
    /// Should spawn 1089 enemies
    /// </summary>
    [TestCase]
    public void UnitCalculateHardDifficulty()
    {
        var difficultyTable = GD.Load<DifficultyTable>("res://src/Nathan/Tests/SampleHardTable.tres");
        DifficultyCalculator calculator = DifficultyCalculatorFactory.CreateCalculator(
            difficultyTable,
            Difficulty.Hard
        );
        List<SpawnOrder> spawnOrders = calculator.CalculateSpawnOrder(1);
        AssertArray(spawnOrders).HasSize(1089);
    }

    [TestCase]
    public void StressCalculateDifficultyTest()
    {
        var difficultyTable = GD.Load<DifficultyTable>("res://src/Nathan/Tests/StressTestSampleTable.tres");
        
        ISceneRunner runner = ISceneRunner.Load("res://src/Nathan/Tests/NathanSampleScene.tscn");
        Label fpsCounter = (Label) runner.FindChild("FPSCounter", true);
        AssertThat(fpsCounter).IsInstanceOf<Label>();
        string fps = Performance.GetMonitor(Performance.Monitor.TimeFps).ToString();
        fpsCounter.Text = fps;
        // return;

        bool under_load = true;
        int total_enemies = 0;
        while (under_load) {
            runner.SimulateFrames(1);
            DifficultyTable current_table = (DifficultyTable) AutoFree(difficultyTable.Duplicate(true));
            DifficultyCalculator calculator = DifficultyCalculatorFactory.CreateCalculator(
                current_table,
                Difficulty.Hard
            );
            // Get a round of enemies
            List<SpawnOrder> spawnOrders = calculator.CalculateSpawnOrder(1);
            total_enemies += spawnOrders.Count;
            // Get Performance metrics
            double frameTime = Performance.GetMonitor(Performance.Monitor.TimeProcess);
            fps = (1.0 / frameTime).ToString();
            fpsCounter.Text = fps;
            // Check fps
            if (frameTime > 0.1){
                // Less than 10 fps break the loop
                under_load = false;
            }
            
        }
        ConsoleLogger logger = new ConsoleLogger(LoggerLevel.Trace);
        logger.Debug($"Spawned: {total_enemies}, before frame time took more than 100ms.");
        AssertThat($"Spawned: {total_enemies}, before frame time took more than 100ms.").IsNotNull();

    }
}
