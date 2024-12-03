using Godot;
using System;
using RoundManager;
using DifficultyCalculator;

public partial class LevelSelector : Control
{
    [Export]
    ButtonGroup difficutlySelection;

    [Export]
    ButtonGroup mapSelection;
    Difficulty currentDifficulty = Difficulty.Easy;
    AvailableMaps currentMap = AvailableMaps.Default;

    [Signal]
    public delegate void setDifficultyEventHandler(int difficulty);
    [Signal]
    public delegate void setMapEventHandler(int map);

    public override void _Ready()
    {
        return;
    }

    public override void _Process(double delta)
    {
    }

    public void OnStartPressed() {
        EmitSignal(SignalName.setDifficulty, (int)currentDifficulty);
        EmitSignal(SignalName.setMap, (int)currentMap);
        this.Visible = false;
    }

    public void OnEasyDifficultyPressed() {
        currentDifficulty = Difficulty.Easy;
    }
    public void OnMediumDifficultyPressed() {
        currentDifficulty = Difficulty.Medium;
    }
    public void OnHardDifficultyPressed() {
        currentDifficulty = Difficulty.Hard;
    }
    public void OnDefaultMapPressed() {
        currentMap = AvailableMaps.Default;
    }
    public void OnMultipathMapPressed() {
        currentMap = AvailableMaps.Multipath;
    }
    public void OnMeadowsMapPressed() {
        currentMap = AvailableMaps.Meadows;
    }

}
