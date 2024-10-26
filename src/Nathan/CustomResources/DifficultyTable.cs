using Godot;
using System;
namespace RoundManager;

/// <summary>
/// A Resource defining the difficult table
/// </summary>
[GlobalClass]
public partial class DifficultyTable : Resource {

    /// <summary>
    // An array of the valid enemy ranks that can be used.
    /// </summary>
    [Export]
    public Godot.Collections.Array<int> EnemyRanks;

    /// <summary>
    /// An Array of the total amount of enemy "value" to spawn each round
    /// </summary>
    [Export]
    public int[] RoundDifficultyValue;
}

