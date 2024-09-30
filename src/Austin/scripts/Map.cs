using Godot;
using System;

public partial class Map : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Area2D>("TowreZones").Monitoring = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public bool validTowerLocation(Node2D tower) {
		Area2D towerZone = GetNode<Area2D>("TowerZones");
		towerZone.Monitoring = true;

		bool canPlace = false;
		if (!towerZone.OverlapsBody(tower)) {
			canPlace = true;
		}

		towerZone.Monitoring = false;
		return canPlace;
	}
}
