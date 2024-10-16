
using GdMUT;
using Godot;

namespace Austins_Test {
public class LevelManagerTests {
    //strings
    private static string baseScenePath = "res://src/Austin/scenes";
    private static string baseScriptPath = "res://src/Austin/scripts";
    private static string levelManagerScenePath = baseScenePath + "/level_manager.tscn";
    private static string mapScenePath = baseScenePath + "/map.tscn";
    private static string levelResourcePath = baseScriptPath + "/custom_resources/level.cs";
    //packed scenes
    private PackedScene levelManagerScene;
    private Level levelResource;
    //instances
    public LevelManager levelManagerInstance;

    public LevelManagerTests() {
        //Get the resources
        levelResource = new Level();
        levelResource.mapScene = GD.Load<PackedScene>(mapScenePath);

        //Get the scenes
        levelManagerScene = GD.Load<PackedScene>(levelManagerScenePath);

        //Instantiate the scenes
        levelManagerInstance = levelManagerScene.Instantiate<LevelManager>();
        levelManagerInstance.setLevel(levelResource);
    }

    public Result loadMap_test() {
        levelManagerInstance.loadMap();
        if (levelManagerInstance.isMapLoaded()) {
            return Result.Success;
        } else {
            return Result.Failure;
        }
    }

    public Result playerHealth_test() {
        //tk Add code to decrease the player's health
        levelManagerInstance.Health = -100;
        int currentHealth = levelManagerInstance.Health;
        string resultMessage = "PlayerHealth=" + currentHealth.ToString();
        return currentHealth >= 0 ? new Result(true, resultMessage) : new Result(false, resultMessage);
    }
}
}
