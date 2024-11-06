using Godot;
using System;
using GdUnit4;
using Chicken;
using static GdUnit4.Assertions;

namespace ClaytonTest {
	
	[TestSuite]
	public partial class FrankestTests : Node
	{
		
		public BaseChicken chicken3;
		[Before] //load in chickens
		public void ChickenLoader3() {
			//load the Frankest chicken
			chicken3 = GD.Load<PackedScene>("res://src/Clayton/Enemy/Frankest.tscn").Instantiate<Frankest>();
		}
		
		[TestCase]
		public void HealthCheck3() {
			int x = 900;
			int y = chicken3.Health;
			AssertThat(y).IsEqual(x);
		}
		[TestCase]
		public void DamageCheck3() {
			int x = 90;
			int y = chicken3.damageAmount;
			AssertThat(y).IsEqual(x);
		}
		
		[TestCase]
		public void SpeedCheck3() {
			int x = 400;
			int y = chicken3.Speed;
			AssertThat(y).IsEqual(x);
		}
		
		
		
		[After]
		public void ChickenDeLoader3(){
			chicken3.Free();
			
		}
	
	}
}
