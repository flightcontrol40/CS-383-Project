using Godot;
using System.Collections.Generic;

public partial class Tower : Node2D
{
	[Export]
	public NodePath TowerPath;
	[Export]
	public NodePath TowerHeadPath;
	[Export]
	public PackedScene BulletScene;

	private Node2D Tower2;
	private Node2D TowerHead;
	private List<Area2D> enemies = new List<Area2D>();
	private bool building = true;
	private bool canPlace = false;
	private Area2D currentEnemy;
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
			range.Visible = false;
			if (enemies.Count > 0)
			{
				currentEnemy = enemies[0];
				TowerHead.LookAt(currentEnemy.GlobalPosition);
			}
		}
		else
		{
			range.Visible = true;
			GlobalPosition = GetGlobalMousePosition();
			if (canPlace)
			{
				range.Modulate = new Color(0, 0, 0, 1);
				if (Input.IsActionJustPressed("click"))
				{
					building = false;
					GetParent().Call("tower_built");
				}
			}
			else
			{
				range.Modulate = new Color(1, 1, 1, 1);
			}
		}
	}

	private void _on_Sight_area_entered(Area2D area)
	{
		if (area.IsInGroup("Enemy") && !building)
		{
			enemies.Add(area);
		}
	}

	private void _on_Sight_area_exited(Area2D area)
	{
		if (area.IsInGroup("Enemy") && !building)
		{
			enemies.Remove(area);
		}
	}

	private void _on_ShootTimer_timeout()
	{
		if (!building && currentEnemy != null && enemies.Count > 0)
		{
			if (currentEnemy == enemies[0])
			{
				Node2D bullet = (Node2D)BulletScene.Instantiate();
				bullet.GlobalPosition = GlobalPosition;
				bullet.Set("target", currentEnemy);  // Make sure the 'target' variable is accessible in the Bullet script
				GetParent().AddChild(bullet);
			}
		}
	}

	private void _on_placement_area_entered(Area2D area)
	{
		if (area.IsInGroup("AddPlatform") && building)
		{
			canPlace = true;
		}
	}

	private void _on_placement_area_exited(Area2D area)
	{
		if (area.IsInGroup("AddPlatform") && building)
		{
			canPlace = false;
		}
	}
}
