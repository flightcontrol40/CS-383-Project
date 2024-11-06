using Godot;
using System;
using Chicken;

public partial class Frank : BaseChicken{

	public Frank(){
		Speed = 200; // Set tier 2 speed 
		Health = 300; // Set tier 2 health
		damageAmount = 30; // Set tier 2 damage 
		EnemyRank = 2; // Set rank for round manager money
	}
	
	public override void TakeDamage(int damageCounter){
		this.Health -= damageCounter; // decrement health
		if (this.Health <= 0){
			// Spawn chicken of lower tier
			BaseChicken chicken = GD.Load<PackedScene>("res://src/Clayton/Enemy/BaseChicken.tscn").Instantiate<BaseChicken>(); 
			chicken.SetProgress(this.Progress); //set new chicken location where chicken died
			chicken.Start(path); //start new chicken on the path
			EmitSignal(SignalName.EnemySplit, chicken); // tell round manager chicken has split	
			EmitSignal(SignalName.EnemyDied, this); // tell round manager old chicken has died	
		}
	}
	
	public override void _Ready() {
		base._Ready(); // Call base _Ready function for collision detection
	}
}
