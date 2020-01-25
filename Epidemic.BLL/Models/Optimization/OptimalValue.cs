using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Epidemic.BLL.Models.Optimization
{
    public class OptimalValue : IComparable
    {
        public double Cost { set; get; }
        public double P1 { set; get; }
        public double P10 { set; get; }
        public double P1k { set; get; }

        public double P2 { set; get; }
        public double P20 { set; get; }
        public double P2k { set; get; }

        public int CompareTo(object obj)
        {
            if (obj == null) return 1;

            OptimalValue opt = obj as OptimalValue;
            if (opt != null)
                return this.Cost.CompareTo(opt.Cost);
            else
                throw new ArgumentException("Object is not a OptimalValue");
        }
    }
}
