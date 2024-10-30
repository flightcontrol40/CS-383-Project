using Godot;
using System;
using Chicken;

public partial class Frank : BaseChicken{
	new public int Health = 300;
	new public double speed = 200;
	new public int damageAmount { get; } = 30;
	//new public int EnemyRank { get; } = 5;

	public override void TakeDamage(int damageCounter){
		this.Health -= damageCounter;
		if (this.Health <= 0){
			//change image / downgrade health, damage, speed
		}
	}
	// base: emit death
	//change image / downgrade health, damage, speed
	

}
