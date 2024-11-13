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
			int x = 250;
			int y = chicken3.Speed;
			AssertThat(y).IsEqual(x);
		}
		[TestCase]
		public void RankCheck3() {
			int x = 3;
			int y = chicken3.EnemyRank;
			AssertThat(y).IsEqual(x);
		}		
		[TestCase]
		public void StartedCheck3() {
			bool check = false;
			bool chick = chicken3.started;
			AssertThat(chick).IsEqual(check);
		}
		[TestCase]
		public void VisibleTest3() {
			chicken3._Ready();
			bool VCheck = false;
			bool VChick = chicken3.Visible;
			AssertThat(VChick).IsEqual(VCheck);			
		}
		[TestCase]
		public void DamageTest3(){
			int NHealth = chicken3.Health;
			chicken3.TakeDamage(50);
			AssertThat(chicken3.Health).IsEqual(NHealth-50);
		}
		[After]
		public void ChickenDeLoader3(){
			chicken3.Free();
		}
	}
}
