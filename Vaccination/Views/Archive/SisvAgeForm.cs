using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Epidemic.BLL.Controllers;
using Epidemic.BLL.Controllers.Clinic;
using Epidemic.BLL.Models;
using Epidemic.DAL.Entity;
using Vaccination.Controllers;

namespace Vaccination.Views
{
    public partial class SisvAgeForm : Form
    {
        // Vaccination efficient
        private double _ef;
        // Cost of cured
        private double _ci;
        // Cost of first vaccination
        private double _cv1;
        // Cost of second vaccination
        private double _cv2;
        // інфляція
        private double _inflation;

        private double _a1;
        private double _a2;
        private double _a3;
        private double _a4;
        private VaccinationMode _vaccinationMode;
        private double _p1;
        private double _p2;

        private ClinicController _clinicController;
        private ClinicCounterController _clinicCounterController;
        private SisvAgeController _sisvAgeController;
        private List<DataTable> _sisvAgeData;
        private SisAgeData _sisAgeData;
        private List<SisvAge> _sisvResults;
        private DateTime _forecastStartDate;
        private int _t0;
        private List<string> _ageGroups;

        public SisvAgeForm(SisAgeData sisAgeData, DateTime forecastStartDate)
        {
            InitializeComponent();
            _forecastStartDate = forecastStartDate;
            _sisAgeData = sisAgeData;
            
            _sisvResults = new List<SisvAge>();
            // example only
            _ef = 0.95;
            _ci = 5000;
            _cv1 = 80;
            _cv2 = 150;
            _inflation = 0.05;
            _a1 = 0.8;
            _a2 = 0.7;
            _a3 = 1;
            _a4 = 1;
            _vaccinationMode = VaccinationMode.Static;
            _p1 = 0.1;
            _p2 = 0.2;

            splitContainer1.SplitterDistance = 70;
            splitContainer2.SplitterDistance = 40;
            splitContainer3.SplitterDistance = (int)(this.Width * 0.5);
            splitContainer4.SplitterDistance = 40;
            splitContainer5.SplitterDistance = 40;

            analyzeListBox.Items.Add("I(B)");
            analyzeListBox.Items.Add("I(Cv1)");
            analyzeListBox.Items.Add("I(Cv2)");
            analyzeListBox.Items.Add("I(Ci)");
            analyzeListBox.Items.Add("I(Cv1/Ci)");
            analyzeListBox.Items.Add("I(Cv2/Ci)");
            analyzeListBox.Items.Add("I(p1)");
            analyzeListBox.Items.Add("I(p2)");
            analyzeListBox.Items.Add("p1(Cv1)");
            analyzeListBox.Items.Add("p1(Cv2)");
            analyzeListBox.Items.Add("p1(Ci)");
            analyzeListBox.Items.Add("p2(Cv1)");
            analyzeListBox.Items.Add("p2(Cv2)");
            analyzeListBox.Items.Add("p2(Ci)");
            analyzeListBox.SelectedIndex = 0;

            sensitiveListBox.Items.Add("I(t)");
            sensitiveListBox.Items.Add("p1(t)");
            sensitiveListBox.Items.Add("p2(t)");
            sensitiveListBox.SelectedIndex = 0;

            textBoxP1.Enabled = false;
            textBoxP2.Enabled = false;
            radioButtonStatic.Checked = true;
            //radioButtonDynamic.Checked = true;

            InitializeParameters();
        }

        private void InitializeParameters()
        {
            efTextBox.Text = _ef.ToString();
            ciTextBox.Text = _ci.ToString();
            cv1TextBox.Text = _cv1.ToString();
            cv2TextBox.Text = _cv2.ToString();
            textBoxA1.Text = _a1.ToString();
            textBoxA2.Text = _a2.ToString();
            textBoxA3.Text = _a3.ToString();
            textBoxA4.Text = _a4.ToString();
            inflationTextBox.Text = _inflation.ToString();
            textBoxP1.Text = _p1.ToString();
            textBoxP2.Text = _p2.ToString();
        }

        private void CalculateButton_Click(object sender, EventArgs e)
        {
            Calculate();
        }

        private void SisvAgeForm_Load(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "sisv.xslx";
            saveFileDialog1.Title = "Зберегти дані";
            saveFileDialog1.InitialDirectory = "звіти\\";
            saveFileDialog1.Filter = "excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            chartS.Series.Clear();
            chartI.Series.Clear();
            chartBeta.Series.Clear();
            chartGamma.Series.Clear();
            chartAnalyze.Series.Clear();
            chartIv1.Series.Clear();
            chartIv2.Series.Clear();
            chartP1.Series.Clear();
            chartP2.Series.Clear();
            chartV1.Series.Clear();
            chartV2.Series.Clear();
            sensitiveChart.Series.Clear();

            Calculate();
        }

        private bool InputData()
        {
            try
            {
                _ef = Convert.ToDouble(efTextBox.Text);
                _a1 = Convert.ToDouble(textBoxA1.Text);
                _a2 = Convert.ToDouble(textBoxA2.Text);
                _a3 = Convert.ToDouble(textBoxA3.Text);
                _a4 = Convert.ToDouble(textBoxA4.Text);
                _cv1 = Convert.ToDouble(cv1TextBox.Text);
                _cv2 = Convert.ToDouble(cv2TextBox.Text);
                _ci = Convert.ToDouble(ciTextBox.Text);
                _inflation = Convert.ToDouble(inflationTextBox.Text);
                if (checkBoxVaccMode.Checked == true)
                {
                    _p1 = Convert.ToDouble(textBoxP1.Text);
                    _p2 = Convert.ToDouble(textBoxP2.Text);
                }
            }
            catch
            {
                MessageBox.Show("Недопустимий формат даних");
                return false;
            }
            return true;
        }

        private void Calculate()
        {
            if (InputData() == false) return;

            // Calculates clinic model
            _clinicController = new ClinicController(_sisAgeData);
            _clinicCounterController = new ClinicCounterController(_clinicController.Clinic);
            _clinicCounterController.DefineAll();

            // Calculates SISV.
            _sisvAgeController = new SisvAgeController()
            {
                Efficient = _ef,
                A1 = _a1,
                A2 = _a2,
                A3 = _a3,
                A4 = _a4,
                Cv1 = _cv1,
                Cv2 = _cv2,
                Ci = _ci,
                Inflation = _inflation,
                VaccMode = _vaccinationMode,
                P1 = _p1,
                P2 = _p2
            };
            _sisvAgeController.DefineAll(_sisAgeData, _clinicCounterController.ChanceOfInfection, _forecastStartDate);
            _sisvResults.Add(_sisvAgeController.Sisv);
            _sisvAgeData = IncomingData.Instance.CloneSisvAgeData();
            _sisvAgeData.Add(_sisvAgeController.AddSisTable());

            // Recalculate clinic model.
            _clinicController = new ClinicController(_sisvAgeController.Sisv);
            _clinicCounterController = new ClinicCounterController(_clinicController.Clinic);
            _clinicCounterController.DefineAll();
            _clinicController.BuildClinicSisAgeTables();

            _ageGroups = _sisvAgeData.Select(sisvAgeTable => sisvAgeTable.Columns[4].ColumnName).ToList();
            _ageGroups[_ageGroups.Count - 1] = "Всі";
            ageListBox.DataSource = _ageGroups;
            ageListBox.SelectedIndex = 0;
            Display(dataGridView1, _sisvAgeData[0]);
            ShowOptimization();
        }

        private void AgeListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = ageListBox.SelectedIndex;
            Display(dataGridView1, _sisvAgeData[index]);
        }

        private void OptimizationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowOptimization();
        }

        private void ShowOptimization()
        {
            new OptimizationReportForm(_sisvAgeController.Sisv).Show();
        }
        
        #region Display Methods

        private void Display(DataGridView dataGridView, DataTable data)
        {
            dataGridView.DataSource = data;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            int columnId = 2;
            int tDate = _t0;
            ChartController.ChartConfig(chartS, "S", data, columnId);
            ChartController.DisplayMarker(chartS, _forecastStartDate, Convert.ToDouble(data.Rows[tDate][columnId++]));
            if (ageListBox.SelectedIndex < _ageGroups.Count - 1) columnId++;

            ChartController.ChartConfig(chartI, "I", data, columnId);
            ChartController.DisplayMarker(chartI, _forecastStartDate,
                Convert.ToDouble(data.Rows[tDate][columnId++]));
            ChartController.ChartConfig(chartBeta, "beta", data, columnId);
            ChartController.DisplayMarker(chartBeta, _forecastStartDate,
                Convert.ToDouble(data.Rows[tDate][columnId++]));
            ChartController.ChartConfig(chartGamma, "gamma", data, columnId);
            ChartController.DisplayMarker(chartGamma, _forecastStartDate,
                Convert.ToDouble(data.Rows[tDate][columnId++]));
            ChartController.ChartConfig(chartP1, "p1", data, columnId);
            ChartController.DisplayMarker(chartP1, _forecastStartDate,
                Convert.ToDouble(data.Rows[tDate][columnId++]));
            ChartController.ChartConfig(chartP2, "p2", data, columnId);
            ChartController.DisplayMarker(chartP2, _forecastStartDate,
                Convert.ToDouble(data.Rows[tDate][columnId++]));
            ChartController.ChartConfig(chartV1, "V1", data, columnId);
            ChartController.DisplayMarker(chartV1, _forecastStartDate,
                Convert.ToDouble(data.Rows[tDate][columnId++]));
            ChartController.ChartConfig(chartV2, "V2", data, columnId);
            ChartController.DisplayMarker(chartV2, _forecastStartDate,
                Convert.ToDouble(data.Rows[tDate][columnId++]));
            ChartController.ChartConfig(chartIv1, "Iv1", data, columnId);
            ChartController.DisplayMarker(chartIv1, _forecastStartDate,
                Convert.ToDouble(data.Rows[tDate][columnId++]));
            ChartController.ChartConfig(chartIv2, "Iv2", data, columnId);
            ChartController.DisplayMarker(chartIv2, _forecastStartDate,
                Convert.ToDouble(data.Rows[tDate][columnId++]));
            Analyze();
            Sensitive();
        }

        private void Sensitive()
        {
            if (_sisvAgeController == null) return;
            int a = ageListBox.SelectedIndex;
            SisvAge sisv = _sisvAgeController.Sisv;
            sensitiveChart.Series.Clear();
            sensitiveChart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
            sensitiveChart.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            sensitiveChart.ChartAreas[0].AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;
            sensitiveChart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gainsboro;
            sensitiveChart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gainsboro;
            
            switch (sensitiveListBox.SelectedIndex)
            {
                case 0: DisplayInfectedFromT(a); break;
                case 1: DisplayP1FromT(); break;
                case 2: DisplayP2FromT(); break;
            }
        }

        private void DisplayInfectedFromT(int a)
        {
            for (int k = 0; k < _sisvResults.Count; k++)
            {
                SisvAge sisv = _sisvResults[k];
                ConfigSensitiveChart(k, sisv);
                for (int t = 0; t < sisv.Date.Count; t++)
                {
                    if(a < _ageGroups.Count - 1)
                        sensitiveChart.Series[k].Points.AddXY(sisv.Date[t], sisv.I[t][a]);
                    else sensitiveChart.Series[k].Points.AddXY(sisv.Date[t], sisv.Itotal[t]);
                }
            }
        }

        private void DisplayP1FromT()
        {
            for (int k = 0; k < _sisvResults.Count; k++)
            {
                SisvAge sisv = _sisvResults[k];
                ConfigSensitiveChart(k, sisv);
                for (int t = 0; t < sisv.Date.Count; t++)
                {
                    sensitiveChart.Series[k].Points.AddXY(sisv.Date[t], sisv.P1[t]);
                }
            }
        }

        private void DisplayP2FromT()
        {
            for (int k = 0; k < _sisvResults.Count; k++)
            {
                SisvAge sisv = _sisvResults[k];
                ConfigSensitiveChart(k, sisv);
                for (int t = 0; t < sisv.Date.Count; t++)
                {
                    sensitiveChart.Series[k].Points.AddXY(sisv.Date[t], sisv.P2[t]);
                }
            }
        }

        private void ConfigSensitiveChart(int k, SisvAge sisv)
        {
            string name = analyzeListBox.SelectedItem +
                          $" при Cv1={sisv.Cv1}; Cv2={sisv.Cv2}; Ci={sisv.Ci}";
            while (true)
            {
                if (sensitiveChart.Series.FindByName(name) == null) break;
                name += "*";
            }
            sensitiveChart.Series.Add(new Series());
            sensitiveChart.Series[k].XValueType = ChartValueType.DateTime;
            sensitiveChart.Series[k].ChartType = SeriesChartType.Line;
            sensitiveChart.Series[k].Name = name;
        }

        private void Analyze()
        {
            if (_sisvAgeController == null) return;
            int a = ageListBox.SelectedIndex;
            SisvAge sisv = _sisvAgeController.Sisv;
            _t0 = sisv.Date.IndexOf(_forecastStartDate);
            if(chartAnalyze.Series.Count == 0) chartAnalyze.Series.Add(analyzeListBox.SelectedItem.ToString());
            chartAnalyze.Series[0].Name = analyzeListBox.SelectedItem.ToString();
            chartAnalyze.Series[0].Points.Clear();
            chartAnalyze.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            chartAnalyze.ChartAreas[0].AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;
            chartAnalyze.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gainsboro;
            chartAnalyze.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gainsboro;
            chartAnalyze.Series[0].ChartType = SeriesChartType.Line;
            switch (analyzeListBox.SelectedIndex)
            {
                case 0: DisplayInfectedFromBirth(sisv, a); break;
                case 1: DisplayInfectedFromCv1(sisv, a); break;
                case 2: DisplayInfectedFromCv2(sisv, a); break;
                case 3: DisplayInfectedFromCi(sisv, a); break;
                case 4: DisplayInfectedFromCv1Ci(sisv, a); break;
                case 5: DisplayInfectedFromCv2Ci(sisv, a); break;
                case 6: DisplayInfectedFromP1(sisv, a); break;
                case 7: DisplayInfectedFromP2(sisv, a); break;
                case 8: DisplayCv1FromP1(sisv); break;
                case 9: DisplayCv2FromP1(sisv); break;
                case 10: DisplayCiFromP1(sisv); break;
                case 11: DisplayCv1FromP2(sisv); break;
                case 12: DisplayCv2FromP2(sisv); break;
                case 13: DisplayCiFromP2(sisv); break;
            }
            chartAnalyze.Series[0].Sort(PointSortOrder.Ascending, "X");
        }
        
        private void DisplayCiFromP2(SisvAge sisv)
        {
            for (int t = _t0; t < sisv.Date.Count; t++)
            {
                chartAnalyze.Series[0].Points.AddXY(Math.Round(sisv.CiList[t], 2), sisv.P2[t]);
            }
        }

        private void DisplayCv2FromP2(SisvAge sisv)
        {
            for (int t = _t0; t < sisv.Date.Count; t++)
            {
                chartAnalyze.Series[0].Points.AddXY(Math.Round(sisv.Cv2List[t], 2), sisv.P2[t]);
            }
        }

        private void DisplayCv1FromP2(SisvAge sisv)
        {
            for (int t = _t0; t < sisv.Date.Count; t++)
            {
                chartAnalyze.Series[0].Points.AddXY(Math.Round(sisv.Cv1List[t], 2), sisv.P2[t]);
            }
        }

        private void DisplayCiFromP1(SisvAge sisv)
        {
            for (int t = _t0; t < sisv.Date.Count; t++)
            {
                chartAnalyze.Series[0].Points.AddXY(Math.Round(sisv.CiList[t], 2), sisv.P1[t]);
            }
        }

        private void DisplayCv2FromP1(SisvAge sisv)
        {
            for (int t = _t0; t < sisv.Date.Count; t++)
            {
                chartAnalyze.Series[0].Points.AddXY(Math.Round(sisv.Cv2List[t], 2), sisv.P1[t]);
            }
        }

        private void DisplayCv1FromP1(SisvAge sisv)
        {
            for (int t = _t0; t < sisv.Date.Count; t++)
            {
                chartAnalyze.Series[0].Points.AddXY(Math.Round(sisv.Cv1List[t], 2), sisv.P1[t]);
            }
        }

        private void DisplayInfectedFromP2(SisvAge sisv, int a)
        {
            //chartAnalyze.Series[0].ChartType = SeriesChartType.Point;
            for (int t = _t0; t < sisv.Date.Count; t++)
            {
                if (a < _ageGroups.Count - 1)
                    chartAnalyze.Series[0].Points.AddXY(Math.Round(sisv.P2[t], 2), sisv.I[t][a]);
                else chartAnalyze.Series[0].Points.AddXY(Math.Round(sisv.P2[t], 2), sisv.Itotal[t]);
            }
        }

        private void DisplayInfectedFromP1(SisvAge sisv, int a)
        {
            //chartAnalyze.Series[0].ChartType = SeriesChartType.Point;
            for (int t = _t0; t < sisv.Date.Count; t++)
            {
                if (a < _ageGroups.Count - 1)
                    chartAnalyze.Series[0].Points.AddXY(Math.Round(sisv.P1[t], 2), sisv.I[t][a]);
                else chartAnalyze.Series[0].Points.AddXY(Math.Round(sisv.P1[t], 2), sisv.Itotal[t]);
            }
        }

        private void DisplayInfectedFromCv2Ci(SisvAge sisv, int a)
        {
            for (int t = _t0; t < sisv.Date.Count; t++)
            {
                if (a < _ageGroups.Count - 1)
                    chartAnalyze.Series[0].Points.AddXY(Math.Round(sisv.Cv2List[t] / sisv.CiList[t], 2), sisv.I[t][a]);
                else chartAnalyze.Series[0].Points.AddXY(Math.Round(sisv.Cv2List[t] / sisv.CiList[t], 2), sisv.Itotal[t]);
            }
        }

        private void DisplayInfectedFromCv1Ci(SisvAge sisv, int a)
        {
            for (int t = _t0; t < sisv.Date.Count; t++)
            {
                if (a < _ageGroups.Count - 1)
                    chartAnalyze.Series[0].Points.AddXY(Math.Round(sisv.Cv1List[t] / sisv.CiList[t], 2), sisv.I[t][a]);
                else chartAnalyze.Series[0].Points.AddXY(Math.Round(sisv.Cv1List[t] / sisv.CiList[t], 2), sisv.Itotal[t]);
            }
        }

        private void DisplayInfectedFromCi(SisvAge sisv, int a)
        {
            for (int t = _t0; t < sisv.Date.Count; t++)
            {
                if (a < _ageGroups.Count - 1)
                    chartAnalyze.Series[0].Points.AddXY(Math.Round(sisv.CiList[t], 2), sisv.I[t][a]);
                else chartAnalyze.Series[0].Points.AddXY(Math.Round(sisv.CiList[t], 2), sisv.Itotal[t]);
            }
        }

        private void DisplayInfectedFromCv2(SisvAge sisv, int a)
        {
            for (int t = _t0; t < sisv.Date.Count; t++)
            {
                if (a < _ageGroups.Count - 1)
                    chartAnalyze.Series[0].Points.AddXY(Math.Round(sisv.Cv2List[t], 2), sisv.I[t][a]);
                else chartAnalyze.Series[0].Points.AddXY(Math.Round(sisv.Cv2List[t], 2), sisv.Itotal[t]);
            }
        }

        private void DisplayInfectedFromCv1(SisvAge sisv, int a)
        {
            for (int t = _t0; t < sisv.Date.Count; t++)
            {
                if (a < _ageGroups.Count - 1)
                    chartAnalyze.Series[0].Points.AddXY(Math.Round(sisv.Cv1List[t], 2), sisv.I[t][a]);
                else chartAnalyze.Series[0].Points.AddXY(Math.Round(sisv.Cv1List[t], 2), sisv.Itotal[t]);
            }
        }

        private void DisplayInfectedFromBirth(SisvAge sisv, int a)
        {
            //chartAnalyze.Series[0].ChartType = SeriesChartType.Point;
            for (int t = 0; t < sisv.Date.Count; t++)
            {
                if (a < _ageGroups.Count - 1)
                    chartAnalyze.Series[0].Points.AddXY(sisv.Birth[t], sisv.I[t][a]);
                else chartAnalyze.Series[0].Points.AddXY(sisv.Birth[t], sisv.Itotal[t]);
            }
        }

        #endregion

        private void AnalyzeListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Analyze();
        }

        private void SensitiveListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Sensitive();
        }

        private void ClinicModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new ClinicSisAgeModelForm(_clinicController).Show();
        }

        private void CheckBoxVaccMode_CheckedChanged(object sender, EventArgs e)
        {
            textBoxP1.Enabled = checkBoxVaccMode.Checked;
            textBoxP2.Enabled = checkBoxVaccMode.Checked;
            if (checkBoxVaccMode.Checked == true)
            {
                _vaccinationMode = VaccinationMode.Custom;
                groupBoxOptType.Enabled = false;
            }
            else
            {
                groupBoxOptType.Enabled = true;
                if (radioButtonStatic.Checked == true)
                    _vaccinationMode = VaccinationMode.Static;
                else _vaccinationMode = VaccinationMode.Dynamic;
            }
        }

        private void ToolStripMenuItemClear_Click(object sender, EventArgs e)
        {
            _sisvResults.Clear();
            sensitiveChart.Series.Clear();
        }

        private void RadioButtonStatic_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonStatic.Checked == true)
                _vaccinationMode = VaccinationMode.Static;
            else _vaccinationMode = VaccinationMode.Dynamic;
        }

        private void RadioButtonDynamic_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonDynamic.Checked == false)
                _vaccinationMode = VaccinationMode.Static;
            else _vaccinationMode = VaccinationMode.Dynamic;
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "sisv.xlsx";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveFileDialog1.FileName;
                new ProgressForm(new List<DataTable> { _sisvAgeData[ageListBox.SelectedIndex] }, fileName).Show();
            }
        }

        private void SaveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "sisv.xlsx";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveFileDialog1.FileName;
                new ProgressForm(_sisvAgeData, fileName).Show();
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

       
    }
}
