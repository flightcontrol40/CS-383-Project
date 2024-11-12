using DifficultyCalculator;
using Godot;
using System;

public partial class Main : Node
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        // This is necessary for now, until a level selection mmenu is FULLY implimeneted
        // To my (Austin's) knowledge it has been start, but not complete
        if (Input.IsActionJustPressed("loadLevel")) {
            GetNode<LevelManager>("LevelManager").setDifficulty(Difficulty.Easy);
            GetNode<LevelManager>("LevelManager").OnLoadLevel();
        }
    }
}
