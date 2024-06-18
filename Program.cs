using Algs4;

namespace Percolation;

class Program
{
    static void Main(string[] args)
    {
        WeightedQuickUnionUF uf = new WeightedQuickUnionUF(10);

        uf.Union(0, 1);
        Console.WriteLine("0 and 1 connected? " + uf.Connected(0, 1));
        Console.WriteLine("0 and 2 connected? " + uf.Connected(0, 2));
    }
}
