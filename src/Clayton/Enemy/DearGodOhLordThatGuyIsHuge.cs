using Godot;
using System;
using Chicken;

public partial class DearGodOhLordThatGuyIsHuge: BaseChicken{
	new public int Health = 2500;
	new public double speed = 0.1;

	new public int damageAmount { get; } = 250;

	//new public int EnemyRank { get; } = 250;

	public override void TakeDamage(int damageCounter){
		this.Health -= damageCounter;
		if (this.Health <= 0){
			//change image / downgrade health, damage, speed
		}
	}
}
