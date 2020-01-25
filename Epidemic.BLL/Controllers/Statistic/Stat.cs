using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Epidemic.BLL.Controllers.Statistic
{
    public class Stat
    {
        public static double MathExpection(List<double> list)
        {
            return list.Select((x) => x / list.Count).Sum();
        }

        public static double Deviation(List<double> list)
        {
            double aver = MathExpection(list);
            return list.Select((x) => Math.Pow(x - aver, 2) / list.Count).Sum();
        }

        public static double AverageSquareDeviation(List<double> list)
        {
            return Math.Sqrt(Deviation(list));
        }
    }
}
