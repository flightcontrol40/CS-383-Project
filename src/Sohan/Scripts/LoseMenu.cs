using Godot;
using System;

public partial class LoseMenu : BaseMenu
{
	protected override void OnExitPressed()
	{
		GD.Print("LoseMenu OnExitPressed - Returning to main menu.");
		GetTree().ChangeSceneToFile("res://src/Sohan/Scenes/main_menu.tscn"); 
	}
}
