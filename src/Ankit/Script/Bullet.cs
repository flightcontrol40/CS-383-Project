using Godot;
using System;
using Chicken; // Static binding: Compiles a reference to the Chicken namespace containing enemy classes. Ensure the namespace matches your project's structure.

// The 'Bullet' class is declared as 'partial', allowing its definition to be split across multiple files if needed.
public partial class Bullet : Area2D // Subclass: 'Bullet' inherits from 'Area2D', making 'Area2D' the superclass.
{
    [Export]
    public float Speed = 400; // Speed at which the bullet moves, settable in the editor (dynamic binding).

    public Vector2 Direction; // The direction for the bullet to travel, set at runtime (dynamic binding).

    public override void _Ready() // Overrides the _Ready method from Area2D, called when the node is added to the scene.
    {
        Connect("body_entered", new Callable(this, nameof(OnBodyEntered))); // Dynamically binds the 'body_entered' signal to the 'OnBodyEntered' method.
        Connect("screen_exited", new Callable(this, nameof(OnScreenExited))); // Dynamically binds the 'screen_exited' signal to the 'OnScreenExited' method.
    }

    public override void _Process(double delta) // Overrides the _Process method from Area2D, called every frame the node is active.
    {
        Position += Direction * (float)(Speed * delta); // Updates position based on direction and speed, casting delta to float to match Godot's requirements.
    }

    private void OnBodyEntered(Node body) // Called dynamically when another body enters the bullet's area.
    {
        if (body is BaseChicken chicken) // Dynamic check to see if the object is of type BaseChicken.
        {
            chicken.TakeDamage(10); // Calls the TakeDamage method on the chicken, assuming such a method exists and is public.
            QueueFree(); // Frees the bullet node from memory and removes it from the scene, cleaning up resources.
        }
    }

    private void OnScreenExited() // Called dynamically when the bullet exits the screen area.
    {
        QueueFree(); // Frees the bullet node from memory and removes it from the scene, similar to above.
    }
}
