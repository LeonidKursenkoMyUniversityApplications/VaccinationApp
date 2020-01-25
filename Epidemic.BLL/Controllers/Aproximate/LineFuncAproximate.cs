using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epidemic.BLL.Models;

namespace Epidemic.BLL.Controllers.Aproximate
{
    public class LineFuncAproximate : IFuncAproximate
    {
        public AproximateFunction GetFunction(double[] y, double[] x)
        {
            if (y.Length != x.Length) throw new Exception("Неоднакова розмірність масивів даних");
            var func = new AproximateFunction();
            double sumXi = x.Sum();
            double sumYi = y.Sum();
            double sumXiYi = x.Select((xi, i) => xi * y[i]).Sum();
            double sumXiPow2 = x.Select(xi => xi * xi).Sum();
            int n = y.Length;
            // Desc: y = A * X + B
            func.A = (sumXi * sumYi - n * sumXiYi) / 
                     (sumXi * sumXi - n * sumXiPow2);
            func.B = (sumXi * sumXiYi - sumXiPow2 * sumYi) /
                     (sumXi * sumXi - n * sumXiPow2);
            
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
            return func.A * x + func.B;
        }
    }
}
