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
    private List<SpawnOrder> spawnQueue;
    private List<BaseChicken> liveEnemies;
    // private System.Timers.Timer spawnTimer;
    // private Difficulty difficulty;
    private double currentTime;
    private double nextSpawnTime;
    private bool roundRunning = false;

    public override void _Ready()
    {
        this.spawnQueue = new List<SpawnOrder> {};
        this.liveEnemies = new List<BaseChicken> {};
        base._Ready();
    }

    /// <summary>
    /// Loads a level from the level data object.
    /// </summary>
    /// <param name="levelData"></param>
    public void loadLevel(Level levelData, Difficulty difficulty) {
        this.levelData = levelData;
        this.difficultyCalculator = DifficultyCalculatorFactory.CreateCalculator(
            levelData.difficultyTable,
            difficulty
        );
    }

    public void startRound() {
        this.spawnQueue = this.difficultyCalculator.CalculateSpawnOrder(
            this.levelData.currentRoundNum
        );
        this.roundRunning = true;
        if (spawnQueue.Count > 0){
            this.nextSpawnTime = currentTime + (spawnQueue[0].spawnDelay / 1000.0);
        }
    }

    private void spawnEnemy(){
        if (spawnQueue.Count == 0){
            return;
        }
        GD.Print("Im spawning a chicken!");
        SpawnOrder order = spawnQueue[0];
        spawnQueue.RemoveAt(0);
        order.Enemy.EnemyDied += HandleEnemyDiesSignal;
        order.Enemy.EndOfPath += HandleEnemyFinishedSignal;
        order.Enemy.Start(this.levelData.getPath());
        this.liveEnemies.Add(order.Enemy);
        this.nextSpawnTime = this.currentTime + (order.spawnDelay / 1000.0);
        GD.Print(order.Enemy);
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
        this.levelData.playerHealth -= enemy.damageAmount;
        // Free the enemy
        liveEnemies.Remove(enemy);
        enemy.QueueFree();
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


    public override void _Process(double delta) {
        this.currentTime += delta;
        if ( this.roundRunning == true) {
            if (currentTime > nextSpawnTime && spawnQueue.Count > 0){
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
                this.levelData.maxRound == this.levelData.currentRoundNum)
                {
                    this.roundRunning = false;
                    EmitSignal(SignalName.GameWon);
                    cleanLevel();
                }
            else if ( this.spawnQueue.Count() == 0 && this.liveEnemies.Count() == 0){
                this.roundRunning = false;
                this.levelData.currentRoundNum++;
            }
        }
        base._Process(delta);
    }

    public void onLevelLoadSignal(Level level){
    }

    [Signal]
    public delegate void GameLostEventHandler();

    [Signal]
    public delegate void GameWonEventHandler();

}
