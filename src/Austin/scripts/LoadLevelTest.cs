using Godot;
using System;
using RoundManager;

public partial class LoadLevelTest : Node2D
{
    LevelManager lm;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        lm = GetNode<LevelManager>("LevelManager");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("loadDefaultDifficulty")) {
            lm.setDifficutly(Difficulty.Medium);
        }
        if (Input.IsActionJustPressed("loadLevel")) {
            lm.OnLoadLevel();
        }
    }
    
    public void OnLoadRound(int difficulty, Level l) {
        GD.Print($"Recieved signal to start round on difficulty: {difficulty}");
    }
}
