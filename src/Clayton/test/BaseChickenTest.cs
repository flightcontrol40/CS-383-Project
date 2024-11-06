using Godot;
using System;
using GdUnit4;
using Chicken;
using static GdUnit4.Assertions;

namespace ClaytonTest {
	
	[TestSuite]
	public partial class BaseChickenTests : Node
	{
		
		public BaseChicken chicken;
		[Before] //load in chickens
		public void ChickenLoader() {
			//load the base chicken
			chicken = GD.Load<PackedScene>("res://src/Clayton/Enemy/BaseChicken.tscn").Instantiate<BaseChicken>();
			
		}
		
		[TestCase]
		public void HealthCheck() {
			int x = 100;
			int y = chicken.Health;
			AssertThat(y).IsEqual(x);
		}
		[TestCase]
		public void DamageCheck() {
			int x = 10;
			int y = chicken.damageAmount;
			AssertThat(y).IsEqual(x);
		}
		
		[TestCase]
		public void SpeedCheck() {
			int x = 150;
			int y = chicken.Speed;
			AssertThat(y).IsEqual(x);
		}
		
		
		
		[After]
		public void ChickenDeLoader(){
			chicken.Free();
			
		}
	
	}
}
