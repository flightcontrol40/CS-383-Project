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
	private TowerType selectedTowerType;

	private enum TowerType { Basic, Sniper, Cannon }

	public override void _Ready()
	{
		// Get references to UI elements
		moneyLabel = GetNode<Label>("Shop Panel/MoneyLabel");
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
		SelectTower(TowerType.Basic, basicTowerPrice);
	}

	// Signal handler for Sniper Tower Button
	private void _on_SniperTowerButton_pressed()
	{
		SelectTower(TowerType.Sniper, sniperTowerPrice);
	}

	// Signal handler for Cannon Tower Button
	private void _on_CannonTowerButton_pressed()
	{
		SelectTower(TowerType.Cannon, cannonTowerPrice);
	}

	// Function to select a tower type and initiate the purchase
	private void SelectTower(TowerType tType, int towerPrice)
	{
		selectedTowerType = tType;
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
			BaseTower towerInstance = CreateTowerInstance(selectedTowerType);
			if (towerInstance != null)
			{
				EmitSignal(SignalName.TowerBought);
				GD.Print($"{selectedTowerType} purchased and ready to be placed!");

				// Add the tower instance to the scene (example logic for adding it)
				AddSibling(towerInstance); // Modify this if you have a specific area for tower placement
				var main = (GetParent() as Main);
				main.newTower = towerInstance;
				main.PlacingTurret = true;
			}
		}
		else
		{
			GD.Print("Not enough money!");
		}
	}

	// Factory method to create tower instances based on type name
	private BaseTower CreateTowerInstance(TowerType tType)
	{
		BaseTower newTower;
		switch (tType)
		{
			case TowerType.Basic:
				newTower = GD.Load<PackedScene>("res://src/Ankit/Scenes/tower1.tscn").Instantiate() as Tower1;
				break;
			case TowerType.Sniper:
				newTower = GD.Load<PackedScene>("res://src/Ankit/Scenes/tower2.tscn").Instantiate() as Tower2;
				break;
			case TowerType.Cannon:
				newTower = GD.Load<PackedScene>("res://src/Ankit/Scenes/tower3.tscn").Instantiate() as Tower3;
				break;
			default:
				GD.Print("Unknown tower type.");
				return null;
		}
		return newTower ?? null;
	}
}
