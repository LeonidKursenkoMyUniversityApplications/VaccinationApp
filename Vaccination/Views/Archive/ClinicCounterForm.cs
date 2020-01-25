using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Epidemic.BLL.Controllers.Clinic;
using Epidemic.DAL.Entity;
using Epidemic.DAL.Repository;
using Vaccination.Controllers;

namespace Vaccination.Views
{
    public partial class ClinicCounterForm : Form
    {
        private DataTable _table;
        private DataTable _commonTable;

        public ClinicCounterForm()
        {
            InitializeComponent();
            _table = IncomingData.Instance.ClinicCounterTable;
            _commonTable = IncomingData.Instance.CommonClinicCounterTable;
            Display(_table, detailDataGridView);
            Display(_commonTable, commonDataGridView);
            commonDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            ChartController.ChartConfig(chart1, "Скільки разів були інфіковані", _commonTable, 1);
            chart1.Series[0].XValueType = ChartValueType.Int32;

            
            saveFileDialog1.Title = "Зберегти дані";
            saveFileDialog1.InitialDirectory = "звіти\\";
            saveFileDialog1.Filter = "excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
        }

        private void ClinicCounterForm_Load(object sender, EventArgs e)
        {

            //var repos = new EpidemicRepository();
            //repos.Write(_table, @"results\Clinic model.xlsx", 5);
        }

        private void Display(DataTable data, DataGridView dataGridView)
        {
            dataGridView.DataSource = data;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
        
        private void SaveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "Статистика по хворих.xlsx";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveFileDialog1.FileName;
                new ProgressForm(new List<DataTable> {  _commonTable }, fileName).Show();
            }
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
