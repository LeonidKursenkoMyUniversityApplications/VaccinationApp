using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Epidemic.BLL.Models;

namespace Epidemic.BLL.Controllers.Aproximate
{
    public class AproximateBetaGamma
    {
        private List<DateTime> dates;
        private List<double> betas;
        private List<double> gammas;
        private List<double> betasForecast;
        private List<double> gammasForecast;
        private List<List<double>> betasByPeriod;
        private List<List<double>> gammasByPeriod;
        private List<AproximateFunction> betaFuncList;
        private List<AproximateFunction> gammaFuncList;

        public DataTable BetaTable { get; set; }
        public DataTable GammaTable { get; set; }

        public AproximateBetaGamma(Sis sis, int lastIndex)
        {
            dates = new List<DateTime>();
            betas = new List<double>();
            gammas = new List<double>();
            int last = lastIndex;
            for (int i = 0; i < sis.Date.Count; i++)
            {
                if (sis.Date[i].Day == 1)
                {
                    dates.Add(sis.Date[i]);
                    betas.Add(sis.Beta[i]);
                    gammas.Add(sis.Gamma[i]);
                    if (i == lastIndex) last = betas.Count - 1;
                }
            }
            int maxPeriod = 12;
            MakeAproximate(maxPeriod, last);
            MakeForecast(maxPeriod, last);
            ConvertToDaysFormat(sis.Date);
            BetaTable = Create("beta", sis.Beta, betasForecast);
            GammaTable = Create("gamma", sis.Gamma, gammasForecast);
        }

        private void MakeAproximate(int maxPeriod, int lastIndex)
        {
            betasByPeriod = new List<List<double>>();
            gammasByPeriod = new List<List<double>>();
            for (int i = 0; i < maxPeriod; i++)
            {
                betasByPeriod.Add(new List<double>());
                gammasByPeriod.Add(new List<double>());
            }
            for (int i = 0; i < lastIndex; i++)
            {
                int j = i % maxPeriod;
                betasByPeriod[j].Add(betas[i]);
                gammasByPeriod[j].Add(gammas[i]);
            }
            betaFuncList = new List<AproximateFunction>();
            gammaFuncList = new List<AproximateFunction>();
            for (int i = 0; i < maxPeriod; i++)
            {
                double[] ts = new double[betasByPeriod[i].Count];
                for (int j = 0; j < betasByPeriod[i].Count; j++) ts[j] = j;
                LineFuncAproximate aproximate = new LineFuncAproximate();
                var betaAprFunc = aproximate.GetFunction(betasByPeriod[i].ToArray(), ts);
                betaFuncList.Add(betaAprFunc);
                var gammaAprFunc = aproximate.GetFunction(gammasByPeriod[i].ToArray(), ts);
                gammaFuncList.Add(gammaAprFunc);
            }
        }

        private void MakeForecast(int maxPeriod, int lastIndex)
        {
            betasForecast = new List<double>();
            gammasForecast = new List<double>();
            //for (int i = 0; i < lastIndex; i++)
            //{
            //    betasForecast.Add(betas[i]);
            //    gammasForecast.Add(gammas[i]);
            //}
            LineFuncAproximate aproximate = new LineFuncAproximate();
            for (int i = 0; i < dates.Count; i++)
            {
                double result = aproximate.GetResult(betaFuncList[i % maxPeriod], i / maxPeriod);
                betasForecast.Add(result);
                result = aproximate.GetResult(gammaFuncList[i % maxPeriod], i / maxPeriod);
                gammasForecast.Add(result);
            }
        }

        private void ConvertToDaysFormat(List<DateTime> datesList)
        {
            List<double> newBetas = new List<double>();
            List<double> newGammas = new List<double>();
            int j = 0;
            for (int i = 0; i < datesList.Count; i++)
            {
                newBetas.Add(betasForecast[j]);
                newGammas.Add(gammasForecast[j]);
                if (i < datesList.Count - 1 && datesList[i].Month != datesList[i + 1].Month) j++;
            }
            betasForecast = newBetas;
            gammasForecast = newGammas;
        }

        private DataTable Create(string name, List<double> list, List<double> forecastList)
        {
            DataTable data = new DataTable();
            data.Columns.Add(new DataColumn(name, typeof(double)));
            data.Columns.Add(new DataColumn(name + "*", typeof(double)));
            data.Columns.Add(new DataColumn("delta", typeof(double)));
            data.Columns.Add(new DataColumn("sigma", typeof(double)));

            for (int i = 0; i < list.Count; i++)
            {
                data.Rows.Add();
                data.Rows[i][0] = list[i];
                data.Rows[i][1] = forecastList[i];
                data.Rows[i][2] = Math.Abs(list[i] - forecastList[i]);
                data.Rows[i][3] = Math.Abs((list[i] - forecastList[i]) / list[i]) * 100;
            }
            return data;
        }
    }
}
