using Godot;
using System;

public partial class MultiPath : Path
{
    [Export]
    private Path2D path;
    [Export]
    private Path2D path2;
    private bool pathChoice;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        pathChoice = true;
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
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
