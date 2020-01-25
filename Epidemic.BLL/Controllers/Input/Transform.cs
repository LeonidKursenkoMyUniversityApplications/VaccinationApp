using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Epidemic.BLL.Controllers.Input
{
    public class Transform
    {
        public static double ToDouble(string str)
        {
            double val;
            try
            {
                val = Convert.ToDouble(str);
                //if(val < 0) throw new Exception();
                return val;
            }
            catch
            {
                throw new Exception("Недопустимий формат числа: " + str);
            }
        }

        public static int ToInt32(string str)
        {
            int val;
            try
            {
                val = Convert.ToInt32(str);
                //if (val < 0) throw new Exception();
                return val;
            }
            catch
            {
                throw new Exception("Недопустимий формат цілого числа:" + str);
            }
        }
    }
}
