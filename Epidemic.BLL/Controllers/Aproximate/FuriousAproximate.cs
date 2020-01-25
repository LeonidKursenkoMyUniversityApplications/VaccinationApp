using System;
using System.Collections.Generic;
using System.Data;
using Epidemic.BLL.Models;

namespace Epidemic.BLL.Controllers.Aproximate
{
    public class FuriousAproximate
    {
        #region Attributes
        //private double eps;
        // Amount of dots.
        //private int kVals;
        private FuriousSeries f;
        // Approximated values of y.
        private List<double> y;
        // Values of mistake.
        private List<double> delta;

        private List<double> sigma;

        private int _firstIndex;
        private int _lastIndex;
        #endregion

        #region Properties
        public DataTable DataBeta { set; get; }
        public DataTable DataGamma { set; get; }
        public FuriousSeries F { get => f; set => f = value; }

        #endregion

        public FuriousAproximate(string name, List<double> list, int firstIndex, int lastIndex)
        {
            _firstIndex = firstIndex;
            _lastIndex = lastIndex;
            int kVals = lastIndex - firstIndex;
            Furious(list, kVals - 1);
            DataBeta = Create(name, list);
        }

        #region Methods for beta      
        private DataTable Create(string name, List<double> list)
        {
            DataTable data = new DataTable();
            data.Columns.Add(new DataColumn(name, typeof(double)));
            data.Columns.Add(new DataColumn(name + "*", typeof(double)));
            data.Columns.Add(new DataColumn("delta", typeof(double)));
            data.Columns.Add(new DataColumn("sigma", typeof(double)));

            for (int t = 0; t < list.Count; t++)
            {
                data.Rows.Add();
                data.Rows[t][0] = list[t];
                data.Rows[t][1] = y[t];
                data.Rows[t][2] = delta[t];
                data.Rows[t][3] = sigma[t];
            }

            return data;
        }

        private void FuriousKoef(List<double> list, int k)
        {
            List<double> ys = new List<double>();
            for (int i = _firstIndex; i < _lastIndex; i++)
            {
                ys.Add(list[i]);
            }

            // Number of harmonics.
            int m = GetHarmonicCount(ys, k);
            F = new FuriousSeries(m + 1);
            for (int n = 0; n <= m; n++)
            {
                for (int i = 0; i <= k - 1; i++)
                {
                    F.A[n] += ys[i] * Math.Cos(n * 2 * i * Math.PI / k);
                    F.B[n] += ys[i] * Math.Sin(n * 2 * i * Math.PI / k);
                }
                F.A[n] *= 2.0 / k;
                F.B[n] *= 2.0 / k;
                if (n == 0)
                {
                    F.A[n] /= 2;
                    F.B[n] = 0;
                }
            }
        }

        private void Furious(List<double> ys, int k)
        {
            y = new List<double>();
            delta = new List<double>();
            sigma = new List<double>();

            FuriousKoef(ys, k);
            // for (int t = 0; t <= k; t++)
            for (int t = 0; t < ys.Count; t++)
            {
                y.Add(0);
                for (int n = 0; n < F.A.Length; n++)
                {
                    // Approximated value of y[t]
                    y[t] += F.A[n] * Math.Cos(n * t * 2 * Math.PI / (k + 1)) +
                            F.B[n] * Math.Sin(n * t * 2 * Math.PI / (k + 1));
                }
                y[t] = CorrectValue(y[t]);
                delta.Add(Math.Abs(ys[t] - y[t]));
                sigma.Add(Math.Abs(ys[t] - y[t]) / ys[t] * 100);
            }

        }

        #endregion

        #region Additional methods

        private static int GetHarmonicCount(List<double> values, int k)
        {
            int harmonicCounter = 0;
            if (values[0] > values[1]) harmonicCounter++;
            for (int i = 1; i < k - 1; i++)
            {
                if (values[i - 1] <= values[i] &&
                    values[i] > values[i + 1])
                    harmonicCounter++;
            }
            if (values[k - 1] <= values[k]) harmonicCounter++;
            return harmonicCounter;
        }

        private static double CorrectValue(double value)
        {
            return (value < 0) ? 0 : value;
        }

        #endregion
    }
}
