using Godot;

public partial class MainMenu : Control
{
    private Button startGameButton;
    private Button exitGameButton;
    private Control levelSelectionMenu;

    public override void _Ready()
    {
        // Get the Start Game button and the Level Selection Menu
        startGameButton = GetNode<Button>("VBoxContainer/Button");
        exitGameButton = GetNode<Button>("VBoxContainer/Button3");

        // Ensure the Level Selection Menu exists
        levelSelectionMenu = GetNodeOrNull<Control>("../LevelSelector");
        if (levelSelectionMenu == null)
        {
            GD.PrintErr("Level Selection Menu not found! Please ensure it exists in the scene tree.");
            return;
        }

        // Connect button signals
        startGameButton.Pressed += OnStartPressed;
        exitGameButton.Pressed += OnExitPressed;

        // Debug visibility states
        GD.Print($"MainMenu Visibility (initial): {this.Visible}");
        GD.Print($"LevelSelectionMenu Visibility (initial): {levelSelectionMenu.Visible}");

        // Ensure menus are hidden initially
        levelSelectionMenu.Visible = false; // Hide Level Selection Menu
        this.Visible = true;                // Show Main Menu
    }

    private void OnStartPressed()
    {
        GD.Print("Start button pressed. Opening Level Selection Menu.");
        this.Visible = false;               // Hide Main Menu
        levelSelectionMenu.Visible = true; // Show Level Selection Menu
    }

    private void OnExitPressed()
    {
        GD.Print("Exit button pressed. Exiting the game...");
        GetTree().Quit(); // Quit the game
    }
}
