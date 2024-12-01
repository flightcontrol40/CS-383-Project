using Godot;
using System;

public partial class PauseMenu : BaseMenu
{
	private Button resumeButton;
	private Button restartButton;
	private Button optionsButton;
	private Button exitButton;
	private TextureButton textureButton;
	private Panel panel;
	private VBoxContainer vboxContainer;
	private Label label;

	public override void _Ready()
	{
		// Get nodes for buttons and other components
		resumeButton = GetNode<Button>("VBoxContainer/Button4");
		restartButton = GetNode<Button>("VBoxContainer/Button");
		optionsButton = GetNode<Button>("VBoxContainer/Button2");
		exitButton = GetNode<Button>("VBoxContainer/Button3");
		textureButton = GetNode<TextureButton>("TextureButton");
		panel = GetNode<Panel>("Panel");
		vboxContainer = GetNode<VBoxContainer>("VBoxContainer");
		label = GetNode<Label>("Label");

		// Initially hide the entire pause menu except the TextureButton
		panel.Visible = false;
		vboxContainer.Visible = false;
		label.Visible = false;

		// Connect TextureButton to the method for showing the pause menu
		textureButton.Pressed += OnTextureButtonPressed;
	}

	private void OnTextureButtonPressed()
	{
		GD.Print("Pause button pressed. Pausing the game and showing the pause menu.");
		GetTree().Paused = true; // Pause the game
		panel.Visible = true;     // Show the panel
		vboxContainer.Visible = true; // Show the VBoxContainer
		label.Visible = true;     // Show the label
	}

	// Button handlers connected through the editor signals
	private void OnResumePressed()
	{
		GD.Print("Resuming game...");
		GetTree().Paused = false; // Resume the game
		panel.Visible = false;    // Hide the entire pause menu
		vboxContainer.Visible = false;
		label.Visible = false;
	}

	private void OnRestartPressed()
	{
		GD.Print("Restarting game...");
		GetTree().Paused = false; // Ensure the game is unpaused before restarting
		GetTree().ChangeSceneToFile("res://src/Austin/Scenes/main.tscn"); // Replace with your actual scene path
	}

	private void OnOptionsPressed()
	{
		GD.Print("Opening options...");
		// Add your options menu logic here
	}

	private void OnExitPressed()
	{
		GD.Print("Exiting to main menu from pause menu.");
		GetTree().Paused = false; // Ensure the game is unpaused before exiting
		GetTree().ChangeSceneToFile("res://src/Sohan/Scenes/main_menu.tscn"); // Adjust path if needed
	}
}
