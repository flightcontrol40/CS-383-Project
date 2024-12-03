using Godot;
using System;
using RoundManager;
using DifficultyCalculator;

public partial class LevelSelector : Control
{
    [Export]
    private ButtonGroup difficutlySelection;
    [Export]
    private ButtonGroup mapSelection;
    private Difficulty currentDifficulty = Difficulty.Easy;
    private AvailableMaps currentMap = AvailableMaps.Default;
    private LevelManager lm;
    public override void _Ready()
    {
        lm = GetNode<LevelManager>("LevelManager");
    }

    public override void _Process(double delta)
    {
    }

    public void OnStartPressed() {
        GD.Print("1");
        lm.setDifficulty(currentDifficulty);
        GD.Print("2");
        lm.setMap(currentMap);
        GD.Print("3");
        lm.OnLoadLevel();
        GD.Print("4");
        this.Visible = false;
    }

    public void OnEasyDifficultyPressed() {
        GD.Print("Easy selected");
        currentDifficulty = Difficulty.Easy;
    }
    public void OnMediumDifficultyPressed() {
        GD.Print("Medium selected");
        currentDifficulty = Difficulty.Medium;
    }
    public void OnHardDifficultyPressed() {
        GD.Print("Hard selected");
        currentDifficulty = Difficulty.Hard;
    }
    public void OnDefaultMapPressed() {
        GD.Print("Default map selected");
        currentMap = AvailableMaps.Default;
    }
    public void OnMultipathMapPressed() {
        GD.Print("Multipath map selected");
        currentMap = AvailableMaps.Multipath;
    }
    public void OnMeadowsMapPressed() {
        GD.Print("Meadows map selected");
        currentMap = AvailableMaps.Meadows;
    }

}
