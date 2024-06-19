using System;
using System.Diagnostics;

using namespace SamUtilities;

class Helpers
{
    public static void RunTest(Func<bool> test, string funcName) 
    {
        Console.WriteLine($"Running {funcName} test...");

        bool passed = test();

        if (!passed) 
        {
            Console.WriteLine($"{funcName} test failed!");
            return;
        }

        Console.WriteLine($"{funcName} test passed!");
    }

    public static void TimeFunction(Func<dynamic> function, string funcName)
    {
        var stopwatch = new Diagnostics.Stopwatch();
        stopwatch.Start();

        dynamic result = function();

        stopwatch.Stop();
        TimeSpan ts = stopwatch.Elapsed;
        string elapsedTime = String.Format("{0:00}:{1:00}", ts.Minutes, ts.Seconds);
        Console.WriteLine($"Execution Time: {elapsedTime} (mm:ss)");
    }
}