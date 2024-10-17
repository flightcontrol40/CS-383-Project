using Godot;
using System;

public partial class Bullet : Area2D
{
	private Vector2 move = Vector2.Zero;
	public float speed = 3.0f;
	private Vector2 lookVec = Vector2.Zero;
	public Node2D target; 
	public Sprite2D sprite2D;

	
public override void _PhysicsProcess(double delta)
{
    GD.Print("Current Position: ", GlobalPosition);  // Debug output
    if (target != null)
    {
        lookVec = target.GlobalPosition - GlobalPosition;
        GD.Print("Target Position: ", target.GlobalPosition);  // Debug output
        GD.Print("Look Vector: ", lookVec);  // Debug output

        if (lookVec.Length() > 0)
        {
            move = lookVec.Normalized() * speed;
            GlobalPosition += move * (float)delta;
            GD.Print("Moved To: ", GlobalPosition);  // Debug output
        }
        else
        {
            GD.Print("No Movement: Look Vector is Zero Length");  // No movement
        }
    }
    else
    {
        GD.Print("Target is null");  // Check target initialization
    }
}



	public void UpdateLookVector()
{
    if (target != null)
    {
        lookVec = target.GlobalPosition - GlobalPosition;
        sprite2D.LookAt(target.GlobalPosition);
    }
}
}
