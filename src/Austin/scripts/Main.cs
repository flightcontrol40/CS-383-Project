using DifficultyCalculator;
using Godot;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

public partial class Main : Node
{
    public bool PlacingTurret = false;
    private bool secondclick = false;
    public BaseTower newTower;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // This is necessary for now, until a level selection menu is FULLY implimeneted
        // To my (Austin's) knowledge it has been start, but not complete
        GetNode<LevelManager>("LevelManager").setDifficulty(Difficulty.Easy);
        GetNode<LevelManager>("LevelManager").OnLoadLevel();
        if (Input.IsActionJustPressed("loadLevel"))
        {
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if(PlacingTurret)
        {
            if (newTower == null)
            {
                PlacingTurret = false;
            }
            else
            {
                newTower.GlobalPosition = GetViewport().GetMousePosition();
            }
            if (Input.IsActionJustReleased("place_tower"))
            {
                if(secondclick == false)
                {
                    secondclick = true;
                }
                else
                {
                    secondclick = false;
                    PlacingTurret = false;
                }
            }

        }
    }

}
