using System;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Epidemic.BLL.Controllers;
using Epidemic.BLL.Models;
using Epidemic.DAL.Entity;
using Epidemic.DAL.Repository;
using Vaccination.Controllers;

namespace Vaccination.Views
{
    public partial class SirForm : Form
    {
        private AproximateForm aproximateForm;

        public SirForm()
        {
            InitializeComponent();
            splitContainer1.SplitterDistance = (int)(this.Width * 0.3);
        }

        private void SirForm_Load(object sender, EventArgs e)
        {
            SirController sirController = new SirController();
            sirController.DefineAll();

            dataGridView1.DataSource = IncomingData.Instance.SirData;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            int columnId = 1;
            ChartController.ChartConfig(chart1, "S", IncomingData.Instance.SirData, columnId++);
            ChartController.ChartConfig(chart2, "I", IncomingData.Instance.SirData, columnId++);
            ChartController.ChartConfig(chart3, "R", IncomingData.Instance.SirData, columnId++);
            ChartController.ChartConfig(chart4, "beta", IncomingData.Instance.SirData, columnId++);
            ChartController.ChartConfig(chart5, "gamma", IncomingData.Instance.SirData, columnId++);
            chart1.Series[0].XValueType = ChartValueType.Int32;
            chart2.Series[0].XValueType = ChartValueType.Int32;
            chart3.Series[0].XValueType = ChartValueType.Int32;
            chart4.Series[0].XValueType = ChartValueType.Int32;
            chart5.Series[0].XValueType = ChartValueType.Int32;

            //var repos = new EpidemicRepository();
            //repos.Write(IncomingData.Instance.SirData, @"results\SIR.xlsx", 1);
        }

        private void betaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            aproximateForm = new AproximateForm(ForecastMode.Realistic)
            {
                EpidemicModel = SirData.Instance
            };
            aproximateForm.Show();
        }
    }
}
