// File: src/Ankit/Scripts/Tower2.cs
using Godot;


public partial class Tower2 : BaseTower 
{
    protected override void InitializeComponents()
    {
        base.InitializeComponents();
        SetupSpawnPoints(2);  // Two spawn points for Tower2
    }
    protected override void InitializeTowerProperties()
    {
        // Rapid fire tower configuration
        ShootingInterval = 0.3f;    // Faster shooting
        RotationSpeed = 7.0f;       // Faster rotation
        BulletsPerShot = 2;         // Double shots
        BulletSpeed = 350f;         // Faster bullets
        BulletDamage = 8;           // Less damage per bullet

        GD.Print("=== Rapid Fire Tower Properties Initialized ===");
        GD.Print($"Type: Rapid Fire Tower");
        GD.Print($"Shooting Interval: {ShootingInterval}");
        GD.Print($"Rotation Speed: {RotationSpeed}");
        GD.Print($"Bullets Per Shot: {BulletsPerShot}");
        GD.Print($"Bullet Speed: {BulletSpeed}");
        GD.Print($"Bullet Damage: {BulletDamage}");
        GD.Print("===========================================");
    }

    protected override IBulletBuilder CreateBulletBuilder()
    {
        return new RapidBulletBuilder();
    }


    protected override void FireBullets()
{
    GD.Print("TOWER 2 Firing Pattern");
    GD.Print($"- Double Shot Working!");
    GD.Print($"- Bullets: {BulletsPerShot}, Damage: {BulletDamage}");
    GD.Print("Tower2: Attempting to fire bullets with animation");
    try 
    {
        PlayShootingAnimation(); // Triggers animation before firing
        base.FireBullets();
        GD.Print("Tower2: Bullets fired successfully with animation");
    }
    catch (System.Exception e)
    {
        GD.PrintErr($"Tower2: Error firing bullets with animation: {e.Message}");
    }
}

}