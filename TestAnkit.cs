namespace GdMUT;
using Godot;
using System;

/// <summary>
/// This is a test class for GDMUT. This is purely for demonstration. If you added
/// this into your project, feel free to delete it =).
/// </summary>

public static class TestAnkit
{

private const string BULLET_TEST = "res://src/Ankit/Scenes/Bullet.tscn";
private const string TOWER_TEST = "res://src/Ankit/Scenes/Tower.tscn";


#if TOOLS
	/// <summary>
	/// An example of a result that will pass.
	/// </summary>
	/// <returns>The result.</returns>
	// [CSTestFunction]
	// public static Result ExamplePass()
	// {
	// 	int x = 0;
	// 	x *= 100;
	// 	return (x == 0) ? Result.Success : Result.Failure;
	// }


[CSTestFunction]
    public static Result TestBulletInitialization()
    {
        var bulletScene = GD.Load<PackedScene>(BULLET_TEST);
        if (bulletScene == null)
            return new Result(false, "Failed to load Bullet scene.");

        var bullet = (Bullet)bulletScene.Instantiate();
        if (bullet == null)
            return new Result(false, "Failed to instantiate Bullet from the scene.");

        return (bullet.speed == 3.0f) 
            ? new Result(true, "Bullet is correctly initialized with speed 3.0f.")
            : new Result(false, "Bullet speed initialization is incorrect.");
    }

[CSTestFunction]
public static Result TestBulletMovement()
{
    var bulletScene = GD.Load<PackedScene>(BULLET_TEST);
    var bullet = (Bullet)bulletScene.Instantiate();

    var mockTarget = new Node2D();
    bullet.AddChild(mockTarget); // Ensure the target is part of the scene tree if needed
    mockTarget.GlobalPosition = new Vector2(100, 100);
    bullet.target = mockTarget;

    bullet._Ready();

    var initialPosition = bullet.GlobalPosition;
    bullet._PhysicsProcess(1/60.0);  // Simulate one frame of physics

    var moved = bullet.GlobalPosition != initialPosition;

    GD.Print("Initial Position: ", initialPosition);
    GD.Print("Post-Physics Position: ", bullet.GlobalPosition);

    return moved
        ? new Result(true, "Bullet moves correctly towards target.")
        : new Result(false, "Bullet did not move as expected. It remained at " + bullet.GlobalPosition.ToString());
}

[CSTestFunction]
public static Result TestBulletStress_TowerShoot()
{
    // Load the Tower scene
    var towerScene = GD.Load<PackedScene>(TOWER_TEST);  // Update with actual tower scene path
    if (towerScene == null)
        return new Result(false, "Failed to load Tower scene.");

    // Instantiate the tower
    var tower = (Tower)towerScene.Instantiate();
    if (tower == null)
        return new Result(false, "Failed to instantiate Tower from the scene.");

    int bulletCount = 1000000;  // Number of bullets to shoot
    var stopwatch = new System.Diagnostics.Stopwatch();  // To measure the time

    // Start the timer
    stopwatch.Start();

    // Simulate shooting 1,000,000 bullets
    for (int i = 0; i < bulletCount; i++)
    {
        tower._on_ShootTimer_timeout();  // Call the bullet shooting method
    }

    // Stop the timer
    stopwatch.Stop();
    
    // Check if the time is within acceptable limits (for stress test performance)
    long elapsedMilliseconds = stopwatch.ElapsedMilliseconds;
    GD.Print("Time taken for 1,000,000 bullet shots: ", elapsedMilliseconds, " ms");

    // You can set a reasonable limit for performance testing
    if (elapsedMilliseconds < 60000)  // Assuming 60 seconds as a performance benchmark
    {
        return new Result(true, $"Stress test passed! 1,000,000 bullet shots: Time taken: {elapsedMilliseconds} ms.");
    }
    else
    {
        return new Result(false, $"Stress test failed. Time taken is too long: {elapsedMilliseconds} ms.");
    }
}

	
#endif
}
