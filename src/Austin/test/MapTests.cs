
using Godot;
using GdUnit4;
using static GdUnit4.Assertions;

namespace AustinsTests {

    [TestSuite]
    public class MapTest {
        private Map map;
        private Map multipathMap;
        private const string basePath = "res://src/Austin";
        private const string mapScenePath = basePath + "/scenes/map.tscn";
        private const string multipathMapScenePath = basePath + "/scenes/multipath_map.tscn";

        [Before]
        public void initMapTests() {
            map = GD.Load<PackedScene>(mapScenePath).Instantiate<Map>();
            multipathMap = GD.Load<PackedScene>(multipathMapScenePath).Instantiate<Map>();
        }

        [TestCase]
        //Requires multipath implimentation
        public void Unit_multiPathReturn() {
            Path path = multipathMap.GetNode<Path>("Path");

            AssertThat(map.GetNode<Path>("Path").getPath()).IsNotNull();
        }

        [After]
        public void endMapTests() {
            map.Free();
            multipathMap.Free();
        }
    }

}
