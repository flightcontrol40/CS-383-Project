using Chicken;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;

public partial class TestScene : Node2D
{
    private PackedScene mapScene;
    private List<Level> levels;
    private PackedScene chickenScene;
    public bool ChickensAtEnd { get; private set;} = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        mapScene = GD.Load<PackedScene>("res://src/Austin/scenes/map.tscn");
        chickenScene = GD.Load<PackedScene>("res://src/Clayton/Enemy/BaseChicken.tscn");

        levels = new List<Level>();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        return;
    }

    public void makeLevel(int numLevels) {
        for (int i = 0; i < numLevels; i++) {
            Level newLevel = new Level();
            newLevel.mapScene = GD.Load<PackedScene>("res://src/Austin/scenes/map.tscn");
            levels.Add(newLevel);

            Map newMap = newLevel.loadMap();
            AddChild(newMap);
        }
    }

    public void freeLevels() {
        foreach (Level level in levels) {
            level.Free();
        }
    }

    public void runChicken() {
        ChickensAtEnd = false;
        List<BaseChicken> chickens = new List<BaseChicken>();

        foreach (Level level in levels) {
            BaseChicken newChicken = ChickenFactory.MakeKFC(17);
            chickens.Add(newChicken);

            newChicken.Start(level.MapInstance.GetNode<Path>("Path").getPath());
            newChicken.EndOfPath += chickenAtEnd;
        }
    }

    public void chickenAtEnd(BaseChicken enemy) {
        ChickensAtEnd = true;
        enemy.QueueFree();
    }

}
