using GdMUT;
using Chicken;
using Godot;
using System;

public partial class TestClayton : PathFollow2D
{
    private const string TEST_CHICKEN = "res://src/Clayton/Enemy/BaseChicken.tscn";
    [CSTestFunction]
    public static Result HealthTest()
    {
        try
        {
            var scene = ResourceLoader.Load<PackedScene>(TEST_CHICKEN);
            var instance = scene.Instantiate<BaseChicken>();
        
            int x = instance.Health;

            return (x == 100) ? new Result(true, "Proved that health is 100") : Result.Failure;
        }
        catch (Exception ex)
        {
            return new Result(false, $"Exception occurred: {ex.Message}");
        }
    }
    
        [CSTestFunction]
    public static Result SpeedTest()
    {
        try
        {
            var scene = ResourceLoader.Load<PackedScene>(TEST_CHICKEN);
            var instance = scene.Instantiate<BaseChicken>();
        
            double x = instance.speed;

            return (x == 0.1) ? new Result(true, "Proved that speed is 0.1") : Result.Failure;
        }
        catch (Exception ex)
        {
            return new Result(false, $"Exception occurred: {ex.Message}");
        }
    }
    
    
    [CSTestFunction]
    public static Result StressTest()
    {
        var watch = new System.Diagnostics.Stopwatch();
        var scene2 = ResourceLoader.Load<PackedScene>(TEST_CHICKEN); 
        int counter = 0;
        System.Collections.Generic.Dictionary<int,System.TimeSpan> Results = new System.Collections.Generic.Dictionary<int,System.TimeSpan> {};
        for (int i = 0; i < 600000; i++)
        {
            watch.Start();
            if (i % 100000 == 0)
            {
                Results.Add(i, watch.Elapsed);
                watch.Reset();
            }
            if (Results.Count > 1000){ Results.Clear();}
            var instance2 = scene2.Instantiate<BaseChicken>();
            counter += 1;
        }
        var asString = string.Join(System.Environment.NewLine, Results);
        return (counter == 700000) ? new Result(true, $"Results per 100,000 Chickens instantiated {asString}") : Result.Failure;
            
        
    }
    
    
}
