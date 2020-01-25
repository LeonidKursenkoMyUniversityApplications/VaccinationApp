using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Epidemic.BLL.Controllers.Statistic;
using Epidemic.BLL.Models;

namespace Epidemic.BLL.Controllers.Aproximate
{
    public class StochasticAproximate
    {
        private List<DateTime> _dates;
        private List<double> _betas;
        private List<double> _betaMins;
        private List<double> _betaMaxs;
        private List<double> _betasForecast;
        private double _gamma;
        private List<double> _gammasForecast;
        private List<List<double>> _betasByPeriod;
        private ForecastMode _forecastMode;
        private int _startIndex;

        public DataTable BetaTable { get; set; }
        public DataTable GammaTable { get; set; }

        public StochasticAproximate(List<DateTime> dates, List<double> betas, List<double> gammas, int lastIndex, ForecastMode forecastMode)
        {
            _gamma = gammas[0];
            _dates = new List<DateTime>();
            _betas = new List<double>();
            int last = lastIndex;
            _forecastMode = forecastMode;
            for (int i = 0; i < dates.Count; i++)
            {
                _dates.Add(dates[i]);
            }
            _startIndex = dates.IndexOf(new DateTime(2014, 1, 1));
            for (int i = 0; i < betas.Count; i++)
            {
                _betas.Add(betas[i]);
                if (i == lastIndex) last = _betas.Count - 1;
            }
            int maxPeriod = 12;
            MakeAproximate(maxPeriod, last);
            MakeForecast(maxPeriod, last);
            ConstForecast();
        }

        private void MakeAproximate(int maxPeriod, int lastIndex)
        {
            _betasByPeriod = new List<List<double>>();
            _betaMins = new List<double>();
            _betaMaxs = new List<double>();

            for (int i = 0; i < maxPeriod; i++)
            {
                _betasByPeriod.Add(new List<double>());
            }
            for (int i = _startIndex; i < lastIndex; i++)
            {
                int j = i % maxPeriod;
                _betasByPeriod[j].Add(_betas[i]);
            }
            for (int i = 0; i < maxPeriod; i++)
            {
                double mathExpection = Stat.MathExpection(_betasByPeriod[i]);
                double averSquereDeviation = Stat.AverageSquareDeviation(_betasByPeriod[i]);
                _betaMins.Add(mathExpection - averSquereDeviation);
                _betaMaxs.Add(mathExpection + averSquereDeviation);
            }
        }

        private double GetForecast(int t)
        {
            if(_forecastMode == ForecastMode.Optimistic) return _betaMins[t % 12];
            if(_forecastMode == ForecastMode.Pessimistic) return _betaMaxs[t % 12];
            return (_betaMaxs[t % 12] +_betaMins[t % 12]) / 2;
        }

        private void MakeForecast(int maxPeriod, int lastIndex)
        {
            _betasForecast = new List<double>();
            for (int t = 0; t < _dates.Count; t++)
            {
                double result = GetForecast(t);
                _betasForecast.Add(result);
            }
        }

        public List<double> GetBetaForecast(int lastIndex)
        {
            var betas = new List<double>();
            double trend = 1;
            double step = 0.04;
            for (int t = lastIndex; t < _betasForecast.Count; t++)
            {
                betas.Add(trend * _betasForecast[t]);
                //if (t % 12 == 0) trend += step;
            }
            return betas;
        }

        public List<double> GetGammaForecast(int lastIndex)
        {
            var gammas = new List<double>();
            for (int t = lastIndex; t < _gammasForecast.Count; t++)
            {
                gammas.Add(_gammasForecast[t]);
            }
            return gammas;
        }

        private void ConstForecast()
        {
            _gammasForecast = new List<double>();
            for (int t = 0; t < _dates.Count; t++)
            {
                _gammasForecast.Add(_gamma);
            }
        }
    }
}
