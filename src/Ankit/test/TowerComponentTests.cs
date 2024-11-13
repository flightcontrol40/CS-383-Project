using GdUnit4;
using Godot;
using static GdUnit4.Assertions;

[TestSuite]
public class TowerComponentTests
{
    private PackedScene _towerScene;

    [Before]
    public void Setup()
    {
        _towerScene = GD.Load<PackedScene>("res://src/Ankit/Scenes/Tower1.tscn");
    }

    [TestCase]
    public void TestTowerSceneStructure()
    {
        var tower = AutoFree(_towerScene.Instantiate<Tower1>());
        
        AssertThat(tower).IsNotNull();
        AssertThat(tower.GetNode<Sprite2D>("Sprite2D")).IsNotNull()
            .OverrideFailureMessage("Sprite2D node missing");
        AssertThat(tower.GetNode<AnimatedSprite2D>("Towerbody")).IsNotNull()
            .OverrideFailureMessage("Towerbody node missing");
        AssertThat(tower.GetNode<AnimatedSprite2D>("Towerhead")).IsNotNull()
            .OverrideFailureMessage("Towerhead node missing");
    }

    [TestCase]
    public void TestTowerHeadComponents()
    {
        var tower = AutoFree(_towerScene.Instantiate<Tower1>());
        tower._Ready();

        var towerHead = tower.GetNode<AnimatedSprite2D>("Towerhead");
        AssertThat(towerHead).IsNotNull()
            .OverrideFailureMessage("Towerhead node is missing");

        var bulletSpawnPoint = towerHead.GetNode<Marker2D>("BulletSpawnPoint");
        AssertThat(bulletSpawnPoint).IsNotNull()
            .OverrideFailureMessage("BulletSpawnPoint node is missing");
        AssertThat(towerHead.Position).IsEqual(new Vector2(0, -24))
            .OverrideFailureMessage("Towerhead position is incorrect");
    }

    [TestCase]
    public void TestTowerSightArea()
    {
        var tower = AutoFree(_towerScene.Instantiate<Tower1>());
        tower._Ready();

        var sight = tower.GetNode<Area2D>("Sight");
        AssertThat(sight).IsNotNull()
            .OverrideFailureMessage("Sight node is missing");
        AssertThat(sight.IsInGroup("Sight")).IsTrue()
            .OverrideFailureMessage("Sight node should be in 'Sight' group");
        AssertThat(sight.GetNode<CollisionShape2D>("CollisionShape2D")).IsNotNull()
            .OverrideFailureMessage("CollisionShape2D missing in Sight node");
    }

    [TestCase]
    public void TestTowerPlacementArea()
    {
        var tower = AutoFree(_towerScene.Instantiate<Tower1>());
        tower._Ready();

        var placement = tower.GetNode<Area2D>("Placement");
        AssertThat(placement).IsNotNull()
            .OverrideFailureMessage("Placement area node is missing");

        var collision = placement.GetNode<CollisionShape2D>("CollisionShape2D");
        AssertThat(collision).IsNotNull()
            .OverrideFailureMessage("CollisionShape2D node is missing in Placement");
        AssertThat(collision.Position).IsEqual(new Vector2(0, -3.5f))
            .OverrideFailureMessage("CollisionShape2D position is incorrect");
    }

    [TestCase]
public void TestTowerTimerSetup()
{
    var tower = AutoFree(_towerScene.Instantiate<Tower1>());
    tower._Ready();

    var timer = tower.GetNode<Timer>("Timer");
    AssertThat(timer).IsNotNull()
        .OverrideFailureMessage("Timer node is missing");
    AssertThat(timer.WaitTime).IsEqual(2.0f)
        .OverrideFailureMessage("Timer wait time should be 2.0");
    AssertThat(timer.OneShot).IsFalse()
        .OverrideFailureMessage("Timer should not be one shot");
    AssertThat(timer.ProcessMode).IsEqual(Node.ProcessModeEnum.Pausable)  // Changed to match your scene
        .OverrideFailureMessage("Timer process mode should be Pausable");
}

    [TestCase]
    public void TestTowerAnimationSetup()
    {
        var tower = AutoFree(_towerScene.Instantiate<Tower1>());
        tower._Ready();

        var body = tower.GetNode<AnimatedSprite2D>("Towerbody");
        var head = tower.GetNode<AnimatedSprite2D>("Towerhead");

        AssertThat(body.SpriteFrames).IsNotNull()
            .OverrideFailureMessage("Towerbody sprite frames missing");
        AssertThat(head.SpriteFrames).IsNotNull()
            .OverrideFailureMessage("Towerhead sprite frames missing");
        AssertThat(body.TextureFilter).IsEqual(CanvasItem.TextureFilterEnum.Nearest)
            .OverrideFailureMessage("Texture filter incorrect");
    }

    [TestCase]
    public void TestTowerInitialState()
    {
        var tower = AutoFree(_towerScene.Instantiate<Tower1>());
        tower._Ready();
        
        AssertThat(tower.CanBePlaced()).IsTrue()
            .OverrideFailureMessage("Tower should be placeable initially");
        AssertThat(tower.CurrentTarget).IsNull()
            .OverrideFailureMessage("CurrentTarget should be null at initialization");
        
        var sightArea = tower.GetNode<Area2D>("Sight");
        AssertThat(sightArea).IsNotNull()
            .OverrideFailureMessage("Sight area node is missing");
    }

    [TestCase]
    public void TestTowerSceneValidation()
    {
        var tower = AutoFree(_towerScene.Instantiate<Tower1>());
        tower._Ready();

        var bulletSpawnPoint = tower.GetNode<Marker2D>("Towerhead/BulletSpawnPoint");
        AssertThat(bulletSpawnPoint).IsNotNull()
            .OverrideFailureMessage("BulletSpawnPoint missing");
        AssertThat(bulletSpawnPoint.Position).IsNotNull()
            .OverrideFailureMessage("BulletSpawnPoint position not set");

        var timer = tower.GetNode<Timer>("Timer");
        AssertThat(timer).IsNotNull()
            .OverrideFailureMessage("Timer node missing");
    }
}