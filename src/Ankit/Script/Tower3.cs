// File: src/Ankit/Scripts/Tower3.cs
using Godot;

// public partial class Tower3 : BaseTower
// {
//     public override void _Ready()
//     {
//         base._Ready();
//         // Set tower-specific values - heavy tower configuration
//         ShootingInterval = 3.0f;    // Slower shooting
//         RotationSpeed = 3.0f;       // Slower rotation
//         BulletsPerShot = 3;         // Triple shots
//         BulletSpeed = 250f;         // Slower bullets
//         BulletDamage = 15;          // More damage
    

//         GD.Print("Tower3: Properties set successfully:");
//         GD.Print($"  - Shooting Interval: {ShootingInterval}");
//         GD.Print($"  - Rotation Speed: {RotationSpeed}");
//         GD.Print($"  - Bullets Per Shot: {BulletsPerShot}");
//         GD.Print($"  - Bullet Speed: {BulletSpeed}");
//         GD.Print($"  - Bullet Damage: {BulletDamage}");
    
//     }

//     protected override IBulletBuilder CreateBulletBuilder()
//     {
//         return new HeavyBulletBuilder();
//     }
// }

public partial class Tower3 : BaseTower 
{   
    protected override void InitializeComponents()
    {
        base.InitializeComponents();
        SetupSpawnPoints(3);  // Three spawn points for Tower3
    }
    protected override void InitializeTowerProperties()
    {
        // Heavy tower configuration
        ShootingInterval = 1.0f;    // Slower shooting
        RotationSpeed = 3.0f;       // Slower rotation
        BulletsPerShot = 3;         // Triple shots
        BulletSpeed = 250f;         // Slower bullets
        BulletDamage = 15;          // More damage

        GD.Print("=== Heavy Tower Properties Initialized ===");
        GD.Print($"Type: Heavy Tower");
        GD.Print($"Shooting Interval: {ShootingInterval}");
        GD.Print($"Rotation Speed: {RotationSpeed}");
        GD.Print($"Bullets Per Shot: {BulletsPerShot}");
        GD.Print($"Bullet Speed: {BulletSpeed}");
        GD.Print($"Bullet Damage: {BulletDamage}");
        GD.Print("=========================================");
    }

    protected override IBulletBuilder CreateBulletBuilder()
    {
        return new HeavyBulletBuilder();
    }

    protected override void FireBullets()
    {
        GD.Print("TOWER 3 Firing Pattern");
        GD.Print($"- Triple Shot Working!");
        GD.Print($"- Bullets: {BulletsPerShot}, Damage: {BulletDamage}");
        GD.Print("Tower3: Attempting to fire bullets with animation");
        try 
        {
            PlayShootingAnimation(); // Triggers animation before firing
            base.FireBullets();
            GD.Print("Tower3: Bullets fired successfully with animation");
        }
        catch (System.Exception e)
        {
            GD.PrintErr($"Tower3: Error firing bullets with animation: {e.Message}");
        }
    }
}