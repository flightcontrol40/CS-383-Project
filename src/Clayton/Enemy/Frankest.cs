using Godot;
using System;
using Chicken;

public partial class Frankest : BaseChicken{

	public override void TakeDamage(int damageCounter){
		this.Health -= damageCounter; // decrement health
		if (this.Health <= 0){
			EmitSignal(SignalName.EnemySplit, this); // tell round manager chicken has split			
			Frank chicken = GD.Load<PackedScene>("res://src/Clayton/Enemy/Frank.tscn").Instantiate<Frank>(); //spawn new chicken
			chicken.SetProgress(this.Progress); //set new chicken location where chicken died
			chicken.Start(path); //start new chicken on the path
			this.QueueFree(); // free old chicken
			 
		}
	}
	
	public override void _Ready() {
		speed = 1000;
		Health = 900;
		damageAmount = 90;
	}
}
