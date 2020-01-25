using System;
using System.Data;
using Epidemic.BLL.Models;
using Epidemic.DAL.Entity;

namespace Epidemic.BLL.Controllers
{
    public class SirController
    {
        #region Methods

        public void DefineAll()
        {
            SirData sirData = SirData.Instance;
            IncomingData incomingData = IncomingData.Instance;
            DataTable sirTable = incomingData.SirData;
            if (sirTable.Columns.Contains("beta") == true) return;
            CreateTable();
            //ConvertTableToDaysFormat();
            ConvertDataToDaysFormat();
        }
        
        private void CreateTable()
        {
            //DataTable sirTable = new DataTable();
            IncomingData incomingData = IncomingData.Instance;
            DataTable sirTable = incomingData.SirData;
            SirData sirData = SirData.Instance;

            sirTable.Columns.Add(new DataColumn("r", typeof(double)));
            
            for(int i = 0; i < sirTable.Rows.Count; i++)
            {
                sirTable.Rows[i][sirTable.Columns.Count - 1] = sirData.R[i];
            }

            sirTable.Columns.Add(new DataColumn("beta", typeof(double)));

            for (int i = 0; i < sirTable.Rows.Count; i++)
            {
                sirTable.Rows[i][sirTable.Columns.Count - 1] = sirData.Beta[i];
            }

            sirTable.Columns.Add(new DataColumn("gamma", typeof(double)));

            for (int i = 0; i < sirTable.Rows.Count; i++)
            {
                sirTable.Rows[i][sirTable.Columns.Count - 1] = sirData.Gamma[i];
            }

            incomingData.SirData = sirTable;
        }

        private void ConvertTableToDaysFormat()
        {
            IncomingData incomingData = IncomingData.Instance;
            DataTable sirTable = incomingData.SirData;
            SirData sirData = SirData.Instance;

            // Add new column
            sirTable.Columns.Add(new DataColumn("Дата", typeof(DateTime)));
            sirTable.Columns["Дата"].SetOrdinal(0);

            for (int i = 0; i < sirTable.Rows.Count; i++)
            {
                sirTable.Rows[i]["Дата"] = sirData.Date[i];
            }

            // Deletes 'week' column
            sirTable.Columns.RemoveAt(1);

            incomingData.SirData = sirTable;
        }

        private void ConvertDataToDaysFormat()
        {
            SirData sirData = SirData.Instance;

            for (int i = sirData.S.Count - 1; i >= 0; i--)
            {
                AddDays(sirData, i);
            }

            UpdateTableDaysFormat();
        }

        private void AddDays(SirData data, int index)
        {
            int currIndex = index + 1;

            for (int i = 6; i > 0; i--)
            {
                data.S.Insert(currIndex, data.S[index]);
                data.I.Insert(currIndex, data.I[index]);
                data.R.Insert(currIndex, data.R[index]);
                data.Beta.Insert(currIndex, data.Beta[index]);
                data.Gamma.Insert(currIndex, data.Gamma[index]);
            }
        }

        private void UpdateTableDaysFormat()
        {
            IncomingData incomingData = IncomingData.Instance;
            DataTable sirTable = incomingData.SirData;
            SirData sirData = SirData.Instance;

            sirTable.Clear();
            sirTable.Columns[0].ColumnName = "День";
            for (int i = 0; i < sirData.S.Count; i++)
            {
                sirTable.Rows.Add();
                sirTable.Rows[i][0] = i + 1;
                sirTable.Rows[i][1] = sirData.S[i];
                sirTable.Rows[i][2] = sirData.I[i];
                sirTable.Rows[i][3] = sirData.R[i];
                sirTable.Rows[i][4] = sirData.Beta[i];
                sirTable.Rows[i][5] = sirData.Gamma[i];
            }
            incomingData.SirData = sirTable;
        }
        #endregion
    }
}
