using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Epidemic.DAL.Provider;

namespace Epidemic.DAL.Repository
{
    public class EpidemicRepository : IEpidemicRepository
    {
        private readonly IProvider _provider;
        public DataTable EpidemicData { set; get; }
        public DataTable SirData { set; get; }
        public DataTable SisData { set; get; }
        public List<DataTable> SisAgeData { set; get; }
        public DataTable AgeStructureDataForSisAge { set; get; }


        public EpidemicRepository()
        {
            _provider = new EpidemicProvider();
        }

        public string[,] Read(string fileName, int pageIndex)
        {
            return _provider.Read(fileName, pageIndex);
        }

        public void Write(DataTable dataTable, string fileName, int pageIndex)
        {
            string[,] data = ConvertDataTableToArray(dataTable);
            _provider.Write(data, fileName, pageIndex);
        }

        public string[,] ConvertDataTableToArray(DataTable dataTable)
        {
            int cols = dataTable.Columns.Count;
            int rows = dataTable.Rows.Count + 1;
            string[,] data = new string[rows, cols];

            for (int i = 0; i < cols; i++)
            {
                data[0, i] = dataTable.Columns[i].ColumnName;
            }

            for (int i = 1; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    data[i, j] = dataTable.Rows[i - 1][j].ToString();
                }
            }

            return data;
        }

        public void PrepareData(string sisAgeFileName, string populationFileName)
        {
            // Reads data for SIS and SIS with ages.
            var data = Read(sisAgeFileName, 2);
            // Transforms excel data to dataTable data
            TransformData(data);
            // Loads additional data for SIS with age.
            data = Read(populationFileName, 3);
            TransformToSDataForSisAge(data);
            EditSisAgeData();
            AddGroupToSisAge();
        }

        private void TransformData(string[,] data)
        {
            EpidemicData = new DataTable();
            EpidemicData.Columns.Add(new DataColumn(data[0, 0], typeof(int)));
            EpidemicData.Columns.Add(new DataColumn(data[0, 1], typeof(string)));
            for (int i = 2; i < data.GetLength(1); i++)
                EpidemicData.Columns.Add(new DataColumn(data[0, i], typeof(int)));

            for (int i = 1; i < data.GetLength(0); i++)
            {
                EpidemicData.Rows.Add();
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    if (j == 1)
                        EpidemicData.Rows[i - 1][j] = data[i, j];
                    else
                        EpidemicData.Rows[i - 1][j] = Convert.ToInt32(data[i, j]);
                }
            }

            ConvertTableToDaysFormat();
            TransformToSisData();
            TransformToSisAgeData();
        }

        private void ConvertTableToDaysFormat()
        {
            // Add new column 'date'
            EpidemicData.Columns.Add(new DataColumn("Дата", typeof(DateTime)));
            EpidemicData.Columns["Дата"].SetOrdinal(0);

            int monthCounter = 0;
            for (int i = 0; i < EpidemicData.Rows.Count; i++)
            {
                EpidemicData.Rows[i]["Дата"] = new DateTime((int)EpidemicData.Rows[i][1], ++monthCounter, 1);
                if (monthCounter == 12) monthCounter = 0;
            }

            // Deletes 'year' and 'month' columns
            EpidemicData.Columns.RemoveAt(1);
            EpidemicData.Columns.RemoveAt(1);
        }

        private void TransformToSisData()
        {
            SisData = new DataTable();
            int colsCounter = 4;
            for (int i = 0; i < colsCounter; i++)
                SisData.Columns.Add(
                    new DataColumn(EpidemicData.Columns[i].ColumnName, EpidemicData.Columns[i].DataType));

            for (int i = 0; i < EpidemicData.Rows.Count; i++)
            {
                SisData.Rows.Add();
                for (int j = 0; j < colsCounter; j++)
                    SisData.Rows[i][j] = EpidemicData.Rows[i][j];
            }
        }

        private void TransformToSisAgeData()
        {
            SisAgeData = new List<DataTable>();
            DataTable table;
            int colsCounter = 4;
            for (int k = colsCounter; k < EpidemicData.Columns.Count; k++)
            {
                table = new DataTable();
                for (int i = 0; i < colsCounter; i++)
                {
                    table.Columns.Add(
                        new DataColumn(EpidemicData.Columns[i].ColumnName,
                            EpidemicData.Columns[i].DataType));
                }
                // For current age group
                table.Columns.Add(
                    new DataColumn(EpidemicData.Columns[k].ColumnName,
                        EpidemicData.Columns[k].DataType));

                for (int i = 0; i < EpidemicData.Rows.Count; i++)
                {
                    table.Rows.Add();
                    for (int j = 0; j < colsCounter; j++)
                        table.Rows[i][j] = EpidemicData.Rows[i][j];
                    table.Rows[i][colsCounter] = EpidemicData.Rows[i][k];
                }
                SisAgeData.Add(table);
            }
        }

        private void TransformToSDataForSisAge(string[,] data)
        {
            AgeStructureDataForSisAge = new DataTable();
            int colsCounter = data.GetLength(1);
            for (int i = 0; i < colsCounter; i++)
                AgeStructureDataForSisAge.Columns.Add(new DataColumn(data[0, i], typeof(int)));

            for (int i = 1; i < data.GetLength(0); i++)
            {
                AgeStructureDataForSisAge.Rows.Add();
                for (int j = 0; j < colsCounter; j++)
                    AgeStructureDataForSisAge.Rows[i - 1][j] = Convert.ToInt32(data[i, j]);
            }
        }

        private void EditSisAgeData()
        {
            for (int k = 0; k < SisAgeData.Count; k++)
                for (int i = 0; i < SisAgeData[k].Rows.Count; i++)
                {
                    // it`s a comment: St = Nt - It
                    SisAgeData[k].Rows[i][2] = AgeStructureDataForSisAge.AsEnumerable()
                        .First(r => r.Field<int>(0) == ((DateTime)SisAgeData[k].Rows[i][0]).Year)
                        .Field<int>(k + 1) - (int)SisAgeData[k].Rows[i][3];
                }
        }

        private void AddGroupToSisAge()
        {
            DataTable dataTable = SisAgeData[0].Copy();
            dataTable.Columns[dataTable.Columns.Count - 1].ColumnName = "Від 17 років";
            int columnS = 2;
            int columnI = 4;
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                dataTable.Rows[i][columnS] = (int)SisData.Rows[i][columnS] - (int)SisAgeData[0].Rows[i][columnS];
                dataTable.Rows[i][columnI] = (int)SisData.Rows[i][columnI - 1] - (int)SisAgeData[0].Rows[i][columnI];
            }

            SisAgeData.Add(dataTable);
        }

        private void TransformToSirData(string[,] data)
        {
            SirData = new DataTable();
            //SirData.Columns.Add(new DataColumn(data[0, 0], typeof(int)));
            //SirData.Columns.Add(new DataColumn(data[0, 1], typeof(double)));
            SirData.Columns.Add(new DataColumn("Тиждень", typeof(int)));
            SirData.Columns.Add(new DataColumn(data[0, 5], typeof(double)));
            SirData.Columns.Add(new DataColumn(data[0, 4], typeof(double)));

            for (int i = 1; i < data.GetLength(0); i++)
            {
                SirData.Rows.Add();
                SirData.Rows[i - 1][0] = Convert.ToInt32(data[i, 3]);
                SirData.Rows[i - 1][1] = Convert.ToDouble(data[i, 5]);
                SirData.Rows[i - 1][2] = Convert.ToDouble(data[i, 4]);
            }
        }
    }
}
