using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Epidemic.BLL.Controllers;
using Epidemic.BLL.Controllers.Clinic;
using Epidemic.BLL.Models;
using Epidemic.DAL.Entity;
using Epidemic.DAL.Repository;
using Vaccination.Controllers;

namespace Vaccination.Views
{
    public partial class SisAgeForm : Form
    {
        private SisAgeController _sisAgeController;
        private List<DataTable> _sisAgeData;
        private ClinicController _clinicController;
        private DateTime _forecastStartDate;
        private DateTime _forecastEndDate;
        private ForecastMode _forecastMode;
        private List<string> _ageGroups;

        public SisAgeForm(SisAgeController sisAgeController, DateTime forecastEndDate, ForecastMode forecastMode)
        {
            InitializeComponent();
            Inititialize();
            _forecastEndDate = forecastEndDate;
            _sisAgeController = sisAgeController;
            _forecastMode = forecastMode;
        }

        private void Inititialize()
        {
            splitContainer1.SplitterDistance = 40;
            splitContainer2.SplitterDistance = (int) (this.Width * 0.4);
        }

        private void SisAgeForm_Load(object sender, EventArgs e)
        {
            _forecastStartDate = _sisAgeController.SisAgeData.Data[0]
                .Date[_sisAgeController.SisAgeData.Data[0].Date.Count - 1].AddMonths(1);
            _sisAgeController.AddForecast(_forecastEndDate, _forecastMode);
            _sisAgeData = IncomingData.Instance.CloneSisAgeData();
            _sisAgeData.Add(_sisAgeController.AddSisTable());
            //_sisAgeController.CalculateEquilibrium();

            _ageGroups = _sisAgeData.Select(sisAgeTable => sisAgeTable.Columns[4].ColumnName).ToList();
            _ageGroups[_ageGroups.Count - 1] = "Всі";
            ageListBox.DataSource = _ageGroups;
            ageListBox.SelectedIndex = 0;
            Display(dataGridView1, _sisAgeData[0]);
            _sisAgeController.BuildCommonSisAgeInfoTables();
            _clinicController = new ClinicController(_sisAgeController.SisAgeData);
            _clinicController.BuildClinicSisAgeTables();

            saveFileDialog1.FileName = "sis.xslx";
            saveFileDialog1.Title = "Зберегти дані";
            saveFileDialog1.InitialDirectory = "звіти\\";
            saveFileDialog1.Filter = "excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
        }

        private void Display(DataGridView dataGridView, DataTable data)
        {
            dataGridView.DataSource = data;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            chartIFromB.Series[0].ChartType = SeriesChartType.Line;
            chartIFromB.Series[0].Name = "I(B)";
            chartIFromB.Series[0].Points.Clear();
            chartIFromB.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            chartIFromB.ChartAreas[0].AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;
            chartIFromB.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gainsboro;
            chartIFromB.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gainsboro;

            SisAgeData sis = _sisAgeController.SisAgeData;
            int a = ageListBox.SelectedIndex;
            int columnId = 2;
            int tDate = _sisAgeController.GetTbyDate(_forecastStartDate);

            ChartController.ChartConfig(chartS, "S", data, columnId);
            ChartController.DisplayMarker(chartS, _forecastStartDate, Convert.ToDouble(data.Rows[tDate][columnId++]));
            if (ageListBox.SelectedIndex < _ageGroups.Count - 1)
            {
                columnId++;
                for (int t = 0; t < sis.Data[a].Date.Count; t++)
                {
                    //int birth = (int)Math.Round((sis.Data[0].S[t] + sis.Data[0].I[t]) / 12.0);
                    chartIFromB.Series[0].Points.AddXY(sis.Birth[t], sis.Data[a].I[t]);
                }
            }
            else
            {
                for (int t = 0; t < sis.Data[0].Date.Count; t++)
                {
                    chartIFromB.Series[0].Points.AddXY(sis.Birth[t], sis.Data[0].Itotal[t]);
                }
            }
            chartIFromB.Series[0].Sort(PointSortOrder.Ascending, "X");

            ChartController.ChartConfig(chartI, "I", data, columnId);
            ChartController.DisplayMarker(chartI, _forecastStartDate,
                Convert.ToDouble(data.Rows[tDate][columnId++]));
            ChartController.ChartConfig(chartBeta, "beta", data, columnId);
            ChartController.DisplayMarker(chartBeta, _forecastStartDate,
                Convert.ToDouble(data.Rows[tDate][columnId++]));
            ChartController.ChartConfig(chartGamma, "gamma", data, columnId);
            ChartController.DisplayMarker(chartGamma, _forecastStartDate,
                Convert.ToDouble(data.Rows[tDate][columnId]));
        }
        
        private void ListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = ageListBox.SelectedIndex;
            Display(dataGridView1, _sisAgeData[index]);
        }

        //private void AproximateToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    var aproximateForm = new AproximateForm(_forecastMode)
        //    {
        //        EpidemicModel = GetSelectedSis(),
        //        Text = $"Апроксимація для вікової групи \"{ageListBox.SelectedItem.ToString()}\""
        //    };
        //    aproximateForm.Show();
        //}

        private SisAge GetSelectedSis()
        {
            var sisAgeData = _sisAgeController.SisAgeData;
            return sisAgeData.Data[ageListBox.SelectedIndex];
        }
        
        private void ClinicModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var clinicCounterController = new ClinicCounterController(_clinicController.Clinic);
            clinicCounterController.DefineAll();
            new ClinicSisAgeModelForm(_clinicController).Show();
        }

        private void AddVaccineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new SisvAgeForm(_sisAgeController.SisAgeData, _forecastStartDate).Show();
        }

        private void AlternativeViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new CommonSisAgeInfoForm().Show();
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "sis.xlsx";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveFileDialog1.FileName;
                new ProgressForm(new List<DataTable>{_sisAgeData[ageListBox.SelectedIndex]}, fileName).Show();
            }
        }

        private void SaveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "sis.xlsx";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveFileDialog1.FileName;
                new ProgressForm(_sisAgeData, fileName).Show();
            }
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void HelpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Path.GetFullPath("Довідка.pdf"));
        }

        private void AboutProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox1().Show();
        }

        

        //private void equilibriumToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    new EquilibriumForm(_sisAgeController.SisAgeData, _forecastStartDate).Show();
        //}
    }
}
