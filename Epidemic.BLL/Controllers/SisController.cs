using System;
using System.Data;
using Epidemic.BLL.Models;
using Epidemic.DAL.Entity;

namespace Epidemic.BLL.Controllers
{
    public class SisController
    {
    
        #region Methods

        public void DefineAll()
        {
            IncomingData incomingData = IncomingData.Instance;
            DataTable sisTable = incomingData.SisData;
            if (sisTable.Columns.Contains("beta") == true) return;
            CreateTable();
            //ConvertDataToDaysFormat();
        }

        private void CreateTable()
        {
            IncomingData incomingData = IncomingData.Instance;
            DataTable sisTable = incomingData.SisData;
            SisData sisData = SisData.Instance;
            
            sisTable.Columns.Add(new DataColumn("beta", typeof(double)));

            for (int i = 0; i < sisTable.Rows.Count; i++)
            {
                sisTable.Rows[i][sisTable.Columns.Count - 1] = sisData.Beta[i];
            }
            
            sisTable.Columns.Add(new DataColumn("gamma", typeof(double)));

            for (int i = 0; i < sisTable.Rows.Count; i++)
            {
                sisTable.Rows[i][sisTable.Columns.Count - 1] = sisData.Gamma[i];
            }

            incomingData.SisData = sisTable;
        }

        private void ConvertDataToDaysFormat()
        {
            SisData sisData = SisData.Instance;
            for (int i = sisData.Date.Count - 1; i >= 0; i--)
            {
                AddDays(sisData, i);
            }
            UpdateTableDaysFormat();
        }

        private void AddDays(SisData data, int index)
        {
            DateTime d = data.Date[index];
            int daysCounter = DateTime.DaysInMonth(d.Year, d.Month) - 1;
            int currIndex = index + 1;

            for (int i = daysCounter; i > 0; i--)
            {
                data.Date.Insert(currIndex, data.Date[index].AddDays(i));
                data.N.Insert(currIndex, data.N[index]);
                data.S.Insert(currIndex, data.S[index]);
                data.I.Insert(currIndex, data.I[index]);
                data.Beta.Insert(currIndex, data.Beta[index]);
                data.Gamma.Insert(currIndex, data.Gamma[index]);
            }
        }

        private void UpdateTableDaysFormat()
        {
            IncomingData incomingData = IncomingData.Instance;
            DataTable sisTable = incomingData.SisData;
            SisData sisData = SisData.Instance;

            sisTable.Clear();

            for (int i = 0; i < sisData.Date.Count; i++)
            {
                sisTable.Rows.Add();
                sisTable.Rows[i][0] = sisData.Date[i];
                sisTable.Rows[i][1] = sisData.N[i];
                sisTable.Rows[i][2] = sisData.S[i];
                sisTable.Rows[i][3] = sisData.I[i];
                sisTable.Rows[i][4] = sisData.Beta[i];
                sisTable.Rows[i][5] = sisData.Gamma[i];
            }

            incomingData.SisData = sisTable;
        }
        #endregion
    }
}
