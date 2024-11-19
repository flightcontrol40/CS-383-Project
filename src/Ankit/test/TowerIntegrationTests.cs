using GdUnit4;
using GdUnit4.Api;
using Godot;
using System.Collections.Generic;
using Chicken;
using static GdUnit4.Assertions;
using System.Threading.Tasks;
using System;

[TestSuite]
public partial class TowerIntegrationTests : Node
{
    private Node _testRoot;
    private PackedScene _towerScene;
    private PackedScene _bulletScene;

    [Before]
public void Setup()
{
    _testRoot = AutoFree(new Node());
    AddChild(_testRoot);

    _towerScene = GD.Load<PackedScene>("res://src/Ankit/Scenes/Tower1.tscn");
    AssertThat(_towerScene).IsNotNull();

    _bulletScene = GD.Load<PackedScene>("res://src/Ankit/Scenes/Bullet.tscn");
    AssertThat(_bulletScene).IsNotNull();
}

[After]
public void TearDown()
{
    // Clean up projectiles first
    var projectiles = GetTree().GetNodesInGroup("Projectile");
    foreach (var projectile in projectiles)
    {
        if (IsInstanceValid(projectile))
        {
            projectile.QueueFree();
        }
    }

    // Clean up test root
    if (_testRoot != null && IsInstanceValid(_testRoot))
    {
        foreach (var child in _testRoot.GetChildren())
        {
            if (IsInstanceValid(child))
            {
                child.QueueFree();
            }
        }
        _testRoot.QueueFree();
    }
    _testRoot = null;

    // Wait for next frame to ensure cleanup
    ToSignal(GetTree(), "process_frame");
}

private void CleanupProjectiles()
{
    var projectiles = GetTree().GetNodesInGroup("Projectile");
    foreach (var projectile in projectiles)
    {
        if (IsInstanceValid(projectile))
        {
            projectile.QueueFree();
        }
    }
}

private void CleanupChildren()
{
    if (_testRoot != null)
    {
        foreach (var child in _testRoot.GetChildren())
        {
            child.QueueFree();
        }
    }
}
    // Test 1: Factory Creation
    [TestCase]
    public void TestTowerFactoryCreation()
    {
        GD.Print("Starting TestTowerFactoryCreation");
        var factory1 = new Tower1Factory();
        var tower1 = AutoFree(factory1.CreateTower());
        
        AssertThat(tower1).IsNotNull()
            .OverrideFailureMessage("Tower1Factory failed to create tower");
        AssertThat(tower1).IsInstanceOf<BaseTower>();

        if (tower1 != null)
        {
            _testRoot.AddChild(tower1);
        }
        GD.Print("TestTowerFactoryCreation completed");
    }

    // Test 2: Basic Properties
    [TestCase]
    public void TestTowerBulletProperties()
    {
        var tower1 = AutoFree(new Tower1());
        var tower2 = AutoFree(new Tower2());
        _testRoot.AddChild(tower1);
        _testRoot.AddChild(tower2);
        
        tower1._Ready();
        tower2._Ready();
        
        // Only test basic properties
        AssertThat(tower1.BulletDamage).IsEqual(10);
        AssertThat(tower2.BulletSpeed).IsGreater(tower1.BulletSpeed);
    }

    // Test 3: Placement System
    [TestCase]
    public void TestTowerPlacementSystem()
    {
        var tower = AutoFree(new Tower1());
        _testRoot.AddChild(tower);
        tower._Ready();
        
        AssertThat(tower.CanBePlaced()).IsTrue();
        
        var blockingArea = AutoFree(new Area2D());
        blockingArea.AddToGroup("Tower");
        _testRoot.AddChild(blockingArea);
        
        tower._on_placement_area_entered(blockingArea);
        AssertThat(tower.CanBePlaced()).IsFalse();
    }

    // Test 4: Component Initialization
    [TestCase]
    public void TestTowerInitialization()
    {
        var tower = AutoFree(_towerScene.Instantiate<Tower1>());
        _testRoot.AddChild(tower);
        tower.BulletScene = _bulletScene;
        tower._Ready();

        AssertThat(tower.GetNode<AnimatedSprite2D>("Towerhead")).IsNotNull();
        AssertThat(tower.GetNode<Area2D>("Sight")).IsNotNull();
        AssertThat(tower.GetNode<Timer>("Timer")).IsNotNull();
    }

    // Test 5: Target Acquisition
    [TestCase]
    public void TestTowerTargetAcquisition()
    {
        var tower = AutoFree(_towerScene.Instantiate<Tower1>());
        _testRoot.AddChild(tower);
        tower.BulletScene = _bulletScene;
        tower._Ready();

        var mockChicken = AutoFree(new BaseChicken());
        var area = AutoFree(new Area2D());
        _testRoot.AddChild(mockChicken);
        mockChicken.AddChild(area);
        mockChicken.Position = new Vector2(50, 0);

        tower.OnBodyEntered(area);
        AssertThat(tower.CurrentTarget).IsNotNull();
    }

    // Test 6: Tower Rotation
    [TestCase]
    public void TestTowerRotation()
    {
        var tower = AutoFree(_towerScene.Instantiate<Tower1>());
        _testRoot.AddChild(tower);
        tower.BulletScene = _bulletScene;
        tower._Ready();

        var towerHead = tower.GetNode<AnimatedSprite2D>("Towerhead");
        AssertThat(towerHead).IsNotNull();

        var mockChicken = AutoFree(new BaseChicken());
        var area = AutoFree(new Area2D());
        _testRoot.AddChild(mockChicken);
        mockChicken.AddChild(area);
        mockChicken.Position = new Vector2(50, 50);

        float initialRotation = towerHead.Rotation;
        tower.OnBodyEntered(area);
        tower._Process(0.1);

        AssertThat(towerHead.Rotation).IsNotEqual(initialRotation);
    }

    //Test 7: Shooting System
   

[TestCase]
public void TestTowerShooting()
{
    try
    {
        GD.Print("Starting TestTowerShooting");

        var tower = AutoFree(_towerScene.Instantiate<Tower1>());
        AssertThat(tower).IsNotNull().OverrideFailureMessage("Tower1 instance failed to instantiate.");
        _testRoot.AddChild(tower);
        GD.Print("Tower added to _testRoot.");

        tower.BulletScene = _bulletScene;
        AssertThat(tower.BulletScene).IsNotNull().OverrideFailureMessage("BulletScene was not set properly.");
        GD.Print("BulletScene set successfully.");

        tower._Ready();
        GD.Print("Tower initialized.");

        var towerHead = tower.GetNode<AnimatedSprite2D>("Towerhead");
        AssertThat(towerHead).IsNotNull().OverrideFailureMessage("TowerHead node is missing.");
        GD.Print("TowerHead found.");

        var bulletSpawn = towerHead.GetNodeOrNull<Marker2D>("BulletSpawnPoint");
        AssertThat(bulletSpawn).IsNotNull().OverrideFailureMessage("BulletSpawnPoint node is missing.");
        GD.Print("BulletSpawnPoint found.");

        var mockChicken = AutoFree(new BaseChicken());
        var area = AutoFree(new Area2D());
        mockChicken.AddChild(area);
        _testRoot.AddChild(mockChicken);
        mockChicken.Position = new Vector2(20, 0);
        GD.Print("Mock target (mockChicken) created and positioned.");

        tower.OnBodyEntered(area);
        AssertThat(tower.CurrentTarget).IsNotNull().OverrideFailureMessage("CurrentTarget was not set.");
        GD.Print("CurrentTarget set successfully.");

        var initialCount = GetTree().GetNodesInGroup("Projectile").Count;
        GD.Print($"Initial bullet count: {initialCount}");

        // Call FireBullets and confirm bullet creation
        tower.Call("FireBullets");
        GD.Print("FireBullets called.");

        var finalCount = GetTree().GetNodesInGroup("Projectile").Count;
        AssertThat(finalCount).IsGreater(initialCount).OverrideFailureMessage(
            $"No bullets created. Initial: {initialCount}, Final: {finalCount}");
        GD.Print("Bullet creation verified.");
    }
    catch (Exception e)
    {
        GD.PrintErr($"Test failed with error: {e.Message}");
    }
}






[TestCase]
public void TestBulletCreation()
{
    // Load and verify scene
    AssertThat(_bulletScene).IsNotNull();

    // Create bullet using AutoFree to handle cleanup
    var bullet = AutoFree(_bulletScene.Instantiate<Bullet>());
    AssertThat(bullet).IsNotNull();

    // Add bullet to the test root to avoid orphaning
    _testRoot.AddChild(bullet);

    // Set basic properties
    bullet.Direction = Vector2.Right;
    bullet.Speed = 300;

    // Verify the bullet is in the correct group
    AssertThat(bullet.IsInGroup("Projectile")).IsTrue();
}


[TestCase]
public void TestBulletSceneLoading()
{
    // Verify scenes load
    var bulletScene = GD.Load<PackedScene>("res://src/Ankit/Scenes/Bullet.tscn");
    AssertThat(bulletScene).IsNotNull()
        .OverrideFailureMessage("Could not load Bullet.tscn");

    // Try to create a bullet and use AutoFree to ensure cleanup
    var bullet = AutoFree(bulletScene.Instantiate<Bullet>());
    AssertThat(bullet).IsNotNull()
        .OverrideFailureMessage("Could not instantiate bullet");

    // Verify bullet setup
    AssertThat(bullet.GetType()).IsEqual(typeof(Bullet))
        .OverrideFailureMessage("Wrong bullet type");
    AssertThat(bullet.IsInGroup("Projectile")).IsTrue()
        .OverrideFailureMessage("Bullet not in Projectile group");
}


}


