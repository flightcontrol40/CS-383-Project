using Godot;
using System;
using Chicken;

public partial class Frank : BaseChicken{

	public Frank(){
		Speed = 200;
		Health = 300;
		damageAmount = 30;
		EnemyRank = 2;
	}
	
	public override void TakeDamage(int damageCounter){
		this.Health -= damageCounter; // decrement health
		if (this.Health <= 0){
			BaseChicken chicken = GD.Load<PackedScene>("res://src/Clayton/Enemy/BaseChicken.tscn").Instantiate<BaseChicken>(); //spawn new chicken
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
