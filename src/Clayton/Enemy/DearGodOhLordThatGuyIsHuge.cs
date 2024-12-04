using Godot;
using System;
using Chicken;

public partial class DearGodOhLordThatGuyIsHuge: BaseChicken{

	public DearGodOhLordThatGuyIsHuge(){
		Speed = 100; // Set tier 4 speed
		Health = 2500; // Set tier 4 health
		damageAmount = 250; // Set tier 4 damage 
		EnemyRank = 4; // Set rank for round manager money
	}

	public override void TakeDamage(int damageCounter){
		this.Health -= damageCounter; // decrement health
		if ((this.Health <= 0) && (this.Dead == false)){
			this.Dead = true; //prevents multiple splits from being called
			var rand = new Random();
			// Spawn chicken of lower tier
			Frankest chicken;
			for (int i = 0; i < 4; i++){
				chicken = GD.Load<PackedScene>("res://src/Clayton/Enemy/Frankest.tscn").Instantiate<Frankest>();
				chicken.SetProgress(this.Progress + ((float)rand.NextDouble() * 100)); //set new chicken location where chicken died
				chicken.Start(path); //start new chicken on the path
				EmitSignal(SignalName.EnemySplit, chicken); // tell round manager chicken has split
			}
			EmitSignal(SignalName.EnemyDied, this); // tell round manager old chicken has died
		}
	}
	
	public override void _Ready() {
		base._Ready(); // Call base _Ready function for collision detection
	}
}
