using System;
using System.Threading.Tasks;
using System.IO;

namespace MatrixSolver
{
    class MatrixSolver
    {

        static double[] ReadRow(string input, int row)
        {
            StreamReader sr = File.OpenText(input); 

            sr.ReadLine(); 
            for (int i = 0; i < row; i++)
                sr.ReadLine();

            string[] data = sr.ReadLine().Split(' ');
            sr.Close();

            double[] rowData = new double[data.Length];
            for (int i = 0; i < rowData.Length; i++)
                rowData[i] = double.Parse(data[i]);

            return rowData;
        }

        static double[] ReadCol(string input, int col)
        {
            StreamReader sr = File.OpenText(input);

            string[] header = sr.ReadLine().Split(' ');

            int rows = int.Parse(header[0]); 

            string[] data = new string[rows];

            for (int i = 0; i < rows; i++)

                data[i] = (sr.ReadLine()).Split(' ')[col];

            sr.Close(); 

            double[] colData = new double[data.Length];

            for (int i = 0; i < colData.Length; i++)

                colData[i] = double.Parse(data[i]);

            return colData;
        }

        static double Mul(double[] rowData, double[] colData)
        {
            double result = 0;
            for (int i = 0; i < rowData.Length; i++) 
                result += rowData[i] * colData[i];
            return result;
        }

        static async Task<double> MulAsync(string inputLeft, string inputRight, int row, int col)
        {
            double[] rowData, colData;

            rowData = await Task.Run(() => ReadRow(inputLeft, row)); 

            colData = await Task.Run(() => ReadCol(inputRight, col));

            return await Task.Run<double>(() => Mul(rowData, colData));
        }

        public static async void Solve(string inputLeft, string inputRight, string output) 
        {
            StreamReader sr = File.OpenText(inputLeft); 
            string[] input = sr.ReadLine().Split(' '); 
            int rowsLeft = int.Parse(input[0]);
            int colsLeft = int.Parse(input[1]);
            sr.Close();

            sr = File.OpenText(inputRight); 
            input = sr.ReadLine().Split(' '); 
            int rowsRight = int.Parse(input[0]);  
            int colsRight = int.Parse(input[1]);  
            sr.Close();

            if ((rowsLeft != colsRight) || (colsLeft != rowsRight)) 
                throw new Exception("Matrix cols and rows does not equal.");

            StreamWriter sw = new StreamWriter(output);
            for (int i = 0; i < rowsLeft; ++i)
            {
                Task<double>[] mulTasks = new Task<double>[colsRight]; 
                for (int j = 0; j < colsRight; ++j)
                {
                    mulTasks[j] = MulAsync(inputLeft, inputRight, i, j); 
                }
                
                double[] results = await Task.WhenAll<double>(mulTasks);

                sw.WriteLine(String.Join(" ", results));
            }
            sw.Close();
        }
    }
}
