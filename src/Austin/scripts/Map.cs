using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Map : Node2D
{
    public override void _Ready()
    {
        GetNode<Area2D>("TowerZones").Monitoring = false;
    }

    public override void _Process(double delta)
    {
    }

    // Checks if a tower can be placed based on its area
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
