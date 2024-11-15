// File: src/Ankit/Scripts/Bullet.cs
using Godot;
using System;
using Chicken;

public partial class Bullet : Area2D
{
    [Export]
    public float Speed = 400;
    
    [Export]
    public int Damage { get; set; } = 10;

    public Vector2 Direction;

    private VisibleOnScreenNotifier2D screenNotifier;

    public override void _Ready()

    {
            base._Ready();
            AddToGroup("Projectile");

        Connect("area_entered", new Callable(this, nameof(OnAreaEntered)));

        screenNotifier = GetNode<VisibleOnScreenNotifier2D>("VisibleOnScreenNotifier2D");
        if (screenNotifier != null)
        {
            screenNotifier.Connect("screen_exited", new Callable(this, nameof(OnScreenExited)));
        }
        else
        {
            GD.PrintErr("VisibleOnScreenNotifier2D not found in Bullet scene");
        }

        if (Direction != Vector2.Zero)
        {
            Rotation = Direction.Angle();
        }

        SetupCollisions();
    }

    private void SetupCollisions()
    {
        CollisionLayer = 4;
        CollisionMask = 2;
    }

    public override void _Process(double delta)
    {
        Position += Direction * (float)(Speed * delta);
    }

    private void OnAreaEntered(Area2D area)
    {
        Node parent = area.GetParent();
        
        if (parent is BaseChicken chicken)
        {
            chicken.TakeDamage(Damage);
            QueueFree();
        }
    }

    private void OnScreenExited()
    {
        QueueFree();
    }
}