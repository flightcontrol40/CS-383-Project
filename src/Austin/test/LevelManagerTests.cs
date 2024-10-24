using System;
using GdMUT;
using Godot;

public partial class LevelManagerTests : Node {
    //strings
    private static string baseScenePath = "res://src/Austin/scenes";
    private static string baseScriptPath = "res://src/Austin/scripts";
    private static string levelManagerScenePath = baseScenePath + "/level_manager.tscn";
    private static string mapScenePath = baseScenePath + "/map.tscn";
    private static string levelResourcePath = baseScriptPath + "/custom_resources/Level.cs";
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
        levelManagerInstance.level = levelResource;
    }

    [CSTestFunction]
    public static Result Unit_doubleLoadMap() {
        init();

        levelResource.loadMap();
        Map mapVal = levelResource.loadMap();

        Result retVal = (mapVal == null) ? new Result(true, "Loading a second map returned null") : new Result(false, "A second map was loaded");

        levelResource.unloadMap();

        return retVal;
    }

    [CSTestFunction]
    public static Result Unit_minRound() {
        init();

        levelManagerInstance.level.CurrentRoundNum = -100;
        int currentRoundNumber= levelManagerInstance.level.CurrentRoundNum;
        string resultMessage = "maxRound=" + currentRoundNumber.ToString();

        return currentRoundNumber >= 0 ? new Result(true, resultMessage) : new Result(false, resultMessage);
    }

    [CSTestFunction]
    public static Result Unit_maxRound() {
        init();

        levelManagerInstance.level.CurrentRoundNum = levelManagerInstance.level.maxRound + 1;
        int currentRoundNumber= levelManagerInstance.level.CurrentRoundNum;
        string resultMessage = "roundNumber=" + currentRoundNumber.ToString() + " <= maxRound=" + levelManagerInstance.level.maxRound;

        return currentRoundNumber <= levelManagerInstance.level.maxRound ? new Result(true, resultMessage) : new Result(false, resultMessage);
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
}