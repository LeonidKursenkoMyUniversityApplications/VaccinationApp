using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Epidemic.BLL.Controllers.Aproximate;
using Epidemic.BLL.Models;
using Epidemic.DAL.Entity;

namespace Epidemic.BLL.Controllers
{
    public class SisAgeController
    {
        private List<DataTable> _sisAges;
        private List<DataTable> _sisAgesSource;

        public ForecastMode ForecastMode { set; get; }
        public SisAgeData SisAgeData { set; get; }
        public BirthMode BirthMode { set; get; }
        public DeathMode DeathMode { set; get; }
        public double BirthPercent { set; get; }
        public double DeathPercent { set; get; }
        public int Birth { set; get; }
        public int Death { set; get; }

        private ForecastAproximate _forecastController;

        public SisAgeController()
        {
            _sisAges = IncomingData.Instance.SisAgeData;
            _sisAgesSource = new List<DataTable>();
            for (int a = 0; a < _sisAges.Count; a++)
                _sisAgesSource.Add(_sisAges[a].Copy());
        }

        #region Methods

        public void RestoreSourceData()
        {
            _sisAges = new List<DataTable>();
            for (int a = 0; a < _sisAgesSource.Count; a++)
                _sisAges.Add(_sisAgesSource[a].Copy());
            IncomingData.Instance.SisAgeData = _sisAges;
        }

        public void DefineAll()
        {
            _sisAges = IncomingData.Instance.SisAgeData;
            SisAgeData = new SisAgeData(_sisAges);

            if (_sisAges[0].Columns.Contains(Titles.Beta) == true) return;

            for (int i = 0; i < _sisAges.Count; i++)
            {
                CreateTable(i);
            }
            IncomingData.Instance.SisAgeData = _sisAges;
        }

        public void AddForecast(DateTime forecastDate, ForecastMode forecastMode)
        {
            ForecastMode = forecastMode;
            SisAgeData.ForecastMode = forecastMode;
            SisAgeData.BirthMode = BirthMode;
            SisAgeData.DeathMode = DeathMode;
            SisAgeData.BirthConst = Birth;
            SisAgeData.DeathConst = Death;
            SisAgeData.BirthPercent = BirthPercent;
            SisAgeData.DeathPercent = DeathPercent;
            int lastIndex = 0;
            for (int a = 0; a < SisAgeData.Data.Count; a++)
            {
                var sis = SisAgeData.Data[a];
                lastIndex = sis.Date.Count - 1;
                
                for (int t = lastIndex; !(sis.Date[t].Month == forecastDate.Month &&
                               sis.Date[t].Year == forecastDate.Year); t++)
                {
                    sis.Date.Add(sis.Date[t].AddMonths(1));
                    sis.N.Add(sis.N[t]);
                    sis.S.Add(0);
                    sis.Itotal.Add(0);
                    sis.I.Add(0);
                    sis.Death.Add(0);
                    //sis.Beta.Add(0);
                    //sis.Gamma.Add(0);
                    if (a == 0)
                    {
                        SisAgeData.DeathTotal.Add(0);
                        SisAgeData.Birth.Add(0);
                    }
                }
                lastIndex++;
                var forecast = new StochasticAproximate(sis.Date, sis.Beta, sis.Gamma, lastIndex, forecastMode);
                SisAgeData.Data[a].Beta.AddRange(forecast.GetBetaForecast(lastIndex));
                SisAgeData.Data[a].Gamma.AddRange(forecast.GetGammaForecast(lastIndex));
            }
            _forecastController = new ForecastAproximate(SisAgeData, lastIndex, forecastMode);

            for (int t = lastIndex; t < SisAgeData.Data[0].Date.Count; t++)
            {
                int n = 0;
                int iTotal = 0;
                int deathTotal = 0;
                for (int a = 0; a < SisAgeData.Data.Count; a++)
                {
                    var sis = SisAgeData.Data[a];
                    Simulate1Period(SisAgeData, t, a, lastIndex);
                    iTotal += sis.I[t];
                    deathTotal += sis.Death[t];
                    n += SisAgeData.Data[a].S[t] + SisAgeData.Data[a].I[t];
                }
                for (int a = 0; a < SisAgeData.Data.Count; a++)
                {
                    SisAgeData.Data[a].Itotal[t] = iTotal;
                    SisAgeData.Data[a].N[t] = n;
                }
                SisAgeData.DeathTotal[t] = deathTotal;
            }
            SisAgeData.CalculateTotalParams(lastIndex);
            for (int a = 0; a < SisAgeData.Data.Count; a++) UpdateTable(a);
        }
        
        private void Simulate1Period(SisAgeData sisAge, int t, int a, int t0)
        {
            if (t < 1) return;
            var sis = SisAgeData.Data[a];
            //sis.I[t] = _forecastController.GetForecast(SisAgeData, t, a);
            t--;

            //if (sis.S[t] == 0 || sis.Itotal[t] == 0)
            //    sis.Beta[t] = 0;
            //else
            //{
            //    sis.Beta[t] = (sis.I[t + 1] - sis.I[t] * (1 - sis.Gamma[t])) / sis.S[t] / sis.Itotal[t] * sis.N[t];
            //    sis.Beta[t] = Correct(sis.Beta[t]);
            //}
            //if (t == sis.Beta.Count - 2) sis.Beta[t + 1] = sis.Beta[t];
            //sis.Gamma[t + 1] = sis.Gamma[t];

            sis.I[t + 1] = (int)Math.Round(sis.I[t] +
                sis.Beta[t] * sis.Itotal[t] / sis.N[t] * sis.S[t] -
                sis.Gamma[t] * sis.I[t]);

            sis.S[t + 1] = (int) Math.Round(sis.S[t] -
                sis.Beta[t] * sis.Itotal[t] / sis.N[t] * sis.S[t] +
                sis.Gamma[t] * sis.I[t] + sis.Death[t]);

            if (a == 0)
            {
                sis.S[t + 1] += SisAgeData.Birth[t] -
                    (int) Math.Round(SisAgeData.Birth[t - 11] * 
                    SisAgeData.GetAlivePercent(t, t - 11, a));
                if (BirthMode == BirthMode.Dynamic)
                    SisAgeData.Birth[t + 1] = (int) Math.Round(BirthPercent * sis.N[t]);
                else SisAgeData.Birth[t + 1] = Birth;
            }
            else
            {
                if ((t - t0 + 1) / 12 >= a)
                {
                    sis.S[t + 1] += (int)Math.Round(SisAgeData.Birth[t - 12 * a + 1] *
                        SisAgeData.GetAlivePercent(t, t - 12 * a + 1, a - 1));
                    if (a < SisAgeData.Data.Count - 1)
                    {
                        sis.S[t + 1] -= (int)Math.Round(SisAgeData.Birth[t - 12 * (a + 1) + 1] *
                            SisAgeData.GetAlivePercent(t, t - 12 * (a + 1) + 1, a - 1));
                    }
                }
                else
                {
                    sis.S[t + 1] += (int)Math.Round((double)SisAgeData.Data[a - 1].S[t - 11] / 12 *
                            SisAgeData.GetAlivePercent(t, t - 11, a - 1));
                    if (a < SisAgeData.Data.Count - 1)
                    {
                        sis.S[t + 1] -= (int)Math.Round((double)sis.S[t - 11] / 12 *
                            SisAgeData.GetAlivePercent(t, t - 23, a - 1));
                    }
                }
            }
            sis.S[t + 1] = Correct(sis.S[t + 1]);
            if (DeathMode == DeathMode.Dynamic)
                sis.Death[t + 1] = (int)Math.Round(-DeathPercent * (sis.S[t] + sis.I[t]));
            else sis.Death[t + 1] = (int)Math.Round((double)-Death * (sis.S[t] + sis.I[t]) / sis.N[t]);
            //if (Math.Abs(sis.Death[t + 1]) > sis.S[t])
            //    sis.Death[t + 1] = sis.S[t];
        }

        private void Simulate1Period0(SisAgeData sisAge, int t, int a, int lastIndex)
        {
            if (t < 1) return;
            var sis = SisAgeData.Data[a];
            sis.I[t] = _forecastController.GetForecast(SisAgeData, t, a);
            t--;

            sis.Beta[t] = (sis.I[t + 1] - sis.I[t] * (1 - sis.Gamma[t])) / sis.S[t] / sis.Itotal[t] * sis.N[t];
            if (t == sis.Beta.Count - 2) sis.Beta[t + 1] = sis.Beta[t];
            sis.Gamma[t + 1] = sis.Gamma[t];

            sis.I[t + 1] = (int)Math.Round(sis.I[t] +
                sis.Beta[t] * sis.Itotal[t] / sis.N[t] * sis.S[t] -
                sis.Gamma[t] * sis.I[t]);

            sis.S[t + 1] = (int)Math.Round(sis.S[t] -
                sis.Beta[t] * sis.Itotal[t] / sis.N[t] * sis.S[t] +
                sis.Gamma[t] * sis.I[t] + sis.Death[t]);

            if (a == 0)
            {
                sis.S[t + 1] += SisAgeData.Birth[t] -
                    (int)Math.Round(SisAgeData.Birth[t - 11] *
                    SisAgeData.GetAlivePercent(t, t - 11, a));
                if (BirthMode == BirthMode.Dynamic)
                    SisAgeData.Birth[t + 1] = (int)Math.Round(BirthPercent * sis.N[t]);
                else SisAgeData.Birth[t + 1] = Birth;
            }
            else
            {
                sis.S[t + 1] += (int)Math.Round((double)SisAgeData.Data[a - 1].S[t - 11] / 12 *
                    SisAgeData.GetAlivePercent(t, t - 11, a - 1));
                if (a < SisAgeData.Data.Count - 1)
                {
                    sis.S[t + 1] -= (int)Math.Round((double)sis.S[t - 11] / 12 *
                        SisAgeData.GetAlivePercent(t, t - 11, a));
                }
            }
            sis.S[t + 1] = Correct(sis.S[t + 1]);
            if (DeathMode == DeathMode.Dynamic)
                sis.Death[t + 1] = (int)Math.Round(-DeathPercent * (sis.S[t] + sis.I[t]));
            else sis.Death[t + 1] = (int)Math.Round((double)-Death * (sis.S[t] + sis.I[t]) / sis.N[t]);
        }

        private int Correct(int val)
        {
            return val < 0 ? 0 : val;
        }

        private double Correct(double val)
        {
            return val < 0 ? 0 : val;
        }

        private void CreateTable(int index)
        {
            DataTable sisTable = _sisAges[index];
            sisTable.Columns[0].ColumnName = Titles.Date;
            sisTable.Columns[1].ColumnName = Titles.N;
            sisTable.Columns[2].ColumnName = Titles.Sus;
            sisTable.Columns[3].ColumnName = Titles.InfTotal;
            sisTable.Columns[4].ColumnName = $"{Titles.Inf} {index}-{index + 1} років";
            if (_sisAges.Count - 1 == index)
                sisTable.Columns[4].ColumnName = $"{Titles.Inf} {index} і більше років";
            for (int i = 0; i < sisTable.Rows.Count; i++)
                sisTable.Rows[i][3] = SisAgeData.Data[index].Itotal[i];
            sisTable.Columns.Add(new DataColumn(Titles.Beta, typeof(double)));
            for (int i = 0; i < sisTable.Rows.Count; i++)
                sisTable.Rows[i][sisTable.Columns.Count - 1] = SisAgeData.Data[index].Beta[i];
            sisTable.Columns.Add(new DataColumn(Titles.Gamma, typeof(double)));
            for (int i = 0; i < sisTable.Rows.Count; i++)
                sisTable.Rows[i][sisTable.Columns.Count - 1] = SisAgeData.Data[index].Gamma[i];
            _sisAges[index] = sisTable;
        }

        public DataTable AddSisTable()
        {
            DataTable sisTable = _sisAges[0].Clone();
            sisTable.Columns[2].ColumnName = Titles.Sus;
            sisTable.Columns.RemoveAt(4);
            var sis = SisAgeData.Data;
            for (int t = 0; t < sis[0].Date.Count; t++)
            {
                sisTable.Rows.Add();
                sisTable.Rows[t][0] = sis[0].Date[t];
                sisTable.Rows[t][1] = sis[0].N[t];
                sisTable.Rows[t][2] = sis[0].N[t] - sis[0].Itotal[t];
                sisTable.Rows[t][3] = sis[0].Itotal[t];
                sisTable.Rows[t][4] = SisAgeData.BetaTotal[t];
                sisTable.Rows[t][5] = SisAgeData.GammaTotal[t];
            }
            return sisTable;
        }
        
        private void UpdateTable(int index)
        {
            DataTable sisTable = _sisAges[index];
            sisTable.Clear();

            for (int i = 0; i < SisAgeData.Data[index].Date.Count; i++)
            {
                sisTable.Rows.Add();
                sisTable.Rows[i][0] = SisAgeData.Data[index].Date[i];
                sisTable.Rows[i][1] = SisAgeData.Data[index].N[i];
                sisTable.Rows[i][2] = SisAgeData.Data[index].S[i];
                sisTable.Rows[i][3] = SisAgeData.Data[index].Itotal[i];
                sisTable.Rows[i][4] = SisAgeData.Data[index].I[i];
                sisTable.Rows[i][5] = SisAgeData.Data[index].Beta[i];
                sisTable.Rows[i][6] = SisAgeData.Data[index].Gamma[i];
            }

            _sisAges[index] = sisTable;
        }
        
        public DataTable GetCommonTable(int columnIndex)
        {
            DataTable table = new DataTable();
            // Date column
            table.Columns.Add(
                new DataColumn(_sisAges[0].Columns[0].ColumnName,
                    _sisAges[0].Columns[0].DataType));
            // Other columns
            var columnNames = IncomingData.Instance.AgeGroups;
            if(columnNames == null || columnNames.Count != _sisAges.Count)
                throw new Exception("Помилка через неоднакову розмірність таблиць");
            for (int i = 0; i < _sisAges.Count; i++)
            {
                table.Columns.Add(
                    new DataColumn(columnNames[i],
                        _sisAges[i].Columns[columnIndex].DataType));
            }
            for (int i = 0; i < _sisAges[0].Rows.Count; i++)
            {
                table.Rows.Add();
                table.Rows[i][0] = _sisAges[0].Rows[i][0];
                for (int j = 0; j < _sisAges.Count; j++)
                    table.Rows[i][j + 1] = _sisAges[j].Rows[i][columnIndex];
            }
            return table;
        }

        public List<string> BuildCommonSisAgeInfoTables()
        {
            List<DataTable> tables = new List<DataTable>
            {
                // Susceptible
                GetCommonTable(2),
                // Infected
                GetCommonTable(4),
                // Beta
                GetCommonTable(5),
                // Gamma
                GetCommonTable(6)
            };
            IncomingData.Instance.CommonSisAgeInfoData = tables;
            return new List<string>()
            {
                "Сприйнятливі",
                "Інфіковані",
                "Параметр передачі збудника",
                "Швидкість одужання"
            };
        }

        public int GetTbyDate(DateTime date)
        {
            return SisAgeData.Data[0].Date.IndexOf(date);
        }
        
        #endregion
    }
}
