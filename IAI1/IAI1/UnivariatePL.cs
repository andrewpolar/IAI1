using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace NOT_MIT_KAN_1
{
    class UnivariatePL
    {
        private int _points;
        private double _xmin;
        private double _xmax;
        private double _ymin;
        private double _ymax;
        private double _deltax;
        private double[] _y = null;
        private Random _rnd = new Random();

        public UnivariatePL(double xmin, double xmax, double ymin, double ymax, int points)
        {
            _points = points;
            _xmin = xmin;
            _xmax = xmax;
            SetLimits();
            _ymin = ymin;
            _ymax = ymax;

            _y = new double[points];
            for (int i = 0; i < _points; ++i)
            {
                _y[i] = _rnd.Next(10, 1000) / 1000.0;
            }
            double min = _y.Min();
            double max = _y.Max();
            if (min == max) max = min + 1.0;
            for (int i = 0; i < _points; ++i)
            {
                _y[i] = _y[i] * (ymax - ymin) + ymin;
            }
        }

        private void SetLimits()
        {
            double range = _xmax - _xmin;
            _xmin -= 0.01 * range;
            _xmax += 0.01 * range;
            _deltax = (_xmax - _xmin) / (_points - 1);
        }

        private void FitDefinition(double x)
        {
            if (x < _xmin)
            {
                _xmin = x;
                SetLimits();
            }
            if (x > _xmax)
            {
                _xmax = x;
                SetLimits();
            }
        }

        public double GetDerrivative(double x)
        {
            int low = (int)((x - _xmin) / _deltax);
            return (_y[low + 1] - _y[low]) / _deltax;
        }
 
        public void Update(double x, double delta, double mu)
        {
            FitDefinition(x);
            int left = (int)((x - _xmin) / _deltax);
            double leftx = x - _xmin - left * _deltax;
            double rightx = _deltax - leftx;
            _y[left + 1] += delta * leftx / _deltax * mu;
            _y[left] += delta * rightx / _deltax * mu;
        }

        public double GetFunctionValue(double x)
        {
            FitDefinition(x);
            int left = (int)((x - _xmin) / _deltax);
            double leftx = x - _xmin - left * _deltax;
            return (_y[left + 1] - _y[left]) / _deltax * leftx + _y[left];
        }

        //Functions returning data for plotting pictures
        public double[] GetFunctionPoints(int N)
        {
            double delta = (_xmax - _xmin) / (N - 1);
            List<double> points = new List<double>();
            double gap = 0.01 * (_xmax - _xmin);
            for (int i = 0; i < N; ++i)
            {
                double x = _xmin + delta * i;
                if (x < _xmin + gap) x = _xmin + gap;
                if (x > _xmax - gap) x = _xmax - gap;
                points.Add(GetFunctionValue(x));
            }
            return points.ToArray();
        }

        public (double xmin, double xmax) GetLimits()
        {
            return (_xmin, _xmax);
        }
    }
}