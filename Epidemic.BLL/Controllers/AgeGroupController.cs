using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Epidemic.BLL.Controllers.Aproximate;
using Epidemic.BLL.Models;
using Epidemic.DAL.Entity;

namespace Epidemic.BLL.Controllers
{
    public class AgeGroupController
    {
        private int infectedId = 4;
        private int susceptibleId = 2;
        private int allPopulationId = 1;

        private double[][] GetValuesOfAgeGroups(List<DataTable> sisAgeTables, int colIndex)
        {
            double[][] valuesOfAgeGroups = new double[sisAgeTables[0].Rows.Count][];
            for (int i = 0; i < valuesOfAgeGroups.Length; i++)
            {
                valuesOfAgeGroups[i] = new double[sisAgeTables.Count - 2];
                for (int j = 1; j < sisAgeTables.Count - 1; j++)
                    valuesOfAgeGroups[i][j - 1] = (int) sisAgeTables[j].Rows[i][colIndex];
            }
            return valuesOfAgeGroups;
        }

        private double[][] TransformToDistributionValues(double[][] values)
        {
            for (int i = 0; i < values.Length; i++)
                for (int j = 1; j < values[i].Length; j++)
                    values[i][j] += values[i][j - 1];
            return values;
        }

        private double[] ageGroups = {1, 4, 9, 14, 17};
        private int[][] GetNewValues<T> (List<DataTable> sisAgeTables, int colIndex, 
            List<AgeGroup> newAgeGroups) where T : IFuncAproximate, new()
        {
            var groups = GetValuesOfAgeGroups(sisAgeTables, colIndex);
            groups = TransformToDistributionValues(groups);
            var tFuncAproximate = new T();
            List<AproximateFunction> tFunctions = 
                tFuncAproximate.GetFunctions(groups, ageGroups);
            int[][] newGroups = new int[groups.Length][];
            for (int i = 0; i < newGroups.Length; i++)
            {
                newGroups[i] = new int[newAgeGroups.Count];
                for (int j = 0; j < newGroups[0].Length; j++)
                {
                    double result = tFuncAproximate.GetResult(tFunctions[i], newAgeGroups[j].MaxAge);
                    newGroups[i][j] = (int) Math.Round(result, 0);
                }
                for (int k = newGroups[0].Length - 1; k >= 1; k--)
                    newGroups[i][k] -= newGroups[i][k - 1];
                newGroups[i][0] = (int)groups[i][0];
            }
            return newGroups;
        }

        public List<DataTable> GetNewSisAgeTables(List<DataTable> sisAgeTables, List<AgeGroup> newAgeGroups)
        {
            var newInfectedForSisAge = GetNewValues<HyperbolicFuncAproximate>(sisAgeTables, 
                infectedId, newAgeGroups);
            var newSusceptibleForSisAge = GetNewValues<LineFuncAproximate>(sisAgeTables, 
                susceptibleId, newAgeGroups);
            List<DataTable> newSisAgeTables = new List<DataTable>();
            var table = sisAgeTables[0].Copy();
            for (int i = 0; i < newAgeGroups.Count; i++)
            {
                var sisTable = table.Copy();
                if (i < newAgeGroups.Count - 1)
                {
                    sisTable.Columns[infectedId].ColumnName =
                        $"Від {newAgeGroups[i].MinAge} до {newAgeGroups[i].MaxAge} років";
                    sisTable.Columns[susceptibleId].ColumnName =
                        $"Сприйнятливі S {newAgeGroups[i].MinAge}-{newAgeGroups[i].MaxAge} років";
                }
                else
                {
                    sisTable.Columns[infectedId].ColumnName =
                        $"Від {newAgeGroups[i].MinAge} і більше років";
                    sisTable.Columns[susceptibleId].ColumnName =
                        $"Сприйнятливі S  {newAgeGroups[i].MinAge} і більше років";
                }
                for (int j = 0; j < sisTable.Rows.Count; j++)
                {
                    sisTable.Rows[j][infectedId] = newInfectedForSisAge[j][i];
                    sisTable.Rows[j][susceptibleId] = newSusceptibleForSisAge[j][i];
                }
                // correct last Table
                if (i == newAgeGroups.Count - 1)
                {
                    for (int j = 0; j < sisTable.Rows.Count; j++)
                    {
                        int sum = 0;
                        for (int k = 0; k < newAgeGroups.Count - 1; k++)
                        {
                            sum += newSusceptibleForSisAge[j][k];
                        }
                        sisTable.Rows[j][susceptibleId] = (int) sisTable.Rows[j][allPopulationId] - sum;
                    }
                }
                newSisAgeTables.Add(sisTable);
            }
            return newSisAgeTables;
        }

        public List<DataTable> GetNewSisAgeTables(List<DataTable> sisAgeTables)
        {
            List<AgeGroup> ageGroups = new List<AgeGroup>()
            {
                new AgeGroup(){MinAge = 0, MaxAge = 1},
                new AgeGroup(){MinAge = 1, MaxAge = 2},
                new AgeGroup(){MinAge = 2, MaxAge = 3},
                new AgeGroup(){MinAge = 3, MaxAge = 4},
                new AgeGroup(){MinAge = 4, MaxAge = 5},
                new AgeGroup(){MinAge = 5, MaxAge = 6},
                new AgeGroup(){MinAge = 6, MaxAge = 7},
                new AgeGroup(){MinAge = 7, MaxAge = 8},
                new AgeGroup(){MinAge = 8, MaxAge = 9},
                new AgeGroup(){MinAge = 9, MaxAge = 10},
                new AgeGroup(){MinAge = 10, MaxAge = 11},
                new AgeGroup(){MinAge = 11, MaxAge = 12},
                new AgeGroup(){MinAge = 12, MaxAge = 13},
                new AgeGroup(){MinAge = 13, MaxAge = 14},
                new AgeGroup(){MinAge = 14, MaxAge = 15},
                new AgeGroup(){MinAge = 15, MaxAge = 16},
                new AgeGroup(){MinAge = 16, MaxAge = 17},
                new AgeGroup(){MinAge = 17, MaxAge = 18},
                new AgeGroup(){MinAge = 18, MaxAge = 120}
            };
            SaveAgeGroup(ageGroups);
            var newSisAgeTables = GetNewSisAgeTables(sisAgeTables, ageGroups);
            return newSisAgeTables;
        }

        private void SaveAgeGroup(List<AgeGroup> ageGroups)
        {
            List<string> list = new List<string>();
            foreach (var ageGroup in ageGroups)
            {
                list.Add($"{ageGroup.MinAge}-{ageGroup.MaxAge}");
            }
            list[list.Count - 1] = $"{ageGroups[list.Count - 1].MinAge} і більше";
            IncomingData.Instance.AgeGroups = list;
        }
    }
}
