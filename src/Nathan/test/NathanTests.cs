namespace RoundManager;
using Godot;
using GdUnit4;
using static GdUnit4.Assertions;

using System.Collections.Generic;
using System.Threading.Tasks;
using GdUnit4.Asserts;
using Castle.Components.DictionaryAdapter.Xml;


[TestSuite]
public class DifficultyCalculatorTests {
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
        
        AssertArray(spawnOrders).HasSize(581);
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
        AssertArray(spawnOrders).HasSize(726);
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
        AssertArray(spawnOrders).HasSize(1089);
    }


    [TestCase]
    public async Task SpawnChickenTest(){
        ISceneRunner runner = ISceneRunner.Load("res://src/Nathan/test/NathanSampleScene.tscn");
        runner.MaximizeView();
        await runner.SimulateFrames(360);
        RoundManager round = runner.Scene().GetNode<RoundManager>("RoundManager");
        SpawnOrder order = new SpawnOrder(
            Chicken.ChickenFactory.MakeKFC(19),
            250
        );
        round.spawnQueue.Add(order);
        round.roundRunning = true;
        await runner.SimulateFrames(600);


    }


    [TestCase]
    public async Task StressCalculateDifficultyTest() {
        ISceneRunner runner = ISceneRunner.Load("res://src/Nathan/test/NathanSampleScene.tscn");
        runner.MaximizeView();
        bool under_load = true;
        string total_enemies = "";
        while (under_load) {
            var count = runner.Invoke("spawn_round", 1);
            await runner.SimulateFrames(1);
            Label fpsCounter = runner.Scene().GetNode<Label>("CanvasLayer/Container/VSplitContainer/FPSLabels/FPSCounter");
            Label enemyCounter = runner.Scene().GetNode<Label>("CanvasLayer/Container/VSplitContainer/EnemiesLabel/EnemiesCounter");
            AssertThat(fpsCounter).IsInstanceOf<Label>();
            AssertThat(enemyCounter).IsInstanceOf<Label>();
            double currentFPS = Performance.GetMonitor(Performance.Monitor.TimeFps);
            fpsCounter.Text = currentFPS.ToString();
            enemyCounter.Text = count.ToString();
            // Check fps
            if (currentFPS < 15){
                // Less than 15 fps break the loop
                under_load = false;
                total_enemies = $"Spawned: {count}, before FPS < 15.";
            }
        }
        AssertString(total_enemies).HasLength(
            0,
            IStringAssert.Compare.GREATER_THAN
        );
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
        for (int i =0; i< 10; i++){
            await runner.SimulateFrames(360);
        }
    }


}
