using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Epidemic.BLL.Models;
using Epidemic.DAL.Entity;

namespace Epidemic.BLL.Controllers.Clinic
{
    public class ClinicController
    {
        public SisAgeData SisAgeData { get; set; }
        public ClinicModel Clinic { get; set; }

        public ClinicController(SisAgeData sisAgeData)
        {
            SisAgeData = sisAgeData;
            Clinic = TranformData(sisAgeData);
            Clinic.CalculateP2();
            Clinic.CalculateP1();
        }

        public ClinicController(SisvAge sisv)
        {
            SisAgeData = null;
            Clinic = TranformData(sisv);
            Clinic.CalculateP2();
            Clinic.CalculateP1();
        }

        private ClinicModel TranformData(SisAgeData sisAgeData)
        {
            var clinic = new ClinicModel();
            clinic.Date = new List<DateTime>();
            clinic.S = new List<List<double>>();
            clinic.I = new List<List<double>>();
            clinic.P1 = new List<List<double>>();
            clinic.P2 = new List<List<double>>();
            for (int t = 0; t < sisAgeData.Data[0].Date.Count; t++)
            {
                var date = sisAgeData.Data[0].Date[t];
                if (t != 0 && date.Month == sisAgeData.Data[0].Date[t - 1].Month)
                    continue;
                clinic.Date.Add(new DateTime(date.Year, date.Month, date.Day));
                clinic.S.Add(new List<double>());
                clinic.I.Add(new List<double>());
                clinic.P1.Add(new List<double>());
                clinic.P2.Add(new List<double>());
                for (int a = 0; a < SisAgeData.Data.Count; a++)
                {
                    double n = sisAgeData.Data[a].S[t] + sisAgeData.Data[a].I[t];
                    double s = sisAgeData.Data[a].S[t] / n;
                    double i = sisAgeData.Data[a].I[t] / n;
                    int it = clinic.S.Count - 1;
                    clinic.S[it].Add(s);
                    clinic.I[it].Add(i);
                    clinic.P1[it].Add(0);
                    clinic.P2[it].Add(0);
                }
            }
            clinic.Info = GetParamsInfo(sisAgeData);
            return clinic;
        }

        private ClinicModel TranformData(SisvAge sisv)
        {
            var clinic = new ClinicModel();
            clinic.Date = new List<DateTime>();
            clinic.S = new List<List<double>>();
            clinic.I = new List<List<double>>();
            clinic.P1 = new List<List<double>>();
            clinic.P2 = new List<List<double>>();
            for (int t = 0; t < sisv.Date.Count; t++)
            {
                var date = sisv.Date[t];
                if (t != 0 && date.Month == sisv.Date[t - 1].Month) continue;
                clinic.Date.Add(new DateTime(date.Year, date.Month, date.Day));
                clinic.S.Add(new List<double>());
                clinic.I.Add(new List<double>());
                clinic.P1.Add(new List<double>());
                clinic.P2.Add(new List<double>());
                for (int a = 0; a < sisv.S[t].Count; a++)
                {
                    double n = sisv.S[t][a] + sisv.I[t][a] + sisv.V1[t][a] + sisv.V2[t][a] + 
                        sisv.Iv1[t][a] + sisv.Iv2[t][a];
                    double s = sisv.S[t][a] / n;
                    double i = sisv.I[t][a] / n;
                    int it = clinic.S.Count - 1;
                    clinic.S[it].Add(s);
                    clinic.I[it].Add(i);
                    clinic.P1[it].Add(0);
                    clinic.P2[it].Add(0);
                }
            }
            clinic.Info = GetParamsInfo(sisv);
            return clinic;
        }

        private DataTable GetCommonTable(List<List<double>> data)
        {
            DataTable table = new DataTable();
            var commonTable = IncomingData.Instance.CommonSisAgeInfoData[0];
            // Date column
            table.Columns.Add(
                new DataColumn(commonTable.Columns[0].ColumnName,
                    commonTable.Columns[0].DataType));
            // Other columns
            var columnNames = IncomingData.Instance.AgeGroups;
            if (columnNames == null || columnNames.Count != data[0].Count)
                throw new Exception("Помилка через неоднакову розмірність таблиць");
            for (int i = 1; i < commonTable.Columns.Count; i++)
            {
                table.Columns.Add(
                    new DataColumn(columnNames[i - 1], typeof(double)));
            }

            for (int t = 0; t < Clinic.Date.Count; t++)
            {
                table.Rows.Add();
                table.Rows[t][0] = Clinic.Date[t];
                for (int a = 0; a < data[t].Count; a++)
                {
                    table.Rows[t][a + 1] = data[t][a];
                }
            }
            return table;
        }

        public List<string> BuildClinicSisAgeTables()
        {
            List<DataTable> tables = new List<DataTable>
            {
                // Susceptible
                GetCommonTable(Clinic.S),
                // Infected
                GetCommonTable(Clinic.I),
                // Beta
                GetCommonTable(Clinic.P1)
            };
            IncomingData.Instance.ClinicSisAgeData = tables;
            return new List<string>()
            {
                "Сприйнятливі",
                "Інфіковані",
                "Ймовірність захворювання"
            };
        }

        private string GetParamsInfo(SisAgeData sis)
        {
            string info = "";
            if (sis.BirthMode == BirthMode.Const)
                info += $" народжуваності ({sis.BirthConst} осіб);";
            else info += $" коефіцієнт народжуваності ({sis.BirthPercent});";
            if (sis.DeathMode == DeathMode.Const)
                info += $" смертності ({sis.DeathConst} осіб).";
            else info += $" коефіцієнт смертності ({sis.DeathPercent}).";
            //if (sis.ForecastMode == ForecastMode.Optimistic)
            //    info += " прогноз оптимістичний.";
            //else if (sis.ForecastMode == ForecastMode.Realistic)
            //    info += " прогноз реалістичний.";
            //else info += " прогноз песимістичний.";
            return info;
        }

        private string GetParamsInfo(SisvAge sisv)
        {
            string info = $" вартість однієї дози вакцини ({sisv.Cv1} грн);" +
                $" вартость двох доз вакцини ({sisv.Cv2} грн);" +
                $" витрати при захворюванні ({sisv.Ci} грн);";
            if (sisv.BirthMode == BirthMode.Const)
                info += $" народжуваність ({sisv.BirthConst} осіб);";
            else info += $" коефіцієнт народжуваності ({sisv.BirthPercent});";
            if (sisv.DeathMode == DeathMode.Const)
                info += $" смертність ({sisv.DeathConst} осіб).";
            else info += $" коефіцієнт смертності ({sisv.DeathPercent}).";
            //if (sisv.ForecastMode == ForecastMode.Optimistic)
            //    info += " прогноз оптимістичний.";
            //else if (sisv.ForecastMode == ForecastMode.Realistic)
            //    info += " прогноз реалістичний.";
            //else info += " прогноз песимістичний.";
            return info;
        }
    }
}
