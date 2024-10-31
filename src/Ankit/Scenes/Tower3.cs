using Godot;

// Tower3 modifies the shooting interval and fires 4 bullets per shot, showcasing a more powerful attack.
public partial class Tower3 : Tower1
{
    public override void _Ready()
    {
        base._Ready();
        ShootingInterval = 0.6f; // Faster shooting interval
        RotationSpeed = 7.5f; // Increased rotation speed for better tracking
    }

    // Override the shooting method to fire 4 bullets at once
    protected override void OnShootTimerTimeout()
    {
        FireBullets(3); // Fire 3 bullets
    }
}
