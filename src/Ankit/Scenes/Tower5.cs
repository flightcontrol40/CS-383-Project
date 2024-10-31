using Godot;

// Tower5 maintains the rapid firing ability of Tower4 but introduces the fastest shooting interval, suitable for high-damage scenarios.
public partial class Tower5 : Tower1
{
    public override void _Ready()
    {
        base._Ready();
        ShootingInterval = 0.4f; // Fastest shooting interval among the towers
        RotationSpeed = 8.5f; // Further increased rotation speed for highly responsive tracking
    }

    // Override the shooting method to fire 5 bullets at once
    protected override void OnShootTimerTimeout()
    {
        FireBullets(5); // Fire 5 bullets, same as Tower4 but at a faster rate
    }
}
