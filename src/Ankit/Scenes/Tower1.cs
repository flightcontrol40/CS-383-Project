using Godot; // Static binding: using directive resolves Godot namespace references at compile time.
using System;
using Chicken;


public partial class Tower1 : Node2D // Subclass: Tower inherits from Node2D, making Node2D the superclass.
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

    // 'FireBullets' is a protected method, meaning it can be accessed within this class and by any subclasses derived from it.
// This method is responsible for instantiating and launching a specified number of bullets towards a currently targeted enemy.
protected void FireBullets(int count)
{
    // Check if there is a current target and safely cast it to Node2D to ensure it is the correct type for manipulation.
    if (currentTarget != null && currentTarget is Node2D target)
    {
        // Loop through the number of bullets to fire.
        for (int i = 0; i < count; i++)
        {
            // Dynamically create a Bullet instance from a preconfigured PackedScene. This is a form of dynamic binding where the specific bullet instance is not determined until runtime.
            Bullet bullet = BulletScene.Instantiate<Bullet>();
            // Set the bullet's starting position at the tower's head, which dynamically aligns the bullet with the tower's current position on the game map.
            bullet.GlobalPosition = towerHead.GlobalPosition;
            // Calculate the direction from the tower to the target, normalizing it to ensure consistent speed. This calculation is dynamically done at runtime.
            bullet.Direction = (target.GlobalPosition - towerHead.GlobalPosition).Normalized();
            // Dynamically adds the newly created bullet as a child of the tower node, allowing it to appear in the game scene and interact based on the game's physics and rendering systems.
            AddChild(bullet);
        }
    }
}


   // 'OnShootTimerTimeout' is a private method triggered by a Timer node's timeout signal.
// This method encapsulates the action taken when the timer, set to regulate shooting intervals, elapses.
protected virtual void OnShootTimerTimeout()
{
    // Calls 'FireBullets' with a parameter of 1, indicating to fire one bullet. This parameter can be adjusted depending on the tower's desired firepower.
    // This use of 'FireBullets' demonstrates dynamic binding as the method may behave differently if overridden in a subclass.
    FireBullets(1); // Adjust the number as necessary
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
