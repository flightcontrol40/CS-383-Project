using GdUnit4;
using Godot;
using static GdUnit4.Assertions;

[TestSuite]
public class BulletTests
{
    private PackedScene _bulletScene;

    [Before]
    public void Setup()
    {
        // Load the bullet scene for all tests
        _bulletScene = GD.Load<PackedScene>("res://src/Ankit/Scenes/Bullet.tscn");
    }

    [After]
    public void TearDown()
    {
        // Cleanup is handled by AutoFree
    }

    [TestCase]
    public void TestBulletInitialization()
    {
        // AutoFree will handle cleanup
        var bullet = AutoFree(_bulletScene.Instantiate<Bullet>());
        
        AssertThat(bullet).IsNotNull()
            .OverrideFailureMessage("Bullet should be created successfully");
        AssertThat(bullet.Speed).IsEqual(400f)
            .OverrideFailureMessage("Default speed should be 400");
        AssertThat(bullet.Damage).IsEqual(10)
            .OverrideFailureMessage("Default damage should be 10");
    }

    [TestCase]
    public void TestBulletSceneStructure()
    {
        var bullet = AutoFree(_bulletScene.Instantiate<Bullet>());
        
        AssertThat(bullet.GetNode<Sprite2D>("Bullet")).IsNotNull()
            .OverrideFailureMessage("Bullet sprite node missing");
        AssertThat(bullet.GetNode<CollisionShape2D>("CollisionShape2D")).IsNotNull()
            .OverrideFailureMessage("Collision shape missing");
        AssertThat(bullet.GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D")).IsNotNull()
            .OverrideFailureMessage("Screen notifier missing");
    }

    [TestCase]
    public void TestBulletCollisionSetup()
    {
        var bullet = AutoFree(_bulletScene.Instantiate<Bullet>());
        bullet._Ready(); // Ensure _Ready is called to set up collisions

        AssertThat(bullet.CollisionLayer).IsEqual(4)
            .OverrideFailureMessage("Bullet collision layer should be 4");
        AssertThat(bullet.CollisionMask).IsEqual(2)
            .OverrideFailureMessage("Bullet collision mask should be 2");
        AssertThat(bullet.IsInGroup("Projectile")).IsTrue()
            .OverrideFailureMessage("Bullet should be in 'Projectile' group");
    }

    [TestCase]
    public void TestStandardBulletBuilder()
    {
        var builder = new StandardBulletBuilder();
        builder.CreateBullet(_bulletScene);
        builder.SetSpeed(300f);
        builder.SetDamage(15);
        
        var bullet = AutoFree(builder.GetResult());
        AssertThat(bullet.Speed).IsEqual(300f)
            .OverrideFailureMessage("Standard bullet speed incorrect");
        AssertThat(bullet.Damage).IsEqual(15)
            .OverrideFailureMessage("Standard bullet damage incorrect");
    }

    [TestCase]
    public void TestRapidBulletBuilder()
    {
        var builder = new RapidBulletBuilder();
        builder.CreateBullet(_bulletScene);
        builder.SetSpeed(300f);
        builder.SetProperties(Vector2.Zero, Vector2.Right);
        
        var bullet = AutoFree(builder.GetResult());
        AssertThat(bullet.Speed).IsGreater(300f)
            .OverrideFailureMessage("Rapid bullet speed should be higher than base");
    }

    [TestCase]
    public void TestHeavyBulletBuilder()
    {
        var builder = new HeavyBulletBuilder();
        builder.CreateBullet(_bulletScene);
        builder.SetDamage(10);
        builder.SetProperties(Vector2.Zero, Vector2.Right);
        
        var bullet = AutoFree(builder.GetResult());
        AssertThat(bullet.Damage).IsEqual(20)
            .OverrideFailureMessage("Heavy bullet damage should be doubled");
    }

    [TestCase]
    public void TestBulletMovement()
    {
        var bullet = AutoFree(_bulletScene.Instantiate<Bullet>());
        bullet.Speed = 100f;
        bullet.Direction = Vector2.Right;
        
        var initialPosition = bullet.Position;
        bullet._Process(1.0);
        
        AssertThat(bullet.Position.X).IsGreater(initialPosition.X)
            .OverrideFailureMessage("Bullet didn't move right");
        AssertThat(bullet.Position.Y).IsEqual(initialPosition.Y)
            .OverrideFailureMessage("Bullet didn't maintain vertical position");
    }
}