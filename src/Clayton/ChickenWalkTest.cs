using Godot;
using System;
using Chicken;

public partial class ChickenWalkTest : Node2D
{
	public override void _Ready()
	{
		// Example cost value to decide which chicken to create
		int cost = 1;

		// Create a chicken instance using the factory
		BaseChicken chicken = GD.Load<PackedScene>("res://src/Clayton/Enemy/BaseChicken.tscn").Instantiate<BaseChicken>();

		// Add the chicken to the scene tree
		//AddChild(chicken);

		// Get the path for the chicken to follow
		Path2D path = GetNode<Path2D>("Map/Path/Path2D");

		// Start the chicken on the path
		chicken.Start(path);
	}
}
