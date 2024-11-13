using Godot;
using System;

public partial class LevelSelectionMenu : Node2D
{
	private Button level1Button;
	private Button easyButton;
	private Button mediumButton;
	private Button hardButton;

	public override void _Ready()
	{
		// Get buttons
		level1Button = GetNode<Button>("VBoxContainer/Level1Button");
		easyButton = GetNode<Button>("VBoxContainer/DifficultyContainer/EasyButton");
		mediumButton = GetNode<Button>("VBoxContainer/DifficultyContainer/MediumButton");
		hardButton = GetNode<Button>("VBoxContainer/DifficultyContainer/HardButton");

		// Connect signals
		level1Button.Pressed += OnLevel1Pressed;
		easyButton.Pressed += OnEasyPressed;
		mediumButton.Pressed += OnMediumPressed;
		hardButton.Pressed += OnHardPressed;
	}

	private void OnLevel1Pressed()
	{
		GD.Print("Opening Difficulty Selection for Level 1...");
		GetTree().ChangeSceneToFile("res://src/Sohan/Scenes/DifficultySelectionMenu.tscn");
	}


	private void OnEasyPressed()
	{
		// Load main scene for Level 1 Easy difficulty
		GD.Print("Starting Level 1 - Easy...");
		GetTree().ChangeSceneToFile("res://src/Austin/scenes/main.tscn");
	}

	private void OnMediumPressed()
	{
		// Load main scene for Level 1 Medium difficulty
		GD.Print("Starting Level 1 - Medium...");
		GetTree().ChangeSceneToFile("res://src/Austin/scenes/main.tscn");
	}

	private void OnHardPressed()
	{
		// Load main scene for Level 1 Hard difficulty
		GD.Print("Starting Level 1 - Hard...");
		GetTree().ChangeSceneToFile("res://src/Austin/scenes/main.tscn");
	}
}
