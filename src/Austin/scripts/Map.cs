using Godot;

public partial class Map : Node2D
{
    public override void _Ready()
    {
        GetNode<Area2D>("TowerZones").Monitoring = false;
    }

    public override void _Process(double delta)
    {
        return;
    }

    /// <summary>
    /// Checks if a tower can be placed in a certain location
    /// </summary>
    /// <param name="tower">The tower to check the location of</param>
    /// <returns>True if the tower is in a valid location, false if not</returns>
    public bool validTowerLocation(Node2D tower) {
        // Get tower and and setup
        Area2D towerZone = GetNode<Area2D>("TowerZones");
        towerZone.Monitoring = true;

        // Check if tower's body overlaps the no zone
        bool canPlace = false;
        if (!towerZone.OverlapsBody(tower)) {
            canPlace = true;
        }

        towerZone.Monitoring = false;
        return canPlace;
    }
}
