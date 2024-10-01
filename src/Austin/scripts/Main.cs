using Godot;
using System;

public partial class Main : Node
{
	[Export]
	private PackedScene tower;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("place_tower")) {
			placeTower();
		}
	}

	public void placeTower(Vector2 spot) {
		TowerController newTower = (TowerController)tower.Instantiate();
		newTower.GlobalPosition = spot;
	}
}
