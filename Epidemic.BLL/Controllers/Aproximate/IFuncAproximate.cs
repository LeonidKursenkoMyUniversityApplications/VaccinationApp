using System.Collections.Generic;
using Epidemic.BLL.Models;

namespace Epidemic.BLL.Controllers.Aproximate
{
    public interface IFuncAproximate
    {
        AproximateFunction GetFunction(double[] y, double[] x);
        List<AproximateFunction> GetFunctions(double[][] ys, double[] x);
        double GetResult(AproximateFunction func, double x);
    }
}
