using System;
using System.Windows.Forms;
using Epidemic.BLL.Controllers;
using Epidemic.BLL.Models;
using Epidemic.DAL.Entity;
using Epidemic.DAL.Repository;
using Vaccination.Controllers;

namespace Vaccination.Views
{
    public partial class SisForm : Form
    {
        private AproximateForm aproximateForm;
        private SisController sisController;

        public SisForm()
        {
            InitializeComponent();
            splitContainer1.SplitterDistance = (int)(this.Width * 0.3);
        }

        private void SisForm_Load(object sender, EventArgs e)
        {
            sisController = new SisController();
            sisController.DefineAll();

            dataGridView1.DataSource = IncomingData.Instance.SisData;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            int columnId = 2;
            ChartController.ChartConfig(chart1, "S", IncomingData.Instance.SisData, columnId++);
            ChartController.ChartConfig(chart2, "I", IncomingData.Instance.SisData, columnId++);
            ChartController.ChartConfig(chart4, "beta", IncomingData.Instance.SisData, columnId++);
            ChartController.ChartConfig(chart5, "gamma", IncomingData.Instance.SisData, columnId++);

            //var repos = new EpidemicRepository();
            //repos.Write(IncomingData.Instance.SisData, @"results\SIS.xlsx", 1);
        }

        

        private void betaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            aproximateForm = new AproximateForm(ForecastMode.Realistic)
            {
                EpidemicModel = SisData.Instance
            };
            aproximateForm.Show();
        }
    }
}
