using Godot;
using System;
using Chicken;
using System.Collections.Generic;
using System.Linq;

public static class _Preloader{
    public static PackedScene bulletScene =  ResourceLoader.Load<PackedScene>("res://src/Ankit/Scenes/bullet.tscn");
}

public partial class BaseTower : Node2D
{
    // [Export]
    public PackedScene BulletScene { get; set; }
    [Export]
    public NodePath TowerHeadPath { get; set; }

    // Properties for tower configuration
    private float _shootingInterval = 2.0f;
    private float _rotationSpeed = 5.0f;
    private int _bulletsPerShot = 1;
    private float _bulletSpeed = 500f;
    private int _bulletDamage = 10;

    private float _lastKnowAngle = 0f;
    private Vector2 _lastKnowDirection = Vector2.Zero;

    // Virtual properties with protected setters
    public virtual float ShootingInterval 
    { 
        get => _shootingInterval;
        protected set => _shootingInterval = value;
    }

    public virtual float RotationSpeed 
    { 
        get => _rotationSpeed;
        protected set => _rotationSpeed = value;
    }

    public virtual int BulletsPerShot 
    { 
        get => _bulletsPerShot;
        protected set => _bulletsPerShot = value;
    }

    public virtual float BulletSpeed 
    { 
        get => _bulletSpeed;
        protected set => _bulletSpeed = value;
    }

    public virtual int BulletDamage 
    { 
        get => _bulletDamage;
        protected set => _bulletDamage = value;
    }

    protected AnimatedSprite2D towerHead;
    protected Timer shootTimer;
    protected Area2D sightArea;
    protected BaseChicken currentTarget;
    protected bool isValidPlacement = true;
    protected List<Marker2D> bulletSpawnPoints = new List<Marker2D>();
    protected IBulletBuilder bulletBuilder;
    protected List<BaseChicken> ChickensInRange;
    protected bool targetInSight = false;


    // Public methods for round manager
    public bool IsActive => shootTimer != null && !shootTimer.IsStopped();
    public bool HasTarget => currentTarget != null && IsInstanceValid(currentTarget);
    public bool CanBePlaced() => isValidPlacement;
    public Area2D SightArea => GetNodeOrNull<Area2D>("Sight");
    public Node2D CurrentTarget => currentTarget;

    // Checks if a given chicken can be targeted by measuring the distance.
    protected virtual bool CanTargetChicken(BaseChicken chicken)
    {
        GD.Print("CanTargetChicken: Start check");
       
        if (!IsInstanceValid(chicken))
        {
            GD.Print("CanTargetChicken: Chicken is not valid");
            return false;
        }
    
        float distance = GlobalPosition.DistanceTo(chicken.GlobalPosition);
        var shape = sightArea?.GetNode<CollisionShape2D>("CollisionShape2D")?.Shape as CircleShape2D;
        float range = shape?.Radius ?? 0;
    
        // 
        GD.Print($"CanTargetChicken: Distance={distance}, Range={range}, Position={GlobalPosition}, ChickenPos={chicken.GlobalPosition}");
        bool canTarget = distance <= range;
        GD.Print($"CanTargetChicken: Can target? {canTarget}");
    
        return canTarget;
    }

    public bool IsValidPosition()
    {
        Area2D PlacementArea = GetNode<Area2D>("Placement");
        return !PlacementArea.HasOverlappingAreas();
    }
    
    public void ToggleCollisionShapes()
    {
        Area2D Placement = GetNode<Area2D>("Placement");
        Area2D Sight = GetNode<Area2D>("Sight");

        Placement.Visible = !Placement.Visible;
        Sight.Visible = !Sight.Visible;
    }
    

    // Cleanup for resources and nodes upon exiting the scene.
    public override void _ExitTree()
    {
        base._ExitTree();
        
        if (towerHead != null && IsInstanceValid(towerHead)) 
            towerHead.QueueFree();
            
        if (shootTimer != null && IsInstanceValid(shootTimer)) 
            shootTimer.QueueFree();
            
        if (sightArea != null && IsInstanceValid(sightArea)) 
            sightArea.QueueFree();
            
        foreach (var spawnPoint in bulletSpawnPoints)
        {
            if (IsInstanceValid(spawnPoint))
                spawnPoint.QueueFree();
        
        }
    }

    // Sets up tower components and properties when added to the scene.
    public override void _Ready()
    {
        if (!IsInstanceValid(this)) return;
        BulletScene = _Preloader.bulletScene;
        ChickensInRange = new List<BaseChicken>();   
        bulletBuilder = CreateBulletBuilder();
        InitializeComponents();
        ValidateSceneSetup();
        
        InitializeTowerProperties();
    }

    // Creates a default bullet builder (can be overridden by subclasses).
    protected virtual IBulletBuilder CreateBulletBuilder()
    {
        return new StandardBulletBuilder();
    }

    // Initializes essential components of the tower.
    protected virtual void InitializeComponents()
    {
        // Get required nodes
        towerHead = GetNodeOrNull<AnimatedSprite2D>("Towerhead");
        if (towerHead == null)
        {
            GD.PrintErr("TowerHead not found!");
            SetProcess(false);
            return;
        }
    
        sightArea = GetNodeOrNull<Area2D>("Sight");
        if (sightArea == null)
        {
            GD.PrintErr("Sight not found!");
            SetProcess(false);
            return;
        }
    
        shootTimer = GetNodeOrNull<Timer>("Timer");
        if (shootTimer == null)
        {
            GD.PrintErr("Timer not found!");
            SetProcess(false);
            return;
        }
    
        // Initialize timer first
        shootTimer.WaitTime = ShootingInterval;
        shootTimer.OneShot = false;
        shootTimer.Connect(Timer.SignalName.Timeout, Callable.From(OnShootTimerTimeout));
        shootTimer.Start();
        GD.Print($"Timer started with interval: {ShootingInterval}");

        // Initialize spawn points
        SetupSpawnPoints(1); // Base tower starts with 1 spawn point

    
        // Get or create bullet spawn point
        bulletBuilder = CreateBulletBuilder();
        if (bulletBuilder == null)
        {
            GD.PrintErr("Failed to create BulletBuilder!");
            SetProcess(false);
            return;
        }

        if (!VerifyComponents())
        {
            GD.PrintErr("Component verification failed!");
            SetProcess(false);
            return;
        }

        GD.Print("Tower initialization completed successfully");
    }
    
    protected virtual void SetupSpawnPoints(int count)
    {
        // Clear any existing spawn points
        foreach (var point in bulletSpawnPoints)
        {
            if (IsInstanceValid(point))
                point.QueueFree();
        }
        bulletSpawnPoints.Clear();

        // Create spawn points
        float spacing = 15f;
        float startX = -(spacing * (count - 1) / 2f);

        for (int i = 0; i < count; i++)
        {
            var spawnPoint = new Marker2D();
            towerHead.AddChild(spawnPoint);
            float xPos = (count == 1) ? 0 : startX + (spacing * i);
            spawnPoint.Position = new Vector2(xPos, -45.4737f);
            bulletSpawnPoints.Add(spawnPoint);
            GD.Print($"Created spawn point {i + 1} at position: {spawnPoint.Position}");
        }
    }
    // Verifies that all necessary components are set up correctly.
    private bool VerifyComponents()
    {
        if (!IsInstanceValid(towerHead))
        {
            GD.PrintErr("TowerHead validation failed");
            return false;
        }
    
        if (!IsInstanceValid(sightArea))
        {
            GD.PrintErr("SightArea validation failed");
            return false;
        }
    
        if (!IsInstanceValid(shootTimer))
        {
            GD.PrintErr("ShootTimer validation failed");
            return false;
        }
    
         if (bulletSpawnPoints.Count == 0 || bulletSpawnPoints.Any(p => !IsInstanceValid(p)))
        {
            GD.PrintErr("BulletSpawnPoints validation failed");
            return false;
        }
    
        if (bulletBuilder == null)
        {
            GD.PrintErr("BulletBuilder validation failed");
            return false;
        }
    
        return true;
    }

    // Validates that all required nodes are present; disables processing if any are missing.
    protected virtual void ValidateSceneSetup()
    {
        if (towerHead == null)
            SetProcess(false);
        if (sightArea == null)
            SetProcess(false);
        if (shootTimer == null)
            SetProcess(false);
        if (BulletScene == null)
            SetProcess(false);
        if (bulletSpawnPoints.Count == 0)
            SetProcess(false);
    }

    // When a chicken enters the sight area, add it to ChickensInRange
    public void OnBodyEntered(Area2D area)
    {
        Node parent = area.GetParent();
        GD.Print($"OnBodyEntered: Parent type is {parent?.GetType().Name ?? "null"}");

        if (parent is BaseChicken chicken)
        {
            ChickensInRange.Add(chicken);
        }
    }

    // When a chicken enters the sight area, remove it from ChickensInRange
    public void OnBodyExited(Area2D area)
    {
        Node parent = area.GetParent();
        if (parent is BaseChicken chicken)
        {
            ChickensInRange.Remove(ChickensInRange.Find(c => c.Equals(chicken)));
        }
    }

    // Fires bullets at the target when ready and displays shooting animation.
    // protected virtual void FireBullets()

    // {
    //     // ShowTowerAttack();


    //     /*GD.Print("BASE TOWER Firing Pattern");
    //     GD.Print($"- Using Default Pattern");
    //     GD.Print($"- Bullets Per Shot: {BulletsPerShot}");
    //     GD.Print($"- Bullet Speed: {BulletSpeed}");
    //     GD.Print($"- Bullet Damage: {BulletDamage}");
    //     GD.Print("FireBullets: Starting bullet firing process");
    //     */
    //     if (BulletScene == null)
    //     {
    //         GD.PrintErr("BulletScene is null, cannot create bullet.");
    //         return;
    //     }
    
    //     if (currentTarget == null || !IsInstanceValid(currentTarget))
    //     {
    //         GD.PrintErr("No valid target, cannot fire.");
    //         return;
    //     }
    
    //     if (bulletSpawnPoint == null || !IsInstanceValid(bulletSpawnPoint))
    //     {
    //         GD.PrintErr("Bullet spawn point is null or invalid.");
    //         return;
    //     }
    
    //     // GD.Print("FireBullets: Playing shooting animation");
    //     // PlayShootingAnimation();
    
    //     for (int i = 0; i < BulletsPerShot; i++)
    //     {
    //         var bullet = BulletScene.Instantiate<Bullet>();
    //         if (bullet != null)
    //         {
    //             bullet.Position = bulletSpawnPoint.GlobalPosition;
    //             bullet.Direction = (currentTarget.GlobalPosition - bullet.Position).Normalized();
    //             bullet.Speed = BulletSpeed;
    //             bullet.Damage = BulletDamage;
    
    //             AddSibling(bullet);
    //             bullet.AddToGroup("Projectile");
    
    //             // GD.Print($"Bullet {i + 1} created at {bullet.Position} with direction {bullet.Direction}");
    //         }
    //         else
    //         {
    //             GD.PrintErr($"Failed to create bullet {i + 1}");
    //         }
    //     }
    // }


    protected virtual void FireBullets()
{
    if (BulletScene == null || currentTarget == null || !IsInstanceValid(currentTarget))
        return;

    // Play the shooting animation
    if (towerHead != null && towerHead.SpriteFrames != null)
    {
        towerHead.Play("default"); // Play the default animation
        
        // Reset animation after a short delay
        GetTree().CreateTimer(0.3f).Timeout += () =>
        {
            towerHead.Stop();
        };
    }

    foreach (var spawnPoint in bulletSpawnPoints)
    {
        if (!IsInstanceValid(spawnPoint))
            continue;

        for (int i = 0; i < BulletsPerShot; i++)
        {
            var bullet = BulletScene.Instantiate<Bullet>();
            if (bullet != null)
            {
                var shape = sightArea?.GetNode<CollisionShape2D>("CollisionShape2D")?.Shape as CircleShape2D;
                float range = shape?.Radius ?? 0;
                bullet.Position = spawnPoint.GlobalPosition;
                bullet.TowerPosition = GlobalPosition;
                bullet.Direction = (currentTarget.GlobalPosition - bullet.Position).Normalized();
                bullet.Speed = BulletSpeed;
                bullet.Damage = BulletDamage;
                if (range != 0){
                bullet.TowerRange = range;

                }
                AddSibling(bullet);
                bullet.AddToGroup("Projectile");
            }
        }
    }
}

    // Plays the tower's shooting animation.
    protected virtual void PlayShootingAnimation()
    {
        if (towerHead != null && towerHead.SpriteFrames != null)
        {
            // Set animation speed and start animation playback
            towerHead.SpeedScale = 2.0f;
            towerHead.Play("shooting"); // Assume there is an animation named "shooting"
    
            // Reset animation speed after firing
            GetTree().CreateTimer(0.3f).Timeout += () =>
            {
                towerHead.SpeedScale = 1.0f;
                towerHead.Stop();
            };
        }
    }
    

    // Checks conditions for firing bullets when the timer times out.
    protected virtual void OnShootTimerTimeout()
    {
        try
        {
            if (currentTarget != null && IsInstanceValid(currentTarget))
            {
                GD.Print("Timer timeout - Attempting to fire bullets");
                FireBullets();
            }
            else
            {
                GD.Print("Timer timeout - No valid target, skipping fire");
            }
        }
        catch (Exception e)
        {
            GD.PrintErr($"Error in OnShootTimerTimeout: {e.Message}");
        }
    }
    
    //Checks whether the tower can shoot (valid target, spawn point, and bullet scene).
    protected virtual bool CanShoot()
    {
        return IsInstanceValid(currentTarget) && 
               bulletSpawnPoints.Count > 0 && 
               bulletSpawnPoints.All(point => IsInstanceValid(point)) && 
               BulletScene != null && 
               IsInstanceValid(GetTree()?.Root);
    }
    
    //Handles placement validation when entering and exiting restricted areas.
    public void _on_placement_area_entered(Area2D area)
    {
        if (area.IsInGroup("BlockPlacement") || area.IsInGroup("Tower"))
        {
            isValidPlacement = false;
            Modulate = new Color(1, 0, 0, 0.5f);
        }
    }

    public void _on_placement_area_exited(Area2D area)
    {
        if (area.IsInGroup("BlockPlacement") || area.IsInGroup("Tower"))
        {
            isValidPlacement = true;
            Modulate = new Color(1, 1, 1, 1);
        }
    }

    /// <summary>
    /// Checks each BaseChicken in range of the turret, and sets the turret's
    /// current target to the furthest along BaseChicken.
    /// </summary>
    protected BaseChicken DoubleCheckTarget()
    {
        if (ChickensInRange.Count > 0)
        {
            BaseChicken furthestChicken = currentTarget;
            float furthestDistance = 0;
            foreach (var c in ChickensInRange)
            {
                if (c.Progress > furthestDistance)
                {
                    furthestDistance = c.Progress;
                    furthestChicken = c;
                }
            }
            currentTarget = furthestChicken;
        }
        else currentTarget = null;
        return currentTarget;
    }

    protected void AimAtTarget(double delta)
    {
        if (currentTarget == null) return;
        try
        {

            // Calculate direction and target angle
            Vector2 direction = currentTarget.GlobalPosition - GlobalPosition;
            float targetAngle = GetAngleTo(currentTarget.GlobalPosition) + ((float)Math.PI / 2);

            // Calculate rotation with increased speed for better responsiveness
            float rotationAmount = Mathf.Min(1.0f, RotationSpeed * (float)delta * 3.5f);

            // Rotate tower head
            float newRotation = Mathf.LerpAngle(
                towerHead.Rotation,
                targetAngle,
                rotationAmount
            );
            towerHead.Rotation = newRotation;

            // Check for shooting conditions
            float angleDifference = Mathf.Abs(Mathf.AngleDifference(towerHead.Rotation, targetAngle));
            if (angleDifference < 0.9f && shootTimer != null && !shootTimer.IsStopped()) // Increased tolerance
            {
                //FireBullets();
            }

            // Debug info for testing
            if (OS.IsDebugBuild())
            {
                // GD.Print($"Target Angle: {targetAngle}, Current: {towerHead.Rotation}, Diff: {angleDifference}");
            }
        }
        catch (ObjectDisposedException e)
        {
            ChickensInRange.Remove(currentTarget);
        }
        catch (Exception e)
        {
            GD.PrintErr($"Error in tower processing: {e.Message}\n" +
                        $"Target position: {currentTarget?.GlobalPosition}\n" +
                        $"Tower position: {GlobalPosition}");
            currentTarget = null; // Reset on error
        }
    }



    // Processes tower's rotation to aim at the target and manage shooting.
    public override void _Process(double delta)
    {
        DoubleCheckTarget();
        AimAtTarget(delta);

        // Early validation of critical components
        if (!ValidateProcessingState())
            return;
    }
    
    // Verifies that necessary components are in a valid state before processing.
    // private bool ValidateProcessingState()
    // {
    //     // Check tower head
    //     if (towerHead == null || !IsInstanceValid(towerHead))
    //     {
    //         GD.PrintErr("Tower head is invalid or null");
    //         return false;
    //     }
    
    //     // Check target
    //     if (currentTarget == null || !IsInstanceValid(currentTarget))
    //     {
    //         currentTarget = null; // Clear invalid target
    //         return false;
    //     }
    
    //     // Check bullet spawn point
    //     if (bulletSpawnPoint == null || !IsInstanceValid(bulletSpawnPoint))
    //     {
    //         GD.PrintErr("Bullet spawn point is invalid or null");
    //         return false;
    //     }
    
    //     // Check bullet builder
    //     if (bulletBuilder == null)
    //     {
    //         GD.PrintErr("Bullet builder is null");
    //         return false;
    //     }
    
    //     return true;
    // }

     private bool ValidateProcessingState()
    {
        if (towerHead == null || !IsInstanceValid(towerHead))
            return false;

        if (currentTarget == null || !IsInstanceValid(currentTarget))
        {
            currentTarget = null;
            return false;
        }

        if (bulletSpawnPoints.Count == 0 || bulletSpawnPoints.Any(p => !IsInstanceValid(p)))
            return false;

        if (bulletBuilder == null)
            return false;

        return true;
    }

    // Initializes default properties for the tower
    protected virtual void InitializeTowerProperties()
    {
        // Default tower properties
        ShootingInterval = 0.5f;
        RotationSpeed = 5.0f;
        BulletsPerShot = 1;
        BulletSpeed = 300f;
        BulletDamage = 10;
    
        GD.Print("=== Base Tower Properties Initialized ===");
        GD.Print($"Type: Base Tower");
        GD.Print($"Shooting Interval: {ShootingInterval}");
        GD.Print($"Rotation Speed: {RotationSpeed}");
        GD.Print($"Bullets Per Shot: {BulletsPerShot}");
        GD.Print($"Bullet Speed: {BulletSpeed}");
        GD.Print($"Bullet Damage: {BulletDamage}");
        GD.Print("=====================================");
    }

    // Added
    public void SetupForTesting(float shootInterval, int bulletsPerShot, float bulletSpeed, float rotationSpeed)
    {
        ShootingInterval = shootInterval;
        BulletsPerShot = bulletsPerShot;
        BulletSpeed = bulletSpeed;
        RotationSpeed = rotationSpeed;
    }
}
