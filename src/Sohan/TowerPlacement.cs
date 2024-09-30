using Godot;
using System;

public class TowerPlacement : Node2D
{
	private bool isPlaced = false;

	public override void _Input(InputEvent @event)
	{
		if (!isPlaced && @event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.ButtonIndex == (int)ButtonList.Left && mouseEvent.Pressed)
			{
				// Place the tower
				Vector2 mousePosition = GetGlobalMousePosition();
				Position = mousePosition;
				isPlaced = true;
				GD.Print("Tower placed at: ", mousePosition);
			}
		}
	}
}
