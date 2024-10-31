using Godot;

// Tower4 fires 5 bullets at a quick interval, making it a high-damage option with quick tracking.
public partial class Tower4 : Tower1
{
    public override void _Ready()
    {
        base._Ready();
        ShootingInterval = 0.5f; // Faster shooting interval for rapid fire
        RotationSpeed = 8.0f; // Faster rotation speed for agile tracking
    }

    // Override the shooting method to fire 5 bullets at once
    protected override void OnShootTimerTimeout()
    {
        FireBullets(4); // Fire 4 bullets
    }
}
