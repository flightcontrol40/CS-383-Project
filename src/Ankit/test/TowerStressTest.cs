
using System.Threading.Tasks;
using Godot;
using GdUnit4;

[TestSuite]
public partial class TowerHardwareStressTest : Node
{
    [TestCase(Timeout = 1800000)] 
    public async Task StressTests()
    {
        ISceneRunner runner = ISceneRunner.Load("res://src/Ankit/test/tower_stress_runner.tscn");
        runner.SetTimeFactor(50);
        
        const int initialTowers = 100;
        const int towersPerRound = 50;
        int totalTowers = initialTowers;
        double worstFPS = 120;
        int peakBulletCount = 0;
        float startTime = Time.GetTicksMsec();

        runner.Invoke("AddTowers", initialTowers);
        GD.Print($"\n=== Starting EXTREME Stress Test with {initialTowers} Towers ===");
        
        await runner.SimulateFrames(60);

        bool testRunning = true;
        int roundNumber = 1;
        string failureReason = "";
        int framesBelow30 = 0;
        int consecutiveLowFPSFrames = 0;

        while (testRunning)
        {
            GD.Print($"\n=== Round {roundNumber} ===");
            
            for (int frame = 0; frame < 300; frame++)
            {
                await runner.SimulateFrames(1);
                
                var currentFPS = Performance.GetMonitor(Performance.Monitor.TimeFps);
                var bulletCount = (int)runner.GetProperty("ActiveBulletCount");
                
                peakBulletCount = Mathf.Max(peakBulletCount, bulletCount);
                worstFPS = Mathf.Min(worstFPS, currentFPS);

                if (currentFPS < 30) framesBelow30++;
                if (currentFPS < 5) 
                    consecutiveLowFPSFrames++;
                else 
                    consecutiveLowFPSFrames = 0;

                if (frame % 30 == 0)
                {
                    float elapsedTime = (Time.GetTicksMsec() - startTime) / 1000f;
                    float memoryUsageMB = OS.GetStaticMemoryUsage() / (1024.0f * 1024.0f);
                    
                    GD.Print($"Status - FPS: {currentFPS:F1} | Bullets: {bulletCount} | Towers: {totalTowers}");
                    GD.Print($"Memory Usage: {memoryUsageMB:F1}MB");
                    GD.Print($"Test Duration: {elapsedTime:F1} seconds");
                    GD.Print($"Frames Below 30 FPS: {framesBelow30}");
                }

                // Check failure conditions
                if (consecutiveLowFPSFrames > 100)
                {
                    testRunning = false;
                    failureReason = "Sustained FPS below 5 for 100 frames";
                    break;
                }
                if (bulletCount > 50000)
                {
                    testRunning = false;
                    failureReason = "Bullet count exceeded 50,000";
                    break;
                }
                
                // Safe memory check using float
                float memoryGB = OS.GetStaticMemoryUsage() / (1024.0f * 1024.0f * 1024.0f);
                if (memoryGB > 3.5f)
                {
                    testRunning = false;
                    failureReason = "Memory usage exceeded 3.5GB";
                    break;
                }
                
                if (framesBelow30 > 1000)
                {
                    testRunning = false;
                    failureReason = "Sustained poor performance (>1000 frames below 30 FPS)";
                    break;
                }
            }

            if (testRunning)
            {
                // Add more towers (safely handle potential overflow)
                int newTowers = Mathf.Min(towersPerRound * (1 + roundNumber / 5), 1000);
                runner.Invoke("AddTowers", newTowers);
                totalTowers += newTowers;
                roundNumber++;
                GD.Print($"\nAdded {newTowers} more towers. New total: {totalTowers}");
                
                runner.Invoke("IncreaseStress");
            }
        }

        float totalTime = (Time.GetTicksMsec() - startTime) / 1000f;
        float finalMemoryMB = OS.GetStaticMemoryUsage() / (1024.0f * 1024.0f);

        // Generate report
        GD.Print("\n============================================");
        GD.Print("     HARDWARE STRESS TEST FAILURE REPORT     ");
        GD.Print("============================================");
        GD.Print($"Test Duration: {totalTime:F1} seconds");
        GD.Print($"Total Rounds Completed: {roundNumber}");
        GD.Print($"Final Tower Count: {totalTowers}");
        GD.Print($"Peak Bullet Count: {peakBulletCount}");
        GD.Print($"Worst FPS Recorded: {worstFPS:F1}");
        GD.Print($"Frames Below 30 FPS: {framesBelow30}");
        GD.Print($"Failure Reason: {failureReason}");
        GD.Print($"Final Memory Usage: {finalMemoryMB:F1}MB");
        GD.Print("--------------------------------------------");
        GD.Print("System Information:");
        GD.Print($"OS: {OS.GetName()}");
        GD.Print($"CPU Cores: {OS.GetProcessorCount()}");
        GD.Print($"Video Adapter: {OS.GetVideoAdapterDriverInfo()}");
        GD.Print("============================================");

        runner.Invoke("Cleanup");
        
        throw new System.Exception($"Hardware stress test completed: {failureReason}");
    }
}
