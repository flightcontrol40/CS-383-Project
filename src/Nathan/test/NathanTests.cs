namespace RoundManager;
using Godot;
using GdUnit4;
using static GdUnit4.Assertions;

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using System.Security.Cryptography.X509Certificates;
using System.Timers;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
    public void SpawnChickenTest(){
        Level level = AutoFree(GD.Load<Level>("res://src/Nathan/test/TestLevel.tres"));
        RoundManager round = AutoFree<RoundManager>(new RoundManager());
        level.loadMap();
        round.loadLevel(level, 1);
        SpawnOrder order = AutoFree( 
            new SpawnOrder(
                AutoFree<Chicken.BaseChicken>(Chicken.ChickenFactory.MakeKFC(Chicken.Cost.ChickenR1)),
                250
            )
        );
        round.spawnQueue.Add(order);
        round.roundRunning = true;
        round.spawnEnemy();
        level.unloadMap();
        AssertArray(round.liveEnemies).IsNotEmpty();
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
        // System.Timers.Timer timer = new System.Timers.Timer(10000);
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
            // await runner.SimulateFrames(1);
            // Check fps
            if (average_fps < 10){
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

    [TestCase]
    public async Task LoadLevelTest(){
        ISceneRunner runner = ISceneRunner.Load("res://src/Nathan/test/NathanSampleScene.tscn");
        runner.MaximizeView();
        await runner.SimulateFrames(360);
        RoundManager round = runner.Scene().GetNode<RoundManager>("RoundManager");
        Level level = GD.Load<Level>("res://src/Nathan/test/TestLevel.tres");
        AssertThat(level).IsInstanceOf<Level>();
        round.loadLevel(level, 1);
        // round.loadLevel(level, (int)Difficulty.Medium);
        await runner.SimulateFrames(360);
        round.startRound();
        await runner.SimulateFrames(360);
        AssertArray(round.liveEnemies).IsNotEmpty();
    }
}
