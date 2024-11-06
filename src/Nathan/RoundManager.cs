using Godot;
namespace RoundManager;
using System.Collections.Generic;
using System.Linq;
using Chicken;

/// <summary>
/// The Round Manager for Processing/tracking round data across the rounds of
/// a level.
/// </summary>
public partial class RoundManager : Node2D {

    private DifficultyCalculator difficultyCalculator;
    private Level levelData;
    public List<SpawnOrder> spawnQueue;
    public List<BaseChicken> liveEnemies;
    private double currentTime;
    private double nextSpawnTime;
    public bool roundRunning = false;

    public override void _Ready() {
        base._Ready();
        this.spawnQueue = new List<SpawnOrder> {};
        this.liveEnemies = new List<BaseChicken> {};
    }

    /// <summary>
    /// Loads a level from the level data object.
    /// </summary>
    /// <param name="levelData"></param>
    public void loadLevel(Level levelData, int difficulty) {
        GD.PrintErr($"Setting Difficulty: {difficulty}, LevelData: {levelData}");
        Difficulty diff = (Difficulty)difficulty;
        this.levelData = levelData;
        this.difficultyCalculator = DifficultyCalculatorFactory.CreateCalculator(
            levelData.difficultyTable,
            diff
        );
        this.AddChild(difficultyCalculator);
        this.startRound();
    }
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
    }

    /// <summary>
    /// Enemy Death Signal Handler.
    /// </summary>
    /// <param name="enemy">The enemy to free.</param>
    private void HandleEnemyDiesSignal(BaseChicken enemy) {
        // Free the enemy
        this.levelData.PlayerMoney += enemy.EnemyRank; // Add to money
        liveEnemies.Remove(enemy);
        enemy.QueueFree();
    }

    /// <summary>
    /// Enemy Finished path Signal Handler.
    /// </summary>
    /// <param name="enemy">The enemy to free.</param>
    private void HandleEnemyFinishedSignal(BaseChicken enemy) {
        // Do Level Damage
        this.levelData.playerHealth -= enemy.damageAmount;
        // Free the enemy
        liveEnemies.Remove(enemy);
        enemy.QueueFree();
    }

    /// <summary>
    /// Enemy Split Event Handler
    /// </summary>
    private void HandleEnemySplit(BaseChicken enemy) {
        this.liveEnemies.Add(enemy); // Start tracking the new chicken
    }
    private void cleanLevel() {
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

    public void onLevelLoadSignal(Level level) {}

    [Signal]
    public delegate void GameLostEventHandler();

    [Signal]
    public delegate void GameWonEventHandler();

}
