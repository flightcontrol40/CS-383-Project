using Godot;

public partial class LoseMenu : Control
{
    private Button restartButton;
    private Button exitButton;
    private Control SettingsMenu;
    private Button optionsGameButton;


    public override void _Ready() {
        restartButton = GetNode<Button>("VBoxContainer/Button");
        optionsGameButton = GetNode<Button>("VBoxContainer/Button2");
        exitButton = GetNode<Button>("VBoxContainer/Button3");
        SettingsMenu = GetNode<Control>("SettingsMenu");

        restartButton.Pressed += EmitRestartSignal;
        optionsGameButton.Pressed += OnOptionsPressed;
        this.SettingsMenu.Connect("return_to_menu",Callable.From(HandleSettingsMenuReturnButton));

        exitButton.Pressed += ExitToMainMenu;
    }

    private void EmitRestartSignal() {
        GD.Print("Restart button pressed.");
        EmitSignal("RestartGame");
    }


    private void ExitToMainMenu()
    {
        GD.Print("Exiting to main menu.");
        GetTree().ChangeSceneToFile("res://src/Austin/Scenes/main.tscn");
    }


    private void OnOptionsPressed(){
        GD.Print("Opening options menu through main menu");
        var sub_menu_visible = (bool)this.SettingsMenu.Get("visible");
        if (sub_menu_visible == false){
            GD.Print("Opening options menu through main menu");
            this.HideMenu();
            this.SettingsMenu.Call("unhide_menu");
        }
    }

    private void HideMenu() {
        this.restartButton.Visible = false;
        this.exitButton.Visible = false;
        this.optionsGameButton.Visible = false;
        MouseFilter = MouseFilterEnum.Ignore;
    }

     private void ShowMenu() {
        this.restartButton.Visible = true;
        this.exitButton.Visible = true;
        this.optionsGameButton.Visible = true;
        MouseFilter = MouseFilterEnum.Stop;
    }

    private void HandleSettingsMenuReturnButton() {
        this.SettingsMenu.Call("hide_menu");
        this.ShowMenu();
    }

}