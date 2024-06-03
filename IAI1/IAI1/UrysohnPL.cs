using System;
using System.Collections.Generic;
using System.Text;

namespace NOT_MIT_KAN_1
{
    class UrysohnPL
    {
        private List<UnivariatePL> _univariateList = new List<UnivariatePL>();

        public UrysohnPL(double[] xmin, double[] xmax, double targetMin, double targetMax, int[] layers)
        { 
            double ymin = targetMin / layers.Length;
            double ymax = targetMax / layers.Length;
            for (int i = 0; i < layers.Length; ++i)
            {
                UnivariatePL pll = new UnivariatePL(xmin[i], xmax[i], ymin, ymax, layers[i]);
                _univariateList.Add(pll);
            }
        }

        public double GetDerrivative(int layer, double x)
        {
            return _univariateList[layer].GetDerrivative(x);
        }

        public void Update(double delta, double[] inputs, double mu)
        {
            int i = 0;
            foreach (UnivariatePL pll in _univariateList)
            {
                pll.Update(inputs[i++], delta / _univariateList.Count, mu);
            }
        }

        public double GetU(double[] inputs)
        {
            double f = 0.0;
            int i = 0;
            foreach (UnivariatePL pll in _univariateList)
            {
                f += pll.GetFunctionValue(inputs[i++]);
            }
            return f;
        }

        //Functions returning data for plotting pictures
        public double[] GetFunctionPoints(int k, int N)
        {
            return _univariateList[k].GetFunctionPoints(N);
        }

        public (double xmin, double xmax) GetLimits(int k)
        {
            return _univariateList[k].GetLimits();
        }

        public int GetUSize()
        {
            return _univariateList.Count;
        }
    }
}
