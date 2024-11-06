using Godot;
using System;
using GdUnit4;
using Chicken;
using static GdUnit4.Assertions;

namespace ClaytonTest {
	
	[TestSuite]
	public partial class FrankTests : Node
	{
		
		public BaseChicken chicken2;
		[Before] //load in chickens
		public void ChickenLoader2() {
			//load the Frank chicken
			chicken2 = GD.Load<PackedScene>("res://src/Clayton/Enemy/Frank.tscn").Instantiate<Frank>();
		}
		
		[TestCase]
		public void HealthCheck2() {
			int x = 300;
			int y = chicken2.Health;
			AssertThat(y).IsEqual(x);
		}
		[TestCase]
		public void DamageCheck2() {
			int x = 30;
			int y = chicken2.damageAmount;
			AssertThat(y).IsEqual(x);
		}
		
		[TestCase]
		public void SpeedCheck2() {
			int x = 200;
			int y = chicken2.Speed;
			AssertThat(y).IsEqual(x);
		}
		
		
		
		[After]
		public void ChickenDeLoader2(){
			chicken2.Free();
			
		}
	
	}
}
