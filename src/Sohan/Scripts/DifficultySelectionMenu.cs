using Godot;
using DifficultyCalculator;

public partial class DifficultySelectionMenu : Control
{
    private Button easyButton;
    private Button mediumButton;
    private Button hardButton;
    private LevelManager levelManager; // Reference to LevelManager
    private PackedScene selectedMap; // Map selected in LevelSelectionMenu

    public override void _Ready()
    {
        GD.Print("DifficultySelectionMenu is ready.");

        // Get difficulty buttons
        easyButton = GetNodeOrNull<Button>("VBoxContainer/Easy");
        mediumButton = GetNodeOrNull<Button>("VBoxContainer/Medium");
        hardButton = GetNodeOrNull<Button>("VBoxContainer/Hard");

        // Get LevelManager from the scene tree
        levelManager = GetParent().GetNodeOrNull<LevelManager>("LevelManager");

        if (easyButton == null || mediumButton == null || hardButton == null || levelManager == null)
        {
            GD.PrintErr("One or more nodes could not be found in DifficultySelectionMenu.");
            return;
        }

        // Connect signals
        easyButton.Pressed += () => OnDifficultySelected("Easy");
        mediumButton.Pressed += () => OnDifficultySelected("Medium");
        hardButton.Pressed += () => OnDifficultySelected("Hard");

        // Hide this menu initially
        this.Visible = false;
    }

    /// <summary>
    /// Set the selected map from LevelSelectionMenu.
    /// </summary>
    public void SetSelectedMap(PackedScene map)
    {
        GD.Print("Map received in DifficultySelectionMenu.");
        selectedMap = map;
    }

    /// <summary>
    /// Called when a difficulty is selected.
    /// </summary>
    private void OnDifficultySelected(string difficulty)
    {
        GD.Print($"{difficulty} difficulty selected.");

        Difficulty selectedDifficulty = Difficulty.Easy;
        if (difficulty == "Medium") selectedDifficulty = Difficulty.Medium;
        if (difficulty == "Hard") selectedDifficulty = Difficulty.Hard;

        // Ensure the selected map and level manager are valid
        if (selectedMap != null && levelManager != null)
        {
            GD.Print("Setting map and difficulty in LevelManager...");
            levelManager.setMap(selectedMap); // Set the map
            levelManager.setDifficulty(selectedDifficulty); // Set the difficulty
            GD.Print("Map and difficulty set. Calling OnLoadLevel...");
            levelManager.OnLoadLevel(); // Start the game
        }
        else
        {
            GD.PrintErr("LevelManager or selectedMap is null! Cannot start the level.");
        }

        // Hide the difficulty selection menu
        this.Visible = false;
    }
}
