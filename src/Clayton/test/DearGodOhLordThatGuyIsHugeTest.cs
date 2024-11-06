using Godot;
using System;
using GdUnit4;
using Chicken;
using static GdUnit4.Assertions;

namespace ClaytonTest {
	
	[TestSuite]
	public partial class DearGodOhLordThatGuyIsHugeTests : Node
	{
		
		public BaseChicken chicken4;
		[Before] //load in chickens
		public void ChickenLoader4() {
			//load the Frankest chicken
			chicken4 = GD.Load<PackedScene>("res://src/Clayton/Enemy/DearGodOhLordThatGuyIsHuge.tscn").Instantiate<DearGodOhLordThatGuyIsHuge>();
		}
		
		[TestCase]
		public void HealthCheck4() {
			int x = 900;
			int y = chicken4.Health;
			AssertThat(y).IsEqual(x);
		}
		[TestCase]
		public void DamageCheck4() {
			int x = 90;
			int y = chicken4.damageAmount;
			AssertThat(y).IsEqual(x);
		}
		
		[TestCase]
		public void SpeedCheck4() {
			int x = 400;
			int y = chicken4.Speed;
			AssertThat(y).IsEqual(x);
		}
		
		
		
		[After]
		public void ChickenDeLoader4(){
			chicken4.Free();
			
		}
	
	}
}
