// File: src/Ankit/Scripts/Factories/ITowerFactory.cs
using Godot;

// Abstract factory class for creating tower instances.
public abstract class TowerFactory
{
    // Scene resource for the tower, used to instantiate new tower instances.
    protected PackedScene towerScene;
    
    // Virtual method for creating a new tower instance from the tower scene.
    // Returns a BaseTower object or null if instantiation fails.
    public virtual BaseTower CreateTower()
    {
        // Checks if the tower scene is loaded; logs an error if not.
        if (towerScene == null)
        {
            GD.PrintErr("Tower scene not loaded!");
            return null;
        }
        
        // Instantiates a new tower from the tower scene.
        var tower = towerScene.Instantiate<BaseTower>();
        if (tower == null)
        {
            GD.PrintErr("Failed to instantiate tower!");
            return null;
        }
        
        // Returns the created tower instance.
        return tower;
    }
}
