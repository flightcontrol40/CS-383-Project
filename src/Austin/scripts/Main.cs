using Godot;
using System;

public partial class Main : Node
{
    [Export]
    private PackedScene tower;
    [Export]
    private bool towerPlaceable = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode<Timer>("MoneyTimer").Start(0.1);
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton eventMouseButton) {
            placeTower(eventMouseButton.Position);
        }
    }

    public void placeTower(Vector2 position) {
        if (towerPlaceable) {
            Node2D newTower = (Node2D)tower.Instantiate();
            newTower.GlobalPosition = position;

            AddChild(newTower);

            towerPlaceable = false;
        }
    }

    public void _on_shop_tower_bought() {
        towerPlaceable = true;

        GD.Print("Why hello there");
    }

    public void _on_money_timer_timeout() {
        //GetNode<Shop>("Shop").playerMoney += 1;
        GetNode<Timer>("MoneyTimer").Start(0.1);
    }
}
