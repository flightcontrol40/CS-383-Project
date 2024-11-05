using Godot;
using System;

namespace Chicken;

public static partial class ChickenFactory {

	public static BaseChicken MakeKFC(int cost){
		BaseChicken chicken;
		if (cost < 20 && cost > 18){
			chicken = GD.Load<PackedScene>("res://src/Clayton/Enemy/DearGodOhLordThatGuyIsHuge.tscn").Instantiate<DearGodOhLordThatGuyIsHuge>();
		}else if (cost < 18 && cost > 16){
			chicken = GD.Load<PackedScene>("res://src/Clayton/Enemy/Frankest.tscn").Instantiate<Frankest>();
		}else if(cost < 15 && cost > 13){
			chicken = GD.Load<PackedScene>("res://src/Clayton/Enemy/Frank.tscn").Instantiate<Frank>();
		}else {
			chicken = GD.Load<PackedScene>("res://src/Clayton/Enemy/BaseChicken.tscn").Instantiate<BaseChicken>();
		}
		return chicken;
	}
}

public partial class BaseChicken : PathFollow2D{

	[Export]
	public int Health = 100;
	public double speed = 100;

	public int damageAmount {protected set; get; } = 10;

	public int EnemyRank { get; } // get rid of maybe

	public Path2D path;
	public bool started = false;
	
	public Area2D _collisionArea;

	/// <summary>
	/// Starts the enemy along the LevelPath
	/// </summary>
	
	public void Start(Path2D LevelPath) {
		this.path = LevelPath;
		this.path.AddChild(this);
		this.started = true;
		SetLoop(false);
		
	}
	/// <summary>
	/// 
	/// </summary>
	/// <param name="delta">The amount of time thats passed since the last call.</param>
	public override void _Process(double delta){
		if (started == true){
			// Increment the progress ratio based on the speed and delta time
			this.SetProgress(Progress + (float)(delta * speed));
			//GD.Print(ProgressRatio);
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
			this.QueueFree();
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
		
	/// <summary>
	// For when chicken splits to lower tier
	/// </summary>
	[Signal]	
	public delegate void EnemySplitEventHandler(BaseChicken enemy);

	public override void _Ready(){
		// Reference the Area2D node
		_collisionArea = GetNode<Area2D>("ChickenSprite/Area2D");

		// Ensure the collision area is found
		if (_collisionArea == null)
		{
			GD.Print("Collision area not found!");
			return;
		}

		// Connect the "body_entered" signal from Area2D to detect collisions
		_collisionArea.Connect("area_entered", new Callable(this, nameof(OnAreaEntered)));
		GD.Print("Connected body_entered signal.");
	}
	
	private void OnAreaEntered(Area2D area)
	{
		if (area.IsInGroup("bullet")){
		GD.Print("Another Area2D entered the Chicken's area:", area.Name);		
		} else {
			return;
		}
		
	}
}


//public partial class Frank : BaseChicken{
	//new public int Health = 300;
	//new public double speed = 200;
	//new public int damageAmount { get; } = 30;
	////new public int EnemyRank { get; } = 5;
//
	//public override void TakeDamage(int damageCounter){
		//this.Health -= damageCounter;
		//if (this.Health <= 0){
			////change image / downgrade health, damage, speed
		//}
	//}
	//// base: emit death
	////change image / downgrade health, damage, speed
	//
//
//}


//public partial class Frankest : BaseChicken{
	//new public int Health = 900;
	//new public double speed = 0.3;
	//new public int damageAmount { get; } = 90;
	////new public int EnemyRank { get; } = 50;
//
	//public override void TakeDamage(int damageCounter){
		//this.Health -= damageCounter;
		//if (this.Health <= 0){
			////change image / downgrade health, damage, speed
		//}
//
//
	//}
//
//}


//public partial class DearGodOhLordThatGuyIsHuge: BaseChicken{
	//new public int Health = 2500;
	//new public double speed = 0.1;
//
	//new public int damageAmount { get; } = 250;
//
	////new public int EnemyRank { get; } = 250;
//
	//public override void TakeDamage(int damageCounter){
		//this.Health -= damageCounter;
		//if (this.Health <= 0){
			////change image / downgrade health, damage, speed
		//}
	//}
//}
