using Godot;
namespace RoundManager;

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Interfaces;

/// <summary>
/// The Round Manager for Processing/tracking round data across the rounds of
/// a level.
/// </summary>
public partial class RoundManager : Node2D {


    private RoundStatusTracker roundStatusTracker;
    private ILevelData lastLevelData;
    private List<SpawnOrder> spawnQueue;
    private List<GodotObject> liveEnemies;
    private Godot.Timer spawnTimer;
    private Difficulty difficulty;


    public RoundManager(ILevelData levelData, Difficulty difficulty){
        this.loadLevel(levelData, difficulty);
        this.spawnTimer = new Godot.Timer();
        spawnTimer.Timeout += this.spawnEnemy;
        this.liveEnemies = new List<GodotObject>();
        this.spawnQueue = new List<SpawnOrder>();
    }

    private void spawnEnemy(){
        if (spawnQueue.Count() == 0){
            return;
        }
        SpawnOrder order = spawnQueue[0];
        spawnQueue.RemoveAt(0);

        // Should be fixed when the base enemy is actually implemented
        
        
        // TODO reimplement this once enemy base class is made
        // order.Enemy.EnemyDied += HandleEnemyDiesSignal; 
        // order.Enemy.EndOfPath += HandleEnemyFinishedSignal;


        order.Enemy.Start(lastLevelData.LevelPath);
        this.liveEnemies.Add((GodotObject)order.Enemy);

        spawnTimer.Start(((double)order.spawnDelay) / 1000.00);
    }

    /// <summary>
    /// Enemy Death Signal Handler.
    /// </summary>
    /// <param name="enemy">The enemy to free.</param>
    private void HandleEnemyDiesSignal(IEnemyType enemy) {
        // Free the enemy
        liveEnemies.Remove(((GodotObject) enemy));
        ((GodotObject) enemy).Free();
    }

    /// <summary>
    /// Enemy Finished path Signal Handler.
    /// </summary>
    /// <param name="enemy">The enemy to free.</param>
    private void HandleEnemyFinishedSignal(IEnemyType enemy) {
        // Do Level Damage
        this.lastLevelData.Health -= enemy.DamageAmount;
        // Free the enemy
        liveEnemies.Remove(((GodotObject) enemy));
        ((GodotObject) enemy).Free();
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
            liveEnemies.Add((GodotObject)spawnOrder.Enemy);
        }
        spawnQueue.Clear();
        while (liveEnemies.Count > 0){
            GodotObject enemy = liveEnemies[0];
            spawnQueue.RemoveAt(0);
            enemy.Free();
        }
    }

    /// <summary>
    /// Called every cycle to process data.
    /// </summary>
    /// <param name="delta"></param>
    public override void _Process(double delta) {
        if ( roundStatusTracker.roundStarted == true) {
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
        roundStatusTracker.roundStarted = true;
        spawnTimer.Start(((double)spawnQueue[0].spawnDelay) / 1000.0);
    }



[Signal]
public delegate void GameLostEventHandler();

[Signal]
public delegate void GameWonEventHandler();

}

