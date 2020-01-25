using System;
using System.Collections.Generic;
using System.Linq;

namespace Epidemic.BLL.Models
{
    public class ClinicModel
    {
        public List<DateTime> Date { set; get; }
        public List<List<double>> S { set; get; }
        public List<List<double>> I { set; get; }
        public List<List<double>> P1 { set; get; }
        public List<List<double>> P2 { set; get; }

        public string Info { set; get; }
        
        public void CalculateP2()
        {
            for (int t = 0; t < Date.Count; t++)
            {
                for (int a = 0; a < S[t].Count; a++)
                {
                    P2[t][a] = 1;
                }
            }
        }
        
        public void CalculateP1()
        {
            for (int t = 0; t < Date.Count; t++)
            {
                int n = S[t].Count;
                for (int a = 0; a < n; a++)
                {
                    if (t > 0 && t % 12 == 0 && a + 1 < n)
                        P1[t][a] = I[t + 1][a + 1];
                    else if (t == Date.Count - 1)
                        P1[t][a] = P1[t - 1][a];
                    else P1[t][a] = I[t + 1][a];
                    //if (t > 0 && t % 12 == 0)
                    //    P1[t][a] = I[t][a + 1];
                    //else if (t == Date.Count - 1)
                    //    P1[t][a] = I[t][a + 1]; // / S[t][a];
                    //else P1[t][a] = I[t + 1][a];
                }
            }
        }

        //public void CalculateP1()
        //{
        //    for (int t = 0; t < Date.Count; t++)
        //    {
        //        int n = S[t].Count - 1;
        //        for (int a = 0; a < n; a++)
        //        {
        //            P1[t][a] = I[t][a + 1]; /// S[t][a];
        //        }
        //    }
        //}


        public List<double> CommonP1
        {
            get
            {
               List<double> p1 = new List<double>();
                for (int t = 0; t < Date.Count; t++)
                {
                    p1.Add(P1[t].Sum() / P1[t].Count);
                }
                return p1;
            }
        }
    }
}
