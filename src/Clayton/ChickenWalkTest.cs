using Godot;
using System;
using Chicken;
using System.Threading.Tasks;

public partial class ChickenWalkTest : Node2D
{
	public override void _Ready()
	{
		TimerCalc1();
		//TimerCalc2();
		//TimerCalc3();
		//TimerCalc4();
		//TimerCalc5();
		//TimerCalc6();
		//TimerCalc7();
		//TimerCalc8();
		//TimerCalc9();
		//TimerCalc10();
		//TimerCalc11();
		//TimerCalc12();
		
	}

	public async Task TimerCalc1(){
		BaseChicken chicken = ChickenFactory.MakeKFC(1);
		Path2D path = GetNode<Path2D>("Map/Path/Path2D");
		chicken.Start(path);
		for(int i = 0; i < 11; i++){
			await ToSignal(GetTree().CreateTimer(2f), "timeout");
			chicken.TakeDamage(10);
		}
	}
	public async Task TimerCalc2(){
		await ToSignal(GetTree().CreateTimer(.5f), "timeout");
		BaseChicken chicken = ChickenFactory.MakeKFC(1);
		Path2D path = GetNode<Path2D>("Map/Path/Path2D");
		chicken.Start(path);
		for(int i = 0; i < 11; i++){
			await ToSignal(GetTree().CreateTimer(1.3f), "timeout");
			chicken.TakeDamage(10);
		}
	}
	public async Task TimerCalc3(){
		await ToSignal(GetTree().CreateTimer(1f), "timeout");
		BaseChicken chicken = ChickenFactory.MakeKFC(1);
		Path2D path = GetNode<Path2D>("Map/Path/Path2D");
		chicken.Start(path);
		for(int i = 0; i < 11; i++){
			await ToSignal(GetTree().CreateTimer(1.6f), "timeout");
			chicken.TakeDamage(10);
		}
	}
	public async Task TimerCalc4(){
		await ToSignal(GetTree().CreateTimer(1.5f), "timeout");
		BaseChicken chicken = ChickenFactory.MakeKFC(1);
		Path2D path = GetNode<Path2D>("Map/Path/Path2D");
		chicken.Start(path);
		for(int i = 0; i < 11; i++){
			await ToSignal(GetTree().CreateTimer(2.2f), "timeout");
			chicken.TakeDamage(10);
		}
	}
	public async Task TimerCalc5(){
		await ToSignal(GetTree().CreateTimer(2.5f), "timeout");
		BaseChicken chicken = ChickenFactory.MakeKFC(14);
		Path2D path = GetNode<Path2D>("Map/Path/Path2D");
		chicken.Start(path);
		for(int i = 0; i < 11; i++){
			await ToSignal(GetTree().CreateTimer(1f), "timeout");
			chicken.TakeDamage(30);
		}
	}
	public async Task TimerCalc6(){
		await ToSignal(GetTree().CreateTimer(3f), "timeout");
		BaseChicken chicken = ChickenFactory.MakeKFC(14);
		Path2D path = GetNode<Path2D>("Map/Path/Path2D");
		chicken.Start(path);
		for(int i = 0; i < 11; i++){
			await ToSignal(GetTree().CreateTimer(1.2f), "timeout");
			chicken.TakeDamage(30);
		}
	}
	public async Task TimerCalc7(){
		await ToSignal(GetTree().CreateTimer(5f), "timeout");
		BaseChicken chicken = ChickenFactory.MakeKFC(14);
		Path2D path = GetNode<Path2D>("Map/Path/Path2D");
		chicken.Start(path);
		for(int i = 0; i < 11; i++){
			await ToSignal(GetTree().CreateTimer(0.8f), "timeout");
			chicken.TakeDamage(30);
		}
	}
	public async Task TimerCalc8(){
		await ToSignal(GetTree().CreateTimer(6.5f), "timeout");
		BaseChicken chicken = ChickenFactory.MakeKFC(14);
		Path2D path = GetNode<Path2D>("Map/Path/Path2D");
		chicken.Start(path);
		for(int i = 0; i < 11; i++){
			await ToSignal(GetTree().CreateTimer(1.5f), "timeout");
			chicken.TakeDamage(30);
		}
	}
	public async Task TimerCalc9(){
		await ToSignal(GetTree().CreateTimer(8f), "timeout");
		BaseChicken chicken = ChickenFactory.MakeKFC(17);
		Path2D path = GetNode<Path2D>("Map/Path/Path2D");
		chicken.Start(path);
		for(int i = 0; i < 21; i++){
			await ToSignal(GetTree().CreateTimer(0.5f), "timeout");
			chicken.TakeDamage(45);
		}
	}
	public async Task TimerCalc10(){
		await ToSignal(GetTree().CreateTimer(9.5f), "timeout");
		BaseChicken chicken = ChickenFactory.MakeKFC(17);
		Path2D path = GetNode<Path2D>("Map/Path/Path2D");
		chicken.Start(path);
		for(int i = 0; i < 21; i++){
			await ToSignal(GetTree().CreateTimer(0.3f), "timeout");
			chicken.TakeDamage(45);
		}
	}
	public async Task TimerCalc11(){
		await ToSignal(GetTree().CreateTimer(10.5f), "timeout");
		BaseChicken chicken = ChickenFactory.MakeKFC(17);
		Path2D path = GetNode<Path2D>("Map/Path/Path2D");
		chicken.Start(path);
		for(int i = 0; i < 21; i++){
			await ToSignal(GetTree().CreateTimer(0.6f), "timeout");
			chicken.TakeDamage(45);
		}
	}
	public async Task TimerCalc12(){
		await ToSignal(GetTree().CreateTimer(15f), "timeout");
		BaseChicken chicken = ChickenFactory.MakeKFC(19);
		Path2D path = GetNode<Path2D>("Map/Path/Path2D");
		chicken.Start(path);
		for(int i = 0; i < 26; i++){
			await ToSignal(GetTree().CreateTimer(0.3f), "timeout");
			chicken.TakeDamage(100);
		}
	}

}
