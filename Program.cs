using Algs4;

namespace PercolationClass;

public class Percolation
{
    private bool[,] grid;
    private int openSites;
    private WeightedQuickUnionUF connectedOpenSites;
    private readonly int TOP;
    private readonly int BOTTOM;
    private int size;

    /* 
    
    SANITY CHECK:
    You are checking by hand that all conditions are handled with a 3x3 square
    N = 3
    */

    public Percolation(int N)  
    {
        size = N;
        grid = new bool[N,N];
        openSites = 0;
        // We want to create "alter egos" for the bottom layer by adding N
        connectedOpenSites = new WeightedQuickUnionUF((N * N) + 2 + N); // add 2 elements for a top and bottom layer
        TOP = (N * N); // DS elements 0 - (N*N) - 1 are sites so N*N is our top layer
        BOTTOM = (N * N) + 1;
    }    

    public void Open(int row, int col)  
    {
        if (row < 0 || row > size || col < 0 || col > size) {
            throw new Exception("Provided row or column is out of bounds!");
        }
        if (IsOpen(row, col)) {
            Console.WriteLine("Square is already open");
            return;
        }

        bool isThisSiteConnectedToTop = (row == 0);
        bool isNorthSiteOpen = (!isThisSiteConnectedToTop && IsOpen((row - 1), col));
        bool isThisSiteConnectedToBottom = (row == (size - 1));
        bool isAlterEgo = (row == (size - 1));
        bool isSouthSiteOpen = (!isThisSiteConnectedToBottom && IsOpen((row + 1), col));
        bool isWestSiteOpen = (col > 0 && IsOpen(row, (col - 1)));
        bool isEastSiteOpen = (col < (size - 1) && IsOpen(row, (col + 1)));
        

        int targetedSite = ConvertCoordinateToSquareNumber(row, col);
        int alterEgoSite = 0;
        if (isAlterEgo) 
        {
            alterEgoSite = targetedSite + size + 2;
        }
        grid[row, col] = true;
        openSites++;
        
        if (isThisSiteConnectedToTop) 
        {
            connectedOpenSites.Union(TOP, targetedSite);
        }
        else if (isNorthSiteOpen) 
        {
            connectedOpenSites.Union(ConvertCoordinateToSquareNumber((row - 1), col), targetedSite);
        } 
        if (isThisSiteConnectedToBottom) 
        {
            connectedOpenSites.Union(BOTTOM, targetedSite);
        }
        else if (isSouthSiteOpen)
        {
            connectedOpenSites.Union(ConvertCoordinateToSquareNumber((row + 1), col), targetedSite);
        }
        if (isWestSiteOpen) 
        {
            connectedOpenSites.Union(ConvertCoordinateToSquareNumber(row, (col - 1)), targetedSite);
        }
        if (isEastSiteOpen) 
        {
            connectedOpenSites.Union(ConvertCoordinateToSquareNumber(row, (col + 1)), targetedSite);
        }

        if (!isAlterEgo) return;
        if (isWestSiteOpen) 
        {
            connectedOpenSites.Union(ConvertCoordinateToSquareNumber(row, (col - 1)), alterEgoSite);
        }
        if (isNorthSiteOpen)
        {
            connectedOpenSites.Union(ConvertCoordinateToSquareNumber((row - 1), col), alterEgoSite);
        }
        if (isEastSiteOpen) 
        {
            connectedOpenSites.Union(ConvertCoordinateToSquareNumber(row, (col + 1)), alterEgoSite);
        }
        if (isSouthSiteOpen) 
        {
            connectedOpenSites.Union(ConvertCoordinateToSquareNumber((row + 1), col), alterEgoSite);
        }
    }  

    public bool IsOpen(int row, int col)  
    {
        if (row < 0 || row > size || col < 0 || col > size) {
            throw new Exception("Provided row or column is out of bounds!");
        }

        return grid[row, col];
    }

    public bool IsFull(int row, int col)
    {
        if (row < 0 || row > size || col < 0 || col > size) {
            throw new Exception("Provided row or column is out of bounds!");
        }

        int targetedSite = ConvertCoordinateToSquareNumber(row, col);

        // When we are checking for bottom row use the alter ego to combat backwash
        if (row == (size - 1))
        {
            return connectedOpenSites.Connected(TOP, (targetedSite + size + 2));
        }

        return connectedOpenSites.Connected(TOP, ConvertCoordinateToSquareNumber(row, col));
    }  

    public int NumberOfOpenSites()  
    {
        return openSites;
    }      

    public bool Percolates()         
    {
        return connectedOpenSites.Connected(TOP, BOTTOM);
    }     

    public int ConvertCoordinateToSquareNumber(int row, int col) 
    {
        if (row < 0 || row >= size || col < 0 || col >= size) {
            throw new Exception("Provided row or column is out of bounds!");
        }
        
        return (row * size) + col;
    }
}

class Program
{
    static void Main(string[] args)
    {
        RunTest(TestGridConstruction, "TestGridConstruction");
        RunTest(TestOpenAndIsOpen, "TestOpenAndIsOpen");
        RunTest(TestNumberOfOpenSites, "TestNumberOfOpenSites");
        RunTest(Test2DTo1DConversion, "Test2DTo1DConversion");
        RunTest(TestIsFull, "TestIsFull");
        RunTest(TestPercolates, "TestPercolates");
        RunTest(TestEdgeCases, "TestEdgeCases");
    }
    
    public static bool TestGridConstruction() 
    {
        Percolation perc = new Percolation(3);

        for (int i = 0; i < 3; i++) 
        {
            for (int j = 0; j < 3; j++) 
            {
                if (perc.IsOpen(i, j)) 
                {
                    return false;
                }
            }
        }

        return true;
    }

    public static bool TestOpenAndIsOpen() 
    {
        Percolation perc = new Percolation(3);

        bool isOpenBeforeFalse = (perc.IsOpen(0, 0) == false);

        perc.Open(0, 0);

        bool isOpenAfterTrue = perc.IsOpen(0, 0);

        return (isOpenBeforeFalse && isOpenAfterTrue);
    }

    public static bool TestNumberOfOpenSites() 
    {
        Percolation perc = new Percolation(3);

        bool OpenSitesBeforeZero = (perc.NumberOfOpenSites() == 0);

        perc.Open(0, 0);
        perc.Open(0, 1);

        bool OpenSitesAfterTwo = (perc.NumberOfOpenSites() == 2);

        return (OpenSitesBeforeZero && OpenSitesAfterTwo);

    }

    public static bool Test2DTo1DConversion() 
    {
        Percolation perc = new Percolation(3);

        bool test1 = (perc.ConvertCoordinateToSquareNumber(0, 0) == 0);
        bool test2 = (perc.ConvertCoordinateToSquareNumber(0, 2) == 2);
        bool test3 = (perc.ConvertCoordinateToSquareNumber(1, 1) == 4);
        bool test4 = (perc.ConvertCoordinateToSquareNumber(2, 2) == 8);

        return (test1 && test2 && test3 && test4);
    }

    public static bool TestIsFull() 
    {
        Percolation perc = new Percolation(3);

        perc.Open(0, 1);
        perc.Open(0, 2);
        perc.Open(1, 2);

        perc.Open(2, 0);

        bool IsBlockedSiteFullFalse = !(perc.IsFull(1, 0));
        bool IsNotConnectedSiteFullFalse = !(perc.IsFull(2, 0));
        bool IsConnectedSiteFullTrue = perc.IsFull(1, 2);

        return (IsBlockedSiteFullFalse && IsNotConnectedSiteFullFalse && IsConnectedSiteFullTrue);
    }

    public static bool TestPercolates() 
    {
        Percolation perc = new Percolation(3);

        bool percolatesBeforeIsFalse = !(perc.Percolates());

        perc.Open(0, 1);
        perc.Open(1, 1);
        perc.Open(2, 1);

        return (percolatesBeforeIsFalse && perc.Percolates());
    }

    public static bool TestEdgeCases() 
    {
        Percolation perc = new Percolation(3);

        // Test for open an already opened square
        bool openSquareTestPassed = false;
        try 
        {
            perc.Open(0, 0);
            perc.Open(0, 0);
            openSquareTestPassed = true;
        }
        catch (Exception) 
        {
            return false;
        }
        
        // Test for out of bounds row and column in Open, IsOpen, IsFull, Convert..
        bool outOfBoundsTestPassed = false;
        try
        {
            perc.Open(4, 2);
            return false;
        }
        catch (Exception)
        {
            outOfBoundsTestPassed = true;
        }

        try
        {
            perc.Open(-1, 2);
            return false;
        }
        catch (Exception)
        {
            outOfBoundsTestPassed = true;
        }

        try 
        {
            perc.Open(2, 4);
            return false;
        }
        catch (Exception) 
        {
            outOfBoundsTestPassed = true;
        }

        try 
        {
            perc.Open(2, -1);
            return false;
        }
        catch (Exception)
        {
            outOfBoundsTestPassed = true;
        }

        
        return (openSquareTestPassed && outOfBoundsTestPassed);
    }

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
}
