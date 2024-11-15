// File: src/Ankit/Scripts/Tower1.cs
using Godot;

public partial class Tower1 : BaseTower 
{
    public override void _Ready()
    {
        base._Ready();
        GD.Print("Tower1: Starting initialization");
        
        // Set tower-specific values
        ShootingInterval = 2.0f;
        RotationSpeed = 5.0f;
        BulletsPerShot = 1;
        BulletSpeed = 300f;
        BulletDamage = 10;

        GD.Print("Tower1: Properties set successfully:");
        GD.Print($"  - Shooting Interval: {ShootingInterval}");
        GD.Print($"  - Rotation Speed: {RotationSpeed}");
        GD.Print($"  - Bullets Per Shot: {BulletsPerShot}");
        GD.Print($"  - Bullet Speed: {BulletSpeed}");
        GD.Print($"  - Bullet Damage: {BulletDamage}");

        ShowTowerAttack();
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

    // Add override for FireBullets to debug shooting
    protected override void FireBullets()
    {

        GD.Print("TOWER 1 Firing Pattern");
        GD.Print($"- Single Shot Working!");
        GD.Print($"- Bullets: {BulletsPerShot}, Damage: {BulletDamage}");
        ShowTowerAttack();
        GD.Print("Tower1: Attempting to fire bullets");
        try 
        {
            base.FireBullets();
            GD.Print("Tower1: Bullets fired successfully");
        }
        catch (System.Exception e)
        {
            GD.PrintErr($"Tower1: Error firing bullets: {e.Message}");
        }
    }



protected override void ShowTowerAttack()  // With override
{
    GD.Print("=== Tower 1 Attack ===");
    GD.Print($"This is Tower 1 special attack!");
    GD.Print($"Single strong shot");
    GD.Print($"Bullets Per Shot: {BulletsPerShot}");
    GD.Print($"Damage: {BulletDamage}");
    GD.Print("=====================");
}
    


}


