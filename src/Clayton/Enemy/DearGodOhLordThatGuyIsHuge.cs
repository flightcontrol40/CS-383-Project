using Godot;
using System;
using Chicken;

public partial class DearGodOhLordThatGuyIsHuge: BaseChicken{

	public DearGodOhLordThatGuyIsHuge(){
		Speed = 250; // Set tier 4 speed
		Health = 2500; // Set tier 4 health
		damageAmount = 250; // Set tier 4 damage 
		EnemyRank = 4;
	}

	public override void TakeDamage(int damageCounter){
		this.Health -= damageCounter; // decrement health
		GD.Print(Health);
		if (this.Health <= 0){
			// Spawn chicken of lower tier
			Frankest chicken = GD.Load<PackedScene>("res://src/Clayton/Enemy/Frankest.tscn").Instantiate<Frankest>();
			chicken.SetProgress(this.Progress); //set new chicken location where chicken died
			chicken.Start(path); //start new chicken on the path
			EmitSignal(SignalName.EnemySplit, chicken); // tell round manager chicken has split		
			EmitSignal(SignalName.EnemyDied, this); // tell round manager old chicken has died	
	}
	
	public override void _Ready() {
		base._Ready(); // Call base _Ready function for collision detection
	}
}
