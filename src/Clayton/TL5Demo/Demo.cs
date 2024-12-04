using Godot;

public partial class Demo : Control
{
	private Button returnButton; // Button for returning to the main menu

	public override void _Ready()
	{
		returnButton = GetNode<Button>("VBoxContainer/Button"); 
		returnButton.Pressed += OnReturnButtonPressed;
	}

	private void OnReturnButtonPressed()
	{
		GD.Print("working");
		GetTree().ChangeSceneToFile("res://src/Austin/scenes/main.tscn");
		//var mainMenu = GetNode<MainMenu>("res://src/Sohan/Scenes/main_menu.tscn"); 
		//mainMenu.EmitSignal(nameof(MainMenu.ReshowMenuEventHandler)); // Emit the signal to show the main menu
	}
}
