using Godot;
using System;

public partial class Main : Node
{
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode<Timer>("MoneyTimer").Start(0.1);

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
        if (Input.IsActionJustPressed("loadLevel")) {
            GetNode<LevelManager>("LevelManager").setDifficutly(RoundManager.Difficulty.Easy);
            GetNode<LevelManager>("LevelManager").OnLoadLevel();
        }
    }

    public void _on_shop_tower_bought() {
    }

    public void _on_money_timer_timeout() {
        GetNode<Timer>("MoneyTimer").Start(0.1);
    }
}
