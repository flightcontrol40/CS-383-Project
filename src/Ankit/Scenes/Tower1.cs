using Godot; // Static binding: using directive resolves Godot namespace references at compile time.
using System;
using Chicken; // Static binding: using directive for the Chicken namespace, resolved at compile time.

public partial class Tower : Node2D // Subclass: Tower inherits from Node2D, making Node2D the superclass.
{
    [Export]
    public PackedScene BulletScene; // Scene for bullets, dynamically set in the Godot editor (dynamic binding).
    [Export]
    public NodePath TowerHeadPath;  // Path to the tower's rotating head, dynamically set in the editor (dynamic binding).
    [Export]
    public float ShootingInterval = 2.0f; // Interval between shots, can be statically or dynamically set in the editor.
    [Export]
    public float RotationSpeed = 5.0f;    // Speed of tower head rotation, settable in the editor.

    private AnimatedSprite2D towerHead; // Dynamically assigned at runtime when the node tree is ready.
    private Timer shootTimer; // Timer for shooting intervals, configured dynamically at runtime.
    private Area2D sightArea; // Area that detects enemies, configured at runtime.
    private Node2D currentTarget; // The current target of the tower, dynamically changes based on gameplay.

    public override void _Ready() // Overridden from Node2D; called when the node is ready, demonstrates dynamic binding.
    {
        towerHead = GetNode<AnimatedSprite2D>(TowerHeadPath); // Dynamically finds the tower head at runtime.
        shootTimer = new Timer();
        AddChild(shootTimer); // Dynamic addition of a node to the scene tree at runtime.
        shootTimer.WaitTime = ShootingInterval;
        shootTimer.OneShot = false;
        shootTimer.Connect("timeout", new Callable(this, nameof(OnShootTimerTimeout))); // Dynamic binding: Connects the timeout signal to OnShootTimerTimeout at runtime.
        shootTimer.Start();

        sightArea = GetNode<Area2D>("SightArea"); // Dynamically finds the sight area node at runtime.
        sightArea.Connect("body_entered", new Callable(this, nameof(OnBodyEntered))); // Dynamic binding: Signal connection is resolved at runtime.
    }

    private void OnBodyEntered(Node body) // Called dynamically when a body enters the sight area.
    {
        if (body is BaseChicken chicken) // Runtime type check to see if the object is a BaseChicken.
        {
            currentTarget = chicken; // Dynamically sets the current target based on game conditions.
        }
    }

    private void OnShootTimerTimeout() // Dynamically called when the shooting interval timer times out.
    {
        if (currentTarget != null && currentTarget is Node2D target) // Runtime check and cast to Node2D.
        {
            Bullet bullet = BulletScene.Instantiate<Bullet>(); // Instance method is dynamically bound at runtime.
            bullet.GlobalPosition = towerHead.GlobalPosition;
            bullet.Direction = (target.GlobalPosition - towerHead.GlobalPosition).Normalized();
            AddChild(bullet); // Adds the bullet to the scene dynamically.
        }
    }

    public override void _Process(double delta) // Overridden method from Node2D called each frame, uses dynamic binding.
    {
        if (currentTarget != null) // Checks dynamically if there's a current target each frame.
        {
            Vector2 direction = currentTarget.GlobalPosition - towerHead.GlobalPosition;
            float targetAngle = direction.Angle();
            towerHead.Rotation = Mathf.LerpAngle(towerHead.Rotation, targetAngle, RotationSpeed * (float) delta); // Dynamically calculates and applies rotation.
        }
    }
}
