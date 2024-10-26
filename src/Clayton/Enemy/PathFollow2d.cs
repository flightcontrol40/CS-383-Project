using Godot;
using System;

public partial class PathFollow2d : PathFollow2D
{
	private float speed = 0.1f; // Speed at which the sprite follows the path
	private float Offset = 0f;

	public override void _Process(double delta)
	{
		// Move the sprite along the path
		Offset += (float)(speed * delta);
		
		 GD.Print("Offset: ", Offset);
		// Increment unit_offset based on speed and delta time

		// Loop the sprite back to the start when it reaches the end of the path
		if (Offset >= 1f)
		{
			Offset = 0f; // Reset to the beginning of the path
		}

		// Update the position of the sprite to match the path
		UpdatePosition();
	}

	private void UpdatePosition()
	{
		// This will set the position of the sprite according to the current offset
		Position = GlobalPosition; // Update the position based on the PathFollow2D's global position

	}
}
