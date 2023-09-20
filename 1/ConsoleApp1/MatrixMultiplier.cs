using System.Diagnostics;

class MatrixMultiplier
{
    private readonly int[][] matrixA;
    private readonly int[][] matrixB;
    private readonly int[][] result;
    private readonly int rowsA;
    private readonly int rowsB;
    private readonly int columnsA;
    private readonly int columnsB;
    private readonly int numberOfThreads;

    public MatrixMultiplier(int[][] matrixA, int[][] matrixB, int numberOfThreads)
    {
        this.matrixA = matrixA;
        this.matrixB = matrixB;
        this.rowsB = matrixB.Length;
        this.rowsA = matrixA.Length;
        this.columnsA = matrixA[0].Length;
        this.columnsB = matrixB[0].Length;
        this.result = new int[rowsA][];
        this.numberOfThreads = numberOfThreads;

        for (int i = 0; i < rowsA; i++)
        {
            this.result[i] = new int[columnsB];
        }
    }

    public int[][] MultiplyM()
    {
        if (columnsA != rowsB){
        Console.WriteLine("Умножение невозможно");
        Process.GetCurrentProcess().Kill();
    }
        Thread[] threads = new Thread[numberOfThreads];
        int chunkSize = rowsA / numberOfThreads;

        for (int i = 0; i < numberOfThreads; i++)
        {
            int startRow = i * chunkSize;
            int endRow = (i == numberOfThreads - 1) ? rowsA : (i + 1) * chunkSize; 
            threads[i] = new Thread(() => Multiply(startRow, endRow));
            threads[i].Start();
        }

        foreach (Thread thread in threads)
        {
            thread.Join();
        }

        return result;
    }

    private void Multiply(int startRow, int endRow)
    {   
        for (int i = startRow; i < endRow; i++)
        {
            for (int j = 0; j < columnsB; j++)
            {
                result[i][j] = 0;
                for (int k = 0; k < matrixB.Length; k++)
                {
                    result[i][j] += matrixA[i][k] * matrixB[k][j];
                }
            }
        }
    
    }
}
