// File: src/Ankit/Scripts/Tower5.cs
using Godot;

public partial class Tower5 : BaseTower
{
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
        ShootingInterval = 1.5f;    // Fast shooting
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
}




//We must call base.Ready() in derived classes
//not pure polymorphism , we are  forced use the base implematation.