using Godot;
using System;

public partial class WinMenu : BaseMenu
{
	protected override void OnExitPressed()
	{
		GD.Print("WinMenu OnExitPressed - Returning to main menu.");
		GetTree().ChangeSceneToFile("res://src/Sohan/Scenes/main_menu.tscn"); // Adjust path if needed
	}
}
