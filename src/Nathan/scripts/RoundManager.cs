using Godot;
namespace RoundManager;

using System;

using System.Collections.Generic;
using System.Linq;
using Chicken;
using DifficultyCalculator;

/// <summary>
/// The Round Manager for Processing/tracking round data across the rounds of
/// a level. The class is also in charge of spawning the enemies, tracking round
/// health and money, signaling when a round ends, and signaling when the player
/// wins or loses
/// </summary>
public partial class RoundManager : Node2D {

    /// <summary>
    /// The DifficultyCalculator for the selected difficulty
    /// </summary>
    private DifficultyCalculator difficultyCalculator;
    
    /// <summary>
    /// The Loaded Level object
    /// </summary>
    private Level levelData;

    /// <summary>
    /// Holds the queue of enemy's to spawn
    /// </summary>
    public List<SpawnOrder> spawnQueue;

    /// <summary>
    /// Holds the currently living enemies
    /// </summary>
    public List<BaseChicken> liveEnemies;
    /// <summary>
    /// Current time to compare against the spawn timer
    /// </summary>
    public double currentTime;
    /// <summary>
    /// The next time to spawn an enemy by
    /// </summary>
    public double nextSpawnTime;

    /// <summary>
    /// Indicates if a round is running.
    /// </summary>
    public bool roundRunning = false;

    [Signal]
    public delegate void updateRoundNumberEventHandler(int roundNum);

    /// <summary>
    /// Called when the object enters the scene tree. Sets up queue objects
    /// </summary>
    public override void _Ready() {
        base._Ready();
        this.spawnQueue = new List<SpawnOrder> {};
        this.liveEnemies = new List<BaseChicken> {};
    }

    /// <summary>
    /// Loads a level from a <c>Level</c> object and creates a difficulty table. 
    /// </summary>
    /// <param name="levelData">The current level data.</param>
    /// <param name="difficulty">Indicates the Difficulty to use.</param>
    public void loadLevel(Level levelData, int difficulty) {
        Difficulty diff = (Difficulty)difficulty;
        this.levelData = levelData;
        this.difficultyCalculator = DifficultyCalculatorFactory.CreateCalculator(
            levelData.difficultyTable,
            diff
        );
        this.AddChild(difficultyCalculator);
    }

    /// <summary>
    /// Called to start a round.
    /// This method starts the next round, getting the <c>SpawnOrder</c>'s 
    /// from the <c>DifficultyCalculator</c>, and begins the spawning timer.
    /// </summary>
    public void startRound() {
        if (this.levelData == null){
            throw new Exception("Round Cannot Be started before a level is loaded");
        }
        if (this.levelData.CurrentRoundNum <= this.levelData.maxRound && roundRunning == false){
            this.spawnQueue = this.difficultyCalculator.CalculateSpawnOrder(
                this.levelData.CurrentRoundNum
            );
            this.roundRunning = true;
            if (spawnQueue.Count > 0){
                this.nextSpawnTime = currentTime + (spawnQueue[0].spawnDelay / 1000.0);
            }
        }
    }

    /// <summary>
    /// Internal method used for spawning the next enemy in the spawn queue
    /// </summary>
    public void spawnEnemy() {
        if (spawnQueue.Count == 0){
            return;
        }
        SpawnOrder order = spawnQueue[0];
        order.Enemy.EnemyDied += HandleEnemyDiesSignal;
        order.Enemy.EndOfPath += HandleEnemyFinishedSignal;
        order.Enemy.EnemySplit += HandleEnemySplit;
        order.Enemy.Start(this.levelData.getPath());
        this.liveEnemies.Add(order.Enemy);
        this.nextSpawnTime = this.currentTime + (order.spawnDelay / 1000.0);
        spawnQueue.RemoveAt(0);
        if (order.Enemy.GetParent() == null){
            this.AddChild(order.Enemy);
        }
    }

    /// <summary>
    /// Enemy Death Signal Handler. 
    /// </summary>
    /// <param name="enemy">The associated enemy.</param>
    public void HandleEnemyDiesSignal(BaseChicken enemy)
    {
        // Free the enemy
        GD.Print("Freeing Chicken");
        this.levelData.PlayerMoney += (enemy.EnemyRank * 5); // Add to money
        liveEnemies.Remove(enemy);
            if (enemy is not null){
                enemy.CallDeferred("queue_free");
            }
    }

    /// <summary>
    /// Enemy Finished path Signal Handler.
    /// </summary>
    /// <param name="enemy">The associated enemy.</param>
    public void HandleEnemyFinishedSignal(BaseChicken enemy) {
        // Do Level Damage
        this.levelData.playerHealth -= enemy.damageAmount;
        // Free the enemy
        liveEnemies.Remove(enemy);
            if (enemy is not null){
                enemy.CallDeferred("queue_free");
            }

    }

    /// <summary>
    /// Enemy Split Event Handler.
    /// </summary>
    /// <param name="enemy">The associated enemy.</param>
    public void HandleEnemySplit(BaseChicken enemy) {
        this.liveEnemies.Add(enemy); // Start tracking the new chicken
        enemy.EnemyDied += HandleEnemyDiesSignal;
        enemy.EndOfPath += HandleEnemyFinishedSignal;
        enemy.EnemySplit += HandleEnemySplit;
    }

    /// <summary>
    /// Clears spawn queue and all loaded enemies. Called before exporting the
    /// level data to save
    /// </summary>
    public void cleanLevel() {
        // Clean the Enemies up.
        foreach ( var spawnOrder in spawnQueue ){
            liveEnemies.Add(spawnOrder.Enemy);
        }
        spawnQueue.Clear();
        foreach (BaseChicken enemy in liveEnemies){
            if (enemy is not null){
                enemy.CallDeferred("queue_free");

            }
        }
        liveEnemies.Clear();
    }


    /// <summary>
    /// Godot Process function called every engine cycle
    /// Processes spawning enemies when the spawn timer is up. Also handles
    /// emitting the end game signals.
    /// </summary>
    /// <param name="delta"></param>
    public override void _Process(double delta) {
        this.currentTime += delta;
        if (this.roundRunning == true) {
            if (this.currentTime > this.nextSpawnTime && spawnQueue.Count > 0){
                this.spawnEnemy();
            }
            if (this.levelData.playerHealth <= 0 ){
                this.roundRunning = false;
                EmitSignal(SignalName.GameLost);
                cleanLevel();
            }
            else if (
                this.levelData.playerHealth > 0 &&
                this.levelData.maxRound <= this.levelData.CurrentRoundNum) {
                    this.roundRunning = false;
                    EmitSignal(SignalName.GameWon);
                    cleanLevel();
                }
            else if ( this.spawnQueue.Count() == 0 && this.liveEnemies.Count() == 0){
                GD.Print($"Round {this.levelData.CurrentRoundNum} Completed");
                this.roundRunning = false;
                this.levelData.CurrentRoundNum++;
                EmitSignal(SignalName.updateRoundNumber, this.levelData.CurrentRoundNum);
            }
        }
        base._Process(delta);
    }


    /// <summary>
    /// Call to do damage to the player.
    /// </summary>
    /// <param name="damageAmount">Amount of damage to do.</param>
    public void TakeDamage(int damageAmount){
        this.levelData.playerHealth -= damageAmount;
    }

    /// <summary>
    /// Signal to emit when the game is lost.
    /// </summary>
    [Signal]
    public delegate void GameLostEventHandler();
    
    /// <summary>
    /// Signal to emit when the game is Won.
    /// </summary>
    [Signal]
    public delegate void GameWonEventHandler();


    private int GetMaxRoundNumber(){
        return this.levelData.difficultyTable.EnemyRanks.Count();
    }

}
