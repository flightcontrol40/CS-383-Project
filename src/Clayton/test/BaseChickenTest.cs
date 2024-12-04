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
        [TestCase]
        public void RankCheck() {
            int x = 1;
            int y = chicken.EnemyRank;
            AssertThat(y).IsEqual(x);
        }        
        [TestCase]
        public void StartedCheck() {
            bool check = false;
            bool chick = chicken.started;
            AssertThat(chick).IsEqual(check);
        }
        [TestCase]
        public void VisibleTest() {
            chicken._Ready();
            bool VCheck = false;
            bool VChick = chicken .Visible;
            AssertThat(VChick).IsEqual(VCheck);
        }
        [TestCase]
        public void DamageTest(){
            int NHealth = chicken.Health;
            chicken.TakeDamage(50);
            AssertThat(chicken.Health).IsEqual(NHealth-50);
        }
        [After]
        public void ChickenDeLoader(){
            chicken.Free();
        }
    }
}
