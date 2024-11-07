using Godot;
using System;

public partial class Path : Node2D
{
    private Path2D path; // Storage for the path

    /// <summary>
    /// Called when the node enters the scene tree for the first time
    /// </summary>
    public override void _Ready()
    {
        path = GetNode<Path2D>("Path2D");
    }

    /// <summary>
    /// Boilerplate function that comes with anything that inherits from a Godot Node.
    /// It does nothing.
    /// </summary>
    /// <param name="delta">Time since frame(equavalent to time between calls)</param>
    public override void _Process(double delta)
    {
        return;
    }

    /// <summary>
    /// Used to access the path member variable.
    /// </summary>
    /// <returns>A Path2D that this class stores</returns>
    public virtual Path2D getPath() {
        return path;
    }
}
