using System;
using System.Collections.Generic;
using System.Linq;
using Epidemic.BLL.Models;

namespace Epidemic.BLL.Controllers.Aproximate
{
    public class HyperbolicFuncAproximate : IFuncAproximate
    {
        public AproximateFunction GetFunction(double[] y, double[] x)
        {
            if(y.Length != x.Length) throw new Exception("Неоднакова розмірність масивів даних");
            var func = new AproximateFunction();
            double sumYiDivXi = y.Select((yi, i) => yi / x[i]).Sum();
            double sum1DivXi = x.Select((xi) => 1d / xi).Sum();
            double sum1DivXiPow2 = x.Select((xi) => 1d / (xi * xi)).Sum();
            double sumYi = y.Sum();
            int n = y.Length;
            // Desc: y = A + B / x
            func.B = (n * sumYiDivXi - sum1DivXi * sumYi) / 
                          (n * sum1DivXiPow2 - Math.Pow(sum1DivXi, 2));
            func.A = sumYi / n - func.B / n * sum1DivXi;
            return func;
        }

        public List<AproximateFunction> GetFunctions(double[][] ys, double[] x)
        {
            var list = new List<AproximateFunction>();
            for (int i = 0; i < ys.GetLength(0); i++)
            {
                list.Add(GetFunction(ys[i], x));
            }
            return list;
        }

        public double GetResult(AproximateFunction func, double x)
        {
            return func.A + func.B / x;
        }
    }
}
