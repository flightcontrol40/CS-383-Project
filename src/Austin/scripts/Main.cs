using Godot;
using System;

public partial class Main : Node
{
	[Export]
	private PackedScene tower;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton eventMouseButton) {
			placeTower(eventMouseButton.Position);
		}
    }

    public void placeTower(Vector2 position) {
		Node2D newTower = (Node2D)tower.Instantiate();
		newTower.GlobalPosition = position;

		AddChild(newTower);
	}
}
