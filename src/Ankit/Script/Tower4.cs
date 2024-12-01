// File: src/Ankit/Scripts/Tower4.cs
using Godot;

public partial class Tower4 : BaseTower 
{
    protected override void InitializeComponents()
    {
        base.InitializeComponents();
        SetupSpawnPoints(4);  // Four spawn points for Tower4
    }
    protected  override void InitializeTowerProperties()
    {
        // Sniper configuration
        ShootingInterval = 1.5f;    // Medium fire rate
        RotationSpeed = 6.0f;       // Good rotation
        BulletsPerShot = 4;         // Quad shot
        BulletSpeed = 400f;         // Very fast
        BulletDamage = 12;          // High damage

        GD.Print("=== Sniper Tower Properties Initialized ===");
        GD.Print($"Type: Sniper Tower");
        GD.Print($"Shooting Interval: {ShootingInterval}");
        GD.Print($"Rotation Speed: {RotationSpeed}");
        GD.Print($"Bullets Per Shot: {BulletsPerShot}");
        GD.Print($"Bullet Speed: {BulletSpeed}");
        GD.Print($"Bullet Damage: {BulletDamage}");
        GD.Print("=========================================");
    }

    protected override IBulletBuilder CreateBulletBuilder()
    {
        return new StandardBulletBuilder();
    }
}