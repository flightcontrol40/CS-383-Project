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
			int x = 2500;
			int y = chicken4.Health;
			AssertThat(y).IsEqual(x);
		}
		[TestCase]
		public void DamageCheck4() {
			int x = 250;
			int y = chicken4.damageAmount;
			AssertThat(y).IsEqual(x);
		}
		[TestCase]
		public void SpeedCheck4() {
			int x = 100;
			int y = chicken4.Speed;
			AssertThat(y).IsEqual(x);
		}
		[TestCase]
		public void RankCheck4() {
			int x = 4;
			int y = chicken4.EnemyRank;
			AssertThat(y).IsEqual(x);
		}		
		[TestCase]
		public void StartedCheck4() {
			bool check = false;
			bool chick = chicken4.started;
			AssertThat(chick).IsEqual(check);
		}		
		[TestCase]
		public void VisibleTest4() {
			chicken4._Ready();
			bool VCheck = false;
			bool VChick = chicken4.Visible;
			AssertThat(VChick).IsEqual(VCheck);			
		}
		[TestCase]
		public void DamageTest4(){
			int NHealth = chicken4.Health;
			chicken4.TakeDamage(50);
			AssertThat(chicken4.Health).IsEqual(NHealth-50);
		}
		[After]
		public void ChickenDeLoader4(){
			chicken4.Free();
		}
	}
}
