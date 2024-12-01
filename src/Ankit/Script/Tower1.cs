// File: src/Ankit/Scripts/Tower1.cs
using Godot;

public partial class Tower1 : BaseTower 
{

    protected override void InitializeComponents()
    {
        base.InitializeComponents();
        SetupSpawnPoints(1);  // Single spawn point for Tower1
    }
    
    protected override void InitializeTowerProperties()
    {
        // Base tower configuration
        ShootingInterval = 0.5f;
        RotationSpeed = 5.0f;
        BulletsPerShot = 1;
        BulletSpeed = 300f;
        BulletDamage = 10;

        GD.Print("=== Base Tower Properties Initialized ===");
        GD.Print($"Type: Base Tower");
        GD.Print($"Shooting Interval: {ShootingInterval}");
        GD.Print($"Rotation Speed: {RotationSpeed}");
        GD.Print($"Bullets Per Shot: {BulletsPerShot}");
        GD.Print($"Bullet Speed: {BulletSpeed}");
        GD.Print($"Bullet Damage: {BulletDamage}");
        GD.Print("=========================================");
    }

    protected override IBulletBuilder CreateBulletBuilder()
    {
        GD.Print("Tower1: Creating bullet builder");
        var builder = new StandardBulletBuilder();
        if (builder != null)
        {
            GD.Print("Tower1: Bullet builder created successfully");
        }
        return builder;
    }

    protected override void FireBullets()
{
    GD.Print("TOWER 1 Firing Pattern");
    GD.Print($"- Single Shot Working!");
    GD.Print($"- Bullets: {BulletsPerShot}, Damage: {BulletDamage}");
    GD.Print("Tower1: Attempting to fire bullets");
    try 
    {
        PlayShootingAnimation(); // Add this line first
        base.FireBullets();
        GD.Print("Tower1: Bullets fired successfully");
    }
    catch (System.Exception e)
    {
        GD.PrintErr($"Tower1: Error firing bullets: {e.Message}");
    }
}
}