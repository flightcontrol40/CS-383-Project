
using Godot;
using GdUnit4;
using static GdUnit4.Assertions;

namespace AustinsTests {

    [TestSuite]
    public class MapTest {
        private Map map;
        private Map multipathMap;
        private Node sceneRoot;
        private Node2D testTower;
        private const string basePath = "res://src/Austin";
        private const string mapScenePath = basePath + "/scenes/map.tscn";
        private const string multipathMapScenePath = basePath + "/scenes/multipath_map.tscn";
        private const string testTowerScenePath = basePath + "/test/test_tower.tscn";
        private ISceneRunner mapTestRunner;

        [Before]
        public void initMapTests() {
            multipathMap = GD.Load<PackedScene>(multipathMapScenePath).Instantiate<Map>();
            map = GD.Load<PackedScene>(mapScenePath).Instantiate<Map>();
            testTower = GD.Load<PackedScene>(testTowerScenePath).Instantiate<Node2D>();

            mapTestRunner = ISceneRunner.Load("res://src/Austin/test/map_runner.tscn");
            sceneRoot = mapTestRunner.Scene();

            sceneRoot.AddChild(map);
            sceneRoot.AddChild(multipathMap);
            sceneRoot.AddChild(testTower);

            map.SetOwner(sceneRoot);
            multipathMap.SetOwner(sceneRoot);
            testTower.SetOwner(sceneRoot);
        }

        [TestCase]
        //Requires multipath implimentation
        public void Unit_multiPathReturn() {
            Path path = multipathMap.GetNode<Path>("Path");

            AssertThat(map.GetNode<Path>("Path").getPath()).IsNotNull();
        }

        [TestCase]
        public void Unit_startMonitoringFalse() {
            AssertThat(map.GetNode<Area2D>("TowerZones").Monitoring).Equals(false);
        }

        [TestCase]
        public void Unit_canPlaceTower() {
            testTower.SetGlobalPosition(new Vector2(64, 192));
            AssertThat(map.validTowerLocation(testTower)).Equals(true);
        }

        [TestCase]
        public void Unit_cannotPlacetower() {
            testTower.SetGlobalPosition(new Vector2(128, 128));
            AssertThat(map.validTowerLocation(testTower)).Equals(false);
        }

        [After]
        public void endMapTests() {
            map.QueueFree();
            multipathMap.QueueFree();
            testTower.QueueFree();
        }
    }

}
