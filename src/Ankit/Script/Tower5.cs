// File: src/Ankit/Scripts/Tower5.cs
using Godot;

public partial class Tower5 : BaseTower
{
    protected override void InitializeComponents()
    {
        base.InitializeComponents();
        SetupSpawnPoints(5);  // Five spawn points for Tower5
    }
    // public override void _Ready()
    // {
    //     base._Ready();
    //     // Set tower-specific values - speed tower configuration
    //     ShootingInterval = 1.5f;    // Fast shooting
    //     RotationSpeed = 8.0f;       // Fastest rotation
    //     BulletsPerShot = 1;         // Single shot
    //     BulletSpeed = 450f;         // Fastest bullets
    //     BulletDamage = 10;          // Normal damage
    // }


    protected override void InitializeTowerProperties()
    {
        // Speed tower configuration
        ShootingInterval = 0.75f;    // Fast shooting
        RotationSpeed = 8.0f;       // Fastest rotation
        BulletsPerShot = 1;         // Single shot
        BulletSpeed = 450f;         // Fastest bullets
        BulletDamage = 10;          // Normal damage

        GD.Print("=== Speed Tower Properties Initialized ===");
        GD.Print($"Type: Speed Tower");
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

    protected override void FireBullets()
    {
        GD.Print("TOWER 5 Firing Pattern");
        GD.Print($"- Single Shot Working!");
        GD.Print($"- Bullets: {BulletsPerShot}, Damage: {BulletDamage}");
        GD.Print("Tower5: Attempting to fire bullets with animation");
        try 
        {
            PlayShootingAnimation(); // Triggers animation before firing
            base.FireBullets();
            GD.Print("Tower5: Bullets fired successfully with animation");
        }
        catch (System.Exception e)
        {
            GD.PrintErr($"Tower5: Error firing bullets with animation: {e.Message}");
        }
    }

}




//We must call base.Ready() in derived classes
//not pure polymorphism , we are  forced use the base implematation.