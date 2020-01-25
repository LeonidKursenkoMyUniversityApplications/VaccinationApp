using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Epidemic.BLL.Controllers.Clinic;
using Epidemic.BLL.Models;
using Epidemic.DAL.Entity;

namespace Epidemic.BLL.Controllers
{
    public class SisvAgeController
    {
        //private ClinicController _clinicController;
        //private ClinicCounterController _clinicCounterController;
        private List<DataTable> _sisvAges;

        //public SisvAgeData SisvAgeData { set; get; }
        public SisvAge Sisv { set; get; }
        public double Efficient { set; get; }
        public double A1 { set; get; }
        public double A2 { set; get; }
        public double A3 { set; get; }
        public double A4 { set; get; }
        public double Cv1 { set; get; }
        public double Cv2 { set; get; }
        public double Ci { set; get; }
        public double Inflation { set; get; }

        public VaccinationMode VaccMode { set; get; }
        public double P1 { set; get; }
        public double P2 { set; get; }

        public SisvAgeController()
        {
            _sisvAges = new List<DataTable>();
        }

        #region Methods

        public void DefineAll(SisAgeData sisAgeData, List<List<List<double>>> chanceOfInfection, DateTime forecastStartDate)
        {
            //_clinicCounterController = new ClinicCounterController(clinic);
            //_clinicCounterController.DefineAll();
            int t0 = sisAgeData.Data[0].Date.IndexOf(forecastStartDate);
            Sisv = new SisvAge(sisAgeData, Cv1, Cv2, Ci, Inflation, t0)
            {
                Efficient = Efficient,
                A1 = A1,
                A2 = A2,
                A3 = A3,
                A4 = A4,
                ChanceOfInfection = chanceOfInfection
            };
            //ConvertToMonthFormat();
            
            if(VaccMode == VaccinationMode.Dynamic)
                Sisv.Simulate(t0);
            else if (VaccMode == VaccinationMode.Custom)
            {
                Sisv.Simulate(P1, P2, t0);
            }
            else
            {
                Sisv.SimulateStatic(t0);
            }
            Sisv.GetPredictedInfections(sisAgeData);
            for (int i = 0; i < Sisv.S[0].Count; i++)
            {
                CreateTable(i);
            }
            IncomingData.Instance.SisvAgeData = _sisvAges;
        }

        private void ConvertToMonthFormat()
        {
            for (int t = Sisv.Date.Count - 1; t >= 0; t--)
            {
                if (Sisv.Date[t].Day != 1)
                {
                    Sisv.Date.RemoveAt(t);
                    Sisv.N.RemoveAt(t);
                    Sisv.S.RemoveAt(t);
                    Sisv.Itotal.RemoveAt(t);
                    Sisv.I.RemoveAt(t);
                    Sisv.Beta.RemoveAt(t);
                    Sisv.Gamma.RemoveAt(t);
                    Sisv.P1.RemoveAt(t);
                    Sisv.P2.RemoveAt(t);
                    Sisv.V1.RemoveAt(t);
                    Sisv.V2.RemoveAt(t);
                    Sisv.W1.RemoveAt(t);
                    Sisv.W2.RemoveAt(t);
                    Sisv.Iv1.RemoveAt(t);
                    Sisv.Iv2.RemoveAt(t);

                    Sisv.DeathTotal.RemoveAt(t);
                    Sisv.Death.RemoveAt(t);
                }
            }
        }
        
        private void CreateTable(int a)
        {
            DataTable sisTable = IncomingData.Instance.SisAgeData[a].Clone();
            sisTable.Columns.Add(new DataColumn(Titles.P1, typeof(double)));
            sisTable.Columns.Add(new DataColumn(Titles.P2, typeof(double)));
            sisTable.Columns.Add(new DataColumn(Titles.V1, typeof(int)));
            sisTable.Columns.Add(new DataColumn(Titles.V2, typeof(int)));
            sisTable.Columns.Add(new DataColumn(Titles.Iv1, typeof(int)));
            sisTable.Columns.Add(new DataColumn(Titles.Iv2, typeof(int)));
            //sisTable.Columns.Add(new DataColumn("w1", typeof(double)));
            //sisTable.Columns.Add(new DataColumn("w2", typeof(double)));
            for (int t = 0; t < Sisv.Date.Count; t++)
            {
                sisTable.Rows.Add();
                sisTable.Rows[t][0] = Sisv.Date[t];
                sisTable.Rows[t][1] = Sisv.N[t];
                sisTable.Rows[t][2] = Sisv.S[t][a];
                sisTable.Rows[t][3] = Sisv.Itotal[t];
                sisTable.Rows[t][4] = Sisv.I[t][a];
                sisTable.Rows[t][5] = Sisv.Beta[t][a];
                sisTable.Rows[t][6] = Sisv.Gamma[t][a];
                sisTable.Rows[t][Titles.P1] = Sisv.P1[t];
                sisTable.Rows[t][Titles.P2] = Sisv.P2[t];
                sisTable.Rows[t][Titles.V1] = Sisv.V1[t][a];
                sisTable.Rows[t][Titles.V2] = Sisv.V2[t][a];
                sisTable.Rows[t][Titles.Iv1] = Sisv.Iv1[t][a];
                sisTable.Rows[t][Titles.Iv2] = Sisv.Iv2[t][a];

                //sisTable.Rows[i]["w1"] = Sisv.W1[i][a];
                //sisTable.Rows[i]["w2"] = Sisv.W2[i][a];
            }
            _sisvAges.Add(sisTable);
        }

        public DataTable AddSisTable()
        {
            DataTable sisTable = _sisvAges[0].Clone();
            sisTable.Columns[2].ColumnName = Titles.Sus;
            sisTable.Columns.RemoveAt(4);
            for (int t = 0; t < Sisv.Date.Count; t++)
            {
                sisTable.Rows.Add();
                sisTable.Rows[t][0] = Sisv.Date[t];
                sisTable.Rows[t][1] = Sisv.N[t];
                sisTable.Rows[t][2] = Sisv.N[t] - Sisv.Itotal[t];
                sisTable.Rows[t][3] = Sisv.Itotal[t];
                sisTable.Rows[t][4] = Sisv.BetaTotal[t];
                sisTable.Rows[t][5] = Sisv.GammaTotal[t];
                sisTable.Rows[t][Titles.P1] = Sisv.P1[t];
                sisTable.Rows[t][Titles.P2] = Sisv.P2[t];
                double v1 = 0;
                double v2 = 0;
                int iv1 = 0;
                int iv2 = 0;
                for (int a = 0; a < Sisv.V1[t].Count; a++)
                {
                    v1 += Sisv.V1[t][a];
                    v2 += Sisv.V2[t][a];
                    iv1 += Sisv.Iv1[t][a];
                    iv2 += Sisv.Iv2[t][a];
                }
                sisTable.Rows[t][Titles.V1] = v1;
                sisTable.Rows[t][Titles.V2] = v2;
                sisTable.Rows[t][Titles.Iv1] = iv1;
                sisTable.Rows[t][Titles.Iv2] = iv2;
            }
            return sisTable;
        }
        
        #endregion
    }

    public enum VaccinationMode { Custom, Dynamic, Static}
}
