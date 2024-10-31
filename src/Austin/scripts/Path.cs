using Godot;
using System;

public partial class Path : Node2D
{
    private Path2D path;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        path = GetNode<Path2D>("Path2D");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public virtual Path2D getPath() {
        return path;
    }
}
