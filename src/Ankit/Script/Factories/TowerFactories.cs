// File: src/Ankit/Scripts/Factories/TowerFactories.cs
using Godot;

public class Tower1Factory : TowerFactory
{
    public Tower1Factory()
    {
        towerScene = GD.Load<PackedScene>("res://src/Ankit/Scenes/Tower1.tscn");
        if (towerScene == null)
        {
            GD.PrintErr("Tower1Factory: Failed to load scene!");
        }
    }
}

public class Tower2Factory : TowerFactory
{
    public Tower2Factory()
    {
        towerScene = GD.Load<PackedScene>("res://src/Ankit/Scenes/Tower2.tscn");
        if (towerScene == null)
        {
            GD.PrintErr("Tower2Factory: Failed to load scene!");
        }
    }
}

public class Tower3Factory : TowerFactory
{
    public Tower3Factory()
    {
        towerScene = GD.Load<PackedScene>("res://src/Ankit/Scenes/Tower3.tscn");
        if (towerScene == null)
        {
            GD.PrintErr("Tower3Factory: Failed to load scene!");
        }
    }
}

public class Tower4Factory : TowerFactory
{
    public Tower4Factory()
    {
        towerScene = GD.Load<PackedScene>("res://src/Ankit/Scenes/Tower4.tscn");
        if (towerScene == null)
        {
            GD.PrintErr("Tower4Factory: Failed to load scene!");
        }
    }
}

public class Tower5Factory : TowerFactory
{
    public Tower5Factory()
    {
        towerScene = GD.Load<PackedScene>("res://src/Ankit/Scenes/Tower5.tscn");
        if (towerScene == null)
        {
            GD.PrintErr("Tower5Factory: Failed to load scene!");
        }
    }
}