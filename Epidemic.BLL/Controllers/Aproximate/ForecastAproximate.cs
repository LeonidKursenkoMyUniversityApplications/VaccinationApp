using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Epidemic.BLL.Models;

namespace Epidemic.BLL.Controllers.Aproximate
{
    public class ForecastAproximate
    {
        private List<List<int>> _iByPeriod;
        private Random _rnd;
        private ForecastMode _forecastMode;
        private int _t0;

        public SisAgeData SisAgeData { set; get; }
        
        public ForecastAproximate(SisAgeData sis, int lastIndex, ForecastMode forecastMode)
        {
            SisAgeData = sis;
            _forecastMode = forecastMode;
            _t0 = lastIndex;
            //Prepare();
            //MakeForecast();
        }

        //private void Prepare()
        //{
        //    _iByPeriod = new List<List<int>>();
        //    for (int a = 0; a < SisAgeData.Data.Count; a++)
        //    {
        //        _iByPeriod.Add(new List<int>());
        //        for (int t = _t0 - 12; t < _t0; t++)
        //        {
        //            _iByPeriod[a].Add(SisAgeData.Data[a].I[t]);
        //        }
        //    }
        //}

        //private void MakeForecast()
        //{
        //    for (int t = _t0; t < SisAgeData.Birth.Count; t++)
        //    {
        //        int iTotal = 0;
        //        for (int a = 0; a < SisAgeData.Data.Count; a++)
        //        {
        //            var sis = SisAgeData.Data[a];
        //            sis.I[t] = GetForecast(t, a);
        //            iTotal += sis.I[t];
        //        }
        //        for (int a = 0; a < SisAgeData.Data.Count; a++)
        //            SisAgeData.Data[a].Itotal[t] = iTotal;
        //    }
        //}

        public int GetForecast(SisAgeData sisAgeData, int t, int a)
        {
            double k = 0.15;
            var sis = sisAgeData.Data[a];
            double n = (double) sis.N[t - 1] / sis.N[t - 12];  //(sis.S[t - 1] + sis.I[t - 1]) / (sis.S[t - 12] + sis.I[t - 12]);
            if (_forecastMode == ForecastMode.Optimistic)
                return (int)Math.Round(sis.I[t - 12] * (1 - k) * n);
            if (_forecastMode == ForecastMode.Pessimistic)
                return (int)Math.Round(sis.I[t - 12] * (1 + k) * n);
            return (int)Math.Round(sis.I[t - 12] * n);
        }

        //private int GetForecast(int t, int a)
        //{
        //    double k = 0.07;
        //    if (_forecastMode == ForecastMode.Optimistic)
        //        return (int)Math.Round(SisAgeData.Data[a].I[t - 12] * (1 - k));
        //    if (_forecastMode == ForecastMode.Pessimistic)
        //        return (int)Math.Round(SisAgeData.Data[a].I[t - 12] * (1 + k));
        //    return SisAgeData.Data[a].I[t - 12];
        //}
    }
}
