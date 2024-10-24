using Godot;
using System;

public partial class Tutorial : Container
{

    bool needingFirstButtonPress = true;
    bool needingTowerPlace = false;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode<Label>("Label").Text = "Click the Basic Tower button to place a tower";
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }

    public override void _Input(InputEvent @event)
    {
        if (needingTowerPlace && @event is InputEventMouseButton eventMouseButton) {
            Hide();
        }
    }

    public void _on_shop_tower_bought() {
        if (needingFirstButtonPress) {
            GetNode<Label>("Label").Text = "Click somewhere on the screen to place a tower";
            needingFirstButtonPress = false;
            needingTowerPlace = true;
        }
    }

}
