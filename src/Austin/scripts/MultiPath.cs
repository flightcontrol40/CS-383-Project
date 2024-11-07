using Godot;
using System;

public partial class MultiPath : Path
{
    private Path2D path; // Storage for the first path
    private Path2D path2; // Storage for the second path
    private bool pathChoice; // Used to determine which path should get returned

    /// <summary>
    /// Called when the node enters the scene tree for the first time.
    /// Sets up the scene by getting the paths, and setting pathChoice.
    /// </summary>
    public override void _Ready()
    {
        pathChoice = true;
        path = GetNode<Path2D>("Path1");
        path2 = GetNode<Path2D>("Path2");
    }

    public override void _Process(double delta)
    {
        return;
    }

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
