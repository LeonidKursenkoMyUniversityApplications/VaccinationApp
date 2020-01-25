using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Epidemic.BLL.Models
{
    public class SisAgeData
    {
        #region Properties
        public List<SisAge> Data { set; get; }
        public List<double> BetaTotal { set; get; }
        public List<double> GammaTotal { set; get; }
        public List<int> DeathTotal { set; get; }
        public List<int> Birth { set; get; }

        public BirthMode BirthMode { set; get; }
        public DeathMode DeathMode { set; get; }
        public double BirthPercent { set; get; }
        public double DeathPercent { set; get; }
        public int BirthConst { set; get; }
        public int DeathConst { set; get; }
        public ForecastMode ForecastMode { set; get; }

        #endregion

        #region Constructors

        public SisAgeData(List<DataTable> sisAges)
        {
            Data = new List<SisAge>();
            for (int i = 0; i < sisAges.Count; i++)
            {
                Data.Add(new SisAge(sisAges[i]));
            }
            
            for (int t = 0; t < Data[0].Date.Count; t++)
            {
                int iTotal = 0;
                for (int a = 0; a < Data.Count; a++)
                    iTotal += Data[a].I[t];
                for (int a = 0; a < Data.Count; a++)
                    Data[a].Itotal[t] = iTotal;
            }

            CalculateTotalParams();
        }

        public SisAgeData()
        {
            
        }
        
        #endregion

        #region Methods

        public void CalculateTotalParams()
        {
            BetaTotal = new List<double>();
            GammaTotal = new List<double>();
            DeathTotal = new List<int>();
            Birth = new List<int>();
            for (int t = 0; t < Data[0].Date.Count; t++)
            {
                double beta = 0;
                double gamma = 0;
                for (int a = 0; a < Data.Count; a++)
                {
                    beta += Data[a].Beta[t] * (Data[a].S[t] + Data[a].I[t]) / Data[a].N[t];
                    gamma += Data[a].Gamma[t] * (Data[a].S[t] + Data[a].I[t]) / Data[a].N[t];

                }
                BetaTotal.Add(beta);
                GammaTotal.Add(gamma);
                Birth.Add((Data[0].S[t] + Data[0].I[t]) / 12);
                if (t < Data[0].Date.Count - 1)
                    DeathTotal.Add(Data[0].N[t + 1] - Data[0].N[t]);
                else DeathTotal.Add(DeathTotal[DeathTotal.Count - 1]);
            }
        }

        public void CalculateTotalParams(int t0)
        {
            for (int t = t0; t < Data[0].Date.Count; t++)
            {
                double beta = 0;
                double gamma = 0;
                for (int a = 0; a < Data.Count; a++)
                {
                    beta += Data[a].Beta[t] * (Data[a].S[t] + Data[a].I[t]) / Data[a].N[t];
                    gamma += Data[a].Gamma[t] * (Data[a].S[t] + Data[a].I[t]) / Data[a].N[t];

                }
                BetaTotal.Add(beta);
                GammaTotal.Add(gamma);
            }
        }

        public double GetAlivePercent(int tk, int t0, int a)
        {
            double alive = 1;
            for (int t = t0; t <= tk; t++)
            {
                int n = Data[a].S[t] + Data[a].I[t];
                if (Data[a].Death[t] >= n)
                {
                    alive = 0;
                    return alive;
                }
                if (n != 0)
                    alive *= (1 + Data[a].Death[t] / n);
                else alive = 0;
            }
            alive = alive < 0 || alive > 1 ? 0 : alive;
            return alive;
        }

        //public SisAgeData Clone()
        //{
        //    SisAgeData sis = new SisAgeData
        //    {
        //        ForecastMode = ForecastMode,
        //        BirthMode = BirthMode,
        //        DeathMode = DeathMode,
        //        BirthConst = BirthConst,
        //        DeathConst = DeathConst,
        //        BirthPercent = BirthPercent,
        //        DeathPercent = DeathPercent,
        //        Data = Data.ToList(),
        //    };
        //    return sis;
        //}

        #endregion
    }
}
