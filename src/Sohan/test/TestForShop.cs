using Godot;
using GdUnit4;
using static GdUnit4.Assertions;

namespace YourGameTests
{
    [TestSuite]
    public partial class ShopTests : Node
    {
        private Shop shop;

        [Before]
        public void SetUp()
        {
            GD.Print("Setting up ShopTests...");

            // Initialize Shop and add it to the scene tree
            shop = new Shop();
            AddChild(shop);
            shop._Ready();  // Ensure Shop's _Ready method is called
        }

        [TestCase]
        public void InitialMoneyTest()
        {
            GD.Print("Running InitialMoneyTest...");
            AssertThat(GetPlayerMoney()).IsEqual(500);
        }

        [TestCase]
        public void BuyBasicTowerTest()
        {
            GD.Print("Running BuyBasicTowerTest...");
            int initialMoney = GetPlayerMoney();

            // Call the method to simulate buying a basic tower
            shop.Call("_on_BasicTowerButton_pressed");

            // Verify that money was deducted correctly
            AssertThat(GetPlayerMoney()).IsEqual(initialMoney - 100);
        }

        [TestCase]
        public void BuySniperTowerTest()
        {
            GD.Print("Running BuySniperTowerTest...");
            int initialMoney = GetPlayerMoney();

            // Call the method to simulate buying a sniper tower
            shop.Call("_on_SniperTowerButton_pressed");

            // Verify that money was deducted correctly
            AssertThat(GetPlayerMoney()).IsEqual(initialMoney - 150);
        }

        [TestCase]
        public void BuyCannonTowerTest()
        {
            GD.Print("Running BuyCannonTowerTest...");
            int initialMoney = GetPlayerMoney();

            // Call the method to simulate buying a cannon tower
            shop.Call("_on_CannonTowerButton_pressed");

            // Verify that money was deducted correctly
            AssertThat(GetPlayerMoney()).IsEqual(initialMoney - 200);
        }

        [TestCase]
        public void InsufficientMoneyTest()
        {
            GD.Print("Running InsufficientMoneyTest...");

            // Set player money to a low amount (e.g., 50)
            SetPlayerMoney(50);

            // Attempt to buy a cannon tower, which costs 200
            shop.Call("_on_CannonTowerButton_pressed");

            // Verify that money was not deducted
            AssertThat(GetPlayerMoney()).IsEqual(50);
        }

        // Boundary Test: Exactly Enough Money to Buy a Tower
        [TestCase]
        public void BoundaryExactMoneyForCannonTowerTest()
        {
            GD.Print("Running BoundaryExactMoneyForCannonTowerTest...");

            // Set player money to the exact price of a cannon tower
            SetPlayerMoney(200);

            // Attempt to buy a cannon tower
            shop.Call("_on_CannonTowerButton_pressed");

            // Verify that money is now zero after purchase
            AssertThat(GetPlayerMoney()).IsEqual(0);
        }

        


        [TestCase]
        public void SelectTowerTypeTest()
        {
            GD.Print("Running SelectTowerTypeTest...");
            shop.Call("_on_SniperTowerButton_pressed");

            string selectedTowerType = (string)shop.Get("selectedTowerType");
            AssertThat(selectedTowerType).IsEqual("SniperTower").OverrideFailureMessage("Expected tower type 'SniperTower' but got a different value.");
        }






        














        [TestCase]
        public void InsufficientMoneyForTowerPurchaseTest()
        {
            GD.Print("Running InsufficientMoneyForTowerPurchaseTest...");

            // Set player money to a low amount that is insufficient for any tower purchase, e.g., 50.
            SetPlayerMoney(50);

            // Try to buy a basic tower, which costs 100.
            shop.Call("_on_BasicTowerButton_pressed");

            // Verify that the money was not deducted because the player didn't have enough money.
            AssertThat(GetPlayerMoney()).IsEqual(50);
        }


        




        [TestCase]
        public void IntensiveStressTestMultiplePurchases()
        {
            GD.Print("Running IntensiveStressTestMultiplePurchases...");

            // Set an extremely high number of iterations for the stress test
            int stressIterations = 50000; // Increased iterations for extended runtime
            SetPlayerMoney(1000000); // Large initial amount to allow continuous purchases

            for (int i = 0; i < stressIterations; i++)
            {
                // Attempt to buy a basic tower each iteration
                shop.Call("_on_BasicTowerButton_pressed");

                // Check if player money remains non-negative after each purchase
                int currentMoney = GetPlayerMoney();
                AssertThat(currentMoney >= 0).IsTrue(); // Ensure money doesn’t go below zero

                // Introduce a small delay to increase the stress on the system
                OS.DelayMsec(1); // 1 millisecond delay per iteration adds up over many iterations

                // Debug print at intervals to monitor progress and performance
                if (i % 1000 == 0)
                {
                    GD.Print($"Iteration {i}: Current money = {currentMoney}");
                }
            }

            GD.Print("IntensiveStressTestMultiplePurchases completed without errors.");
        }





        [After]
        public void TearDown()
        {
            GD.Print("Tearing down ShopTests...");
            shop.QueueFree();
        }

        // Helper method to access the playerMoney field directly
        private int GetPlayerMoney()
        {
            return (int)shop.Get("playerMoney");
        }

        // Helper method to set the playerMoney field directly
        private void SetPlayerMoney(int amount)
        {
            shop.Set("playerMoney", amount);
            shop.Call("UpdateMoneyLabel");
        }
    }
}
