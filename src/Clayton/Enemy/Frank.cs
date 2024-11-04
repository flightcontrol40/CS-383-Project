using Godot;
using System;
using Chicken;

public partial class Frank : BaseChicken{

	public override void TakeDamage(int damageCounter){
		this.Health -= damageCounter; // decrement health
		if (this.Health <= 0){
			BaseChicken chicken = GD.Load<PackedScene>("res://src/Clayton/Enemy/BaseChicken.tscn").Instantiate<BaseChicken>(); //spawn new chicken
			chicken.SetProgress(this.Progress); //set new chicken location where chicken died
			chicken.Start(path); //start new chicken on the path
			EmitSignal(SignalName.EnemySplit, chicken); // tell round manager chicken has split	
			this.QueueFree(); // free old chicken
			 
		}
	}
	
	public override void _Ready() {
		speed = 200;
		Health = 300;
		damageAmount = 30;
	}
}
