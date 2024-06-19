using System;
using System.Linq;
using PercolationClass;
using SamUtilities;

namespace PercolationStats;

public class PercolationStats
{
    private readonly double mean;
    private readonly double stddev;
    private readonly double T;

    public PercolationStats(int N, int T)
    {
        if (N <= 0 || T <= 0)
        {
            throw new ArgumentException();
        }
        this.T = T;
        double[] ratio = new double[T];
        Random rand = new Random();
        for (int i = 0; i < T; i += 1)
        {
            Percolation p = new Percolation(N);
            while (!p.Percolates())
            {
                int randRow = rand.Next(N);
                int randCol = rand.Next(N);
                p.Open(randRow, randCol);
            }
            ratio[i] = ((double)p.NumberOfOpenSites()) / (N * N);
        }

        this.mean = ratio.Average();
        this.stddev = Math.Sqrt(ratio.Select(x => Math.Pow(x - mean, 2)).Sum() / T);
    }

    public double Mean()
    {
        return mean;
    }

    public double Stddev()
    {
        return stddev;
    }

    public double ConfidenceLow()
    {
        return mean - 1.96 * stddev / Math.Sqrt(T);
    }

    public double ConfidenceHigh()
    {
        return mean + 1.96 * stddev / Math.Sqrt(T);
    }

    public static void Main(string[] args)
    {
        int trials = 100, gridSize = 50;
        Func<PercolationStats> constructPercolationStats = () => new PercolationStats(gridSize, trials);
        PercolationStats ps = Helpers.TimeFunction(constructPercolationStats, "PercolationStats");
        Console.WriteLine($"Grid Size: {gridSize} x {gridSize} | Number of Trials: {trials}");
        Console.WriteLine($"The mean percolation threshold is {ps.Mean():F2}");
        Console.WriteLine($"The standard deviation of the percolation threshold is {ps.Stddev():F2}");
        Console.WriteLine($"The 95% confidence interval is [{ps.ConfidenceLow():F3}, {ps.ConfidenceHigh():F3}]");
    }
}