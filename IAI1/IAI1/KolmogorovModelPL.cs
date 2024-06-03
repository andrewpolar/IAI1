using System;
using System.Collections.Generic;
using System.Text;

namespace NOT_MIT_KAN_1
{
    class KolmogorovModelPL
    {
        public List<double[]> _inputs = new List<double[]>();
        public List<double> _target = new List<double>();
        private double[] _xmin = null;
        private double[] _xmax = null;
        private double _targetMin;
        private double _targetMax;
        int[] _interior_structure = null;
        int[] _exterior_structure = null;

        private double _muOuter = 0.2;
        private double _muInner = 0.1;
        private int _nBranches = 5;

        private List<UrysohnPL> _ulist = new List<UrysohnPL>();
        private UrysohnPL _bigU = null;
        private Random _rnd = new Random();

        public KolmogorovModelPL(List<double[]> inputs, List<double> target, int inner, int outer, int branches, double muInner, double muOuter)
        {
            _inputs = inputs;
            _target = target;

            _nBranches = branches;

            if (inputs.Count != target.Count)
            {
                Console.WriteLine("Invalid training data");
                Environment.ExitCode = 0;
            }

            FindMinMax();

            int number_of_inputs = _inputs[0].Length;
            _interior_structure = new int[number_of_inputs];
            for (int i = 0; i < number_of_inputs; i++)
            {
                _interior_structure[i] = inner;
            }
            _exterior_structure = new int[_nBranches];
            for (int i = 0; i < _nBranches; i++)
            {
                _exterior_structure[i] = outer;
            }

            GenerateInitialOperators();
        }

        private void FindMinMax()
        {
            int size = _inputs[0].Length;
            _xmin = new double[size];
            _xmax = new double[size];

            for (int i = 0; i < size; ++i)
            {
                _xmin[i] = double.MaxValue;
                _xmax[i] = double.MinValue;
            }

            for (int i = 0; i < _inputs.Count; ++i)
            {
                for (int j = 0; j < _inputs[i].Length; ++j)
                {
                    if (_inputs[i][j] < _xmin[j]) _xmin[j] = _inputs[i][j];
                    if (_inputs[i][j] > _xmax[j]) _xmax[j] = _inputs[i][j];
                }

            }

            _targetMin = double.MaxValue;
            _targetMax = double.MinValue;
            for (int j = 0; j < _target.Count; ++j)
            {
                if (_target[j] < _targetMin) _targetMin = _target[j];
                if (_target[j] > _targetMax) _targetMax = _target[j];
            }
        }

        public void GenerateInitialOperators()
        {
            _ulist.Clear();
            int points = _inputs[0].Length;
            for (int counter = 0; counter < _nBranches; ++counter)
            {
                UrysohnPL uc = new UrysohnPL(_xmin, _xmax, _targetMin, _targetMax, _interior_structure);
                _ulist.Add(uc);
            }

            double[] min = new double[_nBranches];
            double[] max = new double[_nBranches];
            for (int i = 0; i < _nBranches; ++i)
            {
                min[i] = _targetMin;
                max[i] = _targetMax;
            }

            _bigU = new UrysohnPL(min, max, _targetMin, _targetMax, _exterior_structure);
        }

        private double[] GetVector(double[] data)
        {
            int size = _ulist.Count;
            double[] vector = new double[size];
            for (int i = 0; i < size; ++i)
            {
                vector[i] = _ulist[i].GetU(data);
            }
            return vector;
        }

        public void BuildRepresentation(int nEpochs)
        {
            for (int step = 0; step < nEpochs; ++step)
            {
                double error = 0.0;
                for (int i = 0; i < _inputs.Count; ++i)
                {
                    double[] v = GetVector(_inputs[i]);
                    double model = _bigU.GetU(v);
                    double diff = _target[i] - model;
                    error += diff * diff;

                    for (int k = 0; k < _ulist.Count; ++k)
                    {
                        if (v[k] > _targetMin && v[k] < _targetMax)
                        {
                            double derrivative = _bigU.GetDerrivative(k, v[k]);
                            _ulist[k].Update(diff * derrivative / v.Length, _inputs[i], _muInner);
                        }
                    }
                    _bigU.Update(diff, v, _muOuter);
                }
                error /= _inputs.Count;
                error = Math.Sqrt(error);
                error /= (_targetMax - _targetMin);
                if (0 == step % 25)
                {
                    Console.WriteLine("Training step {0}, relative RMSE {1:0.0000}", step, error);
                }
            }
        }

        public double ComputeOutput(double[] inputs)
        {
            double[] v = GetVector(inputs);
            double output = _bigU.GetU(v);
            return output;
        }

        public double GetTargetMin()
        {
            return _targetMin;
        }

        public double GetTargetMax()
        {
            return _targetMax;
        }

        //Functions returning data for plotting pictures
        public int GetBigUSize()
        {
            return _bigU.GetUSize();
        }

        public double[] GetBigUFunctionPoints(int k, int N)
        {
            return _bigU.GetFunctionPoints(k, N);
        }

        public (double xmin, double xmax) GetBigULimits(int k)
        {
            return _bigU.GetLimits(k);
        }

        public int GetUListSize()
        {
            return _ulist.Count;
        }

        public int GetUSize(int k)
        {
            return _ulist[k].GetUSize();
        }

        public double[] GetUListFunctionPoints(int k, int m, int N)
        {
            return _ulist[k].GetFunctionPoints(m, N);
        }

        public (double xmin, double xmax) GetUListLimits(int k, int m)
        {
            return _ulist[k].GetLimits(m);
        }
    }
}
