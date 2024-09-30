using Godot;
using System;

public class Shop : Node
{
	// Reference to the player's money
	private int playerMoney = 500; // Starting money

	// UI Elements
	private Label moneyLabel;

	// Prices for each tower
	private int basicTowerPrice = 100;
	private int sniperTowerPrice = 150;
	private int cannonTowerPrice = 200;

	// Packed scene for tower placement placeholder
	private PackedScene towerPlaceholderScene = (PackedScene)ResourceLoader.Load("res://Scenes/TowerPlaceholder.tscn");

	// Signal for tracking which tower is selected
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
		SelectTower("BasicTower", basicTowerPrice);
	}

	// Signal handler for Sniper Tower Button
	private void _on_SniperTowerButton_pressed()
	{
		SelectTower("SniperTower", sniperTowerPrice);
	}

	// Signal handler for Cannon Tower Button
	private void _on_CannonTowerButton_pressed()
	{
		SelectTower("CannonTower", cannonTowerPrice);
	}

	// Function to select and display a tower for placement
	private void SelectTower(string towerType, int towerPrice)
	{
		selectedTowerType = towerType;
		BuyTower(towerPrice);
	}

	// Function to handle tower purchases and instantiate placeholder
	private void BuyTower(int towerPrice)
	{
		if (playerMoney >= towerPrice)
		{
			playerMoney -= towerPrice;
			UpdateMoneyLabel();

			// Instance a new tower placeholder and add it to the scene
			TowerPlacement newTowerPlaceholder = (TowerPlacement)towerPlaceholderScene.Instance();
			GetParent().AddChild(newTowerPlaceholder);
			newTowerPlaceholder.InitializeTower(selectedTowerType);
			
			GD.Print($"{selectedTowerType} purchased and ready to be placed!");
		}
		else
		{
			GD.Print("Not enough money!");
		}
	}
}
