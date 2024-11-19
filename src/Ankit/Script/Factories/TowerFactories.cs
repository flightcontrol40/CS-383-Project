// File: src/Ankit/Scripts/Factories/TowerFactories.cs
using Godot;

// Factory class for creating Tower1 instances from a specific scene.
public class Tower1Factory : TowerFactory
{
    public Tower1Factory()
    {
        // Loads the Tower1 scene and assigns it to towerScene.
        towerScene = GD.Load<PackedScene>("res://src/Ankit/Scenes/Tower1.tscn");
        if (towerScene == null)
        {
            GD.PrintErr("Tower1Factory: Failed to load scene!");
        }
    }
}

// Factory class for creating Tower2 instances from a specific scene.
public class Tower2Factory : TowerFactory
{
    public Tower2Factory()
    {
        // Loads the Tower2 scene and assigns it to towerScene.
        towerScene = GD.Load<PackedScene>("res://src/Ankit/Scenes/Tower2.tscn");
        if (towerScene == null)
        {
            GD.PrintErr("Tower2Factory: Failed to load scene!");
        }
    }
}

// Factory class for creating Tower3 instances from a specific scene.
public class Tower3Factory : TowerFactory
{
    public Tower3Factory()
    {
        // Loads the Tower3 scene and assigns it to towerScene.
        towerScene = GD.Load<PackedScene>("res://src/Ankit/Scenes/Tower3.tscn");
        if (towerScene == null)
        {
            GD.PrintErr("Tower3Factory: Failed to load scene!");
        }
    }
}

// Factory class for creating Tower4 instances from a specific scene.
public class Tower4Factory : TowerFactory
{
    public Tower4Factory()
    {
        // Loads the Tower4 scene and assigns it to towerScene.
        towerScene = GD.Load<PackedScene>("res://src/Ankit/Scenes/Tower4.tscn");
        if (towerScene == null)
        {
            GD.PrintErr("Tower4Factory: Failed to load scene!");
        }
    }
}

// Factory class for creating Tower5 instances from a specific scene.
public class Tower5Factory : TowerFactory
{
    public Tower5Factory()
    {
        // Loads the Tower5 scene and assigns it to towerScene.
        towerScene = GD.Load<PackedScene>("res://src/Ankit/Scenes/Tower5.tscn");
        if (towerScene == null)
        {
            GD.PrintErr("Tower5Factory: Failed to load scene!");
        }
    }
}
