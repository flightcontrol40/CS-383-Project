using Godot;
using System;
using System.Collections.Generic;
using RoundManager;
using RoundManager.Interfaces;

/// <summary>
/// A Class to track the current status of the round.
/// </summary>
public partial class RoundStatusTracker : GodotObject {
    public RoundStatusTracker(IDifficultyTable difficultyTable, Difficulty difficulty) {
        this.difficulty = difficulty;
        this.calculator = DifficultyCalculatorFactory.CreateCalculator(
            difficultyTable,
            difficulty
        );
    }

    public bool roundStarted { get; set; }

    /// <summary>
    /// The difficulty calculator for the selected Difficulty.
    /// </summary>
    private DifficultyCalculator calculator {get; init;}

    /// <summary>
    /// Difficulty Enum for the given difficulty.
    /// </summary>
    public Difficulty difficulty { get; set; }

    /// <summary>
    /// Get the SpawnOrders for the current round and difficulty
    /// </summary>
    /// <param name="roundNumber"> The current round number. </param>
    /// <returns>
    /// List<SpawnOrder>: The list of enemies and times for spawning
    /// </returns>
    public List<SpawnOrder> getSpawnOrder(int roundNumber) {
        return calculator.CalculateSpawnOrder(roundNumber);
    }


}
