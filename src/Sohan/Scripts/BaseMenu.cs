using Godot;
using System;

public partial class BaseMenu : Control
{
	protected Button startButton;
	protected Button exitButton;

	public override void _Ready()
	{
		// Get buttons from the scene
		startButton = GetNode<Button>("VBoxContainer/Button");
		exitButton = GetNode<Button>("VBoxContainer/Button3");

		// Connect signals to methods
		startButton.Pressed += OnStartPressed;
		exitButton.Pressed += OnExitPressed;
	}

	private void OnStartPressed()
	{
		GD.Print("Opening Level Selection...");
		GetTree().ChangeSceneToFile("res://src/Sohan/Scenes/LevelSelectionMenu.tscn");
	}

	// Virtual exit method to allow customization in subclasses
	protected virtual void OnExitPressed()
	{
		GD.Print("BaseMenu OnExitPressed - Exiting game.");
		GetTree().Quit();
	}
}
