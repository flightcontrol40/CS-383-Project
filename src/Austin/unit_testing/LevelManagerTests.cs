
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Runtime.CompilerServices;
using GdMUT;
using Godot;

public partial class LevelManagerTests : Node {
    //strings
    private static string baseScenePath = "res://src/Austin/scenes";
    private static string baseScriptPath = "res://src/Austin/scripts";
    private static string levelManagerScenePath = baseScenePath + "/level_manager.tscn";
    private static string mapScenePath = baseScenePath + "/map.tscn";
    private static string levelResourcePath = baseScriptPath + "/custom_resources/level.cs";
    //packed scenes
    private static PackedScene levelManagerScene;
    private static Level levelResource;
    //instances
    public static LevelManager levelManagerInstance;

    public static void init() {
        //Get the resources
        levelResource = new Level();
        levelResource.mapScene = GD.Load<PackedScene>(mapScenePath);

        //Get the scenes
        levelManagerScene = GD.Load<PackedScene>(levelManagerScenePath);

        //Instantiate the scenes
        levelManagerInstance = levelManagerScene.Instantiate<LevelManager>();
        levelManagerInstance.setLevel(levelResource);
    }

    [CSTestFunction] public static Result loadMap() {
        init();

        levelManagerInstance.loadMap();
        if (levelManagerInstance.isMapLoaded()) {
            return Result.Success;
        } else {
            return Result.Failure;
        }
    }

	[CSTestFunction] public static Result minRound() {
        init();

        //tk Add code to decrease the player's health
        levelManagerInstance.RoundNumber = -100;
        int currentRoundNumber= levelManagerInstance.Health;
        string resultMessage = "PlayerHealth=" + currentRoundNumber.ToString();
        return currentRoundNumber >= 0 ? new Result(true, resultMessage) : new Result(false, resultMessage);
    }

    
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
}
