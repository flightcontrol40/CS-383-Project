using Godot;
using GdUnit4;
using static GdUnit4.Assertions;

using System.Threading.Tasks;

namespace AustinsTests {
    
    [TestSuite]
    public partial class LevelManagerTests : Node {

        // Paths to important files
        private const string basePath = "res://src/Austin";
        private const string levelManagerScenePath = basePath + "/scenes/level_manager.tscn";
        private const string mapScenePath = basePath + "/scenes/map.tscn";

        private static LevelManager levelManager;
        [Before]
        public void initLevelManagerTests() {
            levelManager = GD.Load<PackedScene>(levelManagerScenePath).Instantiate<LevelManager>();

            levelManager.level = new Level();

            levelManager.level.mapScene = GD.Load<PackedScene>(mapScenePath);
            levelManager.level.loadMap();

            AutoFree(levelManager);
        }

        [TestCase]
        public void Unit_doubleLoadMap() {
            //Try to load another map
            Map retVal = levelManager.level.loadMap();

            //Check that the value is null
            AssertThat(retVal).IsEqual(null);
        }

        [TestCase]
        public void Unit_minRound() {
            levelManager.level.CurrentRoundNum = -1;

            AssertThat(levelManager.level.CurrentRoundNum).IsGreater(-1);
        }

        [TestCase]
        public void Unit_maxRound() {
            levelManager.level.CurrentRoundNum = levelManager.level.maxRound + 1;

            AssertThat(levelManager.level.CurrentRoundNum).IsLess(levelManager.level.maxRound + 1);
        }

        [TestCase]
        public void Unit_mapLoadedProperty() {
            AssertThat(levelManager.mapLoaded).IsEqual(true);
        }

        [TestCase]
        public void Unit_unlaodingAndLoadingMap() {
            AssertThat(levelManager.mapLoaded).OverrideFailureMessage("This test won't work becasue the Level.mapLoaded property doesn't work").IsEqual(true);

            levelManager.level.unloadMap();

            AssertThat(levelManager.mapLoaded).IsEqual(false);

            levelManager.level.loadMap();
            AssertThat(levelManager.mapLoaded).IsEqual(true);
        }

        [TestCase(Timeout = 60000)]
        public async Task Stress_multipleLevels() {
            const int numInitialLevels = 125;
            bool moreStress = true;
            int totalLevels = numInitialLevels;
            ISceneRunner runner = ISceneRunner.Load("res://src/Austin/test/test_scene.tscn");
            
            //setup the scene runner
            runner.SetTimeFactor(50);
            runner.MaximizeView();

            for (int i = 0; i < numInitialLevels; i++) {
                runner.Invoke("makeLevel");
            }

            while (moreStress) {
                runner.Invoke("runChicken");
                while (!(runner.GetProperty("ChickensAtEnd"))) {
                    await runner.SimulateFrames(1);
                }

                double currentFPS = Performance.GetMonitor(Performance.Monitor.TimeFps);
                AssertThat(currentFPS).OverrideFailureMessage($"Hit {currentFPS} fps at {totalLevels} levels").IsGreaterEqual(15);
                if (currentFPS < 15) {
                    moreStress = false;
                } else {
                    runner.Invoke("makeLevel");
                    totalLevels++;
                }

            }

            GD.Print($"Number of Levels: {totalLevels}");

            runner.Invoke("freeLevels");
        }

        [After]
        public void endLevelMangerTests() {
            levelManager.level.unloadMap();
            levelManager.Free();
        }
    }
}
/*
    [CSTestFunction] public static Result mapStress() {
        init();
        List<Map> mapInstances = new List<Map>();
        var watch = new System.Diagnostics.Stopwatch();
        System.Collections.Generic.Dictionary<int, long> results = new System.Collections.Generic.Dictionary<int, long> {};

        #pragma warning restore format

        for (int i = 0; i < 100000; i++) {
            watch.Start();
            if(i % 1000 == 0) {
                results.Add(i, watch.ElapsedMilliseconds);
                watch.Reset();
            }
            
            mapInstances.Append<Map>(levelResource.mapScene.Instantiate<Map>());
        }

        var asString = string.Join(System.Environment.NewLine, results);
        return new Result(true, $"Results: {asString}");
    }
*/
