using DifficultyCalculator;
using Godot;
using RoundManager;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

public partial class Main : Node
{
    public bool PlacingTurret = false;
    private bool secondclick = false;
    public BaseTower newTower;

    private RoundManager.RoundManager roundm;
    private LevelManager levelm;
    private Button StartRoundButton;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        levelm = GetNode<LevelManager>("LevelManager");
        levelm.setDifficulty(Difficulty.Easy);
        levelm.OnLoadLevel();

        roundm = GetNode<RoundManager.RoundManager>("RoundManager");
        roundm.loadLevel(levelm.level, (int)levelm.baseDifficulty);

        StartRoundButton = GetNode<Button>("Shop/Shop Panel/StartRoundButton");
        StartRoundButton.Pressed += () => { GD.Print("Round Starting"); roundm.startRound(); };
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
