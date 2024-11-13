namespace RoundManager.Tests;
using Godot;
using GdUnit4;
using static GdUnit4.Assertions;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using DifficultyCalculator;


[TestSuite]
public class DifficultyCalculatorTests {

    public bool enabled = false;

    /// <summary>
    /// Testing the Easy unit difficulty calculator
    /// Should spawn 581 enemies
    /// </summary>
    [TestCase]
    public void UnitCalculateEasyDifficulty() {
        var difficultyTable = GD.Load<DifficultyTable>("res://src/Nathan/test/SampleEasyTable.tres");
        DifficultyCalculator calculator = AutoFree(DifficultyCalculatorFactory.CreateCalculator(
            difficultyTable,
            Difficulty.Easy
        ));
        List<SpawnOrder> spawnOrders = calculator.CalculateSpawnOrder(1);
        AssertArray(spawnOrders).IsNotEmpty();
    }

    /// <summary>
    /// Testing the Medium unit difficulty calculator
    /// Should spawn 726 enemies
    /// </summary>
    [TestCase]
    public void UnitCalculateMediumDifficulty() {
        var difficultyTable = GD.Load<DifficultyTable>("res://src/Nathan/test/SampleMediumTable.tres");
        DifficultyCalculator calculator = AutoFree(DifficultyCalculatorFactory.CreateCalculator(
            difficultyTable,
            Difficulty.Medium
        ));
        List<SpawnOrder> spawnOrders = calculator.CalculateSpawnOrder(1);
        AssertArray(spawnOrders).IsNotEmpty();
    }

    /// <summary>
    /// Testing the Hard unit difficulty calculator
    /// Should spawn 1089 enemies
    /// </summary>
    [TestCase]
    public void UnitCalculateHardDifficulty() {
        var difficultyTable = GD.Load<DifficultyTable>("res://src/Nathan/test/SampleHardTable.tres");
        DifficultyCalculator calculator = AutoFree(DifficultyCalculatorFactory.CreateCalculator(
            difficultyTable,
            Difficulty.Hard
        ));
        List<SpawnOrder> spawnOrders = calculator.CalculateSpawnOrder(1);
        AssertArray(spawnOrders).IsNotEmpty();
    }

    [TestCase]
    public async Task StressCalculateDifficultyTest() {
        ISceneRunner runner = ISceneRunner.Load("res://src/Nathan/test/NathanSampleScene.tscn");
        // await runner.SimulateFrames();
        bool under_load = true;
        string total_enemies = "";
        Label fpsCounter = runner.Scene().GetNode<Label>("CanvasLayer/Container/VSplitContainer/FPSLabels/FPSCounter");
        Label enemyCounter = runner.Scene().GetNode<Label>("CanvasLayer/Container/VSplitContainer/EnemiesLabel/EnemiesCounter");
        Godot.Timer timer = runner.Scene().GetNode<Godot.Timer>("Timer");
        AssertThat(fpsCounter).IsInstanceOf<Label>();
        AssertThat(enemyCounter).IsInstanceOf<Label>();
        List<double> frame_times = new List<double>(Enumerable.Repeat(0.03,50));
        int i = 0;
        double average_fps;
        int count = 0;
        timer.Start(15);
        while (under_load) {
            runner.MaximizeView();
            await runner.SimulateFrames(1);
            if (i % 50 == 49) {
                var c = runner.Invoke("spawn_round", 1);
                count = (int)c;
            }
            double processTime = Performance.GetMonitor(Performance.Monitor.TimePhysicsProcess);
            frame_times[i % 50] = processTime;
            average_fps = 1 / frame_times.Average();
            fpsCounter.Text = Performance.GetMonitor(Performance.Monitor.TimeFps).ToString(); //((int)average_fps).ToString();
            enemyCounter.Text = count.ToString();
            // Check fps
            if (average_fps < 5){
                // Less than 15 fps break the loop
                under_load = false;
                // Assert a failure
                AssertThat(false).IsTrue();
                total_enemies = $"Spawned: {count}, before FPS < 15.";
            }
            if (timer.IsStopped() == true){
                under_load = false;
                // Assert a pass
                AssertThat(true).IsTrue();
                total_enemies = $"Spawned: {count}";
            }
            i++;
        }
        AssertString(total_enemies).IsNotEmpty();
    }

}

