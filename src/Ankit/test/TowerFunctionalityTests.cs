using System;
using GdUnit4;
using GdUnit4.Api;
using Godot;
using Chicken;
using static GdUnit4.Assertions;

[TestSuite]
public partial class TowerFunctionalityTests : Node
{
    private PackedScene _towerScene;
    private Node _testRoot;

    


[Before]
public void SetUp()
{
    if (_testRoot != null)
    {
        _testRoot.QueueFree();
        _testRoot = null;
    }

    _testRoot = new Node();
    AddChild(_testRoot);
    
    _towerScene = GD.Load<PackedScene>("res://src/Ankit/Scenes/Tower1.tscn");
    AssertThat(_towerScene).IsNotNull();
}

[After]
public void TearDown()
{
    if (_testRoot != null)
    {
        RemoveChild(_testRoot);
        _testRoot.QueueFree();
        _testRoot = null;
    }
}

    [TestCase]
    public void TestTower1Specifications()
    {
        var tower = AutoFree(_towerScene.Instantiate<Tower1>());
        _testRoot.AddChild(tower);
        tower._Ready();

        AssertThat(tower.ShootingInterval).IsEqual(2.0f)
            .OverrideFailureMessage("Tower1 shooting interval should be 2.0f");
        AssertThat(tower.RotationSpeed).IsEqual(5.0f)
            .OverrideFailureMessage("Tower1 rotation speed should be 5.0f");
        AssertThat(tower.BulletsPerShot).IsEqual(1)
            .OverrideFailureMessage("Tower1 should shoot 1 bullet per shot");
        AssertThat(tower.BulletSpeed).IsEqual(300f)
            .OverrideFailureMessage("Tower1 bullet speed should be 300f");
    }

    [TestCase]
public void TestTower2Specifications()
{
    var tower2Scene = GD.Load<PackedScene>("res://src/Ankit/Scenes/Tower2.tscn");
    var tower = AutoFree(tower2Scene.Instantiate<Tower2>());
    _testRoot.AddChild(tower);
    tower._Ready();

    AssertThat(tower.ShootingInterval).IsEqual(1.0f)
        .OverrideFailureMessage("Tower2 shooting interval should be 1.0f");
    AssertThat(tower.RotationSpeed).IsEqual(7.0f)
        .OverrideFailureMessage("Tower2 rotation speed should be 7.0f");
    AssertThat(tower.BulletsPerShot).IsEqual(2)
        .OverrideFailureMessage("Tower2 should shoot 2 bullets per shot");
    AssertThat(tower.BulletSpeed).IsEqual(350f)
        .OverrideFailureMessage("Tower2 bullet speed should be 350f");
    AssertThat(tower.BulletDamage).IsEqual(8)
        .OverrideFailureMessage("Tower2 bullet damage should be 8");
}
    [TestCase]
    public void TestTower3Specifications()
    {
        var tower3Scene = GD.Load<PackedScene>("res://src/Ankit/Scenes/Tower3.tscn");
        var tower = AutoFree(tower3Scene.Instantiate<Tower3>());
        _testRoot.AddChild(tower);
        tower._Ready();

        AssertThat(tower.ShootingInterval).IsEqual(3.0f)
            .OverrideFailureMessage("Tower3 shooting interval should be 3.0f");
        AssertThat(tower.RotationSpeed).IsEqual(3.0f)
            .OverrideFailureMessage("Tower3 rotation speed should be 3.0f");
        AssertThat(tower.BulletsPerShot).IsEqual(3)
            .OverrideFailureMessage("Tower3 should shoot 3 bullets per shot");
        AssertThat(tower.BulletSpeed).IsEqual(250f)
            .OverrideFailureMessage("Tower3 bullet speed should be 250f");
    }


    [TestCase]
public void TestTower4Specifications()
{
    var tower4Scene = GD.Load<PackedScene>("res://src/Ankit/Scenes/Tower4.tscn");
    var tower = AutoFree(tower4Scene.Instantiate<Tower4>());
    _testRoot.AddChild(tower);
    tower._Ready();

    AssertThat(tower.ShootingInterval).IsEqual(2.5f)
        .OverrideFailureMessage("Tower4 shooting interval should be 2.5f");
    AssertThat(tower.RotationSpeed).IsEqual(6.0f)
        .OverrideFailureMessage("Tower4 rotation speed should be 6.0f");
    AssertThat(tower.BulletsPerShot).IsEqual(4)
        .OverrideFailureMessage("Tower4 should shoot 4 bullets per shot (quad shot)");
    AssertThat(tower.BulletSpeed).IsEqual(400f)
        .OverrideFailureMessage("Tower4 bullet speed should be 400f");
    AssertThat(tower.BulletDamage).IsEqual(12)
        .OverrideFailureMessage("Tower4 bullet damage should be 12");
}

[TestCase]
public void TestTower5Specifications()
{
    var tower5Scene = GD.Load<PackedScene>("res://src/Ankit/Scenes/Tower5.tscn");
    var tower = AutoFree(tower5Scene.Instantiate<Tower5>());
    _testRoot.AddChild(tower);
    tower._Ready();

    AssertThat(tower.ShootingInterval).IsEqual(1.5f)
        .OverrideFailureMessage("Tower5 shooting interval should be 1.5f");
    AssertThat(tower.RotationSpeed).IsEqual(8.0f)
        .OverrideFailureMessage("Tower5 rotation speed should be 8.0f (fastest rotation)");
    AssertThat(tower.BulletsPerShot).IsEqual(1)
        .OverrideFailureMessage("Tower5 should shoot 1 bullet per shot");
    AssertThat(tower.BulletSpeed).IsEqual(450f)
        .OverrideFailureMessage("Tower5 bullet speed should be 450f (fastest bullets)");
    AssertThat(tower.BulletDamage).IsEqual(10)
        .OverrideFailureMessage("Tower5 bullet damage should be 10");
}

[TestCase]
public void TestTower4QuadShot()
{
    var tower4Scene = GD.Load<PackedScene>("res://src/Ankit/Scenes/Tower4.tscn");
    var tower = AutoFree(tower4Scene.Instantiate<Tower4>());
    _testRoot.AddChild(tower);
    tower._Ready();

    // Create mock target
    var mockChicken = AutoFree(new BaseChicken());
    var mockArea = AutoFree(new Area2D());
    mockArea.Position = new Vector2(100, 0);
    mockChicken.Position = new Vector2(100, 0);
    _testRoot.AddChild(mockChicken);
    mockChicken.AddChild(mockArea);
    mockArea.SetMeta("parent", mockChicken);

    // Trigger target detection
    tower.OnBodyEntered(mockArea);

    // Process to allow rotation
    for (int i = 0; i < 10; i++)
    {
        tower._Process(0.1);
    }

    AssertThat(tower.BulletsPerShot).IsEqual(4)
        .OverrideFailureMessage("Tower4 should maintain 4 bullets per shot during operation");
}

[TestCase]
public void TestTower5Speed()
{
    var tower5Scene = GD.Load<PackedScene>("res://src/Ankit/Scenes/Tower5.tscn");
    AssertThat(tower5Scene).IsNotNull();
    
    var tower = AutoFree(tower5Scene.Instantiate<Tower5>());
    AssertThat(tower).IsNotNull();
    _testRoot.AddChild(tower);
    tower._Ready();

    // Verify towerHead exists
    var towerHead = tower.GetNode<AnimatedSprite2D>("Towerhead");
    AssertThat(towerHead).IsNotNull()
        .OverrideFailureMessage("Towerhead node not found");

    // Create mock target
    var mockChicken = AutoFree(new BaseChicken());
    var mockArea = AutoFree(new Area2D());
    _testRoot.AddChild(mockChicken);
    mockChicken.AddChild(mockArea);
    
    // Position target diagonally for guaranteed rotation
    mockChicken.Position = new Vector2(100, 100);
    mockArea.SetMeta("parent", mockChicken);

    float initialRotation = towerHead.Rotation;

    // Trigger target detection
    tower.OnBodyEntered(mockArea);

    // Process multiple frames to ensure rotation
    for (int i = 0; i < 3; i++)
    {
        tower._Process(0.016);
    }

    float rotationDifference = Mathf.Abs(towerHead.Rotation - initialRotation);
    AssertThat(rotationDifference).IsGreater(0.01f)
        .OverrideFailureMessage(
            $"Tower5 should rotate towards target.\n" +
            $"Initial rotation: {initialRotation}\n" +
            $"Final rotation: {towerHead.Rotation}\n" +
            $"Difference: {rotationDifference}"
        );
}

[TestCase]
public void TestTower4Targeting()
{
    var tower4Scene = GD.Load<PackedScene>("res://src/Ankit/Scenes/Tower4.tscn");
    var tower = AutoFree(tower4Scene.Instantiate<Tower4>());
    _testRoot.AddChild(tower);
    tower._Ready();

    // Test initial state
    AssertThat(tower.CurrentTarget).IsNull()
        .OverrideFailureMessage("Tower should start with no target");

    // Create and position target
    var mockChicken = AutoFree(new BaseChicken());
    var mockArea = AutoFree(new Area2D());
    mockArea.Position = new Vector2(100, 0);
    _testRoot.AddChild(mockChicken);
    mockChicken.AddChild(mockArea);
    mockArea.SetMeta("parent", mockChicken);

    // Test target acquisition
    tower.OnBodyEntered(mockArea);
    AssertThat(tower.CurrentTarget).IsNotNull()
        .OverrideFailureMessage("Tower should acquire target");
}

[TestCase]
public void TestTower5Targeting()
{
    var tower5Scene = GD.Load<PackedScene>("res://src/Ankit/Scenes/Tower5.tscn");
    var tower = AutoFree(tower5Scene.Instantiate<Tower5>());
    _testRoot.AddChild(tower);
    tower._Ready();

    // Test initial state
    AssertThat(tower.CurrentTarget).IsNull()
        .OverrideFailureMessage("Tower should start with no target");

    // Create and position target
    var mockChicken = AutoFree(new BaseChicken());
    var mockArea = AutoFree(new Area2D());
    mockArea.Position = new Vector2(100, 0);
    _testRoot.AddChild(mockChicken);
    mockChicken.AddChild(mockArea);
    mockArea.SetMeta("parent", mockChicken);

    // Test target acquisition
    tower.OnBodyEntered(mockArea);
    AssertThat(tower.CurrentTarget).IsNotNull()
        .OverrideFailureMessage("Tower should acquire target");
}



    [TestCase]
    public void TestTowerPlacementValidation()
    {
        var tower = AutoFree(_towerScene.Instantiate<Tower1>());
        _testRoot.AddChild(tower);
        tower._Ready();

        AssertThat(tower.CanBePlaced()).IsTrue()
            .OverrideFailureMessage("Tower should be placeable initially");

        var blockingArea = AutoFree(new Area2D());
        blockingArea.AddToGroup("BlockPlacement");
        _testRoot.AddChild(blockingArea);
        
        tower._on_placement_area_entered(blockingArea);

        AssertThat(tower.CanBePlaced()).IsFalse()
            .OverrideFailureMessage("Tower should not be placeable when blocked");
    }

    [TestCase]
public void TestTowerTargetDetection()
{
    // Create and set up tower
    var tower = AutoFree(_towerScene.Instantiate<Tower1>());
    AssertThat(tower).IsNotNull()
        .OverrideFailureMessage("Failed to instantiate tower");
    _testRoot.AddChild(tower);
    tower._Ready();

    // Verify components
    var towerHead = tower.GetNode<AnimatedSprite2D>("Towerhead");
    AssertThat(towerHead).IsNotNull()
        .OverrideFailureMessage("Towerhead node not found");

    // Create target at a diagonal position
    var mockChicken = AutoFree(new BaseChicken());
    var mockArea = AutoFree(new Area2D());
    _testRoot.AddChild(mockChicken);
    mockChicken.AddChild(mockArea);
    
    // Position diagonally to ensure rotation
    mockChicken.Position = new Vector2(100, 100);
    mockArea.SetMeta("parent", mockChicken);

    float initialRotation = towerHead.Rotation;

    // Trigger target detection
    tower.OnBodyEntered(mockArea);
    AssertThat(tower.CurrentTarget).IsNotNull()
        .OverrideFailureMessage("Tower failed to acquire target");

    // Process multiple frames
    for (int i = 0; i < 5; i++)
    {
        tower._Process(0.016);
    }

    float rotationDifference = Mathf.Abs(towerHead.Rotation - initialRotation);
    AssertThat(rotationDifference).IsGreater(0.01f)
        .OverrideFailureMessage(
            $"Tower should rotate towards target.\n" +
            $"Initial rotation: {initialRotation}\n" +
            $"Final rotation: {towerHead.Rotation}\n" +
            $"Difference: {rotationDifference}\n" +
            $"Target position: {mockChicken.Position}"
        );
}

    [TestCase]
    public void TestTowerTargetLoss()
    {
        var tower = AutoFree(_towerScene.Instantiate<Tower1>());
        _testRoot.AddChild(tower);
        tower._Ready();

        // Setup target
        var mockArea = AutoFree(new Area2D());
        var mockTarget = AutoFree(new Node2D());
        mockArea.AddChild(mockTarget);
        _testRoot.AddChild(mockArea);
        
        // Test target acquisition and loss
        tower.OnBodyEntered(mockArea);
        tower.OnBodyExited(mockArea);
        
        AssertThat(tower.CurrentTarget).IsNull()
            .OverrideFailureMessage("Target should be null after target loss");
    }
}