using System;
using System.Collections.Generic;
using Godot;

namespace RoundManager.Interfaces;


/// <summary>
/// A Interface to represent a Maps difficulty table
/// </summary>
public interface IDifficultyTable
{

    /// <summary>
    // A Dictionary Mapping available enemies 'cost' to the enemy type.
    /// </summary>
    public Dictionary<int, Type> EnemyTypes { get; }

    /// <summary>
    /// An Array of the total amount of enemy "value" to spawn each round
    /// </summary>
    public int[] RoundDifficultyValue { get; }

    /// <summary>
    /// The max Round number for the level
    /// </summary>
    public int MaxRound { get; }

}

/// <summary>
/// Interface used by the RoundManager when loading, unloading, and saving
/// a level.
/// </summary>
public interface ILevelData
{

    /// <summary>
    /// The Remaining Health in the level
    /// </summary>
    public int Health { get; set; }
    /// <summary>
    /// The current round number for the level
    /// </summary>
    public int RoundNumber { get; set; }
    /// <summary>
    /// A getter for returning the levels path
    /// </summary>
    public Path2D LevelPath { get; }
    /// <summary>
    /// The levels Difficulty Table
    /// </summary>
    public IDifficultyTable DifficultyTable { get; }
}

