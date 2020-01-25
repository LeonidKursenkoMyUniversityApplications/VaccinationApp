using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using Epidemic.DAL.Entity;

namespace Epidemic.BLL.Models
{
    public class SirData : EpidemicModel
    {
        #region Attributes

        private static SirData instance;

        #endregion
        
        #region Properties

        public static SirData Instance
        {
            get { return instance ?? (instance = new SirData(IncomingData.Instance.SirData)); }
        }

        //public List<int> N { set; get; }
        public List<double> S { set; get; }
        public List<double> I { set; get; }
        public List<double> R { set; get; }
        public List<DateTime> Date { set; get; }

        #endregion

        #region Constructors

        private SirData(DataTable dt)
        {
            Date = new List<DateTime>();
            GetDate(dt);
            //N = new List<int>();
            //GetN();
            S = new List<double>();
            GetS(dt);
            I = new List<double>();
            GetI(dt);            
            Beta = new List<double>();
            GetBeta();
            CorrectBeta();
            Gamma = new List<double>();
            GetGamma();
            CorrectGamma();
            R = new List<double>();
            GetR();
            //CorrectBetaGamma();
        }

        #endregion

        #region Methods
        //private void GetN()
        //{
        //    IncomingData data = IncomingData.Instance;
        //    for(int i = 0; i < data.SirData.Rows.Count; i++)
        //    {
        //        N.Add((int) data.SirData.Rows[i][2]);
        //    }
        //}
        private void GetDate(DataTable dt)
        {
            int year = 2013;
            int week = 40;
            var startDate = DateOfWeek(year, week);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Date.Add(startDate);
                startDate = startDate.AddDays(7);
            }
        }

        private static DateTime DateOfWeek(int year, int weekOfYear)
        {
            DateTime jan1 = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - jan1.DayOfWeek;

            DateTime firstThursday = jan1.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var weekNum = weekOfYear;
            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }
            var result = firstThursday.AddDays(weekNum * 7);
            return result.AddDays(-3);
        }

        protected override void GetS(DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                S.Add((double)dt.Rows[i][1]);
            }
        }

        protected override void GetI(DataTable dt)
        {
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                I.Add((double)dt.Rows[i][2]);
            }
        }

        protected void GetR()
        {
            double r = 0;
            //R.Add(r);
            for (int i = 0; i < I.Count; i++)
            {
                //r = Gamma[i] * I[i];
                //r = r + R[R.Count - 1];
                r = 1 - S[i] - I[i];
                r = CorrectValue(r);
                R.Add(r);
            }
        }

        protected override void GetBeta()
        {
            double beta;
            for (int t = 0; t < S.Count - 1; t++)
            {
                beta = (S[t] - S[t + 1]) / S[t] / I[t] / 7;
                beta = CorrectValue(beta);
                Beta.Add(beta);
            }
            // Add last value
            Beta.Add(0);
        }

        protected  override void GetGamma()
        {
            double gamma;
            for (int t = 0; t < S.Count - 1; t++)
            {
                //gamma = -(I[t + 1] - I[t] - S[t] * I[t] * Beta[t]) / I[t];
                gamma = S[t] * Beta[t] - (I[t + 1] - I[t]) / I[t];
                gamma = CorrectValue(gamma);
                Gamma.Add(gamma);
            }
            // Add last value
            Gamma.Add(0);
        }

        protected override void CorrectBetaGamma()
        {
            //for (int i = 0; i < Beta.Count; i++)
            //{
            //    Beta[i] /= 7;
            //    Gamma[i] /= 7;
            //}
        }

        #endregion
    }
}
