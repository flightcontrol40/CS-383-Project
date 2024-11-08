using Godot;
namespace RoundManager;
using System.Collections.Generic;
using System.Linq;
using Chicken;

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
    private double currentTime;
    /// <summary>
    /// The next time to spawn an enemy by
    /// </summary>
    private double nextSpawnTime;

    /// <summary>
    /// Indicates if a round is running.
    /// </summary>
    public bool roundRunning = false;

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
        GD.PrintErr($"Setting Difficulty: {difficulty}, LevelData: {levelData}");
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
        GD.Print("Starting Round");
        if (this.levelData.CurrentRoundNum <= this.levelData.maxRound){
            GD.Print($"Calculating SpawnOrder for Round: {this.levelData.CurrentRoundNum}");
            this.spawnQueue = this.difficultyCalculator.CalculateSpawnOrder(
                this.levelData.CurrentRoundNum
            );
            GD.Print($"Spawn Queue: {this.spawnQueue}");
            this.roundRunning = true;
            if (spawnQueue.Count > 0){
                GD.Print("Setting Next Spawn Time");
                this.nextSpawnTime = currentTime + (spawnQueue[0].spawnDelay / 1000.0);
            }
        }
    }

    /// <summary>
    /// Internal method used for spawning the next enemy in the spawn queue
    /// </summary>
    private void spawnEnemy() {
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
        GD.Print($"Spawning Enemy: {order.Enemy}");
    }

    /// <summary>
    /// Enemy Death Signal Handler. 
    /// </summary>
    /// <param name="enemy">The associated enemy.</param>
    private void HandleEnemyDiesSignal(BaseChicken enemy) {
        // Free the enemy
        this.levelData.PlayerMoney += enemy.EnemyRank; // Add to money
        liveEnemies.Remove(enemy);
        enemy.QueueFree();
    }

    /// <summary>
    /// Enemy Finished path Signal Handler.
    /// </summary>
    /// <param name="enemy">The associated enemy.</param>
    private void HandleEnemyFinishedSignal(BaseChicken enemy) {
        // Do Level Damage
        this.levelData.playerHealth -= enemy.damageAmount;
        // Free the enemy
        liveEnemies.Remove(enemy);
        enemy.QueueFree();
    }

    /// <summary>
    /// Enemy Split Event Handler.
    /// </summary>
    /// <param name="enemy">The associated enemy.</param>
    private void HandleEnemySplit(BaseChicken enemy) {
        this.liveEnemies.Add(enemy); // Start tracking the new chicken
    }

    /// <summary>
    /// Clears spawn queue and all loaded enemies. Called before exporting the
    /// level data to save
    /// </summary>
    private void cleanLevel() {
        // Clean the Enemies up.
        foreach ( var spawnOrder in spawnQueue ){
            liveEnemies.Add(spawnOrder.Enemy);
        }
        spawnQueue.Clear();
        foreach (BaseChicken enemy in liveEnemies){
            enemy.QueueFree();
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
        // GD.Print("Processing Round");
        if (this.roundRunning == true) {
            if (this.currentTime > this.nextSpawnTime && spawnQueue.Count > 0){
                GD.Print("Spawning Enemy");
                this.spawnEnemy();
            }
            if (this.levelData.playerHealth < 0 ){
                this.roundRunning = false;
                EmitSignal(SignalName.GameLost);
                cleanLevel();
            }
            else if (
                this.levelData.playerHealth > 0 &&
                this.roundRunning == true &&
                this.levelData.maxRound == this.levelData.CurrentRoundNum)
                {
                    this.roundRunning = false;
                    EmitSignal(SignalName.GameWon);
                    cleanLevel();
                }
            else if ( this.spawnQueue.Count() == 0 && this.liveEnemies.Count() == 0){
                this.roundRunning = false;
                this.levelData.CurrentRoundNum++;
            }
        }
        base._Process(delta);
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

}
