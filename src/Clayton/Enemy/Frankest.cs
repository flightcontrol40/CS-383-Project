using Godot;
using System;
using Chicken;

public partial class Frankest : BaseChicken{

	public Frankest(){
		Speed = 400;
		Health = 900;
		damageAmount = 90;
		EnemyRank = 3;
	}

	public override void TakeDamage(int damageCounter){
		this.Health -= damageCounter; // decrement health
		if (this.Health <= 0){
			Frank chicken = GD.Load<PackedScene>("res://src/Clayton/Enemy/Frank.tscn").Instantiate<Frank>(); //spawn new chicken
			chicken.SetProgress(this.Progress); //set new chicken location where chicken died
			chicken.Start(path); //start new chicken on the path
			EmitSignal(SignalName.EnemySplit, chicken); // tell round manager chicken has split
			EmitSignal(SignalName.EnemyDied, this); // tell round manager old chicken has died	
			 
		}
	}
	
	public override void _Ready() {
		base._Ready();
	}
}
