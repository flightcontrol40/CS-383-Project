using Godot;
using System;

public partial class MultiPath : Path
{
    private Path2D path;
    private Path2D path2;
    private bool pathChoice = true;

    /// <summary>
    /// Called when the node enters the scene tree for the first time.
    /// Sets up the scene by getting the paths, and setting pathChoice.
    /// </summary>
    public override void _Ready()
    {
        path = GetNode<Path2D>("Path1");
        path2 = GetNode<Path2D>("Path2");
    }

    public override void _Process(double delta)
    {
        return;
    }

    /// <summary>
    /// Gets a path from the map in a round robin style
    /// </summary>
    /// <returns>One of the Path2D objects on the map</returns>
    public override Path2D getPath() {
        if (pathChoice) {
            pathChoice = false;
            return path;
        } else {
            pathChoice = true;
            return path2;
        }

    }
}
