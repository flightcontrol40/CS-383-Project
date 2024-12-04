using Godot;
using System;
namespace Chicken;

public enum Cost{
    ChickenR1 = 1,
    ChickenR2 = 2,
    ChickenR3 = 4,
    ChickenR4 = 8,
}

public static partial class ChickenFactory { //Factory Pattern used by round manager 

    public static BaseChicken MakeKFC(Cost cost){
        BaseChicken chicken; // Sets initial chicken variable as BaseChicken
        switch (cost){
            case Cost.ChickenR4:
            // Instantiates Tier 4 chicken
            chicken = GD.Load<PackedScene>("res://src/Clayton/Enemy/DearGodOhLordThatGuyIsHuge.tscn").Instantiate<DearGodOhLordThatGuyIsHuge>();
            break;
            case Cost.ChickenR3:     
            // Instantiates Tier 3 chicken
            chicken = GD.Load<PackedScene>("res://src/Clayton/Enemy/Frankest.tscn").Instantiate<Frankest>();
            break;
            case Cost.ChickenR2:
            // Instantiates Tier 2 chicken
            chicken = GD.Load<PackedScene>("res://src/Clayton/Enemy/Frank.tscn").Instantiate<Frank>();
            break;
            default:
            // Instantiates base chicken
            chicken = GD.Load<PackedScene>("res://src/Clayton/Enemy/BaseChicken.tscn").Instantiate<BaseChicken>();
            GD.Print(chicken.Health);
            break;
        }
        // Returns chicken of varing tier based on cost input into factory
        return chicken;
    }
}

public partial class BaseChicken : PathFollow2D{ //Super Class of BaseChicken

    public GodotObject soundManager;

    [Export]
    public int Health = 100; // Set base health
    public int Speed = 150; // Set base speed
    public int damageAmount {protected set; get; } = 10; // Set base damage
    public int EnemyRank = 1; //set rank for round manager money
    public bool Dead = false;

    public Path2D path; // Create variable for path following
    public bool started = false; // Used in start function to start chicken movement on path
    
    public Area2D _collisionArea; // for use in collision detection (tower bullets)


    /// <summary>
    /// Makes chicken a child to a path so that path following is possible
    /// </summary>

    

    public void Start(Path2D LevelPath) {
        this.path = LevelPath;
        if(this.GetParent() == null){
            LevelPath.AddChild(this);
        }
        else{
            Reparent(LevelPath);
        }
        this.Visible = true; 
        this.started = true;
        SetLoop(false); // Prevents looping of the path
        
    }
    /// <summary>
    /// Starts chicken along path after start function is called
    /// </summary>
    public override void _Process(double delta){
        if(started == true){
            this.SetProgress(Progress + (float)(delta * Speed)); // Increment the progress ratio based on the speed and delta time
            if (this.ProgressRatio >= 1) {
                EmitSignal(SignalName.EndOfPath, this); // Lets round manager / healthbar know when a chicken reaches end of path
            }
        }
   }

    public virtual void TakeDamage(int damageCounter){
        this.Health -= damageCounter; // Decrements health based on damage from towers
        if (Health <= 0 ){
            this.soundManager.Call("play_sfx","chicken_death");

            EmitSignal(SignalName.EnemyDied, this); // Emits death signal if base chicken dies to towers
            this.QueueFree(); // Free the Chicken

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
    // The Godot Signal that emits when a chicken splits to lower tier (Not applicable at base tier)
    /// </summary>
    [Signal]    
    public delegate void EnemySplitEventHandler(BaseChicken enemy);

    public override void _Ready(){
        this.Visible = false;
        // Reference the Area2D node
        _collisionArea = GetNode<Area2D>("Area2D");
        this.soundManager = GetTree().Root.GetNode<Node2D>("SoundManager");

        // Connect the "area_entered" signal from Area2D to detect collisions
        _collisionArea.Connect("area_entered", new Callable(this, nameof(OnAreaEntered)));
    }
    
    private void OnAreaEntered(Area2D area)
    {
        // Detects the "bullet" group when it collides with the chicken's Area2D
        if (area.IsInGroup("bullet")){
            // Calls take damage function to decrement chicken health
        } else {
            return; // Do nothing if not a bullet
        }
    }

    
}
