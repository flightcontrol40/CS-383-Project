using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Map : Node2D
{

    private List<Tower> towers;

    public override void _Ready()
    {
        GetNode<Area2D>("TowerZones").Monitoring = false;
        towers = new List<Tower>();
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

    public void addTower(Node2D tower) {
        //add tower to reference list
        towers.Append(tower);
        //bookkeeping
        AddChild(tower);
        tower.SetOwner(this);
    }

    public void removeTower(Node2D tower) {
        //remove tower from reference list
        towers.Remove((Tower)tower);
        //bookkeeping
        tower.QueueFree();
    }
}
