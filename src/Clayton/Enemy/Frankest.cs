using Godot;
using System;
using Chicken;

public partial class Frankest : BaseChicken{
	new public int Health = 900;
	new public double speed = 0.3;
	new public int damageAmount { get; } = 90;
	//new public int EnemyRank { get; } = 50;

	public override void TakeDamage(int damageCounter){
		this.Health -= damageCounter;
		if (this.Health <= 0){
			//change image / downgrade health, damage, speed
		}


	}

}
