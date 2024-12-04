using Godot;
using System;
using RoundManager;
using DifficultyCalculator;

public partial class LevelSelector : Control
{
    private Difficulty currentDifficulty = Difficulty.Easy;
    private AvailableMaps currentMap = AvailableMaps.Default;
    public override void _Ready()
    {
        return;
    }

    public override void _Process(double delta)
    {
        return;
    }

    public void OnStartPressed() {
        LevelManager lm = GetNode<LevelManager>("../LevelManager");
        lm.setDifficulty(currentDifficulty);
        lm.setMap(currentMap);
        lm.OnLoadLevel();
        Visible = false;
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
