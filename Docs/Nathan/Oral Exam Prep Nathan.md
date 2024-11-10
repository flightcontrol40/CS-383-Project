# Oral Exam Prep Nathan

## Programming Pattern
For this project there are three different patterns that I am using across my work
1. Factory [DifficultyCalculatorFactory](#DifficultyCalculator#DifficultyCalculatorFactory)
2. Composition [RoundManager](#RoundManager#RoundManager)
3. Singleton SoundManager

### Factory
This pattern in used in the [DifficultyCalculatorFactory.CreateCalculator](DifficultyCalculatorFactory#CreateCalculator()) method.
This method returns one of the subclasses of [DifficultCalculator](DifficultCalculator#DifficultCalculator) based on the
passed in [Difficulty](DifficultyCalculator#Difficulty) Enum

### Composition
This pattern in used by the [RoundManager](RoundManager#RoundManager) class.
The [RoundManager](RoundManager#RoundManager) contain's manages and creates other class instances of
[DifficultyCalculator](#DifficultyCalculator#DifficultyCalculator) and [BaseChicken](#Chicken#BaseChicken) classes. 
This allows the RoundManager to properly track round progress, update the level data accordingly, and spawn in 
the enemies each time a round is started

### Singleton
This pattern is used by the SoundManager class.
In Godot there are no true Singleton Objects but the effect and purpose of a Singleton
can be achieved through the global class system. An instance of the SoundManager 
class is loaded into to global scope of the project during the pre-load time. This
allows all other objects and scripts in the game access to a single instance of
the sound manager, and with specific naming schemes and hiding the constructor, 
effectively create a Singleton object in the global scope.

## Dynamic Binding
An example for dynamic binding that I used in the project is the set of DifficultyCalculator classes.
The base class is the DifficultyCalculator class and it has three subclasses:
1. [EasyDifficultyCalculator](DifficultCalculator#EasyDifficultyCalculator)
2. [MediumDifficultCalculator](DifficultCalculator#MediumDifficultCalculator)
3. [HardDifficultCalculator](DifficultCalculator#HardDifficultCalculator)

The base class defines a public virtual method called [CalculateSpawnOrder](DifficultCalculator#DifficultyCalculator#CalculateSpawnOrder())
that returns a list of [SpawnOrders](DifficultyCalculator#SpawnOrder) for the [RoundManager](RoundManager#RoundManager).
Two of the subclasses provide an override method for the interface and the other
uses the parent classes implementation :
- [EasyDifficultyCalculator.CalculateSpawnOrder](EasyDifficultyCalculator#CalculateSpawnOrder)
- [HardDifficultCalculator.CalculateSpawnOrder](HardDifficultCalculator#CalculateSpawnOrder)
- MediumDifficultCalculator Uses the parent implementation [CalculateSpawnOrder](DifficultCalculator#DifficultyCalculator#CalculateSpawnOrder())


## Prefab/Godot Scene
For the Prefab requirement I have the DifficultyCalculator classes

### DifficultyCalculator Scene
The [DifficultyCalculator](#DifficultyCalculator#DifficultyCalculator) set of classes are used for calculating what and when to
spawn enemies in a Godot Project.

There are three variants of the difficulty calculator:
1. [EasyDifficultyCalculator](DifficultCalculator#EasyDifficultyCalculator)
2. [MediumDifficultCalculator](DifficultCalculator#MediumDifficultCalculator)
3. [HardDifficultCalculator](DifficultCalculator#HardDifficultCalculator)

All three can be acquired using the [DifficultyCalculatorFactory](#DifficultyCalculator#DifficultyCalculatorFactory) factory function

### DifficultyCalculator Classes
In Godot there is a specific structure that this scene expects:
1. A [Difficulty Enum](DifficultyCalculator#Difficulty)
2. A [DifficultyTable](RoundManager#DifficultyTable)

### DifficultyCalculator Methods
The enemy spawn orders can then be created with the [CalculateSpawnOrder](DifficultCalculator#DifficultyCalculator#CalculateSpawnOrder()) method.
This function will return a List of [SpawnOrders](DifficultyCalculator#SpawnOrder)

The full API can be viewed [here](https://flightcontrol40.github.io/CS-383-Project/classRoundManager_1_1DifficultyCalculator.html)

## Copyright Example
In this project the copyright/fair use example that im using is the project logo
![Logo](@ref angry_chicken_logo.png)

This is a derivative work of the Chick Fil' A logo:
![Chick-fil-A-Logo](https://logosmarcas.net/wp-content/uploads/2021/08/Chick-fil-A-Logo.png)

I argue that this is fair use as I have recreated it with the same style but it
it still unique from the original

## Test Cases

### Example Test Case

An example for a test case that caught an error in the project is the
HandleEnemyDiedSignalTest().This test caught a bug in the event coupling between the
[RoundManager](RoundManager#RoundManager) and the [BaseChicken](#Chicken#BaseChicken) classes where both were 
attempting to free the [BaseChicken](#Chicken#BaseChicken) Instance causing a double free error.

### Tests
C# Tests
~~~~~~~~~~~~~{.cs}
[TestSuite]
class RoundManagerTests{

    RoundManager round = null;
    Frankest enemy = null;
    SpawnOrder order = null;
    Level level = null;

    bool signal_status = false;


    [Signal]
    public delegate void EnemyDiedEventHandler(BaseChicken enemy);

    [Signal]
    public delegate void EndOfPathEventHandler(BaseChicken enemy);

    [Signal]
    public delegate void EnemySplitEventHandler(BaseChicken enemy);


    [BeforeTest]
    public void SetupTest(){

        round = new RoundManager();
        enemy = GD.Load<PackedScene>("res://src/Clayton/Enemy/Frankest.tscn").Instantiate<Frankest>();
        round._Ready();
        enemy._Ready();
        order = new SpawnOrder(enemy, 100);
        order._Ready();
        round.spawnQueue.Add(order);
        level = GD.Load<Level>("res://src/Nathan/test/TestLevel.tres");
        AssertThat(level).IsInstanceOf<Level>();
    }


    [TestCase]
    public void SpawnChickenTest(){
        round.loadLevel(level, 1);
        level.loadMap();
        round.spawnEnemy();
        AssertArray(round.liveEnemies).IsNotEmpty();
    }


    [TestCase]
    public void LoadLevelTest(){
        round.loadLevel(level, 1);
        level.loadMap();
        round.spawnEnemy();
        AssertArray(round.liveEnemies).IsNotEmpty();
    }


   [TestCase]
    public void HandleEnemyDiedSignalTest(){
        round.loadLevel(level, 1);
        level.loadMap();
        round.spawnEnemy();
        order.Enemy.TakeDamage(500);
        AssertArray(round.liveEnemies).IsEmpty();
    }

   [TestCase]
    public void HandleEnemyFinishedSignalTest(){
        round.loadLevel(level, 1);
        level.loadMap();
        round.spawnEnemy();
        enemy.EmitSignal(Frankest.SignalName.EndOfPath, enemy);
        AssertArray(round.liveEnemies).IsEmpty();
    }

   [TestCase]
    public void HandleEnemySplitSignalTest(){
        round.loadLevel(level, 1);
        level.loadMap();
        round.spawnEnemy();
        enemy.EmitSignal(Frankest.SignalName.EnemySplit, enemy);
        AssertArray(round.liveEnemies).HasSize(2);
    }


    [TestCase]
    public void TestStartRound(){
        round.loadLevel(level, 1);
        level.loadMap();
        round.startRound();
        AssertArray(round.spawnQueue).IsNotEmpty();
        AssertBool(round.roundRunning).IsTrue();
    }

    [TestCase]
    public void TestStartRoundException(){
        try {
        round.startRound();
        // Shouldn't Get here
        AssertBool(false).IsTrue();
        }
        catch (Exception e){
            AssertString(e.Message).IsEqual("Round Cannot Be started before a level is loaded");
        }
    }

    [TestCase]
    public void TestCleanRound(){
        round.loadLevel(level, 1);
        level.loadMap();
        round.startRound();
        round.cleanLevel();
        AssertArray(round.liveEnemies).IsEmpty();
        AssertArray(round.spawnQueue).IsEmpty();
    }

    [TestCase]
    public void TestDamage(){
        round.loadLevel(level, 1);
        level.loadMap();
        round.TakeDamage(50);
        AssertInt(level.playerHealth).IsLess(100);
    }

    [TestCase]
    public void TestLose(){
        round.loadLevel(level, 1);
        level.loadMap();
        round.startRound();
        round.spawnQueue.Clear();
        round.liveEnemies.Clear();
        round._Process(10);
        AssertArray(round.liveEnemies).IsEmpty();
        AssertArray(round.spawnQueue).IsEmpty();
    }

    public void TestSignal(){
        signal_status = true;
    }

    [TestCase]
    public void TestLoseSignal(){
        round.loadLevel(level, 1);
        level.loadMap();
        round.GameLost += TestSignal;
        round.startRound();
        round.spawnQueue.Clear();
        round.liveEnemies.Clear();
        level.playerHealth = 0;
        round._Process(10);
        AssertBool(signal_status).IsTrue();
    }

    [TestCase]
    public void TestWinSignal(){
        round.loadLevel(level, 1);
        level.loadMap();
        round.GameWon += TestSignal;
        round.startRound();
        round.spawnQueue.Clear();
        round.liveEnemies.Clear();
        level.maxRound = level.CurrentRoundNum;
        round._Process(10);
        AssertBool(signal_status).IsTrue();
    }

    [AfterTest]
    public void TestTeardown(){
        round.Free();
        order.Free();
        level.Free();
        signal_status = false;
    }

}


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
~~~~~~~~~~~~~

~~~~~~~~~~~~~{.gd}
extends GdUnitTestSuite
class_name SoundManagerTests

var manager: soundManager = null

func before_test():
   manager = soundManager.new()
   manager._ready()


func test_music_volume()->void:
    manager.set_music_volume(.5)
    assert_float(manager.music_volume).is_equal(.5)

func test_music_volume_underflow()->void:
    manager.set_music_volume(-1)
    assert_float(manager.music_volume).is_equal(0.0)

func test_music_volume_overflow()->void:
    manager.set_music_volume(10000)
    assert_float(manager.music_volume).is_equal(1.0)

func test_music_startup_volume()->void:
    assert_float(manager.music_volume).is_equal(.25)

func test_music_autoplay()->void:
    assert_bool(manager.music_player.autoplay).is_true()

func test_load_music()->void:
    manager.load_music("test_song", "res://src/Nathan/Assets/Magna Bovinia.mp3")
    assert_dict(manager.music).contains_keys(["test_song"])

func test_stop_music()->void:
    manager.stop_music()
    assert_bool(manager.music_player.playing).is_false()

func test_start_music()->void:
    manager.stop_music()
    manager.load_music("test_song", "res://src/Nathan/Assets/Magna Bovinia.mp3")
    manager.play_music("test_song")
    assert_bool(manager.music_player.playing).is_true()


func test_sfx_volume()->void:
    manager.set_sfx_volume(.5)
    assert_float(manager.sfx_volume).is_equal(.5)

func test_sfx_volume_underflow()->void:
    manager.set_sfx_volume(-1)
    assert_float(manager.sfx_volume).is_equal(0.0)

func test_sfx_volume_overflow()->void:
    manager.set_sfx_volume(10000)
    assert_float(manager.sfx_volume).is_equal(1.0)

func test_sfx_startup_volume()->void:
    assert_float(manager.sfx_volume).is_equal(.25)

func test_sfx_autoplay()->void:
    assert_bool(manager.sfx_player.autoplay).is_false()

func test_load_sfx()->void:
    manager.load_sfx("test_sfx", "res://src/Nathan/Assets/Magna Bovinia.mp3")
    assert_dict(manager.sfx).contains_keys(["test_sfx"])

func test_stop_sfx()->void:
    manager.stop_sfx()
    assert_bool(manager.sfx_player.playing).is_false()

func test_start_sfx()->void:
    manager.stop_sfx()
    manager.load_sfx("test", "res://src/Nathan/Assets/Magna Bovinia.mp3")
    manager.play_sfx("test")
    assert_bool(manager.sfx_player.playing).is_true()

func after_test():
    manager.free()
~~~~~~~~~~~~~