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
    [Export]
    public Difficulty baseDifficulty = Difficulty.Easy;

    [Export]
    public DifficultyTable difficultyTable;

    [Export]
    public int playerHealth = 100;

    [Export]
    public int playerMoney = 100;

    [Export]
    public int currentRoundNum = 0;

    [Export]
    public int MaxRound = 1;

    [Export]
    public PackedScene mapScene;
    public Map mapInstance;

    public Map loadMap() {
        if (!IsInstanceValid(mapInstance)) {
            GD.Print("Loaded map");
            mapInstance = mapScene.Instantiate<Map>();
            return mapInstance;
        } else {
            return null;
        }
    }

    public void unloadMap() {
        if (IsInstanceValid(mapInstance)) {
            GD.Print("Unloaded map");
            mapInstance.QueueFree();
        }
    }

    public Path2D getPath(){
        return mapInstance.GetNode<Path>("Path").getPath();
    }
}
