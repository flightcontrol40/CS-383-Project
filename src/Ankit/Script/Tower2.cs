// File: src/Ankit/Scripts/Tower2.cs
using Godot;

public partial class Tower2 : BaseTower
{
    public override void _Ready()
    {
        base._Ready();
        // Set tower-specific values - rapid fire configuration
        ShootingInterval = 1.0f;    // Faster shooting
        RotationSpeed = 7.0f;       // Faster rotation
        BulletsPerShot = 2;         // Double shots
        BulletSpeed = 350f;         // Faster bullets
        BulletDamage = 8;           // Less damage per bullet

        GD.Print("Tower2: Properties set successfully:");
        GD.Print($"  - Shooting Interval: {ShootingInterval}");
        GD.Print($"  - Rotation Speed: {RotationSpeed}");
        GD.Print($"  - Bullets Per Shot: {BulletsPerShot}");
        GD.Print($"  - Bullet Speed: {BulletSpeed}");
        GD.Print($"  - Bullet Damage: {BulletDamage}");
    
        ShowTowerAttack();
    }

    protected override IBulletBuilder CreateBulletBuilder()
    {
        return new RapidBulletBuilder();
    }

    protected override void ShowTowerAttack()  // With override
{
    GD.Print("=== Tower 2 Attack ===");
    GD.Print($"This is Tower 2 special attack!");
    GD.Print($"Single Double shot");
    GD.Print($"Bullets Per Shot: {BulletsPerShot}");
    GD.Print($"Damage: {BulletDamage}");
    GD.Print("=====================");
}

}