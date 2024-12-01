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
    private int rapidTowerPrice = 150;
    private int heavyTowerPrice = 200;
    private int sniperTowerPrice = 250;
    private int speedTowerPrice = 175;

    // Track the selected tower type for purchase
    private TowerType selectedTowerType;

    private enum TowerType
    {
        Basic,      // Tower1
        Rapid,      // Tower2
        Heavy,      // Tower3
        Sniper,     // Tower4
        Speed       // Tower5
    }

    public override void _Ready()
    {
        // Get references to UI elements
        moneyLabel = GetNode<Label>("Shop Panel/MoneyLabel");
        UpdateMoneyLabel();
    }

    private void UpdateMoneyLabel()
    {
        moneyLabel.Text = $"Money: ${playerMoney}";
    }

    // Signal handlers for each tower button
    private void _on_BasicTowerButton_pressed()
    {
        SelectTower(TowerType.Basic, basicTowerPrice);
    }

    private void _on_RapidTowerButton_pressed()
    {
        SelectTower(TowerType.Rapid, rapidTowerPrice);
    }

    private void _on_HeavyTowerButton_pressed()
    {
        SelectTower(TowerType.Heavy, heavyTowerPrice);
    }

    private void _on_SniperTowerButton_pressed()
    {
        SelectTower(TowerType.Sniper, sniperTowerPrice);
    }

    private void _on_SpeedTowerButton_pressed()
    {
        SelectTower(TowerType.Speed, speedTowerPrice);
    }

    private void SelectTower(TowerType tType, int towerPrice)
    {
        selectedTowerType = tType;
        BuyTower(towerPrice);
    }

    private void BuyTower(int towerPrice)
    {
        if (playerMoney >= towerPrice)
        {
            playerMoney -= towerPrice;
            UpdateMoneyLabel();

            BaseTower towerInstance = CreateTowerInstance(selectedTowerType);
            if (towerInstance != null)
            {
                EmitSignal(SignalName.TowerBought);
                GD.Print($"{selectedTowerType} Tower purchased and ready to be placed!");

                AddSibling(towerInstance);
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

    private BaseTower CreateTowerInstance(TowerType tType)
    {
        BaseTower newTower;
        switch (tType)
        {
            case TowerType.Basic:
                newTower = GD.Load<PackedScene>("res://src/Ankit/Scenes/tower1.tscn").Instantiate() as Tower1;
                break;
            case TowerType.Rapid:
                newTower = GD.Load<PackedScene>("res://src/Ankit/Scenes/tower2.tscn").Instantiate() as Tower2;
                break;
            case TowerType.Heavy:
                newTower = GD.Load<PackedScene>("res://src/Ankit/Scenes/tower3.tscn").Instantiate() as Tower3;
                break;
            case TowerType.Sniper:
                newTower = GD.Load<PackedScene>("res://src/Ankit/Scenes/tower4.tscn").Instantiate() as Tower4;
                break;
            case TowerType.Speed:
                newTower = GD.Load<PackedScene>("res://src/Ankit/Scenes/tower5.tscn").Instantiate() as Tower5;
                break;
            default:
                GD.Print("Unknown tower type.");
                return null;
        }
        return newTower ?? null;
    }
}