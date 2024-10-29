using Godot;
using System;

public partial class Tower1 : Node2D
{
    [Export]
    public PackedScene BulletScene;  // Packed scene for the bullet
    private Timer shootTimer;        // Timer for managing shooting intervals
    private AnimatedSprite2D towerSprite; // For tower animations

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        towerSprite = GetNode<AnimatedSprite2D>("TowerSprite");
        towerSprite.Play("idle");  // Assuming there's an 'idle' animation set up

        shootTimer = new Timer();
        AddChild(shootTimer);
        shootTimer.WaitTime = 2.0f;  // Shoot every 2 seconds
        shootTimer.OneShot = false;
        // Corrected connection using new Callable syntax
        shootTimer.Connect("timeout", new Callable(this, nameof(OnShootTimerTimeout)));
        shootTimer.Start();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        // Example: Rotate tower head towards the mouse cursor
        Vector2 targetDirection = GetGlobalMousePosition() - GlobalPosition;
        RotationDegrees = targetDirection.AngleTo(Vector2.Right);
    }

    private void OnShootTimerTimeout()
    {
        // Instantiate and shoot a bullet
        var bullet = BulletScene.Instantiate() as Node2D;
        bullet.GlobalPosition = this.GlobalPosition; // Start at tower position
        AddChild(bullet);
    }
}
