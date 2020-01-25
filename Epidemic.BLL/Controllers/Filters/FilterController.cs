using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Epidemic.BLL.Controllers.Statistic;

namespace Epidemic.BLL.Controllers.Filters
{
    public class FilterController
    {
        public static void TrasholdUp(List<double> list, double topMax, int level = 0)
        {
            for (int i = 0; i < list.Count; i++)
                if (list[i] > topMax) list[i] = topMax;
            for (int j = 0; j <= level; j++)
            {
                double average = Stat.MathExpection(list);
                double averSqDev = Stat.AverageSquareDeviation(list);
                for (int i = 0; i < list.Count; i++)
                    if (list[i] > average + averSqDev)
                        list[i] = average;
            }
        }

        public static void TrasholdDown(List<double> list, int level = 0)
        {
            for (int j = 0; j <= level; j++)
            {
                double average = Stat.MathExpection(list);
                double averSqDev = Stat.AverageSquareDeviation(list);
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i] < average - 2 * averSqDev)
                        list[i] = average - averSqDev;
                }
            }
        }

        
    }
}
