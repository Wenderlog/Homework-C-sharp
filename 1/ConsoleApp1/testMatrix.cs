using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
namespace testMatrix{
class TestMatrix
{
    public void Testing()
        {
            int[] matrixSizes = { 100, 250, 500, 750, 1000 };
            int numberOfRuns = 10;

            foreach (int size in matrixSizes)
            {
                int numRows = size;
                int numCols = size;

                int[][] matrixA = GenerateRandomMatrix(numRows, numCols);
                int[][] matrixB = GenerateRandomMatrix(numRows, numCols);

                double multiThreadedTotalTime = 0;
                double sequentialTotalTime = 0;

                for (int run = 0; run < numberOfRuns; run++)
                {
                    double multiThreadedTime = Time(() =>
                    {
                        int numberOfThreads = Environment.ProcessorCount;
                        MatrixMultiplier multiplier = new MatrixMultiplier(matrixA, matrixB, numberOfThreads);
                        int[][] result = multiplier.MultiplyM();
                    });

                    multiThreadedTotalTime += multiThreadedTime;

                    double sequentialTime = Time(() =>
                    {
                        MatrixMultiplier multiplier = new MatrixMultiplier(matrixA, matrixB, 1);
                        int[][] result = multiplier.MultiplyM();
                    });

                    sequentialTotalTime += sequentialTime;
                }

                double multiThreadedAverageTime = multiThreadedTotalTime / numberOfRuns;
                double sequentialAverageTime = sequentialTotalTime / numberOfRuns;

                Console.WriteLine($"Размер матрицы: {numRows}x{numCols}");
                Console.WriteLine("----------------------------------------------------");
                Console.WriteLine("| Тип                       | Среднее время (ms) |");
                Console.WriteLine("----------------------------------------------------");
                Console.WriteLine($"| Многопоточность          | {multiThreadedAverageTime,-26:F2} |");
                Console.WriteLine($"| Последовательный         | {sequentialAverageTime,-26:F2} |");
                Console.WriteLine("----------------------------------------------------");
                Console.WriteLine();
            }
        }

    static double Time(Action action)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        action();
        stopwatch.Stop();
        return stopwatch.Elapsed.TotalMilliseconds;
    }


    static int[][] GenerateRandomMatrix(int rows, int cols)
    {
        Random random = new Random();
        int[][] matrix = new int[rows][];

        for (int i = 0; i < rows; i++)
        {
            matrix[i] = new int[cols];
            for (int j = 0; j < cols; j++)
            {
                matrix[i][j] = random.Next(1, 10); 
            }
        }

        return matrix;
    }
}
}
