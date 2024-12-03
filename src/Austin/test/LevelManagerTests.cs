using Godot;
using GdUnit4;
using static GdUnit4.Assertions;

using System.Threading.Tasks;
using DifficultyCalculator;

namespace AustinsTests {
    
    [TestSuite]
    public partial class LevelManagerTests : Node {

        // Paths to important files
        private const string basePath = "res://src/Austin";
        private const string levelManagerScenePath = basePath + "/scenes/level_manager.tscn";
        private const string mapScenePath = basePath + "/scenes/map.tscn";

        private static LevelManager levelManager;
        [BeforeTest]
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
        public void Unit_setPlayerHealthBellowMin() {
            levelManager.level.playerHealth = -1;

            AssertThat(levelManager.level.playerHealth).IsGreaterEqual(0);
        }

        [TestCase]
        public void Unit_setPlayerMoneyBellowMin() {
            levelManager.level.PlayerMoney = -1;

            AssertThat(levelManager.level.PlayerMoney).IsGreaterEqual(0);
        }

        [TestCase]
        public void Unit_mapLoadedProperty() {
            AssertThat(levelManager.mapLoaded).IsEqual(true);
        }

        [TestCase]
        public void Unit_unlaodingMap() {
            AssertThat(levelManager.mapLoaded).OverrideFailureMessage("This test won't work becasue the Level.mapLoaded property doesn't work").IsEqual(true);

            levelManager.level.unloadMap();

            AssertThat(levelManager.mapLoaded).IsEqual(false);

            levelManager.level.loadMap();
            AssertThat(levelManager.mapLoaded).IsEqual(true);
        }

        [TestCase]
        public void Unit_loadingMap() {
            AssertThat(levelManager.mapLoaded).OverrideFailureMessage("This test won't work becasue the Level.mapLoaded property doesn't work").IsEqual(true);

            levelManager.level.unloadMap();
            AssertThat(levelManager.mapLoaded).OverrideFailureMessage("Unloading a map failed").IsEqual(false);

            levelManager.level.loadMap();
            AssertThat(levelManager.mapLoaded).IsEqual(true);
        }

        /*
        [TestCase]
        public void Unit_setNullMap() {
            levelManager.setMap(null);

            AssertThat(levelManager.level.mapScene).IsNotNull();
        }
        */

        [TestCase]
        public void Unit_loadLevelLoadsMap() {
            levelManager.level.unloadMap();
            AssertThat(levelManager.mapLoaded).OverrideFailureMessage("Couldn't unload the map").IsEqual(false);

            levelManager.OnLoadLevel();
            AssertThat(levelManager.mapLoaded).IsEqual(true);
        }

        [TestCase]
        public void Unit_defaultDifficutly() {
            AssertThat(levelManager.baseDifficulty).IsEqual(Difficulty.Easy);
        }

        [TestCase]
        public void Unit_easyDifficultyTableMaxRound() {
            levelManager.setDifficulty(Difficulty.Easy);

            AssertThat(levelManager.level.maxRound).IsEqual(100);
        }

        [TestCase]
        public void Unit_easyDifficultyTableEnemyRanks() {
            levelManager.setDifficulty(Difficulty.Easy);

            Godot.Collections.Array<int> easyRanks = new Godot.Collections.Array<int> {
                (int)Chicken.Cost.ChickenR1,
                (int)Chicken.Cost.ChickenR2,
                (int)Chicken.Cost.ChickenR3 };

            AssertThat(levelManager.level.difficultyTable.EnemyRanks).ContainsExactly(easyRanks);
        }

        [TestCase]
        public void Unit_easyDifficultyTableRoundDifficultyValue() {
            levelManager.setDifficulty(Difficulty.Easy);

            int[] easyRoundDifficulty = new int [levelManager.level.maxRound];
            easyRoundDifficulty[0] = 6;
            for (int i = 1; i < easyRoundDifficulty.Length; i++) {
                easyRoundDifficulty[i] = easyRoundDifficulty[i - 1] + 1;
            }

            AssertThat(levelManager.level.difficultyTable.RoundDifficultyValue).ContainsExactly(easyRoundDifficulty);
        }

        [TestCase]
        public void Unit_mediumDifficultyTableMaxRound() {
            levelManager.setDifficulty(Difficulty.Medium);

            AssertThat(levelManager.level.maxRound).IsEqual(100);
        }

        [TestCase]
        public void Unit_mediumDifficultyTableEnemyRanks() {
            levelManager.setDifficulty(Difficulty.Medium);

            Godot.Collections.Array<int> mediumRanks = new Godot.Collections.Array<int> {
                (int)Chicken.Cost.ChickenR1,
                (int)Chicken.Cost.ChickenR2,
                (int)Chicken.Cost.ChickenR3,
                (int)Chicken.Cost.ChickenR4 };

            AssertThat(levelManager.level.difficultyTable.EnemyRanks).ContainsExactly(mediumRanks);
        }

        [TestCase]
        public void Unit_mediumDifficultyTableRoundDifficultyValue() {
            levelManager.setDifficulty(Difficulty.Medium);

            int[] mediumRoundDifficulty = new int [levelManager.level.maxRound];
            mediumRoundDifficulty[0] = 8;
            for (int i = 1; i < mediumRoundDifficulty.Length; i++) {
                mediumRoundDifficulty[i] = mediumRoundDifficulty[i - 1] + 2;
            }

            AssertThat(levelManager.level.difficultyTable.RoundDifficultyValue).ContainsExactly(mediumRoundDifficulty);
        }

        [TestCase]
        public void Unit_hardDifficultyTableMaxRound() {
            levelManager.setDifficulty(Difficulty.Hard);

            AssertThat(levelManager.level.maxRound).IsEqual(100);
        }

        [TestCase]
        public void Unit_hardDifficultyTableEnemyRanks() {
            levelManager.setDifficulty(Difficulty.Hard);

            Godot.Collections.Array<int> hardRanks = new Godot.Collections.Array<int> {
                (int)Chicken.Cost.ChickenR1,
                (int)Chicken.Cost.ChickenR2,
                (int)Chicken.Cost.ChickenR3,
                (int)Chicken.Cost.ChickenR4 };

            AssertThat(levelManager.level.difficultyTable.EnemyRanks).ContainsExactly(hardRanks);
        }

        [TestCase]
        public void Unit_hardDifficultyTableRoundDifficultyValue() {
            levelManager.setDifficulty(Difficulty.Hard);

            int[] hardRoundDifficulty = new int [levelManager.level.maxRound];
            hardRoundDifficulty[0] = 15;
            for (int i = 1; i < hardRoundDifficulty.Length; i++) {
                hardRoundDifficulty[i] = hardRoundDifficulty[i - 1] + 3;
            }

            AssertThat(levelManager.level.difficultyTable.RoundDifficultyValue).ContainsExactly(hardRoundDifficulty);
        }

        [TestCase]
        public void Unit_difficultyTableLoadedByDefault() {
            ISceneRunner runner = ISceneRunner.Load("res://src/Austin/scenes/level_manager.tscn");
            LevelManager differentLManager = (LevelManager)runner.Scene();
            AssertThat(differentLManager.level.difficultyTable).IsNotNull();
        }

        [TestCase]
        public void Unit_setDifficultyEasy() {
            levelManager.setDifficulty(Difficulty.Easy);

            AssertThat(levelManager.baseDifficulty).IsEqual(Difficulty.Easy);
        }

        [TestCase]
        public void Unit_setDifficultyMedium() {
            levelManager.setDifficulty(Difficulty.Medium);

            AssertThat(levelManager.baseDifficulty).IsEqual(Difficulty.Medium);
        }

        [TestCase]
        public void Unit_setDifficultyHard() {
            levelManager.setDifficulty(Difficulty.Hard);

            AssertThat(levelManager.baseDifficulty).IsEqual(Difficulty.Hard);
        }

        [TestCase]
        public void Unit_setMapMeadows() {
            PackedScene meadows = GD.Load<PackedScene>("res://src/Austin/scenes/meadows.tscn");
            levelManager.setMap(AvailableMaps.Meadows);

            AssertThat(levelManager.level.mapScene).IsEqual(meadows);
        }

        [TestCase]
        public void Unit_setMapMultipath() {
            PackedScene multipath = GD.Load<PackedScene>("res://src/Austin/scenes/multipath_map.tscn");
            levelManager.setMap(AvailableMaps.Multipath);

            AssertThat(levelManager.level.mapScene).IsEqual(multipath);
        }

        [TestCase]
        public void Unit_setMapDefault() {
            PackedScene defaultMap = GD.Load<PackedScene>("res://src/Austin/scenes/map.tscn");
            levelManager.setMap(AvailableMaps.Default);

            AssertThat(levelManager.level.mapScene).IsEqual(defaultMap);
        }


        [TestCase(Timeout=60000)]
        public async Task Stress_multipleLevels() {
            const int numInitialLevels = 380;
            bool moreStress = true;
            int totalLevels = numInitialLevels;
            int levelsPerRounnd = 10;
            ISceneRunner runner = ISceneRunner.Load("res://src/Austin/test/level_manager_runner.tscn");
            
            //setup the scene runner
            runner.SetTimeFactor(50);
            runner.MaximizeView();

            runner.Invoke("makeLevel", numInitialLevels);

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
                    runner.Invoke("makeLevel", levelsPerRounnd);
                    totalLevels += levelsPerRounnd;
                }

            }

            GD.Print($"Number of Levels: {totalLevels}");

            runner.Invoke("freeLevels");
        }

        [AfterTest]
        public void endLevelMangerTests() {
            levelManager.level.unloadMap();
            levelManager.Free();
        }
    }
}
