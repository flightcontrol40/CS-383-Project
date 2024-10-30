using Godot;
using System;

public partial class MainMenu : Control
{
	private Button startButton;
	private Button exitButton;

	public override void _Ready()
	{
		// Ensure this logic only runs if the root node name is "MainMenu"
		if (Name == "MainMenu")
		{
			GD.Print("This is Sohan's main menu. Running main menu logic...");

			// Get buttons from the scene
			startButton = GetNode<Button>("VBoxContainer/Button");
			exitButton = GetNode<Button>("VBoxContainer/Button3");

			// Connect signals to methods
			startButton.Pressed += OnStartPressed;
			exitButton.Pressed += OnExitPressed;
		}
		else
		{
			GD.Print("This is not Sohan's main menu. Skipping logic...");
		}
	}

	private void OnStartPressed()
	{
		GD.Print("Starting the game from Sohan's main menu...");
		GetTree().ChangeSceneToFile("res://src/Austin/scenes/main.tscn");
	}

	private void OnExitPressed()
	{
		GD.Print("Exiting the game from Sohan's main menu...");
		GetTree().Quit();
	}
}
