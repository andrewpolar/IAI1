//NOT_MIT_KAN v.1
//Concept: Andrew Polar and Mike Poluektov
//Developer Andrew Polar

//License
//In case if end user finds the way of making a profit by using this code and earns
//billions of US dollars and meet developer bagging change in the street near McDonalds,
//he or she is not in obligation to buy him a sandwich.

//Symmetricity
//In case developer became rich and famous by publishing this code and meet misfortunate
//end user who went bankrupt by using this code, he is also not in obligation to buy
//end user a sandwich.

//Publications:
//https://www.sciencedirect.com/science/article/abs/pii/S0016003220301149
//https://www.sciencedirect.com/science/article/abs/pii/S0952197620303742
//https://arxiv.org/abs/2305.08194

//Some toy datasets are taken from MIT benchmark:
//https://kindxiaoming.github.io/pykan/

//Formula3 is designed by Mike Poluektov.
//Formula4 is epistemic uncertainty test: the area of triangle given by the 
//coordinates of vertices.

using System;
using System.Linq;
using System.Collections.Generic;

namespace NOT_MIT_KAN_1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Formula1 f1 = new Formula1();
            (List<double[]> input, List<double> target) = f1.GenerateData(1000);

            DateTime start = DateTime.Now;
            KolmogorovModelPL kmpl = new KolmogorovModelPL(input, target, 6, 8, 5, 0.2, 0.1);
            kmpl.BuildRepresentation(1000);
            DateTime end = DateTime.Now;
            TimeSpan duration = end - start;
            double time = duration.Minutes * 60.0 + duration.Seconds + duration.Milliseconds / 1000.0;
            Console.WriteLine("Time for building representation {0:####.00} seconds", time);

            double error = 0.0;
            int NTests = 100;
            for (int i = 0; i < NTests; ++i)
            {
                double[] test_input = f1.GetInput();
                double test_target = f1.GetTarget(test_input);
                double model = kmpl.ComputeOutput(test_input);
                error += (test_target - model) * (test_target - model);
            }
            error /= NTests;
            error = Math.Sqrt(error);
            error /= (kmpl.GetTargetMax() - kmpl.GetTargetMin());
            Console.WriteLine("\nRelative RMSE for unseen data {0:0.0000}", error);

            Plotter plotter = new Plotter();
            plotter.PlotFunctions(kmpl, "Charts");

            //Formula2 f2 = new Formula2();
            //(List<double[]> input, List<double> target) = f2.GenerateData(3000);

            //DateTime start = DateTime.Now;
            //KolmogorovModelPL kmpl = new KolmogorovModelPL(input, target, 8, 8, 9, 0.07, 0.07);
            //kmpl.BuildRepresentation(1000);
            //DateTime end = DateTime.Now;
            //TimeSpan duration = end - start;
            //double time = duration.Minutes * 60.0 + duration.Seconds + duration.Milliseconds / 1000.0;
            //Console.WriteLine("Time for building representation {0:####.00} seconds", time);

            //double error = 0.0;
            //int NTests = 100;
            //for (int i = 0; i < NTests; ++i)
            //{
            //    double[] test_input = f2.GetInput();
            //    double test_target = f2.GetTarget(test_input);
            //    double model = kmpl.ComputeOutput(test_input);
            //    error += (test_target - model) * (test_target - model);
            //}
            //error /= NTests;
            //error = Math.Sqrt(error);
            //error /= (kmpl.GetTargetMax() - kmpl.GetTargetMin());
            //Console.WriteLine("\nRelative RMSE for unseen data {0:0.0000}", error);

            //Formula3 f3 = new Formula3();
            //(List<double[]> inputs, List<double> target) = f3.GenerateData(10000);

            //DateTime start = DateTime.Now;
            //KolmogorovModelPL kmpl = new KolmogorovModelPL(inputs, target, 5, 12, 11, 0.05, 0.05);
            //kmpl.BuildRepresentation(1000);
            //DateTime end = DateTime.Now;
            //TimeSpan duration = end - start;
            //double time = duration.Minutes * 60.0 + duration.Seconds + duration.Milliseconds / 1000.0;
            //Console.WriteLine("Time for building representation {0:####.00} seconds", time);

            //double error = 0.0;
            //int NTests = 100;
            //for (int i = 0; i < NTests; ++i)
            //{
            //    double[] test_input = f3.GetInput();
            //    double test_target = f3.GetTarget(test_input);
            //    double model = kmpl.ComputeOutput(test_input);
            //    error += (test_target - model) * (test_target - model);
            //}
            //error /= NTests;
            //error = Math.Sqrt(error);
            //Console.WriteLine("\nRMSE for unseen data {0:0.0000}", error);
            //Console.WriteLine("\nRelative RMSE for unseen data {0:0.0000}", error / (kmpl.GetTargetMax() - kmpl.GetTargetMin()));

            //Formula4 f4 = new Formula4();
            //(List<double[]> inputs, List<double> target) = f4.GenerateData(10000);

            //DateTime start = DateTime.Now;
            //KolmogorovModelPL kmpl = new KolmogorovModelPL(inputs, target, 5, 5, 24, 0.01, 0.01);
            //kmpl.BuildRepresentation(500);
            //DateTime end = DateTime.Now;
            //TimeSpan duration = end - start;
            //double time = duration.Minutes * 60.0 + duration.Seconds + duration.Milliseconds / 1000.0;
            //Console.WriteLine("Time for building representation {0:####.00} seconds", time);

            //double error = 0.0;
            //int NTests = 100;
            //for (int i = 0; i < NTests; ++i)
            //{
            //    double[] test_input = f4.GetInput();
            //    double test_target = f4.GetTarget(test_input);
            //    double model = kmpl.ComputeOutput(test_input);
            //    error += (test_target - model) * (test_target - model);
            //}
            //error /= NTests;
            //error = Math.Sqrt(error);
            //Console.WriteLine("\nRMSE for unseen data {0:0.0000}", error);
            //Console.WriteLine("\nRelative RMSE for unseen data {0:0.0000}", error / 5000.0);
        }
    }

    ////This is unit test for UrysohnPL
    //internal class Program
    //{
    //    static double Function(double x, double y, double z)
    //    {
    //        return Math.Exp(Math.Sin(x)) + Math.Exp(Math.Cos(y)) + Math.Sin(z) / (1.0 + z);
    //    }

    //    static (List<double[]> inputs, List<double> target) GenerateData()
    //    {
    //        double xmin = 2.0;
    //        double xmax = 9.0;
    //        double ymin = 1.0;
    //        double ymax = 8.0;
    //        double zmin = 4.0;
    //        double zmax = 9.0;
    //        int N = 1000;
    //        Random rand = new Random();

    //        List<double[]> inputs = new List<double[]>();
    //        List<double> target = new List<double>();

    //        for (int i = 0; i < N; ++i)
    //        {
    //            double arg1 = rand.Next(10, 1000) / 1000.0 * (xmax - xmin) + xmin;
    //            double arg2 = rand.Next(10, 1000) / 1000.0 * (ymax - ymin) + ymin;
    //            double arg3 = rand.Next(10, 1000) / 1000.0 * (zmax - zmin) + zmin;
    //            inputs.Add(new double[] { arg1, arg2, arg3 });
    //            target.Add(Function(arg1, arg2, arg3));
    //        }
    //        return (inputs, target);
    //    }

    //    static (double[] xmin, double[] xmax, double targetMin, double targetMax) FindMinMax(List<double[]> inputs, List<double> target)
    //    {
    //        int size = inputs[0].Length;
    //        double[] xmin = new double[size];
    //        double[] xmax = new double[size];

    //        for (int i = 0; i < size; ++i)
    //        {
    //            xmin[i] = double.MaxValue;
    //            xmax[i] = double.MinValue;
    //        }

    //        for (int i = 0; i < inputs.Count; ++i)
    //        {
    //            for (int j = 0; j < inputs[i].Length; ++j)
    //            {
    //                if (inputs[i][j] < xmin[j]) xmin[j] = inputs[i][j];
    //                if (inputs[i][j] > xmax[j]) xmax[j] = inputs[i][j];
    //            }
    //        }

    //        double targetMin = double.MaxValue;
    //        double targetMax = double.MinValue;
    //        for (int j = 0; j < target.Count; ++j)
    //        {
    //            if (target[j] < targetMin) targetMin = target[j];
    //            if (target[j] > targetMax) targetMax = target[j];
    //        }

    //        return (xmin, xmax, targetMin, targetMax);
    //    }

    //    static void Main(string[] args)
    //    {
    //        //Generation data
    //        (List<double[]> inputs, List<double> target) = GenerateData();
    //        (double[] xmin, double[] xmax, double targetMin, double targetMax) = FindMinMax(inputs, target);

    //        DateTime start = DateTime.Now;
    //        //Identification
    //        int nEpochs = 5;
    //        int nPoints = 10;
    //        int[] nLayers = new int[inputs[0].Length];
    //        for (int i = 0; i < nLayers.Length; ++i)
    //        {
    //            nLayers[i] = nPoints;
    //        }

    //        UrysohnPL urysohnpl = new UrysohnPL(xmin, xmax, targetMin, targetMax, nLayers);
    //        for (int epoch = 0; epoch < nEpochs; ++epoch)
    //        {
    //            double error = 0.0;
    //            int cnt = 0;
    //            for (int i = 0; i < inputs.Count; ++i)
    //            {
    //                double m = urysohnpl.GetU(inputs[i]);
    //                double delta = target[i] - m;
    //                urysohnpl.Update(delta, inputs[i], 0.1);
    //                error += delta * delta;
    //                ++cnt;
    //            }
    //            error /= cnt;
    //            error = Math.Sqrt(error);
    //            error /= (targetMax - targetMin);

    //            Console.WriteLine("epoch {0}, relative error {1:0.000000}", epoch, error);
    //        }

    //        DateTime end = DateTime.Now;
    //        TimeSpan duration = end - start;
    //        double time = duration.Minutes * 60.0 + duration.Seconds + duration.Milliseconds / 1000.0;
    //        Console.WriteLine("Time for identification {0:####.00} seconds", time);

    //        //Validation
    //        (List<double[]> inputs_test, List<double> target_test) = GenerateData();

    //        double error_test = 0.0;
    //        int cnt_test = 0;
    //        for (int i = 0; i < inputs_test.Count; ++i)
    //        {
    //            double m = urysohnpl.GetU(inputs_test[i]);
    //            double delta = target_test[i] - m;
    //            error_test += delta * delta;
    //            ++cnt_test;
    //        }
    //        error_test /= cnt_test;
    //        error_test = Math.Sqrt(error_test);
    //        error_test /= (targetMax - targetMin);
    //        Console.WriteLine("relative error for validation data {0:0.0000}", error_test);
    //    }
    //}

    ////This is unit test for UnivariatePL
    //internal class Program
    //{
    //    static double Function(double x)
    //    {
    //        const double pi = 3.14;
    //        return Math.Sin(pi * x) + Math.Sin(pi * (2.0 * x + 2.0));
    //    }

    //    static (double[] x, double[] y) GetSample(int N, double xmin, double xmax)
    //    {
    //        Random rand = new Random();
    //        double[] x = new double[N];
    //        double[] y = new double[N];
    //        for (int i = 0; i < N; ++i)
    //        {
    //            x[i] = rand.Next(10, 1000) / 1000.0 * (xmax - xmin) + xmin;
    //            y[i] = Function(x[i]);
    //        }
    //        return (x, y);
    //    }

    //    static void Main(string[] args)
    //    {
    //        //Generate data
    //        (double[] x, double[] y) = GetSample(2000, 0.5, 1.5);
    //        double xmin = x.Min();
    //        double xmax = x.Max();
    //        double ymin = y.Min();
    //        double ymax = y.Max();
    //        int nRecords = x.Length;
    //        int nPoints = 12;
    //        int nEpochs = 5;

    //        ////Identification
    //        UnivariatePL uv = new UnivariatePL(xmin, xmax, ymin, ymax, nPoints);

    //        for (int epoch = 0; epoch < nEpochs; ++epoch)
    //        {
    //            double error = 0.0;
    //            int cnt = 0;
    //            for (int i = 0; i < x.Length; ++i)
    //            {
    //                double v = uv.GetFunctionValue(x[i]);
    //                double diff = y[i] - v;
    //                uv.Update(x[i], diff, 0.05);
    //                error += diff * diff;
    //                ++cnt;
    //            }
    //            error /= cnt;
    //            error = Math.Sqrt(error);
    //            error /= (ymax - ymin);
    //            Console.WriteLine("epoch {0}, error {1:0.000000}", epoch, error);
    //        }

    //        //Validation
    //        (double[] x_test, double[] y_test) = GetSample(100, 0.5, 1.5);
    //        double error_test = 0.0;
    //        int cnt_test = 0;
    //        for (int i = 0; i < x_test.Length; ++i)
    //        {
    //            double v = uv.GetFunctionValue(x[i]);
    //            double diff = y[i] - v;
    //            error_test += diff * diff;
    //            ++cnt_test;
    //        }
    //        error_test /= cnt_test;
    //        error_test = Math.Sqrt(error_test);
    //        error_test /= (ymax - ymin);
    //        Console.WriteLine("Relative error for unseen data after identification {0:0.000000}", error_test);
    //    }
    //}
}

