using Godot;
using System;
using Chicken;
using System.Threading.Tasks;

public partial class MultipathTest : Node2D
{
	public override void _Ready()
	{

		// Create a chicken instance using the factory
		BaseChicken chicken = ChickenFactory.MakeKFC(1);
		BaseChicken chicken2 = ChickenFactory.MakeKFC(1);
		//BaseChicken chicken = GD.Load<PackedScene>("res://src/Clayton/Enemy/Frank.tscn").Instantiate<Frank>();

		// Get the path for the chicken to follow
		Path2D path = GetNode<Path>("Map/Path").getPath();
		Path2D path2 = GetNode<Path>("Map/Path").getPath();

		// Start the chicken on the path
		chicken.Start(path);
		chicken2.Start(path2);
	}
}