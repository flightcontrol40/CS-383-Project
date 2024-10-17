using Godot;
using System.Collections.Generic;

public partial class Tower : Node2D
{
    [Export]
    public NodePath TowerPath;   // Path to the main body of the tower
    [Export]
    public NodePath TowerHeadPath;   // Path to the rotating head of the tower
    [Export]
    public PackedScene BulletScene;  // Packed scene for the bullet

    private Node2D Tower2;
    private Node2D TowerHead;
    public List<Area2D> enemies = new List<Area2D>();  // List of enemies in range
    public bool building = true;
    public bool canPlace = false;
    public Area2D currentEnemy;
    private VisibleOnScreenNotifier2D range;

    public override void _Ready()
    {
        Tower2 = GetNode<Node2D>(TowerPath);
        TowerHead = GetNode<Node2D>(TowerHeadPath);
        range = GetNode<VisibleOnScreenNotifier2D>("Range");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!building)
        {
            range.Visible = false;  // Hide the range when the tower is built

            if (enemies.Count > 0)
            {
                currentEnemy = enemies[0];  // Get the closest enemy
                TowerHead.LookAt(currentEnemy.GlobalPosition);  // Rotate tower head towards the enemy
            }
        }
        else
        {
            range.Visible = true;  // Show range during tower placement
            GlobalPosition = GetGlobalMousePosition();  // Follow mouse for placement

            if (canPlace)
            {
                range.Modulate = new Color(0, 0, 0, 1);  // Change color when placement is valid
                if (Input.IsActionJustPressed("click"))
                {
                    building = false;
                    GetParent().Call("tower_built");
                }
            }
            else
            {
                range.Modulate = new Color(1, 1, 1, 1);  // Reset color when placement is invalid
            }
        }
    }

    // Enemy enters the sight range of the tower
    public void _on_Sight_area_entered(Area2D area)
    {
        if (area.IsInGroup("Enemy") && !building)
        {
            enemies.Add(area);  // Add enemy to the list
        }
    }

    // Enemy exits the sight range of the tower
    public void _on_Sight_area_exited(Area2D area)
    {
        if (area.IsInGroup("Enemy") && !building)
        {
            enemies.Remove(area);  // Remove enemy from the list
        }
    }

    // Shoot a bullet towards the current enemy
    public void _on_ShootTimer_timeout()
    {
        if (!building && currentEnemy != null && enemies.Count > 0)
        {
            // Ensure we are targeting the closest enemy
            if (currentEnemy == enemies[0])
            {
                // Instantiate a bullet and set its position to the tower's position
                Node2D bullet = (Node2D)BulletScene.Instantiate();
                bullet.GlobalPosition = TowerHead.GlobalPosition;  // Set the bullet to shoot from the tower head

                // Pass the enemy target to the bullet (ensure your Bullet script has a target variable)
                bullet.Set("target", currentEnemy);
                GetParent().AddChild(bullet);  // Add the bullet to the scene

                GD.Print("Bullet shot towards enemy at: ", currentEnemy.GlobalPosition);  // Debugging log
            }
        }
    }

    // Handle placement zones
    public void _on_placement_area_entered(Area2D area)
    {
        if (area.IsInGroup("AddPlatform") && building)
        {
            canPlace = true;  // Allow placement when in the right area
        }
    }

    // Handle exiting placement zones
    public void _on_placement_area_exited(Area2D area)
    {
        if (area.IsInGroup("AddPlatform") && building)
        {
            canPlace = false;  // Disable placement when leaving the valid zone
        }
    }
}
