using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using RoundManager.Interfaces;
using Chicken;
namespace RoundManager;


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
    public SpawnOrder(Type enemyType, int spawnDelay) {
        this.spawnDelay = spawnDelay;
        this.Enemy = (BaseChicken)Activator.CreateInstance(enemyType);
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
    public static DifficultyCalculator CreateCalculator(IDifficultyTable difficultyTable, Difficulty difficulty){
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
public partial class DifficultyCalculator : GodotObject {

    protected Dictionary<int, Type> enemyTypes;
    protected int[] RoundDifficultyValue;

    internal DifficultyCalculator(IDifficultyTable difficultyTable) {
        this.enemyTypes = difficultyTable.EnemyTypes;
        this.RoundDifficultyValue = difficultyTable.RoundDifficultyValue;
    }

    protected int getSpawnAmount( int cost, ref int levelValue ) {
        // Calculate the amount of enemies to spawn with the given cost for the
        // Given Level Value
        int amount =  (levelValue / cost) / 2;
        levelValue -= amount;
        return amount;
    }

    public virtual List<SpawnOrder> CalculateSpawnOrder(int roundNumber) {
        List<SpawnOrder> spawnOrders = new() { };
        int levelValue =this.RoundDifficultyValue[roundNumber];
        while ( enemyTypes.Count > 0 ){
            int cost = enemyTypes.Keys.Max();
            int amount = getSpawnAmount(cost, ref levelValue);
            while ( amount > 0) {
                spawnOrders.Add(new SpawnOrder(enemyTypes[cost], 100));
                amount -= 1;
            }
            enemyTypes.Remove(cost);
        }
        return spawnOrders;
    }
}


/// <summary>
/// Difficulty Calculator For the Easy Difficulty
/// </summary>
public partial class EasyDifficultyCalculator : DifficultyCalculator {
    internal EasyDifficultyCalculator(IDifficultyTable difficultyTable) : base(difficultyTable)
    {
        this.enemyTypes = difficultyTable.EnemyTypes;
        this.RoundDifficultyValue = difficultyTable.RoundDifficultyValue;
    }


    public override List<SpawnOrder> CalculateSpawnOrder(int roundNumber) {
        List<SpawnOrder> spawnOrders = new() { };
        int levelValue =this.RoundDifficultyValue[roundNumber];
        // Lower Level Value on Easy
        levelValue = (int)((float)levelValue * 0.8);
        while ( enemyTypes.Count > 0 ){
            int cost = enemyTypes.Keys.Max();
            int amount = getSpawnAmount(cost, ref levelValue);
            while ( amount > 0) {
                spawnOrders.Add(new SpawnOrder(enemyTypes[cost], 100));
            }
        }
        return spawnOrders;
    }
}


/// <summary>
/// Difficulty Calculator For the Medium Difficulty
/// </summary>
public partial class MediumDifficultyCalculator : DifficultyCalculator
{
    internal MediumDifficultyCalculator(IDifficultyTable difficultyTable) : base(difficultyTable)
    {
        this.enemyTypes = difficultyTable.EnemyTypes;
        this.RoundDifficultyValue = difficultyTable.RoundDifficultyValue;
    }
}

/// <summary>
/// Difficulty Calculator For the Hard Difficulty
/// </summary>
public partial class HardDifficultyCalculator : DifficultyCalculator {
    internal HardDifficultyCalculator(IDifficultyTable difficultyTable) : base(difficultyTable)
    {
        this.enemyTypes = difficultyTable.EnemyTypes;
        this.RoundDifficultyValue = difficultyTable.RoundDifficultyValue;
    }



    /// <summary>
    /// Calculates the SpawnOrders for Hard Difficulty
    /// </summary>
    /// <param name="difficultyTable"></param>
    /// <param name="roundNumber"></param>
    /// <returns></returns
    public override List<SpawnOrder> CalculateSpawnOrder( int roundNumber) {
        List<SpawnOrder> spawnOrders = new() { };
        int levelValue =this.RoundDifficultyValue[roundNumber];
        // Raise Level Value on Easy
        levelValue = (int)((float)levelValue * 1.5);
        while ( enemyTypes.Count > 0 ){
            int cost = enemyTypes.Keys.Max();
            int amount = getSpawnAmount(cost, ref levelValue);
            while ( amount > 0) {
                spawnOrders.Add(new SpawnOrder(enemyTypes[cost], 20));
            }
        }
        return spawnOrders;
    }
}

// END
