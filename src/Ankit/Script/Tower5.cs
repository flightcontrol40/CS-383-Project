// File: src/Ankit/Scripts/Tower5.cs
using Godot;

public partial class Tower5 : BaseTower
{
    public override void _Ready()
    {
        base._Ready();
        // Set tower-specific values - speed tower configuration
        ShootingInterval = 1.5f;    // Fast shooting
        RotationSpeed = 8.0f;       // Fastest rotation
        BulletsPerShot = 1;         // Single shot
        BulletSpeed = 450f;         // Fastest bullets
        BulletDamage = 10;          // Normal damage
    }

    protected override IBulletBuilder CreateBulletBuilder()
    {
        return new RapidBulletBuilder();
    }
}