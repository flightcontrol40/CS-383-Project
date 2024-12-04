// namespace GdMUT;
// using Chicken;
// using Godot;
// using System;
// using System.Diagnostics;

// /// <summary>
// /// This is a test class for GDMUT. This is purely for demonstration. If you added
// /// this into your project, feel free to delete it =).
// /// </summary>

// public static class TestSohan
// {

// private const string TEST_CHICKEN = "res://src/Clayton/Enemy/chicken_enemy.tscn";
// #if TOOLS
//     /// <summary>
//     /// An example of a result that will pass.
//     /// </summary>
//     /// <returns>The result.</returns>
    

// [CSTestFunction]
// public static Result ShopBoundaryTest()
// {
//     try
//     {
//         // Create an instance of the Shop class for testing
//         var shopScene = GD.Load<PackedScene>("res://src/Sohan/Shop.tscn");
//         if (shopScene == null)
//             return new Result(false, "Failed to load Shop scene.");

//         var shopInstance = (Shop)shopScene.Instantiate();
//         if (shopInstance == null)
//             return new Result(false, "Failed to instantiate Shop from the scene.");

//         // Test 1: Player has exactly enough money to buy a Basic Tower
//         shopInstance.Set("playerMoney", 100);
//         shopInstance.Call("BuyTower", 100);  // Call BuyTower with 100 price
//         int moneyAfterFirstTest = (int)shopInstance.Get("playerMoney");
//         if (moneyAfterFirstTest != 0)
//             return new Result(false, $"Test 1 Failed: Expected 0, Got {moneyAfterFirstTest}");

//         // Test 2: Player tries to buy a Sniper Tower with insufficient funds
//         shopInstance.Set("playerMoney", 140);
//         shopInstance.Call("BuyTower", 150);  // Call BuyTower with 150 price
//         int moneyAfterSecondTest = (int)shopInstance.Get("playerMoney");
//         if (moneyAfterSecondTest != 140)
//             return new Result(false, $"Test 2 Failed: Expected 140, Got {moneyAfterSecondTest}");

//         return new Result(true, "Both boundary tests for Shop passed successfully.");
//     }
//     catch (Exception ex)
//     {
//         return new Result(false, $"Exception occurred: {ex.Message}");
//     }
// }


// [CSTestFunction]
// public static Result EnsureInitialHealthIs100()
// {
//     try
//     {
//         // Load the PlayerHealth scene
//         var healthBarScene = GD.Load<PackedScene>("res://src/Sohan/PlayerHealth.tscn");
//         if (healthBarScene == null)
//             return new Result(false, "Failed to load PlayerHealth scene.");

//         // Instantiate the PlayerHealth scene
//         var sceneInstance = healthBarScene.Instantiate();
//         if (sceneInstance == null)
//             return new Result(false, "Failed to instantiate PlayerHealth scene.");

//         // Get the HealthBar node from the instantiated scene
//         var healthBarInstance = sceneInstance.GetNode<HealthBar>("HealthBar");
//         if (healthBarInstance == null)
//             return new Result(false, "Failed to find HealthBar node in the scene.");

//         // Access the initial health value
//         int initialHealth = (int)healthBarInstance.Get("currentHealth");

//         // Check if the health is correctly set to 100
//         return (initialHealth == 100)
//             ? new Result(true, "Initial health is correctly set to 100.")
//             : new Result(false, $"Test Failed: Expected 100, but got {initialHealth}");
//     }
//     catch (Exception ex)
//     {
//         return new Result(false, $"Exception occurred: {ex.Message}");
//     }
// }


// [CSTestFunction]
// public static Result UIStressTest()
// {
//     try
//     {
//         // Load the Shop scene
//         var shopScene = GD.Load<PackedScene>("res://src/Sohan/Shop.tscn"); // Adjust the path if necessary
//         if (shopScene == null)
//             return new Result(false, "Failed to load Shop scene.");

//         // Instantiate the Shop node
//         var shop = shopScene.Instantiate() as Shop;
//         if (shop == null)
//             return new Result(false, "Failed to instantiate Shop.");

//         // Start stopwatch to measure time taken for the stress test
//         Stopwatch stopwatch = new Stopwatch();
//         stopwatch.Start();

//         // Perform stress test to purchase 500,000 Basic Towers
//         for (int i = 0; i < 500000; i++)
//         {
//             shop.Call("_on_BasicTowerButton_pressed");
//         }

//         // Stop the stopwatch once the loop completes
//         stopwatch.Stop();

//         // Get the elapsed time in seconds
//         double elapsedSeconds = stopwatch.Elapsed.TotalSeconds;

//         return new Result(true, $"Stress test for buying 500,000 towers completed successfully in {elapsedSeconds:F2} seconds.");
//     }
//     catch (Exception ex)
//     {
//         return new Result(false, $"Exception occurred: {ex.Message}");
//     }
// }

    
// #endif
// }
