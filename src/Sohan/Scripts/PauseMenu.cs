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

    // Make sure everything starts hidden except the pause button
    panel.Visible = false;
    vboxContainer.Visible = false;
    label.Visible = false;
    textureButton.Visible = true;  // Keep pause button visible

    // Connect signals
    resumeButton.Pressed += OnResumePressed;
    restartButton.Pressed += OnRestartPressed;
    optionsButton.Pressed += OnOptionsPressed;
    exitButton.Pressed += OnExitPressed;

    ProcessMode = ProcessModeEnum.Always;
}
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel")) // ESC key
        {
            if (!panel.Visible)
            {
                ShowPauseMenu();
            }
            else
            {
                OnResumePressed();
            }
        }
    }

    private void _on_texture_button_pressed()
    {
        ShowPauseMenu();
    }

    private void ShowPauseMenu()
    {
        GetTree().Paused = true;
        panel.Visible = true;
        vboxContainer.Visible = true;
        label.Visible = true;
    }

    private void HidePauseMenu()
    {
        GetTree().Paused = false;
        panel.Visible = false;
        vboxContainer.Visible = false;
        label.Visible = false;
    }

    private void OnResumePressed()
    {
        GD.Print("Resuming game");
        HidePauseMenu();
    }

    private void OnRestartPressed()
    {
        GD.Print("Restarting game");
        GetTree().Paused = false;
        GetTree().ChangeSceneToFile("res://src/Sohan/Scenes/main_menu.tscn");
    }

    private void OnOptionsPressed()
    {
        GD.Print("Opening options");
        HidePauseMenu();
        // Add your options menu logic here
    }

    protected override void OnExitPressed()
    {
        GD.Print("Exiting to main menu");
        GetTree().Paused = false;
        GetTree().ChangeSceneToFile("res://src/Sohan/Scenes/main_menu.tscn");
    }
}