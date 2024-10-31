using Godot;
using System;
using Chicken;

public partial class DearGodOhLordThatGuyIsHuge: BaseChicken{

	public override void TakeDamage(int damageCounter){
		this.Health -= damageCounter; // decrement health
		GD.Print(Health);
		if (this.Health <= 0){
			EmitSignal(SignalName.EnemySplit, this); // tell round manager chicken has split			
			Frankest chicken = GD.Load<PackedScene>("res://src/Clayton/Enemy/Frankest.tscn").Instantiate<Frankest>(); //spawn new chicken
			chicken.SetProgress(this.Progress); //set new chicken location where chicken died
			chicken.Start(path); //start new chicken on the path
			this.QueueFree(); // free old chicken
		}
	}
	
	public override void _Ready() {
		speed = 250;
		Health = 2500;
		damageAmount = 250;
	}
}
