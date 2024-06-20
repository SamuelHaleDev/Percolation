using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Drawing;
using PercolationClass;
using System.Diagnostics;

namespace PercolationPicture;

public class PercolationPicture
{
    private static readonly int DELAY = 100;
    private static Bitmap bitmap;
    private static Graphics graphics;

    public static void Draw(Percolation perc, int N, string filename)
    {
        int cellSize = 64;
        int textSize = 100;
        bitmap = new Bitmap(N * cellSize, N * cellSize + textSize);
        graphics = Graphics.FromImage(bitmap);

        graphics.Clear(Color.White);

        for (int row = 0; row < N; row++)
        {
            for (int col = 0; col < N; col++)
            {
                bool open = perc.IsOpen(row, col);
                bool full = perc.IsFull(row, col);

                SolidBrush brush;

                if (open && full)
                {
                    brush = new SolidBrush(Color.Blue);
                }
                else if (open)
                {
                    brush = new SolidBrush(Color.White);
                }
                else
                {
                    brush = new SolidBrush(Color.Black);
                }

                graphics.FillRectangle(brush, col*cellSize, row*cellSize, 63, 63);
                brush.Dispose();
            }
        }

        Font font = new Font("SansSerif", 20);
        SolidBrush textBrush = new SolidBrush(Color.Black);
        string openSites = $"{perc.NumberOfOpenSites()} open sites";
        graphics.DrawString(openSites, font, textBrush, 0.0F, bitmap.Height - 100);
        string percolatesText = perc.Percolates() ? "percolates" : "does not percolate";
        graphics.DrawString(percolatesText, font, textBrush, 0.0F, bitmap.Height - 100 + 22 );

        string filenameWithoutExtension = Path.GetFileNameWithoutExtension(filename);
        bitmap.Save($"{filenameWithoutExtension}.png"); // Save the image to a file
    }

    public static void ShowImage(string filename)
    {
        Process.Start(new ProcessStartInfo(filename) { UseShellExecute = true });
    }

    public static void SimulateFromFile(string filename)
    {
        var lines = File.ReadLines(filename).ToList();
        int N = int.Parse(lines[0]);
        Percolation perc = new Percolation(N);

        Draw(perc, N, filename);
        Thread.Sleep(DELAY);

        for (int i = 1; i < lines.Count; i++)
        {
            var parts = lines[i].Split(new [] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int row = int.Parse(parts[0]);
            int col = int.Parse(parts[1]);
            perc.Open(row, col);
            Draw(perc, N, filename);
            Thread.Sleep(DELAY);
        }
    }

    private static string PickRandomFile()
    {
        var files = Directory.GetFiles("inputFiles");
        if (files.Length == 0)
        {
            throw new Exception("Could not find inputFiles");
        }
        var random = new Random();
        return files[random.Next(files.Length)];
    }

    public static void Main(string[] args)
    {
        string filename;
        if (args.Length == 1)
        {
            filename = args[0];
        }
        else
        {
            filename = PickRandomFile();
        }
        Console.WriteLine("Drawing file " + filename);
        SimulateFromFile(filename);
    }
}