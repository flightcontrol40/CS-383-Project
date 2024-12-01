using Chicken;
using DifficultyCalculator;
using Godot;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;

public partial class Main : Node
{
	public bool PlacingTurret = false;
	private bool secondclick = false;
	public BaseTower newTower;

	private RoundManager.RoundManager roundm;
	private LevelManager levelm;
	private Button StartRoundButton;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		levelm = GetNode<LevelManager>("LevelManager");
		levelm.setDifficulty(Difficulty.Easy);
		levelm.OnLoadLevel();

		roundm = GetNode<RoundManager.RoundManager>("RoundManager");
		roundm.loadLevel(levelm.level, (int)levelm.baseDifficulty);

		StartRoundButton = GetNode<Button>("Shop/Shop Panel/StartRoundButton");
		StartRoundButton.Pressed += () => {
			if (!PlacingTurret)
			{
				GD.Print("Round Starting");
				roundm.startRound();
			}
		};
		HealthBar health = GetNode<HealthBar>("PlayerHealth/HealthBar");
		levelm.level.Connect(Level.SignalName.HealthChanged, Callable.From<int>(health.OnHealthChanged));
		//levelm.level.Connect(Level.SignalName.MoneyChanged, )
	}

	private void PlaceTower()
	{
		if (PlacingTurret)
		{
			if (newTower == null)
			{
				PlacingTurret = false;
			}
			else
			{
				newTower.GlobalPosition = GetViewport().GetMousePosition();
				if (Input.IsActionJustReleased("place_tower"))
				{
					if (secondclick == false)
					{
						secondclick = true;
					}
					else if (newTower.IsValidPosition())
					{
						secondclick = false;
						PlacingTurret = false;
						newTower.ToggleCollisionShapes();
						newTower = null;
					}
					else
					{
						GD.PushWarning("Invalid Placement");
					}
				}
				var PlacementColor = newTower.GetNode("Placement/ColorRect") as ColorRect;
				if (newTower.IsValidPosition())
				{
					PlacementColor.Color = new Color(0x3C3C3C6A);
				}
				else
				{
					PlacementColor.Color = new Color(0xFF3C3C9D);
				}
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		PlaceTower();
	}

}
