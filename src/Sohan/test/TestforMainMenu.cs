using Godot;
using GdUnit4;
using static GdUnit4.Assertions;

namespace SohanTests
{
    [TestSuite]
    public partial class MenuTests : Node
    {
        private PackedScene _mainMenuScene;
        private PackedScene _winMenuScene;
        private PackedScene _loseMenuScene;

        private MainMenu _mainMenu;
        private WinMenu _winMenu;
        private LoseMenu _loseMenu;

        [Before]
        public void Setup()
        {
            // Load scenes only once to prevent casting issues
            _mainMenuScene = GD.Load<PackedScene>("res://src/Sohan/Scenes/main_menu.tscn");
            _winMenuScene = GD.Load<PackedScene>("res://src/Sohan/Scenes/Win.tscn");
            _loseMenuScene = GD.Load<PackedScene>("res://src/Sohan/Scenes/Loose_screen.tscn");

            AssertThat(_mainMenuScene).IsNotNull().OverrideFailureMessage("Failed to load main_menu.tscn. Check the path.");
            AssertThat(_winMenuScene).IsNotNull().OverrideFailureMessage("Failed to load Win_screen.tscn. Check the path.");
            AssertThat(_loseMenuScene).IsNotNull().OverrideFailureMessage("Failed to load Loose_screen.tscn. Check the path.");
        }

        private void InstantiateMainMenu()
        {
            _mainMenu = _mainMenuScene.Instantiate<MainMenu>();
            AutoFree(_mainMenu);
            AssertThat(_mainMenu).IsNotNull().OverrideFailureMessage("MainMenu instance is null after instantiation.");
        }

        private void InstantiateWinMenu()
        {
            _winMenu = _winMenuScene.Instantiate<WinMenu>();
            AutoFree(_winMenu);
            AssertThat(_winMenu).IsNotNull().OverrideFailureMessage("WinMenu instance is null after instantiation.");
        }

        private void InstantiateLoseMenu()
        {
            _loseMenu = _loseMenuScene.Instantiate<LoseMenu>();
            AutoFree(_loseMenu);
            AssertThat(_loseMenu).IsNotNull().OverrideFailureMessage("LoseMenu instance is null after instantiation.");
        }

        // Unit Tests
        // This test ensures that the "Exit" button exists within the MainMenu's VBoxContainer.
        // It could catch errors where the button is mistakenly renamed, removed, or moved outside the expected hierarchy, preventing users from exiting the menu.
        [TestCase]
        public void MainMenuExitButtonExists()
        {
            InstantiateMainMenu();

            var vBoxContainer = _mainMenu.GetNode("VBoxContainer");
            AssertThat(vBoxContainer).IsNotNull().OverrideFailureMessage("VBoxContainer node is missing in MainMenu.");

            var exitButton = vBoxContainer.GetNode<Button>("Button3");
            AssertThat(exitButton).IsNotNull().OverrideFailureMessage("Button3 is missing in MainMenu's VBoxContainer.");
        }

        [TestCase]
        public void WinMenuExitButtonExists()
        {
            InstantiateWinMenu();

            var vBoxContainer = _winMenu.GetNode("VBoxContainer");
            AssertThat(vBoxContainer).IsNotNull().OverrideFailureMessage("VBoxContainer node is missing in WinMenu.");

            var exitButton = vBoxContainer.GetNode<Button>("Button3");
            AssertThat(exitButton).IsNotNull().OverrideFailureMessage("Button3 is missing in WinMenu's VBoxContainer.");
        }

        [TestCase]
        public void LoseMenuExitButtonExists()
        {
            InstantiateLoseMenu();

            var vBoxContainer = _loseMenu.GetNode("VBoxContainer");
            AssertThat(vBoxContainer).IsNotNull().OverrideFailureMessage("VBoxContainer node is missing in LoseMenu.");

            var exitButton = vBoxContainer.GetNode<Button>("Button3");
            AssertThat(exitButton).IsNotNull().OverrideFailureMessage("Button3 is missing in LoseMenu's VBoxContainer.");
        }

        [TestCase]
        public void MainMenuStartButtonExists()
        {
            InstantiateMainMenu();
            var startButton = _mainMenu.GetNode("VBoxContainer/Button") as Button;
            AssertThat(startButton).IsNotNull().OverrideFailureMessage("Start button is missing in MainMenu.");
        }

    

        

        // Boundary Tests
        [TestCase]
        public void MenuButtonsShouldBeInteractiveOnReady()
        {
            InstantiateMainMenu();
            var startButton = _mainMenu.GetNode<Button>("VBoxContainer/Button");
            var exitButton = _mainMenu.GetNode<Button>("VBoxContainer/Button3");

            AssertThat(startButton.Disabled).IsFalse().OverrideFailureMessage("Start button should be interactive on Ready.");
            AssertThat(exitButton.Disabled).IsFalse().OverrideFailureMessage("Exit button should be interactive on Ready.");
        }

        [TestCase]
        public void OnlyOneMenuShouldBeActiveAtATime()
        {
            InstantiateMainMenu();
            _mainMenu.Visible = true;

            InstantiateWinMenu();
            _winMenu.Visible = false;

            InstantiateLoseMenu();
            _loseMenu.Visible = false;

            // Check visibility explicitly
            AssertThat(_mainMenu.Visible).IsTrue().OverrideFailureMessage("Expected MainMenu to be visible.");
            AssertThat(_winMenu.Visible).IsFalse().OverrideFailureMessage("Expected WinMenu to be hidden.");
            AssertThat(_loseMenu.Visible).IsFalse().OverrideFailureMessage("Expected LoseMenu to be hidden.");
        }




        
        
        





        // Stress Test
        [TestCase]
        public void StressTestMenuInstantiationAndDestruction()
        {
            GD.Print("Running StressTestMenuInstantiationAndDestruction...");
            int iterations = 1000;

            for (int i = 0; i < iterations; i++)
            {
                var menu = _mainMenuScene.Instantiate<MainMenu>();
                AddChild(menu);
                AssertThat(menu).IsNotNull().OverrideFailureMessage($"Menu instantiation failed at iteration {i}.");

                menu.QueueFree();
                AssertThat(menu.IsQueuedForDeletion()).IsTrue();

                if (i % 100 == 0)
                {
                    GD.Print($"Iteration {i}/{iterations}: Instantiation and destruction successful.");
                }
            }

            GD.Print("Stress test completed successfully.");
        }

        [After]
        public void TearDown()
        {
            _mainMenu?.QueueFree();
            _winMenu?.QueueFree();
            _loseMenu?.QueueFree();
        }
    }
}
