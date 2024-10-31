using Godot;
using Chicken;

public partial class HealthBar : ProgressBar
{
	private int currentHealth = 100;  // Start with max health

	public override void _Ready()
	{
		GD.Print("HealthBar Ready: Connecting signals.");

		// Connect all existing and future enemies' EndOfPath signal
		GetTree().Connect("node_added", new Callable(this, nameof(OnNodeAdded)));

		foreach (Node node in GetTree().GetNodesInGroup("Enemy"))
		{
			ConnectEnemySignal(node as BaseChicken);
		}
	}

	// Triggered when a new enemy is added to the scene tree.
	private void OnNodeAdded(Node node)
	{
		if (node is BaseChicken chicken)
		{
			GD.Print($"New enemy detected: {chicken.Name}. Connecting signal.");
			ConnectEnemySignal(chicken);
		}
	}

	// Connects the EndOfPath signal from an enemy to decrease player health.
	private void ConnectEnemySignal(BaseChicken enemy)
	{
		if (enemy != null)
		{
			GD.Print($"Connecting {enemy.Name}'s EndOfPath signal.");
			enemy.Connect(nameof(BaseChicken.EndOfPathEventHandler),
						  new Callable(this, nameof(OnEnemyReachedEnd)));
		}
	}

	// Called when an enemy reaches the endpoint, triggering a health decrease.
	private void OnEnemyReachedEnd(BaseChicken enemy)
	{
		GD.Print($"{enemy.Name} reached the endpoint! Decreasing health.");
		UpdateHealthBar(enemy.damageAmount); // Using your UpdateHealthBar approach here
	}

	// Updates the health value and visual components
	private void UpdateHealthBar(int healthDecrementAmount)
	{
		currentHealth -= healthDecrementAmount;
		Value = currentHealth;  // Update the health bar's display value

		GD.Print($"Health decreased by {healthDecrementAmount}. Current Health: {currentHealth}");

		if (currentHealth <= 0)
		{
			GD.Print("Game Over! Health reached 0.");
			// Game over logic here
		}

		UpdateHealthBarColor();
	}

	// Modifies the health bar color based on health percentage.
	private void UpdateHealthBarColor()
	{
		float healthPercentage = (float)currentHealth / 100;

		// Change the color gradually from green to red
		if (healthPercentage > 0.6f)
			Modulate = new Color(0, 1, 0); // Green
		else if (healthPercentage > 0.3f)
			Modulate = new Color(1, 1, 0); // Yellow
		else
			Modulate = new Color(1, 0, 0); // Red
	}
}
