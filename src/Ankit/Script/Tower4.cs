// File: src/Ankit/Scripts/Tower4.cs
using Godot;

public partial class Tower4 : BaseTower
{
    public override void _Ready()
    {
        base._Ready();
        // Set tower-specific values - sniper configuration
        ShootingInterval = 2.5f;    // Medium fire rate
        RotationSpeed = 6.0f;       // Good rotation
        BulletsPerShot = 4;         // Quad shot
        BulletSpeed = 400f;         // Very fast
        BulletDamage = 12;          // High damage

        GD.Print("Tower2: Properties set successfully:");
        GD.Print($"  - Shooting Interval: {ShootingInterval}");
        GD.Print($"  - Rotation Speed: {RotationSpeed}");
        GD.Print($"  - Bullets Per Shot: {BulletsPerShot}");
        GD.Print($"  - Bullet Speed: {BulletSpeed}");
        GD.Print($"  - Bullet Damage: {BulletDamage}");
    
    }

    protected override IBulletBuilder CreateBulletBuilder()
    {
        return new StandardBulletBuilder();
    }
}