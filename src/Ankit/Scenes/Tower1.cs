using Godot;
using System.Collections.Generic;

public partial class Tower : Node2D  // Added 'partial' keyword here
{
    [Export]
    public NodePath TowerBodyPath; // Path to the AnimatedSprite2D for the tower's body
    [Export]
    public NodePath TowerHeadPath; // Path to the AnimatedSprite2D for the tower's head
    [Export]
    public PackedScene BulletScene; // Packed scene for the bullet prefab

    private AnimatedSprite2D TowerBody;
    private AnimatedSprite2D TowerHead;
    private Timer ShootTimer;
    public Node2D CurrentTarget; // Current target for the tower

    public override void _Ready()
    {
        TowerBody = GetNode<AnimatedSprite2D>(TowerBodyPath);
        TowerHead = GetNode<AnimatedSprite2D>(TowerHeadPath);
        ShootTimer = new Timer();

        AddChild(ShootTimer);
        ShootTimer.WaitTime = 2.0f;  // Frequency of shots
        ShootTimer.Connect("timeout", new Callable(this, nameof(OnShootTimerTimeout)));
        ShootTimer.Start();

        TowerBody.Play("idle");  // Assuming 'idle' animation set up
        TowerHead.Play("idle");
    }

    private void OnShootTimerTimeout()
    {
        if (CurrentTarget != null && CurrentTarget.IsInsideTree())
        {
            FireBullet(CurrentTarget.GlobalPosition);
        }
    }

    private void FireBullet(Vector2 targetPosition)
    {
        var bulletInstance = BulletScene.Instantiate() as Node2D;
        bulletInstance.GlobalPosition = TowerHead.GlobalPosition;
        AddChild(bulletInstance);
        // Ensure bulletInstance is cast to Bullet or has 'Target' property if Bullet is a specific class
    }

    public override void _Process(double delta)
    {
        if (CurrentTarget != null)
        {
            Vector2 direction = CurrentTarget.GlobalPosition - TowerHead.GlobalPosition;
            TowerHead.Rotation = direction.Angle();
        }
    }
}
