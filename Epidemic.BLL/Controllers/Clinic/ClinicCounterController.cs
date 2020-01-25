using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Epidemic.BLL.Models;
using Epidemic.BLL.Models.ClinicModels;
using Epidemic.DAL.Entity;

namespace Epidemic.BLL.Controllers.Clinic
{
    public class ClinicCounterController
    {
        public int Population { set; get; }
        public List<int> Persons { set; get; }
        public ClinicModel ClinicModel { set; get; }
        public List<List<List<double>>> ChanceOfInfection { set; get; }
        // t - periods, a - age groups, p - quantity
        public List<List<List<int>>> Infects { set; get; }
        //private Random _rnd;
        private int _t0;

        public ClinicCounterController(ClinicModel clinicModel, int startPeriod)
        {
            Persons = new List<int>();
            Population = 100000;
            ClinicModel = clinicModel;
            _t0 = startPeriod;
        }

        public void DefineAll()
        {
            Infects = new List<List<List<int>>>();
            ChanceOfInfection = new List<List<List<double>>>();
            for (int t = _t0; t < ClinicModel.Date.Count; t++)
            {
                Infects.Add(new List<List<int>>());
                ChanceOfInfection.Add(new List<List<double>>());
                for (int a = 0; a < ClinicModel.P1[t].Count; a++)
                {
                    Infects[t - _t0].Add(new List<int>());
                    ChanceOfInfection[t - _t0].Add(new List<double>());
                    Persons = CreatePopulation();
                    Calculate(t, a);
                    Infects[t - _t0][a] = Persons.Where(p => p > 0).ToList();
                    GetChanceOfInfection(t, a);
                }

                // add group all.
                Infects[t - _t0].Add(new List<int>());
                ChanceOfInfection[t - _t0].Add(new List<double>());
                int age = ClinicModel.P1[t].Count;
                //Calculate(t, age);
                List<int> infects = new List<int>();
                for (int a = 0; a < ClinicModel.P1[t].Count; a++)
                {
                    for (int p = 0; p < Infects[t - _t0][a].Count; p++)
                    {
                        if (p >= infects.Count) infects.Add(0);
                        infects[p] += Infects[t - _t0][a][p];
                    }
                }
                Infects[t - _t0].Add(new List<int>());
                ChanceOfInfection[t - _t0].Add(new List<double>());
                Infects[t - _t0][age] = infects;
                GetChanceOfInfection(t, age);
                for (int p = 0; p < Infects[t - _t0][age].Count; p++)
                {
                    ChanceOfInfection[t - _t0][age][p] = (double) Infects[t - _t0][age][p] / Population / age;
                }
            }
        }

        public List<int> CreatePopulation()
        {
            var persons = new List<int>();
            return persons;
        }

        public void Calculate(int t0, int a)
        {
            List<int> allInfected = new List<int>();
            for (int t = t0; t < ClinicModel.P1.Count; t++)
            {
                double p1 = ClinicModel.P1[t][a];
                int newInfected = (int)Math.Round(Population * p1, 0);
                if (allInfected.Count == 0) allInfected.Add(0);
                allInfected[0] += newInfected;
                if (allInfected.Count == 1) allInfected.Add(0);
                newInfected = (int)Math.Round(allInfected[0] * p1, 0);
                allInfected[1] += newInfected;
                if (t > 0 && ClinicModel.Date[t].Year != ClinicModel.Date[t - 1].Year &&
                    a < ClinicModel.P1[t].Count - 1) a++;
            }
            Persons = allInfected;
        }

        public DataTable CreateCommonInfoTable(int t, int a)
        {
            var table = new DataTable();

            table.Columns.Add(new DataColumn("Випадок захворювання", typeof(int)));
            table.Columns.Add(new DataColumn("Кількість хворих осіб", typeof(int)));

            if (Infects[t - _t0][a].Count == 0)
            {
                return table;
            }
            for (int i = 0; i < Infects[t - _t0][a].Count; i++)
            {
                table.Rows.Add();
                table.Rows[i][0] = i + 1;
                table.Rows[i][1] = Infects[t - _t0][a][i];
            }
            return table;
        }

        private void GetChanceOfInfection(int t, int a)
        {
            if (Infects[t - _t0][a].Count == 0)
            {
                ChanceOfInfection[t - _t0][a].Add(0);
                return;
            }
            for (int i = 0; i < Infects[t - _t0][a].Count; i++)
            {
                ChanceOfInfection[t - _t0][a].Add((double)Infects[t - _t0][a][i] / Population);
            }
        }
        
        //private void InfectRandomPersons(int count)
        //{
        //    var rndPersonIndexes = new List<int>();
        //    if (count == 0) return;
        //    while (true)
        //    {
        //        int rndIndex = _rnd.Next(0, Persons.Count); 
        //        //if (rndPersonIndexes.IndexOf(rndIndex) != -1) continue;
        //        rndPersonIndexes.Add(rndIndex);
        //        Persons[rndIndex]++;
        //        //Persons[rndIndex].CounterS++;
        //        if (rndPersonIndexes.Count >= count) break;
        //        if (rndPersonIndexes.Count == Persons.Count) break;
        //    }
        //}

    }
}
