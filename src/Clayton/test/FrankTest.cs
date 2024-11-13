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
		[TestCase]
		public void RankCheck2() {
			int x = 2;
			int y = chicken2.EnemyRank;
			AssertThat(y).IsEqual(x);
		}
		[TestCase]
		public void StartedCheck2() {
			bool check = false;
			bool chick = chicken2.started;
			AssertThat(chick).IsEqual(check);
		}
		[TestCase]
		public void VisibleTest2() {
			chicken2._Ready();
			bool VCheck = false;
			bool VChick = chicken2.Visible;
			AssertThat(VChick).IsEqual(VCheck);			
		}		
		[TestCase]
		public void DamageTest2(){
			int NHealth = chicken2.Health;
			chicken2.TakeDamage(50);
			AssertThat(chicken2.Health).IsEqual(NHealth-50);
		}
		[After]
		public void ChickenDeLoader2(){
			chicken2.Free();
		}
	}
}
