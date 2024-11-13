using Godot;
using System.Collections.Generic;
using Chicken;
using System;
using RoundManager;
namespace DifficultyCalculator;

/// <summary>
/// Enum to represent the current Difficulty
/// </summary>
public enum Difficulty {
    Easy = 0,
    Medium = 1,
    Hard = 2
}

/// <summary>
/// A Single spawn order element for controlling what and when to spawn an enemy
/// </summary>
public partial class SpawnOrder: Node{

    /// <summary>
    /// Creates A spawn Order for a passed type that implements IEnemyType
    /// </summary>
    /// <param name="enemy">The Enemy class use for the spawn.</param>
    /// <param name="spawnDelay">The delay in milliseconds to wait before spawning this enemy</param>
    public SpawnOrder(Chicken.BaseChicken enemy, int spawnDelay) {
        this.spawnDelay = spawnDelay;
        this.Enemy = enemy;
        this.AddChild(enemy);
    }

    /// <summary>
    /// The Enemy class use for the spawn.
    /// </summary>
    public BaseChicken Enemy;

    /// <summary>
    /// The delay in milliseconds to wait before spawning this enemy
    /// </summary>
    public int spawnDelay;

}

/// <summary>
/// Factory Function for getting a DifficultyCalculator class object
/// </summary>
public class DifficultyCalculatorFactory {

    /// <summary>
    /// Get a new RoundManager#DifficultyCalculator class obj based on the Difficulty
    /// </summary>
    /// <param name="difficultyTable">
    /// RoundManager#DifficultyTable to be used by the calculator.
    /// </param>
    /// <param name="difficulty">
    /// The RoundManager#Difficulty to be used for the calculator
    /// </param>
    /// <returns>
    /// A RoundManager#DifficultyCalculator of the passed difficulty
    /// </returns>
    public static DifficultyCalculator CreateCalculator(DifficultyTable difficultyTable, Difficulty difficulty){
        DifficultyCalculator difficultyCalculator;
        switch (difficulty)
        {
            case Difficulty.Easy:
                difficultyCalculator = new EasyDifficultyCalculator(difficultyTable);
                break;
            case Difficulty.Medium:
                difficultyCalculator = new MediumDifficultyCalculator(difficultyTable);
                break;
            case Difficulty.Hard:
                difficultyCalculator = new HardDifficultyCalculator(difficultyTable);
                break;
            default:
                difficultyCalculator = new DifficultyCalculator(difficultyTable);
                break;
        }
        return difficultyCalculator;
    }
}

/// <summary>
/// The Base class for a difficulty calculator. Cannot construct directly, instead
/// use the DifficultyCalculatorFactory#CreateCalculator method.
/// </summary>
public partial class DifficultyCalculator: Node {

    /// <summary>
    /// The current difficulty table that is loaded.
    /// </summary>
    protected DifficultyTable difficultyTable;

    /// <summary>
    /// Internal/Private Constructor
    /// </summary>
    /// <param name="difficultyTable"></param>
    internal DifficultyCalculator(DifficultyTable difficultyTable){
        this.difficultyTable = difficultyTable;
    }

    /// <summary>
    /// Getter function for getting the amount of a particular enemy rank to 
    /// spawn
    /// </summary>
    /// <param name="cost">The cost of the enemy to spawn.</param>
    /// <param name="levelValue">The total amount of enemy "Value" for the level.</param>
    /// <returns>The amount of enemies to spawn.</returns>
    protected int getSpawnAmount( int cost, ref int levelValue ) {
        // Calculate the amount of enemies to spawn with the given cost for the
        // Given Level Value
        int amount =  (levelValue / cost) / 2;
        levelValue -= amount;
        return amount;
    }
    /// <summary>
    /// Gets the available ranks of enemies that can be spawned. 
    /// </summary>
    /// <returns>The Available ranks to spawn</returns>
    protected Godot.Collections.Array<int> getEnemyRanks(){
        Godot.Collections.Array<int> ranks = new Godot.Collections.Array<int>();
        foreach (int rank in Enum.GetValues(typeof(Chicken.Cost))){
            ranks.Add(rank);
        }
        return ranks;
    }
    /// <summary>
    /// Base Virtual function for Calculating the Spawn Order.
    /// This function returns a list of <c>SpawnOrder</c> objects that should
    /// be spawned for the corresponding round
    /// </summary>
    /// <param name="roundNumber">The Current Round Number</param>
    /// <returns></returns>
    public virtual List<SpawnOrder> CalculateSpawnOrder(int roundNumber) {
        List<SpawnOrder> spawnOrders = new() { };
        Godot.Collections.Array<int> enemies = this.getEnemyRanks();
        if ( roundNumber >= this.difficultyTable.RoundDifficultyValue.Length){
            roundNumber = this.difficultyTable.RoundDifficultyValue.Length -1;
        }
        if (roundNumber < 1){
            roundNumber = 1;
        }
        int levelValue = this.difficultyTable.RoundDifficultyValue[roundNumber-1];
        while ( enemies.Count > 0 ){
            int cost = enemies.Max();
            int amount = getSpawnAmount(cost, ref levelValue);
            while ( amount > 0) {
                SpawnOrder order = new SpawnOrder(
                        Chicken.ChickenFactory.MakeKFC((Chicken.Cost)cost),
                        250
                    );
                this.AddChild(order);
                spawnOrders.Add(order);
                amount -= 1;
            }
            enemies.Remove(cost);
        }
        return spawnOrders;
    }
}

/// <summary>
/// Difficulty Calculator For the Easy Difficulty
/// </summary>
public partial class EasyDifficultyCalculator : DifficultyCalculator {


    /// <summary>
    /// Internal/Private Constructor
    /// </summary>
    /// <param name="difficultyTable"></param>
    internal EasyDifficultyCalculator(DifficultyTable difficultyTable) : base(difficultyTable) {
        this.difficultyTable = difficultyTable;
    }
    /// <summary>
    /// Function for Calculating the Spawn Order.
    /// This function returns a list of <c>SpawnOrder</c> objects that should
    /// be spawned for the corresponding round. Easy Mode lowers the enemy
    /// 'spawn budget' by 20%
    /// </summary>
    /// <param name="roundNumber">The Current Round Number</param>
    /// <returns></returns>
    public override List<SpawnOrder> CalculateSpawnOrder(int roundNumber) {
        List<SpawnOrder> spawnOrders = new() { };
        Godot.Collections.Array<int> enemies = this.getEnemyRanks();
        if ( roundNumber >= this.difficultyTable.RoundDifficultyValue.Length){
            roundNumber = this.difficultyTable.RoundDifficultyValue.Length -1;
        }
        if (roundNumber < 1){
            roundNumber = 1;
        }
        // Lower Level Value on Easy
        GD.Print($"Round Number: {roundNumber}");
        int levelValue = this.difficultyTable.RoundDifficultyValue[roundNumber-1];
        levelValue = (int)((float) levelValue * 0.8);
        
        while ( enemies.Count > 0 ){
            int cost = enemies.Max();
            int amount = getSpawnAmount(cost, ref levelValue);
            while ( amount > 0) {
                SpawnOrder order = new SpawnOrder(
                        Chicken.ChickenFactory.MakeKFC((Chicken.Cost)cost),
                        500
                    );
                this.AddChild(order);
                spawnOrders.Add(order);
                amount -= 1;
            }
            enemies.Remove(cost);
        }
        return spawnOrders;
    }
}

/// <summary>
/// Difficulty Calculator For the Medium Difficulty
/// </summary>
public partial class MediumDifficultyCalculator : DifficultyCalculator {

    /// <summary>
    /// Internal/Private Constructor
    /// </summary>
    /// <param name="difficultyTable"></param>
    internal MediumDifficultyCalculator(DifficultyTable difficultyTable) : base(difficultyTable) {
        this.difficultyTable = difficultyTable;
    }
}

/// <summary>
/// Difficulty Calculator For the Hard Difficulty
/// </summary>
/// 
public partial class HardDifficultyCalculator : DifficultyCalculator {

    /// <summary>
    /// Internal/Private Constructor
    /// </summary>
    /// <param name="difficultyTable"></param>
    internal HardDifficultyCalculator(DifficultyTable difficultyTable) : base(difficultyTable) {
        this.difficultyTable = difficultyTable;
    }
    /// <summary>
    /// Function for Calculating the Spawn Order.
    /// This function returns a list of <c>SpawnOrder</c> objects that should
    /// be spawned for the corresponding round. Hard Mode raises the enemy
    /// 'spawn budget' by 50%
    /// </summary>
    /// <param name="roundNumber">The Current Round Number</param>
    /// <returns></returns>
    public override List<SpawnOrder> CalculateSpawnOrder( int roundNumber) {
        List<SpawnOrder> spawnOrders = new() { };
        Godot.Collections.Array<int> enemies = this.getEnemyRanks();
        if ( roundNumber >= this.difficultyTable.RoundDifficultyValue.Length){
            roundNumber = this.difficultyTable.RoundDifficultyValue.Length -1;
        }
        if (roundNumber < 1){
            roundNumber = 1;
        }
        // Raise Level Value on hard
        int levelValue = this.difficultyTable.RoundDifficultyValue[roundNumber-1];
        levelValue = (int)((float)levelValue * 1.5);
        while ( enemies.Count > 0 ){
            int cost = enemies.Max();
            int amount = getSpawnAmount(cost, ref levelValue);
            while ( amount > 0) {
                SpawnOrder order = new SpawnOrder(
                        Chicken.ChickenFactory.MakeKFC((Chicken.Cost)cost),
                        100
                    );
                this.AddChild(order);
                spawnOrders.Add(order);
                amount -= 1;
            }
            enemies.Remove(cost);
        }
        return spawnOrders;
    }
}

// END
