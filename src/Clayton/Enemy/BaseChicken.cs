using Godot;
using System;

namespace Chicken;

public class ChickenFactory {

	public BaseChicken MakeKFC( int chickenRank){
		switch (chickenRank)
		{
			case 4:
				return new DearGodOhLordThatFuckerIsHuge();
			case 3:
				return new Frankest();
			case 2:
				return new Frank();
			default:
				return new BaseChicken();
		} 
	}

}

public partial class BaseChicken : PathFollow2D{

	[Export]
	public int Health = 100;
	public double speed = 0.1;

	public int damageAmount { get; } = 10;

	public int EnemyRank { get; }

	private Path2D path;
	private bool started = false;

	/// <summary>
	/// Starts the enemy along the LevelPath
	/// </summary>
	
	public void Start(Path2D LevelPath) {
		this.path = LevelPath;
		this.path.AddChild(this);
		this.Position = Vector2.Zero;
		this.started = true;
		
	}
	/// <summary>
	/// 
	/// </summary>
	/// <param name="delta">The amount of time thats passed since the last call.</param>
	public override void _Process(double delta){
		if (started){
			// Increment the progress ratio based on the speed and delta time
			this.ProgressRatio += (float)(delta * speed);
			// Check if the progress ratio has reached or exceeded 1
			if (this.ProgressRatio >= 1)
			{
				EmitSignal(SignalName.EndOfPath, this);

			}
			if (Health <= 0 ){
				EmitSignal(SignalName.EnemyDied, this);
			}
		}
   }

	public virtual void TakeDamage(int damageCounter){
		this.Health -= damageCounter;
	}






	/// <summary>
	// The Godot Signal to emit if the enemy dies before reaching the end of the
	// Path2D
	/// </summary>
	[Signal]
	public delegate void EnemyDiedEventHandler(BaseChicken enemy);

	/// <summary>
	// The Godot signal to emit if the enemy reaches the end of the path before 
	// being killed
	/// </summary>
	[Signal]
	public delegate void EndOfPathEventHandler(BaseChicken enemy);
		// if(progress ratio == 1)

}


public partial class Frank : BaseChicken{
	new public int Health = 300;
	new public double speed = 0.2;
	new public int damageAmount { get; } = 30;
	new public int EnemyRank { get; } = 5;

	public override void TakeDamage(int damageCounter){
		this.Health -= damageCounter;
		if (this.Health <= 0){
			//change image / downgrade health, damage, speed
		}
	}
	// base: emit death
	//change image / downgrade health, damage, speed
	

}


public partial class Frankest : BaseChicken{
	new public int Health = 900;
	new public double speed = 0.3;
	new public int damageAmount { get; } = 90;
	new public int EnemyRank { get; } = 50;

	public override void TakeDamage(int damageCounter){
		this.Health -= damageCounter;
		if (this.Health <= 0){
			//change image / downgrade health, damage, speed
		}


	}

}


public partial class DearGodOhLordThatFuckerIsHuge: BaseChicken{
	new public int Health = 2500;
	new public double speed = 0.1;

	new public int damageAmount { get; } = 250;

	new public int EnemyRank { get; } = 250;

	public override void TakeDamage(int damageCounter){
		this.Health -= damageCounter;
		if (this.Health <= 0){
			//change image / downgrade health, damage, speed
		}
	}
}
