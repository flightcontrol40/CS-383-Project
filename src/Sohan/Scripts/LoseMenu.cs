using Godot;

public partial class LoseMenu : Control
{
    private Button restartButton;
    private Button exitButton;

    public override void _Ready()
    {
        restartButton = GetNode<Button>("VBoxContainer/Button");
        exitButton = GetNode<Button>("VBoxContainer/Button3");

        restartButton.Pressed += EmitRestartSignal;
        exitButton.Pressed += ExitToMainMenu;
    }

    private void EmitRestartSignal()
    {
        GD.Print("Restart button pressed.");
        EmitSignal("RestartGame");
    }


    private void ExitToMainMenu()
    {
        GD.Print("Exiting to main menu.");
        GetTree().ChangeSceneToFile("res://src/Austin/Scenes/main.tscn");
    }
}
