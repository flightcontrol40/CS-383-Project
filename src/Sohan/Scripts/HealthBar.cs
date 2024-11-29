using Godot;
using Chicken;

public partial class HealthBar : ProgressBar
{
    private static HealthBar instance; // Singleton instance

    private int currentHealth = 100;  // Start with max health

    // Singleton accessor
    public static HealthBar Instance
    {
        get
        {
            return instance;
        }
    }

    public override void _Ready()
    {
        instance = this;  // Set the instance when the scene is loaded in Godot
        GD.Print("HealthBar Ready: Connecting signals.");

        // Connect all existing and future enemies' EndOfPath signal
        GetTree().Connect("node_added", new Callable(this, nameof(OnNodeAdded)));

        foreach (Node node in GetTree().GetNodesInGroup("Enemy"))
        {
            ConnectEnemySignal(node as BaseChicken);
        }
    }

    private void OnNodeAdded(Node node)
    {
        if (node is BaseChicken chicken)
        {
            GD.Print($"New enemy detected: {chicken.Name}. Connecting signal.");
            ConnectEnemySignal(chicken);
        }
    }

    private void ConnectEnemySignal(BaseChicken enemy)
    {
        if (enemy != null)
        {
            GD.Print($"Connecting {enemy.Name}'s EndOfPath signal.");
            enemy.Connect(BaseChicken.SignalName.EndOfPath, Callable.From<BaseChicken>(OnEnemyReachedEnd));
        }
    }

    public void OnEnemyReachedEnd(BaseChicken enemy)
    {
        GD.Print($"{enemy.Name} reached the endpoint! Decreasing health.");
        UpdateHealthBar(enemy.damageAmount);
    }


	public void TestTriggerEnemyReachedEnd(BaseChicken enemy)
	{
    	OnEnemyReachedEnd(enemy);
	}

	

	public void SetHealth(int health)
	{
    	currentHealth = Mathf.Clamp(health, 0, 100);
    	Value = currentHealth;
    	UpdateHealthBarColor();
	}



    private void UpdateHealthBar(int healthDecrementAmount)
    {
        currentHealth -= healthDecrementAmount;
        currentHealth = Mathf.Max(currentHealth, 0);  // Prevent health from going negative
        Value = currentHealth;  // Update the health bar's display value

        GD.Print($"Health decreased by {healthDecrementAmount}. Current Health: {currentHealth}");

        if (currentHealth <= 0)
        {
            GD.Print("Game Over! Health reached 0.");
            // Game over logic here
        }

        UpdateHealthBarColor();
    }

    private void UpdateHealthBarColor()
    {
        float healthPercentage = (float)currentHealth / 100;

        if (healthPercentage > 0.6f)
            Modulate = new Color(0, 1, 0); // Green
        else if (healthPercentage > 0.3f)
            Modulate = new Color(1, 1, 0); // Yellow
        else
            Modulate = new Color(1, 0, 0); // Red
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }
}
