
using Godot;
using System.Collections.Generic;

public partial class TowerStressRunner : Node2D
{
    private List<BaseTower> towers = new List<BaseTower>();
    private Node2D target;
    private PackedScene towerScene;
    private int currentTowerCount = 0;
    private bool isExtremeModeActive = false;
    
    // UI elements
    private Control uiControl;
    private Panel statsPanel;
    private VBoxContainer statsContainer;
    private Label statusLabel;
    private Label towerCountLabel;
    private Label bulletCountLabel;
    private Label fpsLabel;
    private Label memoryLabel;
    private Label modeLabel;

    // Properties for the test to access
    public int CurrentTowerCount => currentTowerCount;
    public int ActiveBulletCount => GetTree().GetNodesInGroup("Projectile").Count;

    public override void _Ready()
    {
        towerScene = GD.Load<PackedScene>("res://src/Ankit/Scenes/Tower1.tscn");
        
        // Create the camera
        var camera = new Camera2D();
        camera.Position = new Vector2(600, 400);
        camera.Zoom = new Vector2(0.5f, 0.5f);
        AddChild(camera);

        CreateUI();
        CreateTarget();
    }

    private void CreateUI()
    {
        // Create UI control
        uiControl = new Control();
        uiControl.SetAnchorsPreset(Control.LayoutPreset.TopLeft);
        AddChild(uiControl);

        // Create stats panel
        statsPanel = new Panel();
        statsPanel.Position = new Vector2(20, 20);
        statsPanel.Size = new Vector2(280, 250);
        uiControl.AddChild(statsPanel);

        // Create stats container
        statsContainer = new VBoxContainer();
        statsContainer.Position = new Vector2(10, 10);
        statsContainer.Size = new Vector2(260, 230);
        statsPanel.AddChild(statsContainer);

        // Create labels
        var titleLabel = new Label();
        titleLabel.Text = "Hardware Stress Test";
        titleLabel.HorizontalAlignment = HorizontalAlignment.Center;
        statsContainer.AddChild(titleLabel);

        modeLabel = new Label();
        modeLabel.Text = "Mode: Normal";
        statsContainer.AddChild(modeLabel);

        towerCountLabel = new Label();
        towerCountLabel.Text = "Towers: 0";
        statsContainer.AddChild(towerCountLabel);

        bulletCountLabel = new Label();
        bulletCountLabel.Text = "Bullets: 0";
        statsContainer.AddChild(bulletCountLabel);

        fpsLabel = new Label();
        fpsLabel.Text = "FPS: 0";
        statsContainer.AddChild(fpsLabel);

        memoryLabel = new Label();
        memoryLabel.Text = "Memory: 0 MB";
        statsContainer.AddChild(memoryLabel);

        statusLabel = new Label();
        statusLabel.Text = "Status: Initializing...";
        statsContainer.AddChild(statusLabel);
    }

    public void IncreaseStress()
    {
        isExtremeModeActive = true;
        GD.Print("Activating EXTREME mode!");
        
        foreach (var tower in towers)
        {
            if (tower != null && GodotObject.IsInstanceValid(tower))
            {
                tower.SetupForTesting(
                    shootInterval: 0.03f,    // Super fast shooting
                    bulletsPerShot: 8,       // Many bullets
                    bulletSpeed: 800f,        // Fast bullets
                    rotationSpeed: 20f        // Fast rotation
                );
            }
        }
        
        if (modeLabel != null)
            modeLabel.Text = "Mode: EXTREME";
    }

    public void AddTowers(int count)
    {
        float baseRadius = 50f;
        int ringsNeeded = (currentTowerCount + count) / 20 + 1;

        for (int i = 0; i < count; i++)
        {
            var tower = towerScene.Instantiate<BaseTower>();
            if (tower != null)
            {
                int ring = currentTowerCount / 20;
                float ringRadius = baseRadius * (ring + 1);
                float angleInRing = (currentTowerCount % 20) * (2 * Mathf.Pi / 20);

                Vector2 position = new Vector2(
                    Mathf.Cos(angleInRing) * ringRadius,
                    Mathf.Sin(angleInRing) * ringRadius
                );

                tower.Position = position + new Vector2(600, 400);
                
                if (isExtremeModeActive)
                {
                    tower.SetupForTesting(
                        shootInterval: 0.03f,
                        bulletsPerShot: 8,
                        bulletSpeed: 800f,
                        rotationSpeed: 20f
                    );
                }
                else
                {
                    tower.SetupForTesting(
                        shootInterval: 0.05f,
                        bulletsPerShot: 5,
                        bulletSpeed: 600f,
                        rotationSpeed: 15f
                    );
                }

                AddChild(tower);
                towers.Add(tower);
                currentTowerCount++;
                GD.Print($"Added tower {currentTowerCount} at position {tower.Position}");
            }
        }
    }

    private void CreateTarget()
    {
        target = new Node2D();
        target.AddToGroup("Enemy");
        AddChild(target);
        target.Position = new Vector2(600, 400);

        var targetSprite = new ColorRect();
        targetSprite.Size = new Vector2(20, 20);
        targetSprite.Position = new Vector2(-10, -10);
        targetSprite.Color = Colors.Red;
        target.AddChild(targetSprite);
    }

    public override void _Process(double delta)
    {
        UpdateTarget();
        UpdateUI();
    }

    private void UpdateTarget()
    {
        if (target != null)
        {
            float time = (float)Time.GetTicksMsec() / 500.0f;
            float radius = 150f + Mathf.Sin(time * 2) * 50f;
            target.Position = new Vector2(
                600 + Mathf.Cos(time) * radius,
                400 + Mathf.Sin(time * 1.5f) * radius
            );
        }
    }

    private void UpdateUI()
    {
        if (towerCountLabel == null) return;

        float currentFPS = (float)Engine.GetFramesPerSecond();
        float memoryUsageMB = OS.GetStaticMemoryUsage() / (1024f * 1024f);

        towerCountLabel.Text = $"Towers: {currentTowerCount}";
        bulletCountLabel.Text = $"Bullets: {ActiveBulletCount}";
        fpsLabel.Text = $"FPS: {currentFPS:F1}";
        memoryLabel.Text = $"Memory: {memoryUsageMB:F1} MB";

        string status = isExtremeModeActive ? "EXTREME MODE - " : "Normal Mode - ";
        
        if (currentFPS < 5)
        {
            status += "CRITICAL FPS!";
            statusLabel.Modulate = Colors.Red;
        }
        else if (currentFPS < 15)
        {
            status += "Low FPS";
            statusLabel.Modulate = Colors.Yellow;
        }
        else if (ActiveBulletCount > 9000)
        {
            status += "BULLET STORM!";
            statusLabel.Modulate = Colors.Orange;
        }
        else
        {
            status += "Running";
            statusLabel.Modulate = isExtremeModeActive ? Colors.Orange : Colors.White;
        }

        statusLabel.Text = status;
    }

    public void Cleanup()
    {
        GD.Print("Starting cleanup...");
        
        foreach (var tower in towers)
        {
            if (GodotObject.IsInstanceValid(tower))
                tower.QueueFree();
        }
        towers.Clear();
        currentTowerCount = 0;
        isExtremeModeActive = false;

        if (target != null && GodotObject.IsInstanceValid(target))
            target.QueueFree();

        if (statusLabel != null)
        {
            statusLabel.Text = "Status: Cleanup Complete";
            statusLabel.Modulate = Colors.Green;
        }
        
        if (modeLabel != null)
            modeLabel.Text = "Mode: Normal";
            
        GD.Print("Cleanup complete!");
    }
}
