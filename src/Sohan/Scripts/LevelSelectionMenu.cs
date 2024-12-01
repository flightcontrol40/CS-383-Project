using Godot;

public partial class LevelSelectionMenu : Node2D
{
    private Button level1Button;
    private Button level2Button;
    private Button level3Button;
    private DifficultySelectionMenu difficultySelectionMenu; // Reference to DifficultySelectionMenu
    private PackedScene level1Map;
    private PackedScene level2Map;
    private PackedScene level3Map;

    public override void _Ready()
    {
        GD.Print("LevelSelectionMenu is ready!");

        // Get level buttons
        level1Button = GetNodeOrNull<Button>("VBoxContainer/Level1Button");
        level2Button = GetNodeOrNull<Button>("VBoxContainer/Level_2");
        level3Button = GetNodeOrNull<Button>("VBoxContainer/Level_3");

        // Get the DifficultySelectionMenu
        difficultySelectionMenu = GetParent().GetNodeOrNull<DifficultySelectionMenu>("DifficultySelectionMenu");

        if (level1Button == null || level2Button == null || level3Button == null || difficultySelectionMenu == null)
        {
            GD.PrintErr("One or more nodes could not be found in LevelSelectionMenu.");
            return;
        }

        // Connect signals
        level1Button.Pressed += OnLevel1Pressed;
        level2Button.Pressed += OnLevel2Pressed;
        level3Button.Pressed += OnLevel3Pressed;

        // Load maps (replace with actual paths or references)
        level1Map = GD.Load<PackedScene>("res://path_to_level1_map.tscn");
        level2Map = GD.Load<PackedScene>("res://path_to_level2_map.tscn");
        level3Map = GD.Load<PackedScene>("res://path_to_level3_map.tscn");

        GD.Print("LevelSelectionMenu initialization complete.");
    }

    private void OnLevel1Pressed()
    {
        GD.Print("Level 1 Button was pressed!");
        OpenDifficultySelection(level1Map);
    }

    private void OnLevel2Pressed()
    {
        GD.Print("Level 2 Button was pressed!");
        OpenDifficultySelection(level2Map);
    }

    private void OnLevel3Pressed()
    {
        GD.Print("Level 3 Button was pressed!");
        OpenDifficultySelection(level3Map);
    }

    private void OpenDifficultySelection(PackedScene selectedMap)
    {
        if (difficultySelectionMenu != null)
        {
            GD.Print("Opening Difficulty Selection Menu...");
            difficultySelectionMenu.SetSelectedMap(selectedMap);
            difficultySelectionMenu.Visible = true;
            this.Visible = false; // Hide the level selection menu
        }
        else
        {
            GD.PrintErr("DifficultySelectionMenu is null! Cannot open.");
        }
    }
}
