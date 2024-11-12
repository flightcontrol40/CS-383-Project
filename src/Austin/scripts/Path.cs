using Godot;
using System;

public partial class Path : Node2D
{
    private Path2D path;

    public override void _Ready()
    {
        path = GetNode<Path2D>("Path2D");
    }

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
