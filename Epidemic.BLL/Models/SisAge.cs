using System;
using System.Collections.Generic;
using System.Data;
using Epidemic.BLL.Controllers.Statistic;

namespace Epidemic.BLL.Models
{
    public class SisAge : EpidemicModel
    {
        #region Properties
        public List<DateTime> Date { set; get; }
        public List<int> N { set; get; }
        public List<int> S { set; get; }
        public List<int> Itotal { set; get; }
        public List<int> I { set; get; }
        public List<int> Death { set; get; }
        #endregion

        #region Constructors
        public SisAge(DataTable dt)
        {
            Initialize(dt);
        }
        #endregion

        #region Methods

        private void Initialize(DataTable dt)
        {
            Date = new List<DateTime>();
            GetDate(dt);
            N = new List<int>();
            GetN(dt);
            S = new List<int>();
            GetS(dt);
            Itotal = new List<int>();
            GetItotal(dt);
            I = new List<int>();
            GetI(dt);
            Beta = new List<double>();
            GetBeta();
            CorrectBeta(4);
            Gamma = new List<double>();
            GetGamma();
            CorrectGamma();
            DefineGammaAsConst();
            Death = new List<int>();
            RecalculateBetaWithConstGamma();
            CorrectBetaGamma();
        }

        private void GetDate(DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Date.Add((DateTime)dt.Rows[i][0]);
            }
        }

        private void GetN(DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                N.Add((int)dt.Rows[i][1]);
            }
        }

        protected override void GetS(DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                S.Add((int)dt.Rows[i][2]);
            }
        }

        protected void GetItotal(DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Itotal.Add((int)dt.Rows[i][3]);
            }
        }

        protected override void GetI(DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                I.Add((int)dt.Rows[i][4]);
            }
        }

        protected override void GetBeta()
        {
            double beta = 0;
            double beta2 = 0;
            double result = 0;
            int n = S.Count - 2;

            for (int t = 0; t < n; t++)
            {
                //beta2 = ((double)I[t + 1] * I[t + 1] / I[t] - I[t + 2]) /
                //    ((double)I[t + 1] / I[t] * S[t] - S[t + 1]) / Itotal[t] * N[t];
                beta2 = ((double)I[t + 1] * I[t + 1] - I[t] * I[t + 2]) /
                        (I[t + 1] * S[t] - S[t + 1] * I[t]) / Itotal[t] * N[t];

                beta2 = CorrectValue(beta2);
                result = beta + beta2;
                if (t > 0) result /= 2;
                Beta.Add(result);
                beta = beta2;
                if (t == n - 1) Beta.Add(beta2);
            }
            // Add last value
            Beta.Add(0);
        }

        protected override void GetGamma()
        {
            double gamma = 0;
            double gamma2 = 0;
            double result = 0;
            int n = S.Count - 2;

            for (int t = 0; t < n; t++)
            {
                gamma2 = Beta[t] * S[t] / N[t] *Itotal[t] / I[t] - (double)I[t + 1] / I[t] + 1;
                gamma2 = CorrectValue(gamma2);
                result = gamma + gamma2;
                if (t > 0) result /= 2;
                Gamma.Add(result);
                gamma = gamma2;
                if (t == n - 1) Gamma.Add(gamma2);
            }
            // Add last value
            Gamma.Add(0);
        }

        protected override void CorrectBetaGamma()
        {
            for (int i = 0; i < Beta.Count; i++)
            {
                if (Beta[i] < 0) Beta[i] = 0;
            }
        }

        protected void DefineGammaAsConst()
        {
            double averGamma = Stat.MathExpection(Gamma);
            for (int t = 0; t < Gamma.Count; t++)
            {
                Gamma[t] = averGamma;
                //Gamma[t] = 1;
            }
        }

        protected void RecalculateBetaWithConstGamma()
        {
            for (int t = 0; t < Beta.Count - 1; t++)
            {

                Beta[t] = (I[t + 1] - I[t] * (1 - Gamma[t])) / S[t] / Itotal[t] * N[t];
                Death.Add(S[t + 1] + I[t + 1] - S[t] - I[t]);
            }
            Death.Add(Death[Death.Count - 1]);
            Beta[Beta.Count - 1] = Beta[Beta.Count - 2];
        }
        
        #endregion
    }
}
