using System;
using System.Collections;
using System.Diagnostics;
using System.IO;
using System.Threading;
using testMatrix;
class Program
{
    static void Main()
    {
        // TestMatrix testMatrix = new TestMatrix();
        // testMatrix.Testing();
        // Console.WriteLine("Тестирование завершено");
        string matrixAFile = "matrixA.txt";
        string matrixBFile = "matrixB.txt";
        string resultFile = "result.txt";

        int[][] matrixA = LoadMatrix(matrixAFile);
        int[][] matrixB = LoadMatrix(matrixBFile);

        if (matrixA == null || matrixB == null )
        {
            Console.WriteLine("Ошибка при чтении матриц из файлов.");
            Process.GetCurrentProcess().Kill();
        }

        int numberOfThreads = Environment.ProcessorCount;

        MatrixMultiplier multiplier = new MatrixMultiplier(matrixA, matrixB, numberOfThreads);
        int[][] result = multiplier.MultiplyM();

        SaveMatrix(result, resultFile);

        Console.WriteLine("Умножение матриц завершено. Результат записан в файл result.txt.");
    }

    static int[][] LoadMatrix(string filePath)
    {
        try
        {
            string[] lines = File.ReadAllLines(filePath);
            int numRows = lines.Length;
            int[][] matrix = new int[numRows][];

            for (int i = 0; i < numRows; i++)
            {
                string[] values = lines[i].Split(new char[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                int numCols = values.Length;
                matrix[i] = new int[numCols];

                for (int j = 0; j < numCols; j++)
                {
                    if (!int.TryParse(values[j], out matrix[i][j]))
                    {
                        Console.WriteLine("Ошибка при чтении матрицы из файла.");
                        Process.GetCurrentProcess().Kill();;
                    }
                }
            }

            return matrix;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при чтении матрицы из файла: " + ex.Message);
            return null;
        }
    }

    static void SaveMatrix(int[][] matrix, string filePath)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (int[] row in matrix)
                {
                    writer.WriteLine(string.Join(" ", row));
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при записи матрицы в файл: " + ex.Message);
        }
    }
}


