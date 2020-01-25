using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Epidemic.BLL.Models.Optimization;

namespace Epidemic.BLL.Models
{
    public class SisvAge
    {
        #region Properties
        public List<DateTime> Date { set; get; }
        // Все населення
        public List<int> N { set; get; }
        // Народжуваність 
        public List<int> Birth { set; get; }
        // Сприйнятливі
        // [t, a]
        public List<List<int>> S { set; get; }
        // Все хворе населення
        public List<int> Itotal { set; get; }
        // Хворі для для окремих вікових груп 
        public List<List<int>> I { set; get; }
        // Коефіцієнт захворюваності
        public List<List<double>> Beta { set; get; }
        public List<double> BetaTotal { set; get; }
        // Коефіцієнт одуження
        public List<List<double>> Gamma { set; get; }
        public List<double> GammaTotal { set; get; }
        // Процент вакцинованих 1, 2 вакциною в 1 період
        public List<double> P1 { set; get; }
        public List<double> P2 { set; get; }
        // Вакциноване населення для даної вікової групи
        public List<List<int>> V1 { set; get; }
        public List<List<int>> V2 { set; get; }
        // Спад вакцинації
        public List<List<double>> W1 { set; get; }
        public List<List<double>> W2 { set; get; }
        // Смертність
        public List<int> DeathTotal { set; get; }
        public List<List<int>> Death { set; get; }
        // Вакциновані інфіковані
        public List<List<int>> Iv1 { set; get; }
        public List<List<int>> Iv2 { set; get; }
        // other
        public double Efficient { set; get; }
        public double A1 { set; get; }
        public double A2 { set; get; }
        public double A3 { set; get; }
        public double A4 { set; get; }
        public double Cv1 { set; get; }
        public double Cv2 { set; get; }
        public double Ci { set; get; }
        // Витрати за місяць
        public List<double> Cost { set; get; }
        // Витрати за весь час
        public double CostTotal { set; get; }
        public double CostV1Total { set; get; }
        public double CostV2Total { set; get; }
        public double CostInfectedTotal { set; get; }
        // Попереджені випадки інфікування для окремих вікових груп 
        public List<List<int>> PredictedInfections { set; get; }

        // Скільки разів можна захворіти
        //public int InfectedCounter { set; get; }
        // Витрати за місяць
        public List<double> IndividualCostV1 { set; get; }
        public List<double> IndividualCostV2 { set; get; }
        // Ймовірність бути інфікованим н-разів
        public List<List<List<double>>> ChanceOfInfection { set; get; }
        public List<double> Cv1List { set; get; }
        public List<double> Cv2List { set; get; }
        public List<double> CiList { set; get; }
        public double Inflation { set; get; }

        public double BirthPercent { set; get; }
        public double DeathPercent { set; get; }
        public int BirthConst { set; get; }
        public int DeathConst { set; get; }
        public ForecastMode ForecastMode { set; get; }
        public BirthMode BirthMode { set; get; }
        public DeathMode DeathMode { set; get; }
        #endregion
        // Початкова дата прогнозу.
        private int _t0;
        public int StartForecast => _t0;

        #region Constructors

        public SisvAge()
        {
            Date = new List<DateTime>();
            N = new List<int>();
            Birth = new List<int>();
            S = new List<List<int>>();
            Itotal = new List<int>();
            I = new List<List<int>>();
            Beta = new List<List<double>>();
            BetaTotal = new List<double>();
            Gamma = new List<List<double>>();
            GammaTotal = new List<double>();
            P1 = new List<double>();
            P2 = new List<double>();
            V1 = new List<List<int>>();
            V2 = new List<List<int>>();
            W1 = new List<List<double>>();
            W2 = new List<List<double>>();
            Iv1 = new List<List<int>>();
            Iv2 = new List<List<int>>();

            DeathTotal = new List<int>();
            Death = new List<List<int>>();
            Cost = new List<double>();
            CostTotal = 0;
            CostV1Total = 0;
            CostV2Total = 0;
            CostInfectedTotal = 0;
            IndividualCostV1 = new List<double>();
            IndividualCostV2 = new List<double>();
            Cv1List = new List<double>();
            Cv2List = new List<double>();
            CiList = new List<double>();

            PredictedInfections = new List<List<int>>();
        }

        public SisvAge(SisAgeData sisAge, double cv1, double cv2, double ci, double inflation, int forecastStartT) : this()
        {
            Inflation = inflation;
            Cv1 = cv1;
            Cv2 = cv2;
            Ci = ci;
            BirthPercent = sisAge.BirthPercent;
            DeathPercent = sisAge.DeathPercent;
            BirthConst = sisAge.BirthConst;
            DeathConst = sisAge.DeathConst;
            ForecastMode = sisAge.ForecastMode;
            BirthMode = sisAge.BirthMode;
            DeathMode = sisAge.DeathMode;
            
            for (int t = 0; t < sisAge.Data[0].Date.Count; t++)
            {
                Date.Add(new DateTime(sisAge.Data[0].Date[t].Year, 
                    sisAge.Data[0].Date[t].Month, sisAge.Data[0].Date[t].Day));
                N.Add(sisAge.Data[0].N[t]);
                Birth.Add(sisAge.Birth[t]);
                S.Add(new List<int>());
                Itotal.Add(sisAge.Data[0].Itotal[t]);
                DeathTotal.Add(sisAge.DeathTotal[t]);
                I.Add(new List<int>());
                Beta.Add(new List<double>());
                Gamma.Add(new List<double>());
                V1.Add(new List<int>());
                V2.Add(new List<int>());
                Iv1.Add(new List<int>());
                Iv2.Add(new List<int>());
                W1.Add(new List<double>());
                W2.Add(new List<double>());
                Death.Add(new List<int>());
                double beta = 0;
                double gamma = 0;
                for (int a = 0; a < sisAge.Data.Count; a++)
                {
                    S[t].Add(sisAge.Data[a].S[t]);
                    I[t].Add(sisAge.Data[a].I[t]);
                    Beta[t].Add(sisAge.Data[a].Beta[t]);
                    Gamma[t].Add(sisAge.Data[a].Gamma[t]);
                    beta += sisAge.Data[a].Beta[t] * (sisAge.Data[a].S[t] + 
                        sisAge.Data[a].I[t]) / sisAge.Data[a].N[t];
                    gamma += sisAge.Data[a].Gamma[t] * (sisAge.Data[a].S[t] + 
                        sisAge.Data[a].I[t]) / sisAge.Data[a].N[t];
                    V1[t].Add(0);
                    V2[t].Add(0);
                    W1[t].Add(0);
                    W2[t].Add(0);
                    Iv1[t].Add(0);
                    Iv2[t].Add(0);
                    Death[t].Add(sisAge.Data[a].Death[t]);
                }
                BetaTotal.Add(beta);
                GammaTotal.Add(gamma);
                P1.Add(0);
                P2.Add(0);
                Cost.Add(0);
                IndividualCostV1.Add(0);
                IndividualCostV2.Add(0);
                Cv1List.Add(0);
                Cv2List.Add(0);
                CiList.Add(0);
            }
            CorrectCost(forecastStartT);
        }
        #endregion

        #region Methods
        private int GetIv1Total(int t)
        {
            int total = 0;
            for (int a = 0; a < Iv1[t].Count; a++)
            {
                total += Iv1[t][a];
            }
            return total;
        }

        private int GetIv2Total(int t)
        {
            int total = 0;
            for (int a = 0; a < Iv2[t].Count; a++)
            {
                total += Iv2[t][a];
            }
            return total;
        }

        private void CorrectCost(int t0)
        {
            for (int t = t0; t < Date.Count; t++)
            {
                Cv1List[t] = Cv1 * Math.Pow(1 + Inflation / 12, t - t0);
                Cv2List[t] = Cv2 * Math.Pow(1 + Inflation / 12, t - t0);
                CiList[t] = Ci * Math.Pow(1 + Inflation / 12, t - t0);
            }
        }

        private void ResetCost()
        {
            CostTotal = 0;
            CostV1Total = 0;
            CostV2Total = 0;
            CostInfectedTotal = 0;
        }

        public void Simulate(int t0)
        {
            _t0 = t0;
            ResetCost();
            for (int t = _t0; t < Date.Count - 1; t++)
            {
                CostTotal += CostOptimization(t);
                CostV1Total += Birth[t] * P1[t] * Cv1List[t];
                CostV2Total += Birth[t] * P2[t] * Cv2List[t];
            }
            int tk = Date.Count - 1;
            CostTotal += Cost[tk];
            CostV1Total += Birth[tk] * P1[tk] * Cv1List[tk];
            CostV2Total += Birth[tk] * P2[tk] * Cv2List[tk];
            CostInfectedTotal += CostTotal - CostV1Total - CostV2Total;
        }
        
        public void Simulate(double p1, double p2, int t0)
        {
            _t0 = t0;
            ResetCost();
            for (int t = _t0; t < Date.Count - 1; t++)
            {
                Simulate1Period(t, p1, p2);
                CostTotal += Cost[t];
                CostV1Total += Birth[t] * P1[t] * Cv1List[t];
                CostV2Total += Birth[t] * P2[t] * Cv2List[t];
            }
            int tk = Date.Count - 1;
            CostTotal += Cost[tk];
            CostV1Total += Birth[tk] * P1[tk] * Cv1List[tk];
            CostV2Total += Birth[tk] * P2[tk] * Cv2List[tk];
            CostInfectedTotal += CostTotal - CostV1Total - CostV2Total;
        }

        public void SimulateStatic(int t0)
        {
            _t0 = t0;
            ResetCost();
            CostOptimization(_t0);
            Simulate(P1[t0], P2[t0], t0);
        }

        private int Correct(int value)
        {
            return value < 0 ? 0 : value;
        }

        private double Correct(double val)
        {
            return val < 0 ? 0 : val;
        }

        public double GetAlivePercent(int tk, int t0, int a)
        {
            double alive = 1;
            for (int t = t0; t <= tk; t++)
            {
                int n = S[t][a] + I[t][a] + V1[t][a] + V2[t][a] + Iv1[t][a] + Iv2[t][a];
                if (n != 0)
                    alive *= (1 + Death[t][a] / n);
            }
            return alive;
        }

        private void Simulate1Period(int t, double p1, double p2)
        {
            P1[t] = p1;
            P2[t] = p2;
            double ef = Efficient;
            Itotal[t] = 0;
            N[t] = 0;
            // Calculate Itotal.
            for (int a = 0; a < I[t].Count; a++)
            {
                Itotal[t] += I[t][a];
                N[t] += S[t][a] + I[t][a] + V1[t][a] + V2[t][a] + Iv1[t][a] + Iv2[t][a];
            }
            // Calculate I,S for each age group for period t.
            for (int a = 0; a < S[t].Count; a++)
            {
                double beta = Beta[t][a];
                double gamma = Gamma[t][a];
                // Population means N(a,t).
                double population = S[t][a] + I[t][a] + V1[t][a] + V2[t][a] + Iv1[t][a] + Iv2[t][a];
                //if(a == 0) Birth[t] = (int)Math.Round(population / 12, 0);

                I[t + 1][a] = (int) Math.Round(I[t][a] +
                    beta * (Itotal[t] + GetIv1Total(t) + GetIv2Total(t)) / N[t] * S[t][a] -
                    gamma * I[t][a]);

                S[t + 1][a] = (int)Math.Round(S[t][a] -
                    beta * (Itotal[t] + GetIv1Total(t) + GetIv2Total(t)) /
                    N[t] * S[t][a] + gamma * I[t][a] +
                    (S[t][a] + I[t][a]) / population * Death[t][a] +
                    W1[t][a] * V1[t][a] + W2[t][a] * V2[t][a]);

                V1[t + 1][a] = (int)Math.Round(V1[t][a] - W1[t][a] * V1[t][a] -
                    A1 * beta * (Itotal[t] + GetIv1Total(t) + GetIv2Total(t)) / N[t] * V1[t][a] +
                    A3 * gamma * Iv1[t][a]);

                V2[t + 1][a] = (int)Math.Round(V2[t][a] - W2[t][a] * V2[t][a] -
                    A2 * beta * (Itotal[t] + GetIv1Total(t) + GetIv2Total(t)) / N[t] * V2[t][a] +
                    A4 * gamma * Iv2[t][a]);

                // First year.
                if ((t + 1 - _t0) / 12 == a)
                {
                    V1[t + 1][a] += (int)Math.Round(0.625 * ef * Birth[t] * P1[t] +
                        (V1[t][a] + Iv1[t][a]) / population * Death[t][a]);

                    V2[t + 1][a] += (int)Math.Round(ef * P2[t] * Birth[t] +
                        (V2[t][a] + Iv2[t][a]) / population * Death[t][a]);

                    if (a > 0)
                    {
                        S[t + 1][a] += (int)Math.Round(
                            (1 - 0.625 * ef * P1[t - 12 * a + 1] - ef * P2[t - 12 * a + 1]) *
                            Birth[t - 12 * a + 1] * GetAlivePercent(t, t - 12 * a + 1, a - 1));
                        // S[t - 11][a - 1] / 12.0 * GetAlivePercent(t, t - 11, a - 1));
                        // If a not last.
                        if (a < S[t].Count - 1)
                        {
                            S[t + 1][a] -= (int)Math.Round(Birth[t - 12 * (a + 1) + 1] *
                                GetAlivePercent(t, t - 12 * (a + 1) + 1, a - 1));
                            //S[t + 1][a] -= (int)Math.Round(S[t - 11][a] / 12.0 * GetAlivePercent(t, t - 11, a));
                        }
                    }
                    else
                    {
                        S[t + 1][a] += (int) Math.Round(
                            (1 - 0.625 * ef * P1[t] - ef * P2[t]) * Birth[t] -
                            (1 - 0.625 * ef * P1[t - 11] - ef * P2[t - 11]) * Birth[t - 11] *
                            GetAlivePercent(t, t - 11, a));
                    }
                }
                else
                // After first year.
                if (t - 11 - _t0 >= 0 && (t + 1 - _t0) / 12 >= a + 1)
                {
                    V1[t + 1][a] += (int)Math.Round(
                        0.625 * ef * Birth[t + 1 - (a + 1) * 12] * P1[t + 1 - (a + 1) * 12] +
                        (V1[t][a] + Iv1[t][a]) / population * Death[t][a]);

                    V2[t + 1][a] += (int)Math.Round(
                        ef * P2[t + 1 - (a + 1) * 12] * Birth[t + 1 - (a + 1) * 12] +
                        (V2[t][a] + Iv2[t][a]) / population * Death[t][a]);

                    S[t + 1][a] += (int)Math.Round(-W1[t - 11][a] * V1[t - 11][a] - W2[t - 11][a] * V2[t - 11][a]);
                    if (a > 0)
                    {
                        S[t + 1][a] += (int)Math.Round(
                            (1 - 0.625 * ef * P1[t - 12 * a + 1] - ef * P2[t - 12 * a + 1]) *
                            Birth[t - 12 * a + 1] * GetAlivePercent(t, t - 12 * a + 1, a - 1));
                        // S[t - 11][a - 1] / 12.0 * GetAlivePercent(t, t - 11, a - 1));
                        // If a not last.
                        if (a < S[t].Count - 1)
                        {
                            S[t + 1][a] -= (int)Math.Round(
                                (1 - 0.625 * ef * P1[t - 12 * (a + 1) + 1] - ef * P2[t - 12 * (a + 1) + 1]) *
                                Birth[t - 12 * (a + 1) + 1] * 
                                GetAlivePercent(t, t - 12 * (a + 1) + 1, a - 1));
                            //S[t + 1][a] -= (int)Math.Round(S[t - 11][a] / 12.0 * GetAlivePercent(t, t - 11, a));
                        }
                    }
                    else
                    {
                        S[t + 1][a] += (int) Math.Round(
                            (1 - 0.625 * ef * P1[t] - ef * P2[t]) * Birth[t] -
                            (1 - 0.625 * ef * P1[t - 11] - ef * P2[t - 11]) * Birth[t - 11] * GetAlivePercent(t, t - 11, a));
                    }
                    // If a not last.
                    if (a < S[t].Count - 1)
                    {
                        V1[t + 1][a] -= (int)Math.Round(0.625 * ef * P1[t + 1 - (a + 1) * 12] * Birth[t + 1 - (a + 1) * 12] -
                            W1[t - 11][a] * V1[t - 11][a]);

                        V2[t + 1][a] -= (int)Math.Round(ef * P2[t + 1 - (a + 1) * 12] * Birth[t + 1 - (a + 1) * 12] -
                            W2[t - 11][a] * V2[t - 11][a]);
                    }
                }
                // Without vaccine
                else
                {
                    S[t + 1][a] += (int) Math.Round(
                        S[t - 11][a - 1] / 12.0 * GetAlivePercent(t, t - 11, a - 1));
                    // If a not last.
                    if (a > 0 && a < S[t].Count - 1)
                    {
                        S[t + 1][a] -= (int)Math.Round(S[t - 11][a] / 12.0 * GetAlivePercent(t, t - 11, a));
                    }
                }
                //// If a not last.
                //if (a > 0 && a < S[t].Count - 1)
                //{
                //    S[t + 1][a] -= (int)Math.Round(S[t - 11][a] / 12.0 * GetAlivePercent(t, t - 11, a));
                //}
                S[t + 1][a] = Correct(S[t + 1][a]);

                Iv1[t + 1][a] = (int) Math.Round(Iv1[t][a] +
                        A1 * beta * (Itotal[t] + GetIv1Total(t) + GetIv2Total(t)) /
                        N[t] * V1[t][a] - A3 * gamma * Iv1[t][a]);

                Iv2[t + 1][a] = (int) Math.Round(Iv2[t][a] +
                        A2 * beta * (Itotal[t] + GetIv1Total(t) + GetIv2Total(t)) /
                        N[t] * V2[t][a] - A4 * gamma * Iv2[t][a]);
                V1[t + 1][a] = Correct(V1[t + 1][a]);
                V2[t + 1][a] = Correct(V2[t + 1][a]);
            }
            
            IndividualCostV1[t] = GetIndividualCostV1(t);
            IndividualCostV2[t] = GetIndividualCostV2(t);
            GetCostSociety(t);
            if (t == Date.Count - 2)
            {
                Itotal[Itotal.Count - 1] = 0;
                for (int a = 0; a < I[Itotal.Count - 1].Count; a++)
                    Itotal[Itotal.Count - 1] += I[Itotal.Count - 1][a];
                P1[t + 1] = P1[t];
                P2[t + 1] = P2[t];
                IndividualCostV1[t + 1] = GetIndividualCostV1(t + 1);
                IndividualCostV2[t + 1] = GetIndividualCostV2(t + 1);
                GetCostSociety(t + 1);
            }
        }

        public double GetChanceOfInfection(int t, int a)
        {
            double cost = 0;
            if (t - _t0 < 0) return 0;
            for (int i = 0; i < ChanceOfInfection[t - _t0][a].Count; i++)
            {
                cost += ChanceOfInfection[t - _t0][a][i] * (i + 1);
            }
            return cost;
        }
        
        public double GetIndividualCostV1(int t)
        {
            return (1 - 0.625 * Efficient) * GetChanceOfInfection(t, 0) * CiList[t];
            //return GetChanceOfInfection() * CiList[t];
            //return (1 - A1) * Ci * InfectedCounter * Beta[t][0] * (Itotal[t] + GetIv1Total(t) + GetIv2Total(t)) / N[t] ;
        }

        public double GetIndividualCostV2(int t)
        {
            return (1 - Efficient) * GetChanceOfInfection(t, 0) * CiList[t];
            //return GetChanceOfInfection() * CiList[t];
            //return (1 - A2) * Ci * InfectedCounter * Beta[t][0] * (Itotal[t] + GetIv1Total(t) + GetIv2Total(t)) / N[t];
        }

        public double GetCompensationCostV1(int t)
        {
            //double cost = Cv1List[t] - GetChanceOfInfection(t, 0) * CiList[t];
            double cost = Cv1List[t] - IndividualCostV1[t];
            return cost < 0 ? 0 : cost;
        }

        public double GetCompensationCostV2(int t)
        {
            //double cost = Cv2List[t] - GetChanceOfInfection(t, 0) * CiList[t];
            double cost = Cv2List[t] - IndividualCostV2[t];
            return cost < 0 ? 0 : cost;
        }

        private void GetCostSociety(int t)
        {
            double s = 0;
            for (int a = 0; a < S[t].Count; a++)
            {
                s += Beta[t][a] * S[t][a] * (I[t][a] + Iv1[t][a] + Iv2[t][a]) /** GetChanceOfInfection()*/;
            }
            Cost[t] = Birth[t] * (P1[t] * Cv1List[t] + P2[t] * Cv2List[t]) + CiList[t] * s / N[t];
            //Cost[t] = P1[t] * (Cv1List[t] - 0.625 * Efficient * GetChanceOfInfection() * CiList[t]) +
            //    P2[t] * (Cv2List[t] - Efficient * GetChanceOfInfection() * CiList[t]) +
            //    GetChanceOfInfection() * CiList[t];
        }

        // Точність
        private double _eps = 0.01;
        private int _simulationCounter;
        private double CostOptimization(int t)
        {
            _simulationCounter = 0;
            double p1 = 0;
            double p2 = 0;
            double p10 = 0;
            double p1k = 1;
            double p20 = 0;
            double p2k = 1;
            double step = 0.5;
            List<OptimalValue> opts = null;
            int minIndex = 0;
            while (step / 2 > _eps)
            {
                opts = new List<OptimalValue>();
                Do1Iteration(t, p10, p1k, step, p20, p2k, step, opts);
                minIndex = opts.IndexOf(opts.Min());
                p10 = opts[minIndex].P10;
                p1k = opts[minIndex].P1k;
                p20 = opts[minIndex].P20;
                p2k = opts[minIndex].P2k;
                step /= 2;
            }
            Do1Iteration(t: t, p10: p10, p1k: p1k, step1: 0, p20: p20, p2k: p2k, step2: 0, opts: opts);
            Do1Iteration(t: t, p10: p10, p1k: p1k, step1: step, p20: p20, p2k: p2k, step2: 0, opts: opts);
            Do1Iteration(t: t, p10: p10, p1k: p1k, step1: 0, p20: p20, p2k: p2k, step2: step, opts: opts);
            minIndex = opts.IndexOf(opts.Min());
            Simulate1Period(t, opts[minIndex].P1, opts[minIndex].P2);
            return Cost[t];
        }

        private void Do1Iteration(int t, double p10, double p1k, double step1, double p20, double p2k, 
            double step2, List<OptimalValue> opts)
        {
            double p1;
            double p2;
            for (double p1i = p10; p1i < p1k; p1i += step1)
            {
                p1 = p1i + step1 / 2;
                for (double p2i = p20; p2i < p2k; p2i += step2)
                {
                    p2 = p2i + step2 / 2;
                    if(p1 + p2 > 1) continue;
                    GetCostForecast(t, step1, step2, opts, p1, p2, p1i, p2i);
                    _simulationCounter++;
                    if(step2 < _eps) break;
                }
                if (step1 < _eps) break;
            }
        }

        private void GetCostForecast(int t, double step1, double step2, List<OptimalValue> opts, double p1, 
            double p2, double p1i, double p2i)
        {
            double cost = 0;
            for (int ti = t; ti < Date.Count - 1; ti++)
            {
                // Прогноз на 4 місяця
                //if (ti - t == 12) break;
                Simulate1Period(ti, p1, p2);
                cost += Math.Abs(Cost[ti] - 2 * Birth[ti] * (P1[ti] * Cv1List[t] + P2[ti] * Cv2List[t]));
                //cost += Cost[ti];
            }

            opts.Add(new OptimalValue()
            {
                Cost = cost,
                P1 = p1,
                P10 = p1i,
                P1k = p1i + step1,
                P2 = p2,
                P20 = p2i,
                P2k = p2i + step2
            });
        }

        public void GetIndividualStrategy(ref List<int> firstStrategy, ref List<int> secondStrategy,
            ref List<int> withoutVaccination)
        {
            firstStrategy = new List<int>();
            secondStrategy = new List<int>();
            withoutVaccination = new List<int>();
            for (int t = 0; t < Date.Count; t++)
            {
                double costV1 = Cv1List[t] + (1 - 0.625 * Efficient) * GetChanceOfInfection(t, 0) * CiList[t];
                double costV2 = Cv2List[t] + (1 - Efficient) * GetChanceOfInfection(t, 0) * CiList[t];
                double costInf = GetChanceOfInfection(t, 0) * CiList[t];
                if (costV1 < costV2 && costV1 < costInf)
                    firstStrategy.Add(1);
                else firstStrategy.Add(0);
                if (costV2 < costV1 && costV2 < costInf)
                    secondStrategy.Add(1);
                else secondStrategy.Add(0);
                if (costInf < costV1 && costInf < costV2)
                    withoutVaccination.Add(1);
                else withoutVaccination.Add(0);
            }
        }

        public void GetPredictedInfections(SisAgeData sisAgeData)
        {
            PredictedInfections = new List<List<int>>();
            for (int t = 0; t < Date.Count; t++)
            {
                List<int> dInf = new List<int>();
                int diff;
                for (int a = 0; a < I[t].Count; a++)
                {
                    diff = sisAgeData.Data[a].I[t] - I[t][a];
                    dInf.Add(diff);
                }
                diff = sisAgeData.Data[0].Itotal[t] - Itotal[t];
                dInf.Add(diff);
                PredictedInfections.Add(dInf);
            }
        }
        #endregion
    }
}
