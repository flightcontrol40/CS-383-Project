using Godot;

public partial class MainMenu : Control
{
	private Button startGameButton;
	private Button optionsGameButton;
	private Button demoGameButton;

	private Button exitGameButton;
	private Control levelSelectionMenu;
	private Control SettingsMenu;
	private Control demoMenu;

	[Signal]
	public delegate void ReshowMenuEventHandler(); 

	public override void _Ready()
	{
		// Get the Start Game button and the Level Selection Menu
		startGameButton = GetNode<Button>("VBoxContainer/Button");
		optionsGameButton = GetNode<Button>("VBoxContainer/Button2");
		exitGameButton = GetNode<Button>("VBoxContainer/Button3");
		demoGameButton = GetNode<Button>("VBoxContainer/Demo");
		SettingsMenu = GetNode<Control>("SettingsMenu");
		
		demoMenu = GetNode<Control>("Demo");

		// Ensure the Level Selection Menu exists
		levelSelectionMenu = GetNodeOrNull<Control>("../LevelSelector");
		if (levelSelectionMenu == null)
		{
			GD.PrintErr("Level Selection Menu not found! Please ensure it exists in the scene tree.");
			return;
		}

		// Connect button signals
		startGameButton.Pressed += OnStartPressed;
		exitGameButton.Pressed += OnExitPressed;
		optionsGameButton.Pressed += OnOptionsPressed;
		demoGameButton.Pressed += OnDemoPressed;
		this.SettingsMenu.Connect("return_to_menu",Callable.From(HandleSettingsMenuReturnButton));

		// Debug visibility states
		GD.Print($"MainMenu Visibility (initial): {this.Visible}");
		GD.Print($"LevelSelectionMenu Visibility (initial): {levelSelectionMenu.Visible}");

		// Ensure menus are hidden initially
		levelSelectionMenu.Visible = false; // Hide Level Selection Menu
		this.Visible = true;                // Show Main Menu
	}

	private void OnStartPressed()
	{
		GD.Print("Start button pressed. Opening Level Selection Menu.");
		this.Visible = false;
		this.HideMenu();                   // Hide Main Menu
		levelSelectionMenu.Visible = true; // Show Level Selection Menu
	}

	private void OnExitPressed() {
		GD.Print("Exit button pressed. Exiting the game...");
		GetTree().Quit(); // Quit the game
	}
	
	private void OnDemoPressed(){
		//this.Visible = false;
		this.HideMenu();
		demoMenu.Visible = true;
		
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

	private void HandleSettingsMenuReturnButton(){
		this.SettingsMenu.Call("hide_menu");
		this.ShowMenu();
	}


	private void HideMenu() {
		this.startGameButton.Visible = false;
		this.exitGameButton.Visible = false;
		this.optionsGameButton.Visible = false;
		this.demoGameButton.Visible = false;
		MouseFilter = MouseFilterEnum.Ignore;
	}

	 private void ShowMenu() {
		this.startGameButton.Visible = true;
		this.exitGameButton.Visible = true;
		this.optionsGameButton.Visible = true;
		this.demoGameButton.Visible = true;
		MouseFilter = MouseFilterEnum.Stop;
		
		EmitSignal(nameof(ReshowMenu));
	}


}
