using Godot;
using System;

public class Bullet : Area2D
{
    [Export]
    public float Speed = 400;  // Speed of the bullet, can be adjusted in the Godot editor.

    // This method initializes any necessary properties or effects for the bullet when it's ready.
    public override void _Ready()
    {
        Connect("body_entered", this, nameof(OnBodyEntered));  // Connect signal for collision with another body.
        Connect("screen_exited", this, nameof(OnScreenExited)); // Connect signal for bullet exiting the screen.
    }

    // This method updates the bullet's position each frame.
    public override void _Process(double delta)
    {
        // Calculate the velocity vector based on the bullet's current rotation and the desired speed.
        Vector2 velocity = new Vector2(Speed, 0).Rotated(Rotation);
        Position += velocity * (float)delta;  // Update the position of the bullet based on its velocity and delta time.
    }

    // This method handles what happens when the bullet collides with another body.
    private void OnBodyEntered(Node body)
    {
        if (body is Enemy)  // Check if the collided body is an Enemy.
        {
            // Add interaction with enemy here, such as reducing health or triggering effects.
            QueueFree();  // Remove the bullet from the scene after hitting an enemy.
        }
    }

    // This method is called when the bullet exits the screen area, used to clean up off-screen bullets.
    private void OnScreenExited()
    {
        QueueFree();  // Free up the bullet from memory to prevent off-screen resource usage.
    }
}
