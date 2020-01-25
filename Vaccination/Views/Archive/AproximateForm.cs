using System;
using System.Data;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Epidemic.BLL.Controllers;
using Epidemic.BLL.Controllers.Aproximate;
using Epidemic.BLL.Models;
using Vaccination.Controllers;

namespace Vaccination.Views
{
    public partial class AproximateForm : Form
    {
        public EpidemicModel EpidemicModel { get; set; }
        private ForecastMode _forecastMode;

        public AproximateForm(ForecastMode forecastMode)
        {
            InitializeComponent();
            splitContainer1.SplitterDistance = (int)(this.Width * 0.3);
            splitContainer2.SplitterDistance = (int)(this.Width * 0.3);
            _forecastMode = forecastMode;
        }

        private void AproximateForEpidemicModel(ref DataTable table, ref DataTable table2, int firstIndex, int lastIndex)
        {
            FuriousAproximate aproximate = new FuriousAproximate("beta", EpidemicModel.Beta, firstIndex, lastIndex);
            table = aproximate.DataBeta;
            dataGridView1.DataSource = table;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            FuriousAproximate aproximate2 = new FuriousAproximate("gamma", EpidemicModel.Gamma, firstIndex, lastIndex);
            table2 = aproximate2.DataBeta;
            dataGridView2.DataSource = table2;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void AproximateForSis(Sis sis)
        {
            int lastIndex = sis.Date.IndexOf(new DateTime(2015, 1, 1));
            int firstIndex = sis.Date.IndexOf(new DateTime(sis.Date[lastIndex].Year - 1, 1, 1));
            DataTable table = null;
            DataTable table2 = null;
            // Стохастична апроксимація
            var aproximate = new StochasticAproximate(sis.Date, sis.Beta, sis.Gamma, lastIndex, ForecastMode.Realistic);
            table = aproximate.BetaTable;
            dataGridView1.DataSource = table;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            table2 = aproximate.GammaTable;
            dataGridView2.DataSource = table2;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            //AproximateForEpidemicModel(ref table, ref table2, firstIndex, lastIndex);
            ChartController.ChartConfig(chart1, "beta", sis.Date, table, 0, 0);
            ChartController.ChartConfig(chart1, table.Columns[1].ColumnName, sis.Date, table, 1, 1);
            ChartController.ChartConfig(chart2, "gamma", sis.Date, table2, 0, 0);
            ChartController.ChartConfig(chart2, table2.Columns[1].ColumnName, sis.Date, table2, 1, 1);
        }

        private void AproximateForSisAge(SisAge sisAge)
        {
            int lastIndex = sisAge.Date.IndexOf(new DateTime(2015, 1, 1));
            int firstIndex = sisAge.Date.IndexOf(new DateTime(sisAge.Date[lastIndex].Year - 1, 1, 1));
            DataTable table = null;
            DataTable table2 = null;

            // Стохастична апроксимація
            var aproximate = new StochasticAproximate(sisAge.Date, sisAge.Beta, sisAge.Gamma, lastIndex, _forecastMode);
            table = aproximate.BetaTable;
            dataGridView1.DataSource = table;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            table2 = aproximate.GammaTable;
            dataGridView2.DataSource = table2;
            dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            //AproximateForEpidemicModel(ref table, ref table2, firstIndex, lastIndex);
            ChartController.ChartConfig(chart1, "beta", sisAge.Date, table, 0, 0);
            ChartController.ChartConfig(chart1, table.Columns[1].ColumnName, sisAge.Date, table, 1, 1);
            ChartController.ChartConfig(chart2, "gamma", sisAge.Date, table2, 0, 0);
            ChartController.ChartConfig(chart2, table2.Columns[1].ColumnName, sisAge.Date, table2, 1, 1);
        }

        private void AproximateForSir(SirData sir)
        {
            int lastIndex = (int)(sir.S.Count * 0.7);
            DataTable table = null;
            DataTable table2 = null;
            AproximateForEpidemicModel(ref table, ref table2, 0, lastIndex);
            ChartController.SirChartConfig(chart1, "beta", table, 0, 0);
            ChartController.SirChartConfig(chart1, table.Columns[1].ColumnName, table, 1, 1);
            ChartController.SirChartConfig(chart2, "gamma", table2, 0, 0);
            ChartController.SirChartConfig(chart2, table2.Columns[1].ColumnName, table2, 1, 1);
            chart1.Series[0].XValueType = ChartValueType.Int32;
            chart2.Series[0].XValueType = ChartValueType.Int32;
        }

        private void AproximateForm_Load(object sender, EventArgs e)
        {
            if (EpidemicModel is Sis sis) AproximateForSis(sis);
            if (EpidemicModel is SisAge sisAge) AproximateForSisAge(sisAge);
            if (EpidemicModel is SirData sir) AproximateForSir(sir);

        }
    }
}
