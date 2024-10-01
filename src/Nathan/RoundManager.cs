using Godot;
namespace RoundManager;

using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Chicken;
using System;
using System.Timers;



/// <summary>
/// The Round Manager for Processing/tracking round data across the rounds of
/// a level.
/// </summary>
public partial class RoundManager : Node2D {


    private RoundStatusTracker roundStatusTracker;
    private ILevelData lastLevelData;
    private List<SpawnOrder> spawnQueue;
    private List<BaseChicken> liveEnemies;
    private System.Timers.Timer spawnTimer;
    private Difficulty difficulty;
    private double currentTime;
    private double nextSpawnTime;


    public override void _Ready()
    {
        base._Ready();
    }

    public RoundManager(ILevelData levelData, Difficulty difficulty){
        this.loadLevel(levelData, difficulty);
        this.liveEnemies = new List<BaseChicken>();
        this.spawnQueue = new List<SpawnOrder>();
    }

    private void spawnEnemy(){
        if (spawnQueue.Count == 0){
            return;
        }
        SpawnOrder order = spawnQueue[0];
        spawnQueue.RemoveAt(0);
        order.Enemy.EnemyDied += HandleEnemyDiesSignal; 
        order.Enemy.EndOfPath += HandleEnemyFinishedSignal;

        order.Enemy.Start(lastLevelData.LevelPath);
        this.liveEnemies.Add(order.Enemy);
        this.nextSpawnTime = this.currentTime + (order.spawnDelay / 1000.0);
        // spawnTimer.Interval = order.spawnDelay / 1000.00;
        // spawnTimer.Enabled = true;
    }

    /// <summary>
    /// Enemy Death Signal Handler.
    /// </summary>
    /// <param name="enemy">The enemy to free.</param>
    private void HandleEnemyDiesSignal(BaseChicken enemy) {
        // Free the enemy
        liveEnemies.Remove(enemy);
        enemy.QueueFree();
    }

    /// <summary>
    /// Enemy Finished path Signal Handler.
    /// </summary>
    /// <param name="enemy">The enemy to free.</param>
    private void HandleEnemyFinishedSignal(BaseChicken enemy) {
        // Do Level Damage
        this.lastLevelData.Health -= enemy.damageAmount;
        // Free the enemy
        liveEnemies.Remove(enemy);
        enemy.QueueFree();
    }

    /// <summary>
    /// Loads a level from the level data object.
    /// </summary>
    /// <param name="levelData"></param>
    public void loadLevel(ILevelData levelData, Difficulty difficulty) {
        this.lastLevelData = levelData;
        this.difficulty = difficulty;
        roundStatusTracker = new RoundStatusTracker(levelData.DifficultyTable, difficulty);
    }

    public ILevelData unloadLevel() {
        this.cleanLevel();
        return this.lastLevelData;
    }

    private void cleanLevel(){
        // Clean the Enemies up.
        foreach ( var spawnOrder in spawnQueue ){
            liveEnemies.Add(spawnOrder.Enemy);
        }
        spawnQueue.Clear();
        while (liveEnemies.Count > 0){
            BaseChicken enemy = liveEnemies[0];
            spawnQueue.RemoveAt(0);
            enemy.Free();
        }
    }

    /// <summary>
    /// Called every cycle to process data.
    /// </summary>
    /// <param name="delta"></param>
    public override void _Process(double delta) {
        this.currentTime += delta;
        if ( roundStatusTracker.roundStarted == true) {
            if (currentTime > nextSpawnTime && spawnQueue.Count > 0){
                this.spawnEnemy();
            }
            if (lastLevelData.Health < 0 ){
                roundStatusTracker.roundStarted = false;
                EmitSignal(SignalName.GameLost);
                cleanLevel();
            }
            else if ( 
                lastLevelData.Health > 0 && 
                roundStatusTracker.roundStarted == true && 
                lastLevelData.DifficultyTable.MaxRound == lastLevelData.RoundNumber)
                {
                    roundStatusTracker.roundStarted = false;
                    EmitSignal(SignalName.GameWon);
                    cleanLevel();
                }
            else if ( this.spawnQueue.Count() == 0 && this.liveEnemies.Count() == 0){
                roundStatusTracker.roundStarted = false;
                lastLevelData.RoundNumber++;
            }
        }
        base._Process(delta);
    }

    public void startRound() {
        spawnQueue.AddRange(
            this.roundStatusTracker.getSpawnOrder(this.lastLevelData.RoundNumber)
        );
        if (spawnQueue.Count > 0){
            roundStatusTracker.roundStarted = true;
            this .nextSpawnTime = currentTime + (spawnQueue[0].spawnDelay / 1000.0);
        }
    }


    [Signal]
    public delegate void GameLostEventHandler();

    [Signal]
    public delegate void GameWonEventHandler();

}

