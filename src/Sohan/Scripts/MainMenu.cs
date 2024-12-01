using Godot;

public partial class MainMenu : Control
{
    private Button startGameButton;
    private Node2D levelSelectionMenu;

    public override void _Ready()
    {
        // Get the Start Game button and the Level Selection Menu
        startGameButton = GetNode<Button>("VBoxContainer/Button");
        levelSelectionMenu = GetNode<Node2D>("../LevelSelectionMenu");

        // Connect button signals
        startGameButton.Pressed += OnStartPressed;

        // Debug visibility states
        GD.Print($"MainMenu Visibility (initial): {this.Visible}");
        GD.Print($"LevelSelectionMenu Visibility (initial): {levelSelectionMenu.Visible}");

        // Ensure menus are hidden initially
        levelSelectionMenu.Visible = false;
        this.Visible = true; // Ensure MainMenu is visible
    }

    private void OnStartPressed()
    {
        GD.Print("Start button pressed. Opening Level Selection Menu.");
        this.Visible = false; // Hide Main Menu
        levelSelectionMenu.Visible = true; // Show Level Selection Menu
    }
}
