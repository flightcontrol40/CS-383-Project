using Godot;
using System.Collections.Generic;
using Chicken;
using Microsoft.CodeAnalysis.CSharp.Syntax;
namespace RoundManager;

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
public partial class SpawnOrder: GodotObject{

    /// <summary>
    /// Creates A spawn Order for a passed type that implements IEnemyType
    /// </summary>
    /// <param name="enemyType"></param>
    /// <param name="spawnDelay"></param>
    public SpawnOrder(Chicken.BaseChicken enemyType, int spawnDelay) {
        this.spawnDelay = spawnDelay;
        this.Enemy = enemyType; //(BaseChicken)Activator.CreateInstance(enemyType);
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
    /// Get a new Difficulty Calculator class obj based on the Difficulty
    /// </summary>
    /// <param name="difficultyTable">
    /// DifficultyTable to be used by the calculator.
    /// </param>
    /// <param name="difficulty">
    /// The Difficulty to be used for the calculator
    /// </param>
    /// <returns>
    /// A Difficulty Calculator of the passed difficulty
    /// </returns>
    public static DifficultyCalculator CreateCalculator(DifficultyTable difficultyTable, Difficulty difficulty){
        switch (difficulty)
        {
            case Difficulty.Easy:
                return new EasyDifficultyCalculator(difficultyTable);
            case Difficulty.Medium:
                return new MediumDifficultyCalculator(difficultyTable);
            case Difficulty.Hard:
                return new HardDifficultyCalculator(difficultyTable);
            default:
                return new DifficultyCalculator(difficultyTable);
        }
    }

}

/// <summary>
/// The Base class for a difficulty calculator. Cannot construct directly, instead
/// use the <ref>DifficultyCalculatorFactory.CreateCalculator</ref> method.
/// </summary>
public partial class DifficultyCalculator {

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
/// <returns></returns>
    protected Godot.Collections.Array<int> getEnemyRanks(){
        Godot.Collections.Array<int> ranks = new Godot.Collections.Array<int>();
        foreach (int rank in this.difficultyTable.EnemyRanks){
            ranks.Add(rank);
        }
        return ranks;
    }
/// <summary>
/// Base Virtual function for Calculating the Spawn Order.
/// </summary>
/// <param name="roundNumber"></param>
/// <returns></returns>
    public virtual List<SpawnOrder> CalculateSpawnOrder(int roundNumber) {
        List<SpawnOrder> spawnOrders = new() { };
        Godot.Collections.Array<int> enemies = this.getEnemyRanks();
        int levelValue = this.difficultyTable.RoundDifficultyValue[roundNumber-1];
        while ( enemies.Count > 0 ){
            int cost = enemies.Max();
            int amount = getSpawnAmount(cost, ref levelValue);
            while ( amount > 0) {
                spawnOrders.Add(
                    new SpawnOrder(
                        Chicken.ChickenFactory.MakeKFC(cost),
                        100
                    )
                );
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

    internal EasyDifficultyCalculator(DifficultyTable difficultyTable) : base(difficultyTable) {
        this.difficultyTable = difficultyTable;
    }

    public override List<SpawnOrder> CalculateSpawnOrder(int roundNumber) {
        List<SpawnOrder> spawnOrders = new() { };
        Godot.Collections.Array<int> enemies = this.getEnemyRanks();
        // Lower Level Value on Easy
        int levelValue = this.difficultyTable.RoundDifficultyValue[roundNumber-1];
        levelValue = (int)((float) levelValue * 0.8);
        while ( enemies.Count > 0 ){
            int cost = enemies.Max();
            int amount = getSpawnAmount(cost, ref levelValue);
            while ( amount > 0) {
                spawnOrders.Add(
                    new SpawnOrder(
                        Chicken.ChickenFactory.MakeKFC(cost),
                        100
                    )
                );
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
    internal MediumDifficultyCalculator(DifficultyTable difficultyTable) : base(difficultyTable) {
        this.difficultyTable = difficultyTable;
    }
}

/// <summary>
/// Difficulty Calculator For the Hard Difficulty
/// </summary>
/// 
public partial class HardDifficultyCalculator : DifficultyCalculator {

    internal HardDifficultyCalculator(DifficultyTable difficultyTable) : base(difficultyTable) {
        this.difficultyTable = difficultyTable;
    }
    /// <summary>
    /// Calculates the SpawnOrders for Hard Difficulty
    /// </summary>
    /// <param name="difficultyTable"></param>
    /// <param name="roundNumber"></param>
    /// <returns></returns
    public override List<SpawnOrder> CalculateSpawnOrder( int roundNumber) {
        List<SpawnOrder> spawnOrders = new() { };
        Godot.Collections.Array<int> enemies = this.getEnemyRanks();
        // Raise Level Value on hard
        int levelValue = this.difficultyTable.RoundDifficultyValue[roundNumber-1];
        levelValue = (int)((float)levelValue * 1.5);
        while ( enemies.Count > 0 ){
            int cost = enemies.Max();
            int amount = getSpawnAmount(cost, ref levelValue);
            while ( amount > 0) {
                spawnOrders.Add(
                    new SpawnOrder(
                        Chicken.ChickenFactory.MakeKFC(cost),
                        100
                    )
                );
                amount -= 1;
            }
            enemies.Remove(cost);
        }
        return spawnOrders;
    }
}

// END
