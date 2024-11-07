using Godot;
using System;
namespace RoundManager;

/// <summary>
/// A Resource defining the difficult table
/// </summary>
[GlobalClass]
public partial class DifficultyTable : Resource {

    [Export]
    /// <summary>
    // An array of the valid enemy ranks that can be used.
    /// </summary>
    public Godot.Collections.Array<int> EnemyRanks;

    [Export]
    /// <summary>
    /// An Array of the total amount of enemy "value" to spawn each round
    /// </summary>
    public int[] RoundDifficultyValue;
}

