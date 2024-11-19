using Godot;
using System;

public partial class DifficultySelectionMenu : Control
{
	private Button easyButton;
	private Button mediumButton;
	private Button hardButton;

	public override void _Ready()
	{
		// Get buttons
		easyButton = GetNode<Button>("VBoxContainer/EasyButton");
		mediumButton = GetNode<Button>("VBoxContainer/MediumButton");
		hardButton = GetNode<Button>("VBoxContainer/HardButton");

		// Connect signals
		easyButton.Pressed += OnEasyPressed;
		mediumButton.Pressed += OnMediumPressed;
		hardButton.Pressed += OnHardPressed;
	}

	private void OnEasyPressed()
{
	GD.Print("Easy button pressed - attempting to load main.tscn...");
	
	// Unpause the game if it's paused
	if (GetTree().Paused)
	{
		GD.Print("Game is paused, unpausing...");
		GetTree().Paused = false;
	}

	// Load the main.tscn scene
	GetTree().ChangeSceneToFile("res://src/Austin/scenes/main.tscn");
}


	private void OnMediumPressed()
	{
		GD.Print("Starting Level 1 - Medium...");
		GetTree().ChangeSceneToFile("res://src/Austin/scenes/main.tscn");
	}

	private void OnHardPressed()
	{
		GD.Print("Starting Level 1 - Hard...");
		GetTree().ChangeSceneToFile("res://src/Austin/scenes/main.tscn");
	}
}
