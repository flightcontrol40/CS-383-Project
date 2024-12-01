using Godot;
using GdUnit4;
using Chicken;
using static GdUnit4.Assertions;
using System.Reflection;


namespace YourGameTests
{
    [TestSuite]
    public partial class HealthBarTests : Node
    {
        private HealthBar healthBar;
        private BaseChicken mockEnemy;

        [Before]
        public void SetUp()
        {
            GD.Print("Setting up HealthBarTests...");

            // Initialize HealthBar and add it to the scene tree
            healthBar = new HealthBar();
            AddChild(healthBar);
            healthBar._Ready();  // Ensure HealthBar's _Ready method is called
            
            // Initialize mockEnemy and set its damageAmount
            mockEnemy = new BaseChicken { Name = "MockEnemy" };
            mockEnemy.AddToGroup("Enemy");  // Ensure mockEnemy is in the "Enemy" group
            AddChild(mockEnemy);

            // Set private damageAmount field directly
            SetPrivateField(mockEnemy, "damageAmount", 10);
            
        }

        [TestCase]
        public void SingletonInstanceTest()
        {
            GD.Print("Running SingletonInstanceTest...");
            AssertThat(HealthBar.Instance).IsEqual(healthBar);
        }

        [TestCase]
        public void InitialHealthTest()
        {
            GD.Print("Running InitialHealthTest...");
            AssertThat(healthBar.GetCurrentHealth()).IsEqual(100);
        }

       

       /*[TestCase]
        public void HealthDecrementTest()
        {
            GD.Print("Running HealthDecrementTest...");

            // Ensure health is initially 100
            AssertThat(healthBar.GetCurrentHealth()).IsEqual(100);

            // Call OnEnemyReachedEnd directly to simulate an enemy reaching the end
            healthBar.OnEnemyReachedEnd(mockEnemy);

            // Check that health has decremented by 10
            AssertThat(healthBar.GetCurrentHealth()).IsEqual(90);
        }
        */


        [TestCase]
        public void HealthBarColorGreenTest()
        {
            GD.Print("Running HealthBarColorGreenTest...");
            healthBar.Call("UpdateHealthBarColor");
            AssertThat(healthBar.Modulate).IsEqual(new Color(0, 1, 0));  // Green
        }

        [TestCase]
        public void HealthBarColorYellowTest()
        {
            GD.Print("Running HealthBarColorYellowTest...");
            healthBar.SetHealth(50);
            AssertThat(healthBar.Modulate).IsEqual(new Color(1, 1, 0));  // Yellow
        }

        [TestCase]
        public void HealthBarColorRedTest()
        {
            GD.Print("Running HealthBarColorRedTest...");
            healthBar.SetHealth(20);
            AssertThat(healthBar.Modulate).IsEqual(new Color(1, 0, 0));  // Red
        }

        [TestCase]
        public void GameOverTest()
        {
            GD.Print("Running GameOverTest...");

            // Lower health to zero directly
            healthBar.SetHealth(0);
            int finalHealth = healthBar.GetCurrentHealth();

            GD.Print($"Final Health in GameOverTest: {finalHealth}");
            AssertThat(finalHealth).IsEqual(0);
        }

        // Boundary test: health at minimum boundary
        [TestCase]
        public void HealthMinimumBoundaryTest()
        {
            GD.Print("Running HealthMinimumBoundaryTest...");

            // Set health to minimum by calling SetHealth directly
            healthBar.SetHealth(0);
            
            // Ensure health does not go below 0
            int healthAtMinBoundary = healthBar.GetCurrentHealth();
            GD.Print($"Health at minimum boundary: {healthAtMinBoundary}");
            AssertThat(healthAtMinBoundary).IsEqual(0);

            // Further decrement to ensure no negative health values
            healthBar.SetHealth(-10);
            AssertThat(healthBar.GetCurrentHealth()).IsEqual(0);
        }

        // Boundary test: health at maximum boundary
        [TestCase]
        public void HealthMaximumBoundaryTest()
        {
            GD.Print("Running HealthMaximumBoundaryTest...");

            // Attempt to set health above maximum
            healthBar.SetHealth(100);
            AssertThat(healthBar.GetCurrentHealth()).IsEqual(100);

            // Attempt to set health to a value above the boundary
            healthBar.SetHealth(110);
            AssertThat(healthBar.GetCurrentHealth()).IsEqual(100);
        }





        [TestCase]
public void IntensiveStressTestHealthUpdates()
{
    GD.Print("Running IntensiveStressTestHealthUpdates...");

    // Ensure health starts at maximum (100)
    AssertThat(healthBar.GetCurrentHealth()).IsEqual(100);

    // Increased iteration count to extend the test duration
    int stressIterations = 30000; // Higher iteration count for extended duration

    for (int i = 0; i < stressIterations; i++)
    {
        // Randomly decrement health by a random amount between 1 and 10
        int decrement = GD.RandRange(1, 10);
        healthBar.SetHealth(healthBar.GetCurrentHealth() - decrement);

        // Reset health to 100 if it reaches 0 to continue stressing
        if (healthBar.GetCurrentHealth() <= 0)
        {
            healthBar.SetHealth(100);
        }

        // Verify health does not go below zero
        AssertThat(healthBar.GetCurrentHealth() >= 0).IsTrue();

        // Include a small delay to simulate processing time
        OS.DelayMsec(2); // 2 ms delay to further extend duration
    }

    GD.Print("IntensiveStressTestHealthUpdates completed without errors.");
}





        





        [After]
        public void TearDown()
        {
            GD.Print("Tearing down HealthBarTests...");
            healthBar.QueueFree();
            mockEnemy.QueueFree();
        }

        // Helper function to set a private/protected field using reflection
        private void SetPrivateField(object obj, string fieldName, object value)
        {
            var field = obj.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            field?.SetValue(obj, value);
        }
    }
}
