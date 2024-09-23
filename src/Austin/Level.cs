using Godot;
using System;
using System.Security.Cryptography.X509Certificates;

enum Base_Difficulty : int {
   easy = 0,
   medium = 1,
   hard = 2
}


public partial class Level : Node
{
   [Export]
   private Base_Difficulty level_difficulty = Base_Difficulty.easy;

   // Called when the node enters the scene tree for the first time.
   public override void _Ready()
   {
   }
   
   // Called every frame. 'delta' is the elapsed time since the previous frame.
   public override void _Process(double delta)
   {
   }
}
