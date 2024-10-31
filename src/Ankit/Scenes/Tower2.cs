using Godot;

// Tower2 customizes shooting behavior to fire 3 bullets at once with a slightly faster interval than Tower1.
public partial class Tower2 : Tower1
{
    public override void _Ready()
    {
        base._Ready();
        ShootingInterval = 0.8f; // Slightly faster interval
        RotationSpeed = 7.0f; // Further increase in rotation speed for more responsive targeting
    }

    // Override the shooting method to fire 3 bullets at once
    protected override void OnShootTimerTimeout()
    {
        FireBullets(2); // Fire 2 bullets
    }
}
