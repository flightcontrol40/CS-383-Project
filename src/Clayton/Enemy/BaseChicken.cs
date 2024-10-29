using Godot;
using System;

namespace Chicken;

public class ChickenFactory {

	public static BaseChicken MakeKFC(int cost){
		if (cost < 20 || cost > 18){
			BaseChicken chicken4 = new DearGodOhLordThatGuyIsHuge();
			return chicken4;
		}else if (cost < 18 || cost > 16){
			BaseChicken chicken3 = new Frankest();
			return chicken3;
		}else if(cost < 15 || cost > 13){
			BaseChicken chicken2 = new Frank();
			return chicken2;
		}else {
			BaseChicken chicken1 = new BaseChicken();
			return chicken1;
		}
	}
}

public partial class BaseChicken : PathFollow2D{

	[Export]
	public int Health = 100;
	public double speed = 100;

	public int damageAmount { get; } = 10;

	public int EnemyRank { get; }

	private Path2D path;
	private bool started = false;

	/// <summary>
	/// Starts the enemy along the LevelPath
	/// </summary>
	
	public void Start(Path2D LevelPath) {
		GD.Print($"running");
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
		GD.Print($"running2");
		if (started == true){
			// Increment the progress ratio based on the speed and delta time
			this.SetProgress(Progress + (float)(delta * speed));
			GD.Print(Progress);
			GD.Print(GlobalPosition);
			GD.Print(IsVisibleInTree());
			// Check if the progress ratio has reached or exceeded 1
			if (this.ProgressRatio >= 1)
			{
				EmitSignal(SignalName.EndOfPath, this);

			}
		}
   }

	public virtual void TakeDamage(int damageCounter){
		this.Health -= damageCounter;
		if (Health <= 0 ){
			EmitSignal(SignalName.EnemyDied, this);
		}
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
