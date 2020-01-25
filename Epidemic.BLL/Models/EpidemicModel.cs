using System;
using System.Collections.Generic;
using System.Data;
using Epidemic.BLL.Controllers.Filters;

namespace Epidemic.BLL.Models
{
    public abstract class EpidemicModel
    {
        public List<double> Beta { set; get; }
        public List<double> Gamma { set; get; }

        protected abstract void GetS(DataTable dt);
        protected abstract void GetI(DataTable dt);
        protected abstract void GetBeta();
        protected abstract void GetGamma();
        protected abstract void CorrectBetaGamma();

        protected double CorrectValue(double value)
        {
            if (value < 0) return 0;
            if (double.IsNaN(value)) return 0;
            if (value > 10000) return 10000;
            return value;
        }

        private int topMax = 300;
        protected void CorrectBeta(int level = 0)
        {
            FilterController.TrasholdUp(Beta, topMax, level);
            FilterController.TrasholdDown(Beta, level);
        }

        protected void CorrectGamma(int level = 0)
        {
            FilterController.TrasholdUp(Gamma, topMax, level);
            FilterController.TrasholdDown(Gamma, level);
        }
    }
}
