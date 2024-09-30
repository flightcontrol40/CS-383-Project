using Godot;
using System;

public partial class Area2DExample : Area2D
{
	private Vector2 move = Vector2.Zero;
	private float speed = 3.0f;
	private Vector2 lookVec = Vector2.Zero;
	private Node2D target;
	private Sprite2D sprite2D;

	public override void _Ready()
	{
		sprite2D = GetNode<Sprite2D>("Sprite2D");  // Adjust the path as necessary.
		target = GetNode<Node2D>("path/to/your/target") as Node2D;  // Specify the correct node path.
		
		if (target != null)
		{
			sprite2D.LookAt(target.GlobalPosition);
			lookVec = target.GlobalPosition - GlobalPosition;
		}
	}

	public override void _PhysicsProcess(float delta)
	{
		move = Vector2.Zero;
		if (target != null)
		{
			move = move.MoveToward(lookVec, speed * delta);
			move = move.Normalized() * speed;
			GlobalPosition += move;
		}
	}
}
