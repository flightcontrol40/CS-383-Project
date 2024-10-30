using Godot;
using System;

public partial class Tower1 : Node2D
{
    [Export]
    public PackedScene BulletScene; // Packed scene for the bullet, to be instantiated when firing.
    [Export]
    public NodePath TowerHeadPath;  // Path to the node of the tower's head, which will rotate to target enemies.
    [Export]
    public float ShootingInterval = 2.0f; // Time in seconds between each shot.
    [Export]
    public float RotationSpeed = 5.0f;    // Speed at which the tower head rotates towards its target.

    private AnimatedSprite2D towerSprite; // Sprite for the tower's body.
    private AnimatedSprite2D towerHead;   // Sprite for the tower's head, which rotates to aim at the target.
    private Timer shootTimer;             // Timer to manage the shooting intervals.
    private Area2D sightArea;             // Area that detects enemies within the tower's range of sight.
    private Node2D currentTarget;         // The current target that the tower is aiming at.

    public override void _Ready()
    {
        towerSprite = GetNode<AnimatedSprite2D>("Towerbody/TowerSprite"); // Access the sprite for the tower's body.
        towerHead = GetNode<AnimatedSprite2D>(TowerHeadPath); // Access the sprite for the tower's head.

        towerSprite.Play("idle"); // Start the idle animation for the tower's body.
        towerHead.Play("idle");   // Start the idle animation for the tower's head.

        sightArea = GetNode<Area2D>("Sight/CollisionShape2D"); // Access the sight area defined for detecting enemies.
        sightArea.Connect("body_entered", this, nameof(OnBodyEntered)); // Connect signal for entering sight.
        sightArea.Connect("body_exited", this, nameof(OnBodyExited));   // Connect signal for exiting sight.

        shootTimer = new Timer();  // Initialize the shooting timer.
        AddChild(shootTimer);
        shootTimer.WaitTime = ShootingInterval; // Set the shooting interval.
        shootTimer.OneShot = false; // Make the timer continuous.
        shootTimer.Connect("timeout", this, nameof(OnShootTimerTimeout)); // Connect timeout signal to shooting handler.
        shootTimer.Start(); // Start the timer.
    }

    private void OnBodyEntered(Node body)
    {
        if (body is Enemy)  // Ensure the body is an Enemy type.
        {
            currentTarget = body as Node2D; // Set the current target to the enemy that entered the sight.
        }
    }

    private void OnBodyExited(Node body)
    {
        if (body == currentTarget) // Check if the exiting body is the current target.
        {
            currentTarget = null; // Clear the current target.
        }
    }

    private void OnShootTimerTimeout()
    {
        if (currentTarget != null && IsInstanceValid(currentTarget)) // Check if there's a valid target.
        {
            var bullet = BulletScene.Instance() as Node2D; // Instantiate the bullet.
            bullet.GlobalPosition = towerHead.GlobalPosition; // Set bullet's starting position at the tower's head.
            bullet.LookAt(currentTarget.GlobalPosition); // Point bullet towards the target.
            AddChild(bullet); // Add the bullet to the scene.
        }
    }

    public override void _Process(float delta)
    {
        if (currentTarget != null && IsInstanceValid(currentTarget)) // Continuously check if there's a valid target.
        {
            Vector2 direction = currentTarget.GlobalPosition - towerHead.GlobalPosition; // Calculate direction to target.
            float targetAngle = direction.Angle(); // Get the angle towards the target.
            float currentAngle = towerHead.Rotation; // Get current rotation angle of the tower head.
            towerHead.Rotation = Mathf.LerpAngle(currentAngle, targetAngle, RotationSpeed * delta); // Smoothly rotate tower head towards target.
        }
    }
}
