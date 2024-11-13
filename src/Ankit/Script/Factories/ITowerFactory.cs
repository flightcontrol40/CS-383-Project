// File: src/Ankit/Scripts/Factories/ITowerFactory.cs
using Godot;

public abstract class TowerFactory
{
    protected PackedScene towerScene;
    
    // Changed from abstract to virtual with base implementation
    public virtual BaseTower CreateTower()
    {
        if (towerScene == null)
        {
            GD.PrintErr("Tower scene not loaded!");
            return null;
        }
        
        var tower = towerScene.Instantiate<BaseTower>();
        if (tower == null)
        {
            GD.PrintErr("Failed to instantiate tower!");
            return null;
        }
        
        return tower;
    }
}