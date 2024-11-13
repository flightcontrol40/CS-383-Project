using Godot;
using System;
using GdUnit4;
using Chicken;
using static GdUnit4.Assertions;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ClaytonTest {
	[TestSuite]
	public partial class ChickenStressTest : PathFollow2D {
		public PackedScene chickenScene;
		public List<BaseChicken> chickens = new List<BaseChicken>();
		public const int MaxChickens = 1000000;
		public const int MinFPS = 100;
		[Before]
		public void ChickenLoaderS() {
			// Load the BaseChicken scene once to avoid repeated loading
			chickenScene = GD.Load<PackedScene>("res://src/Clayton/Enemy/BaseChicken.tscn");
		}
		[TestCase]
		public async Task StressTest() {
			for (int i = 0; i < MaxChickens; i++) {
				var chicken = chickenScene.Instantiate<BaseChicken>();
				AddChild(chicken);
				chickens.Add(chicken);
				GD.Print("working");
				// Check FPS and break if it drops below the threshold
				double fps = Engine.GetFramesPerSecond();
				if (fps < MinFPS) {
					AssertThat(fps < MinFPS).IsTrue();
					break;
				}
				// Yield to prevent freezing
				await ToSignal(GetTree(), "idle_frame");
			}
		}
		[After]
		public void Cleanup() {
			// Free all chickens to prevent orphans
			foreach (var chicken in chickens) {
				chicken.QueueFree();
			}
			chickens.Clear();
			AssertThat(chickens.Count == 0).IsTrue();		
		}
	}
}
