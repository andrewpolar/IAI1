using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace NOT_MIT_KAN_1
{
    internal class Plotter
    {
        public void PlotFunctions(KolmogorovModelPL kmpl, string folder)
        {
            string dir = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.Combine(dir, folder);

            if (!File.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path);
            }

            try
            {
                var pngFiles = Directory.EnumerateFiles(path, "*.png");
                foreach (string currentFile in pngFiles)
                {
                    Console.WriteLine(currentFile);
                    if (File.Exists(currentFile)) File.Delete(currentFile);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            int size = 32;
            int K = kmpl.GetBigUSize();
            for (int i = 0; i < K; ++i)
            {
                double[] y = kmpl.GetBigUFunctionPoints(i, size);
                (double xmin, double xmax) = kmpl.GetBigULimits(i);
                double delta = (xmax - xmin) / (size - 1);
                double[] x = new double[size];
                for (int j = 0; j < size; ++j)
                {
                    x[j] = xmin + delta * j;
                }
                ScottPlot.Plot myPlot = new ScottPlot.Plot();
                myPlot.Axes.SetLimitsX(xmin, xmax);
                myPlot.Add.Scatter(x, y);
                string name = String.Format("BigU{0}.png", i + 1);
                string fullName = Path.Combine(path, name);
                myPlot.SavePng(fullName, 400, 300);
                myPlot.Dispose();
            }

            int M = kmpl.GetUListSize();
            for (int i = 0; i < M; ++i)
            {
                int N = kmpl.GetUSize(i);
                for (int j = 0; j < N; ++j)
                {
                    double[] y = kmpl.GetUListFunctionPoints(i, j, size);
                    (double xmin, double xmax) = kmpl.GetUListLimits(i, j);

                    double delta = (xmax - xmin) / (size - 1);
                    double[] x = new double[size];
                    for (int r = 0; r < size; ++r)
                    {
                        x[r] = xmin + delta * r;
                    }
                    ScottPlot.Plot myPlot = new ScottPlot.Plot();
                    myPlot.Axes.SetLimitsX(xmin, xmax);
                    myPlot.Add.Scatter(x, y);
                    string name = String.Format("U{0}_{1}.png", i + 1, j + 1);
                    string fullName = Path.Combine(path, name);
                    myPlot.SavePng(fullName, 400, 300);
                    myPlot.Dispose();
                }
            }
        }
    }
}

