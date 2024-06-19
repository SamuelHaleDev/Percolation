using System.Diagnostics;

namespace SamUtilities;

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

    public static T TimeFunction<T>(Func<T> function, string funcName)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        T result = function();

        stopwatch.Stop();
        string elapsedTime = String.Format("{0:00}:{1:00}", stopwatch.Elapsed.Minutes, stopwatch.Elapsed.Seconds);
        Console.WriteLine($"Execution Time: {elapsedTime} (mm:ss)");

        return result;
    }
}