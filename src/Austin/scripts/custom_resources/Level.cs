/////////////////////////////////////////////////////////////
/// This is an implementation of the level data.
/// It contains all data the level needs when it is saved
/// 
/// author: Austin Walker
/// 
using Godot;
using RoundManager;

[GlobalClass]
public partial class Level : Resource
{
    private const string mapScenePath_m = "res://src/Austin/scenes/map.tscn";

    [Export]
    public Difficulty baseDifficulty { get; set; } = Difficulty.Easy;
    [Export]
    public DifficultyTable difficultyTable { get; set; }
    [Export]
    public int playerHealth { get; set; } = 100;
    [Export]
    public int playerMoney { get; set; } = 100;
    [Export]
    public int currentRoundNum { get; set; } = 0;
    [Export]
    public int maxRound { get; set; } = 1;
    [Export]
    public PackedScene mapScene { get; set; } = GD.Load<PackedScene>(mapScenePath_m);
    public Map mapInstance { get; set; }

    public Map loadMap() {
        GD.Print("loadMap()");
        if (!IsInstanceValid(mapInstance)) {
            mapInstance = mapScene.Instantiate<Map>();
            return mapInstance;
        } else {
            return null;
        }
    }

    public void unloadMap() {
        if (IsInstanceValid(mapInstance)) {
            mapInstance.QueueFree();
        }
    }

    public Path2D getPath(){
        return mapInstance.GetNode<Path>("Path").getPath();
    }
}
