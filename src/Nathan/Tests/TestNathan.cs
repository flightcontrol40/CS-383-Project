using System.Collections.Generic;
using System.Diagnostics;
using GdMUT;
using Godot;
using RoundManager;

/// <summary>
/// This is a test class for GDMUT. This is purely for demonstration. If you added
/// this into your project, feel free to delete it =)
/// </summary>
/// 


public class TestsNathan
{

    [CSTestFunction]
    public static Result UnitCalculateEasyDifficulty()
    {
        var difficultyTable = GD.Load<DifficultyTable>("res://src/Nathan/Tests/SampleEasyTable.tres");
        DifficultyCalculator calculator = DifficultyCalculatorFactory.CreateCalculator(
            difficultyTable,
            Difficulty.Easy
        );
        List<SpawnOrder> spawnOrders = calculator.CalculateSpawnOrder(1);
        return (spawnOrders.Count > 1) ? new Result(true, $"Easy Spawn Order Length: {spawnOrders.Count}") : Result.Failure;
    }

    [CSTestFunction]
    public static Result UnitCalculateHardDifficulty()
    {
        var difficultyTable = GD.Load<DifficultyTable>("res://src/Nathan/Tests/SampleHardTable.tres");
        DifficultyCalculator calculator = DifficultyCalculatorFactory.CreateCalculator(
            difficultyTable,
            Difficulty.Hard
        );
        List<SpawnOrder> spawnOrders = calculator.CalculateSpawnOrder(1);
        return (spawnOrders.Count > 1) ? new Result(true, $"Hard Spawn Order Length: {spawnOrders.Count}") : Result.Failure;
    }

    [CSTestFunction]
    public static Result StressCalculateDifficultyTest()
    {
        var difficultyTable = GD.Load<DifficultyTable>("res://src/Nathan/Tests/StressTestSampleTable.tres");
        DifficultyCalculator calculator = DifficultyCalculatorFactory.CreateCalculator(
            difficultyTable,
            Difficulty.Hard
        );
        Stopwatch timer = new Stopwatch();
        timer.Start();
        List<SpawnOrder> spawnOrders = calculator.CalculateSpawnOrder(1);
        timer.Stop();
        return (spawnOrders.Count > 1) ? new Result(true, $"Time to Spawn `{spawnOrders.Count}` Enemies: {timer.Elapsed}") : Result.Failure;
    }


}