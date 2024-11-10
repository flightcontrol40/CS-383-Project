
using Godot;
using GdUnit4;
using static GdUnit4.Assertions;
using Chicken;
using System;
namespace RoundManager.Tests;


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


