
using Godot;
using GdUnit4;
using static GdUnit4.Assertions;

namespace AustinsTests {

    [TestSuite]
    public class MapTest {
        private Map map;
        private const string basePath = "res://src/Austin";
        private const string mapScenePath = basePath + "/scenes/map.tscn";

        [Before]
        public void initMapTests() {
            map = GD.Load<PackedScene>(mapScenePath).Instantiate<Map>();
        }

        [TestCase]
        //Requires multipath implimentation
        public void Unit_multiPathReturn() {
            Path path = map.GetNode<Path>("Path");

            Path2D[] pathReturns = new Path2D[2];
            pathReturns[0] = path.getPath();
            pathReturns[1] = path.getPath();

            AssertThat(pathReturns[0]).OverrideFailureMessage("This should fail for now, need to impliment a version of a map that uses multipath").IsNotSame(pathReturns[1]);

        }
    }

}

public class MapTests {

    public PackedScene MapScene;

    public MapTests() {
        MapScene = GD.Load<PackedScene>("res://src/Austin/scenes/map.tscn");
    }

    

}