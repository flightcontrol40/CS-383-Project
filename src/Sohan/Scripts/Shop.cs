using Godot;
using System;

public partial class Shop : Node
{
	[Signal]
	public delegate void TowerBoughtEventHandler();

	// Starting money for the player
	private int playerMoney = 500;

	// UI element for displaying player's money
	private Label moneyLabel;

    // Prices for each tower type
    private int basicTowerPrice = 100;
    private int sniperTowerPrice = 150;
    private int cannonTowerPrice = 200;

    // Track the selected tower type for purchase
    private string selectedTowerType = "";

    public override void _Ready()
    {
        // Get references to UI elements
        moneyLabel = GetNode<Label>("Panel/MoneyLabel");
        UpdateMoneyLabel();
    }

    private void UpdateMoneyLabel()
    {
        // Update the player's money display
        moneyLabel.Text = $"Money: ${playerMoney}";
    }

    // Signal handler for Basic Tower Button
    private void _on_BasicTowerButton_pressed()
    {
        SelectTower("Tower1", basicTowerPrice);
    }

    // Signal handler for Sniper Tower Button
    private void _on_SniperTowerButton_pressed()
    {
        SelectTower("Tower2", sniperTowerPrice);
    }

    // Signal handler for Cannon Tower Button
    private void _on_CannonTowerButton_pressed()
    {
        SelectTower("Tower3", cannonTowerPrice);
    }

    // Function to select a tower type and initiate the purchase
    private void SelectTower(string towerType, int towerPrice)
    {
        selectedTowerType = towerType;
        BuyTower(towerPrice);
    }

    // Function to handle tower purchase and instantiation
    private void BuyTower(int towerPrice)
    {
        if (playerMoney >= towerPrice)
        {
            playerMoney -= towerPrice;
            UpdateMoneyLabel();

            // Use the TowerFactory to create an instance of the selected tower type
            Node2D towerInstance = CreateTowerInstance(selectedTowerType);
            if (towerInstance != null)
            {
                EmitSignal(SignalName.TowerBought);
                GD.Print($"{selectedTowerType} purchased and ready to be placed!");

                // Add the tower instance to the scene (example logic for adding it)
                AddChild(towerInstance); // Modify this if you have a specific area for tower placement
            }
        }
        else
        {
            GD.Print("Not enough money!");
        }
    }

    // Factory method to create tower instances based on type name
    private Node2D CreateTowerInstance(string towerType)
    {
        switch (towerType)
        {
            case "Tower1":
                return new Tower1();
            case "Tower2":
                return new Tower2();
            case "Tower3":
                return new Tower3();
            case "Tower4":
                return new Tower4();
            case "Tower5":
                return new Tower5();
            default:
                GD.Print("Unknown tower type.");
                return null;
        }
    }
}