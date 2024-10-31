using Godot;
using System;
using Chicken;
using System.Threading.Tasks;

public partial class ChickenWalkTest : Node2D
{
	public override void _Ready()
	{

		// Create a chicken instance using the factory
		BaseChicken chicken = ChickenFactory.MakeKFC(19);
		//BaseChicken chicken = GD.Load<PackedScene>("res://src/Clayton/Enemy/Frank.tscn").Instantiate<Frank>();

		// Get the path for the chicken to follow
		Path2D path = GetNode<Path2D>("Map/Path/Path2D");

		// Start the chicken on the path
		chicken.Start(path);
		TimerCalc(chicken);
	}

	public async Task TimerCalc(BaseChicken chicken){
		for(int i = 0; i < 26; i++){
			await ToSignal(GetTree().CreateTimer(0.1f), "timeout");
			chicken.TakeDamage(100);
		}
	}
}
