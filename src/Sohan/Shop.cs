using Godot;
using System;

public partial class Shop : Node
{
	
	[Signal]
	
	public delegate void TowerBoughtEventHandler();
	
	// Reference to the player's money
	public int playerMoney = 500; // Starting money

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
		moneyLabel = GetNode<Panel>("Panel").GetNode<Label>("Label");
	}

    public override void _Process(double delta)
    {
		UpdateMoneyLabel();
    }

    private void UpdateMoneyLabel()
	{
		// Update the player's money display
		moneyLabel.Text = $"Money: ${playerMoney}";
	}

	// Signal handler for Basic Tower Button
	public void _on_basic_tower_pressed()
	{
		SelectTower("BasicTower", basicTowerPrice);
	}

	// Signal handler for Sniper Tower Button
	private void _on_sniper_tower_button_pressed()
	{
		SelectTower("SniperTower", sniperTowerPrice);
	}

	// Signal handler for Cannon Tower Button
	private void _on_cannon_tower_button_pressed()
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
		GD.Print(playerMoney);
		if (playerMoney >= towerPrice)
		{
			playerMoney -= towerPrice;

			EmitSignal(SignalName.TowerBought);
			
			GD.Print($"{selectedTowerType} purchased and ready to be placed!");
		}
		else
		{
			GD.Print("Not enough money!");
		}
	}

}
