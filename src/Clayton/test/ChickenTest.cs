using Godot;
using System;
using GdUnit4;
using Chicken;
using static GdUnit4.Assertions;

namespace ClaytonTest {
	
	[TestSuite]
	public partial class BaseChickenTests : Node
	{
		
		private static BaseChicken chicken;
		[Before] //load in chickens
	
		public void ChickenLoader() {
			//load the base chicken
			chicken = GD.Load<PackedScene>("res://src/Clayton/Enemy/BaseChicken.tscn").Instantiate<BaseChicken>();
			
		}
		
		[TestCase]
		public void HealthCheck() {
			//code here
			
		}
	
	}
	
	
	
	
	
	
	
}
