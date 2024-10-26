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
            // Connect the EndOfPath signal to HealthBar's OnEnemyReachedEnd method
            enemy.Connect(nameof(BaseChicken.EndOfPathEventHandler), 
                          new Callable(this, nameof(OnEnemyReachedEnd)));
        }
    }

    private void OnEnemyReachedEnd(BaseChicken enemy)
    {
        GD.Print($"{enemy.Name} reached the endpoint! Decreasing health.");
        DecreaseHealth(enemy.damageAmount);
    }

    private void DecreaseHealth(int amount)
    {
        currentHealth -= amount;
        Value = currentHealth;

        GD.Print($"Health decreased by {amount}. Current Health: {currentHealth}");

        if (currentHealth <= 0)
        {
            GD.Print("Game Over! Health reached 0.");
            // Handle game over logic here
        }
    }
}
