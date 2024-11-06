using Godot;
using System;
using Chicken;

public partial class Frankest : BaseChicken{

	public Frankest(){
		Speed = 250; // Set tier 3 speed
		Health = 900; // Set tier 3 health
		damageAmount = 90; // Set tier 3 damage 
		EnemyRank = 3; // Set rank for round manager money
	}

	public override void TakeDamage(int damageCounter){
		this.Health -= damageCounter; // decrement health
		if (this.Health <= 0){
			// Spawn chicken of lower tier
			Frank chicken = GD.Load<PackedScene>("res://src/Clayton/Enemy/Frank.tscn").Instantiate<Frank>();
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
