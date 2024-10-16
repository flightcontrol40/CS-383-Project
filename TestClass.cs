namespace GdMUT;
using Chicken;
using Godot;
using System;

/// <summary>
/// This is a test class for GDMUT. This is purely for demonstration. If you added
/// this into your project, feel free to delete it =).
/// </summary>

public static class TestClass
{

#if TOOLS
    private const string TEST_CHICKEN = "res://src/Clayton/Enemy/chicken_enemy.tscn";
    private static Austins_Test.LevelManagerTests levelTester = new Austins_Test.LevelManagerTests();

    /// <summary>
    /// An example of a result that will pass.
    /// </summary>
    /// <returns>The result.</returns>
    [CSTestFunction]
    public static Result ExamplePass()
    {
        int x = 0;
        x *= 100;
        return (x == 0) ? Result.Success : Result.Failure;
    }

    /// <summary>
    /// An example of a result that will fail.
    /// </summary>
    /// <returns>The result.</returns>
    [CSTestFunction]
    public static Result ExampleFail()
    {
        int x = 0;
        x *= 100;
        return (x != 0) ? Result.Success : Result.Failure;
    }

    /// <summary>
    /// An example of a result that will fail with a custom message.
    /// </summary>
    /// <returns>The result.</returns>
    [CSTestFunction]
    public static Result ExampleCustomFail()
    {
        int x = 0;
        x *= 100;
        return (x != 0)
            ? Result.Success
            : new Result(false, "You can't multiply 0 and expect anything else than 0!");
    }

    /// <summary>
    /// An example of a result that will pass with a custom message.
    /// </summary>
    /// <returns>The result.</returns>
    [CSTestFunction]
    public static Result ExampleCustomSuccess()
    {
        int x = 0;
        x *= 100;
        return (x == 0) ? new Result(true, "Proved that 0 * 100 = 0") : Result.Failure;
    }
    
    [CSTestFunction]
        public static Result ChickenTest()
    {
        try
        {
            var testResulScene = GD.Load<PackedScene>(TEST_CHICKEN);
            if (testResulScene == null)
                return new Result(false, "Failed to load TEST_CHICKEN scene.");
        
            var testResul = (BaseChicken)testResulScene.Instantiate();
            if (testResul == null)
                return new Result(false, "Failed to instantiate BaseChicken from the scene.");
        
            int x = testResul.Health;
    
            return (x == 100) ? new Result(true, "Proved that 100 = 100") : Result.Failure;
        }
        catch (Exception ex)
        {
            return new Result(false, $"Exception occurred: {ex.Message}");
        }
    }

    //-----Austin's Tests-----
    
    //Boundary Tests
    /// <summary>
    /// Tests that a valid instance of a map exists once it gets laoded
    /// </summary>
    /// <returns>Success if current map is a valid instance and failure otherwise</returns>
    [CSTestFunction] public static Result loadedMapValid() {
        return levelTester.loadMap_test();
    }

    /// <summary>
    /// Tests that the player's health stays greater than or equal to 0
    /// </summary>
    /// <returns>Success if player's health >= 0 and failure otherwise, with a note of what the player's heatlh was</returns>
    [CSTestFunction] public static Result playerHealthStayPositive() {
        return levelTester.playerHealth_test();
    }

    //Stress Test

#endif
}
