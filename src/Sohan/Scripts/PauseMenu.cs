using Godot;
using System;

public partial class PauseMenu : BaseMenu
{
    private Button resumeButton;
    private Button restartButton;
    private Button optionsButton;
    private TextureButton pauseButton;
    private Panel panel;
    private VBoxContainer vboxContainer;
    private Label label;
    private Control SettingsMenu;

    public override void _Ready()
    {
        // Get nodes for buttons and other components
        resumeButton = GetNode<Button>("VBoxContainer/Button4");
        restartButton = GetNode<Button>("VBoxContainer/Button");
        optionsButton = GetNode<Button>("VBoxContainer/Button2");
        exitButton = GetNode<Button>("VBoxContainer/Button3");
        pauseButton = GetNode<TextureButton>("TextureButton");
        panel = GetNode<Panel>("Panel");
        vboxContainer = GetNode<VBoxContainer>("VBoxContainer");
        label = GetNode<Label>("Label");
        SettingsMenu = GetNode<Control>("SettingsMenu");

        // Initially hide everything
        HideMenu();
        pauseButton.Hide();

        ConnectSignals();
        MouseFilter = MouseFilterEnum.Ignore;

        // Make sure exit button is visible and connected
        if (exitButton != null)
        {
            exitButton.Visible = true;
            exitButton.Pressed += OnExitPressed;
        }
    }

    private void ConnectSignals()
    {
        pauseButton.Pressed += OnPauseButtonPressed;
        
        // Connect menu buttons
        resumeButton.Pressed += OnResumePressed;
        restartButton.Pressed += OnRestartPressed;
        optionsButton.Pressed += OnOptionsPressed;
        this.SettingsMenu.Connect("return_to_menu",Callable.From(HandleSettingsMenuReturnButton));
    }

    protected override void OnExitPressed()
    {
        GD.Print("Exiting game");
        GetTree().Paused = false;
        GetTree().Quit();
    }

    private void HideMenu()
    {
        panel.Visible = false;
        vboxContainer.Visible = false;
        label.Visible = false;
        MouseFilter = MouseFilterEnum.Ignore;
    }

     private void ShowMenu()
    {
        panel.Visible = true;
        vboxContainer.Visible = true;
        label.Visible = true;
        if (exitButton != null)
        {
            exitButton.Visible = true; // Make sure exit button is visible
        }
        MouseFilter = MouseFilterEnum.Stop;
    }

    private void OnPauseButtonPressed()
    {
        GD.Print("Game paused");
        GetTree().Paused = true;
        ShowMenu();
    }

    private void OnResumePressed()
    {
        GD.Print("Resuming game");
        GetTree().Paused = false;
        HideMenu();
    }

    private void OnRestartPressed()
    {
        GD.Print("Returning to main menu for fresh start");
        GetTree().Paused = false;
        // Clear game state by reloading current scene first
        GetTree().ReloadCurrentScene();
        // Then go to main menu
        GetTree().ChangeSceneToFile("res://src/Sohan/Scenes/main_menu.tscn");
    }

    private void OnOptionsPressed() {
        GD.Print("Opening options menu through main menu");
        var sub_menu_visible = (bool)this.SettingsMenu.Get("visible");
        if (sub_menu_visible == false){
            GD.Print("Opening options menu through main menu");
            this.HideMenu();
            this.SettingsMenu.Call("unhide_menu");
        }
        // GetTree().Paused = false;
        // Go back to main menu which will handle options
        // GetTree().ChangeSceneToFile("res://src/Sohan/Scenes/main_menu.tscn");
    }

    public void EnablePauseMenu()
    {
        if (pauseButton != null)
        {
            pauseButton.Show();
            pauseButton.Visible = true;
            GD.Print("Pause button enabled");
        }
    }

    public void DisablePauseMenu()
    {
        HideMenu();
        if (pauseButton != null)
        {
            pauseButton.Hide();
            GD.Print("Pause button disabled");
        }
    }

    private void HandleSettingsMenuReturnButton(){
        this.SettingsMenu.Call("hide_menu");
        this.ShowMenu();
    }

}

