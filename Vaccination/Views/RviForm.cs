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
using Epidemic.BLL.Controllers.Input;
using Epidemic.BLL.Models;
using Epidemic.DAL.Entity;
using Vaccination.Controllers;

namespace Vaccination.Views
{
    public partial class RviForm : Form
    {
        #region Fields

        private SisAgeController _sisAgeController;
        private ClinicController _clinicController;
        private ClinicCounterController _clinicCounterController;
        private ClinicCounterController _clinicCounterSisvController;
        private SisvAgeController _sisvAgeController;
        private List<DataTable> _clinicTables;
        private List<DataTable> _clinicSisvTables;
        // Clinic counter tables.
        //private DataTable _detailClinicCounterTable;
        private DataTable _commonClinicCounterTable;
        //private DataTable _detailClinicSisvCounterTable;
        private DataTable _commonClinicSisvCounterTable;
        private List<DataTable> _sisAgeData;
        private List<DataTable> _sisvAgeData;
        private List<SisAgeData> _sisResults;
        private List<SisvAge> _sisvResults;
        private List<ClinicModel> _sisClinicResults;
        // k - with different params, t periods, a - age groups.
        private List<List<List<DataTable>>> _sisClinicCounterResults;
        private List<ClinicModel> _sisvClinicResults;
        private List<List<List<DataTable>>> _sisvClinicCounterResults;
        private DateTime _forecastStartDate;
        private DateTime _forecastEndDate;
        // forecast start index
        private int _t0;
        private DateTime _retrospectiveStartDate;
        private BirthMode _birthMode;
        private DeathMode _deathMode;
        private double _birthPercent;
        private double _deathPercent;
        private int _birth;
        private int _death;
        private ForecastMode _forecastMode;
        private List<string> _ageGroups;

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

        private SisvAge _sisv;
        private DataTable _dtOpt;

        private List<int> _firstStrategy = null;
        private List<int> _secondStrategy = null;
        private List<int> _withoutVaccination = null;
        #endregion

        public RviForm()
        {
            InitializeComponent();

            splitContainerSis1.SplitterDistance = 65;
            splitContainerSis2.SplitterDistance = 85;
            splitContainerSis3.SplitterDistance = (int)(this.Width * 0.4);
            splitContainerClinic1.SplitterDistance = 115;
            splitContainerClinic2.SplitterDistance = 65;
            splitContainerClinicCounterCommon.SplitterDistance = (int)(this.Width * 0.15);
            splitContainerClinicCounterCommon2.SplitterDistance = (int)(splitContainerClinicCounterCommon2.Height * 0.15);
            splitContainerClinicCounterCommon3.SplitterDistance = (int)(splitContainerClinicCounterCommon3.Width * 0.6);
            splitContainerSisv1.SplitterDistance = 75;
            splitContainerSisv2.SplitterDistance = 85;
            splitContainerSisv3.SplitterDistance = (int)(this.Width * 0.52);
            splitContainerSisv4.SplitterDistance = 50;
            splitContainerSisv5.SplitterDistance = 80;
            splitContainerSisvClinic1.SplitterDistance = 115;
            splitContainerSisvClinic3.SplitterDistance = (int)(this.Width * 0.15);
            splitContainerSisvClinicCounterCommon2.SplitterDistance = (int)(splitContainerSisvClinicCounterCommon2.Height * 0.15);
            splitContainerSisvClinicCounterCommon3.SplitterDistance = (int)(splitContainerSisvClinicCounterCommon3.Width * 0.6);
        }

        private void RviForm_Load(object sender, EventArgs e)
        {
            ForecastInitialize();
            SisInitialize();

            saveFileDialog1.FileName = "sis.xslx";
            saveFileDialog1.Title = "Зберегти дані";
            saveFileDialog1.InitialDirectory = "звіти\\";
            saveFileDialog1.Filter = "excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            saveFileDialog2.FileName = "sis.png";
            saveFileDialog2.Title = "Зберегти дані";
            saveFileDialog2.InitialDirectory = "звіти\\";
            saveFileDialog2.Filter = "PNG files (*.png)|*.png";
            saveFileDialog2.FilterIndex = 2;
            saveFileDialog2.RestoreDirectory = true;

            FirstInitializeSisv();
            SetTabsText();
        }

        #region Main menu methods

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControlEpidemic.SelectedIndex == 0)
            {
                saveFileDialog1.FileName = "sis.xlsx";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string fileName = saveFileDialog1.FileName;
                    new ProgressForm(new List<DataTable> { _sisAgeData[ageListBox.SelectedIndex] }, fileName).Show();
                }
            }
            if (tabControlEpidemic.SelectedIndex == 1)
            {
                if (tabControlClinic1.SelectedIndex == 0)
                {
                    saveFileDialog1.FileName = "Клінічна модель.xlsx";
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string fileName = saveFileDialog1.FileName;
                        new ProgressForm(new List<DataTable> { _clinicTables[dataListBox.SelectedIndex] }, fileName).Show();
                    }
                }
                else
                {
                    saveFileDialog1.FileName = "Статистика по хворих.xlsx";
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string fileName = saveFileDialog1.FileName;
                        var commonTable = _commonClinicCounterTable;
                        new ProgressForm(new List<DataTable> { commonTable }, fileName).Show();
                    }
                }
            }
            if (tabControlEpidemic.SelectedIndex == 2)
            {
                saveFileDialog1.FileName = "sisv.xlsx";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string fileName = saveFileDialog1.FileName;
                    new ProgressForm(new List<DataTable> { _sisvAgeData[ageListBox2.SelectedIndex] }, fileName).Show();
                }
            }
            if (tabControlEpidemic.SelectedIndex == 3)
            {
                if (tabControlSisvClinic1.SelectedIndex == 0)
                {
                    saveFileDialog1.FileName = "Клінічна модель.xlsx";
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string fileName = saveFileDialog1.FileName;
                        new ProgressForm(new List<DataTable> { _clinicSisvTables[dataSisvListBox.SelectedIndex] }, fileName).Show();
                    }
                }
                else
                {
                    saveFileDialog1.FileName = "Статистика по хворих.xlsx";
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string fileName = saveFileDialog1.FileName;
                        var commonTable = _commonClinicSisvCounterTable; // IncomingData.Instance.CommonClinicCounterTable;
                        new ProgressForm(new List<DataTable> { commonTable }, fileName).Show();
                    }
                }
            }
            SaveOptimizationCost();
        }

        private void SaveOptimizationCost()
        {
            if (tabControlEpidemic.SelectedIndex == 4)
            {
                saveFileDialog1.FileName = "Звіт по витратах на вакцинацію.xlsx";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string fileName = saveFileDialog1.FileName;
                    DataTable dt1 = (DataTable)dataGridViewOpt1.DataSource;
                    DataTable dt2 = (DataTable)dataGridViewIndStrategy.DataSource;
                    new ProgressForm(new List<DataTable> { dt1, dt2 }, fileName).Show();
                }
            }
        }

        private void SaveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControlEpidemic.SelectedIndex == 0)
            {
                saveFileDialog1.FileName = "sis.xlsx";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string fileName = saveFileDialog1.FileName;
                    new ProgressForm(_sisAgeData, fileName).Show();
                }
            }
            if (tabControlEpidemic.SelectedIndex == 1)
            {
                if (tabControlClinic1.SelectedIndex == 0)
                {
                    saveFileDialog1.FileName = "Клінічна модель.xlsx";
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string fileName = saveFileDialog1.FileName;
                        new ProgressForm(_clinicTables, fileName).Show();
                    }
                }
                else
                {
                    saveFileDialog1.FileName = "Статистика по хворих.xlsx";
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string fileName = saveFileDialog1.FileName;
                        var commonTable = _commonClinicCounterTable;
                        new ProgressForm(new List<DataTable> { commonTable }, fileName).Show();
                    }
                }
            }
            if (tabControlEpidemic.SelectedIndex == 2)
            {
                saveFileDialog1.FileName = "sisv.xlsx";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string fileName = saveFileDialog1.FileName;
                    new ProgressForm(_sisvAgeData, fileName).Show();
                }
            }
            if (tabControlEpidemic.SelectedIndex == 3)
            {
                if (tabControlSisvClinic1.SelectedIndex == 0)
                {
                    saveFileDialog1.FileName = "Клінічна модель.xlsx";
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string fileName = saveFileDialog1.FileName;
                        new ProgressForm(_clinicSisvTables, fileName).Show();
                    }
                }
                else
                {
                    saveFileDialog1.FileName = "Статистика по хворих.xlsx";
                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        string fileName = saveFileDialog1.FileName;
                        var commonTable = _commonClinicSisvCounterTable; // IncomingData.Instance.CommonClinicCounterTable;
                        new ProgressForm(new List<DataTable> { commonTable }, fileName).Show();
                    }
                }
            }

            SaveOptimizationCost();
        }

        private void ToolStripMenuItemSave_Click(object sender, EventArgs e)
        {
            SaveImage(sender);
        }

        private void SaveImage(object sender)
        {
            saveFileDialog2.FileName = "Sis.png";
            if (saveFileDialog2.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveFileDialog2.FileName;
                ToolStripMenuItem menuItem = sender as ToolStripMenuItem;
                if (menuItem == null) return;
                ContextMenuStrip menu = menuItem.Owner as ContextMenuStrip;
                if (menu == null) return;
                Chart chart = menu.SourceControl as Chart;
                if (chart == null) return;
                chart.SaveImage(fileName, ChartImageFormat.Png);
            }
        }

        private void ToolStripMenuSave_Click(object sender, EventArgs e)
        {
            SaveImage(sender);
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void AlternativeViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new CommonSisAgeInfoForm().Show();
        }

        private void HelpToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(Path.GetFullPath("Довідка.pdf"));
        }

        private void AboutProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new AboutBox1().Show();
        }
        
        #endregion
        
        #region TabControl methods

        private void TabControlEpidemic_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void PagesHide(bool hide)
        {
            if (hide == true)
            {
                tabControlEpidemic.TabPages.Remove(tabPageEpidemic2);
                tabControlEpidemic.TabPages.Remove(tabPageEpidemic3);
                tabControlEpidemic.TabPages.Remove(tabPageEpidemic4);
                tabControlEpidemic.TabPages.Remove(tabPageEpidemic5);
                tabControlSis.TabPages.Remove(tabPageSisSensitive);
            }
            else
            {
                if(tabControlEpidemic.TabPages.Contains(tabPageEpidemic2)) return;
                tabControlEpidemic.TabPages.Insert(1, tabPageEpidemic2);
                tabControlEpidemic.TabPages.Insert(2, tabPageEpidemic3);
                tabControlEpidemic.TabPages.Insert(3, tabPageEpidemic4);
                tabControlEpidemic.TabPages.Insert(4, tabPageEpidemic5);
                tabControlSis.TabPages.Insert(tabControlSis.TabPages.Count, tabPageSisSensitive);
            }
        }

        private void RadioButtonConstBirth_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonConstBirth.Checked == true)
            {
                textBoxBirth.Enabled = true;
                textBoxBirthPercent.Enabled = false;
            }
        }

        private void RadioButtonDynamicBirth_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonDynamicBirth.Checked == true)
            {
                textBoxBirth.Enabled = false;
                textBoxBirthPercent.Enabled = true;
            }
        }

        private void RadioButtonDeathConst_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonDeathConst.Checked == true)
            {
                textBoxDeath.Enabled = true;
                textBoxDeathPercent.Enabled = false;
            }
        }

        private void RadioButtonDeathDynamic_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonDeathDynamic.Checked == true)
            {
                textBoxDeath.Enabled = false;
                textBoxDeathPercent.Enabled = true;
            }
        }

        private void SetTabsText()
        {
            tabPageSisSus.Text = Titles.Sus;
            tabPageSisInf.Text = Titles.Inf;
            tabPageSisBeta.Text = Titles.Beta;
            tabPageSisGamma.Text = Titles.Gamma;
            tabPageSisInfFromGamma.Text = Titles.InfFromBirth;

            tabPageSisvSus.Text = Titles.Sus;
            tabPageSisvInf.Text = Titles.Inf;
            tabPageSisvBeta.Text = Titles.Beta;
            tabPageSisvGamma.Text = Titles.Gamma;
            tabPageSisvP1.Text = Titles.P1;
            tabPageSisvP2.Text = Titles.P2;
            tabPageSisvV1.Text = Titles.V1;
            tabPageSisvV2.Text = Titles.V2;
            tabPageSisvIv1.Text = Titles.Iv1;
            tabPageSisvIv2.Text = Titles.Iv2;
        }

        #endregion

        #region Other form methods

        private void RviForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.OpenForms["MainForm"]?.Show();
        }

        #endregion

        #region Sis calculate methods

        private void ForecastInitialize()
        {
            _sisResults = new List<SisAgeData>();
            _sisClinicResults = new List<ClinicModel>();
            _sisClinicCounterResults = new List<List<List<DataTable>>>();

            _sisAgeController = new SisAgeController();
            _sisAgeController.DefineAll();

            _retrospectiveStartDate = _sisAgeController.SisAgeData.Data[0].Date[0].AddYears(1);
            SetForecastStartDate();

            dateTimePickerFirst.Value = _forecastStartDate;
            dateTimePickerEnd.MinDate = _forecastStartDate;
            dateTimePickerEnd.MaxDate = _forecastStartDate.AddYears(18);
            dateTimePickerEnd.Value = new DateTime(2019, 5, 1);

            radioButtonRealistic.Checked = true;
            radioButtonDynamicBirth.Checked = true;
            radioButtonDeathDynamic.Checked = true;
            _birthMode = BirthMode.Dynamic;
            _deathMode = DeathMode.Dynamic;
            _birthPercent = 0.000892;
            _deathPercent = 0.001242;
            _birth = 38142;
            _death = 53109;
            textBoxBirthPercent.Text = _birthPercent.ToString();
            textBoxDeathPercent.Text = _deathPercent.ToString();
            textBoxBirth.Text = _birth.ToString();
            textBoxDeath.Text = _death.ToString();

            // Remove tabpages.
            tabControlSis.TabPages.Remove(tabPageSisInfFromGamma);
            tabControlClinicCounter1.TabPages.Remove(tabPageClinicCounter11);
            tabControlSisv.TabPages.Remove(tabPageSisvAnalyze);
            tabControlSisvClinic2.TabPages.Remove(tabPageSisvClinic21);
        }

        private void SetForecastStartDate()
        {
            if (radioButtonRetrospective.Checked == true)
            {
                _forecastStartDate = _retrospectiveStartDate;
                _t0 = _sisAgeController.SisAgeData.Data[0].Date.FindIndex(d => _forecastStartDate.Equals(d));
                _forecastStartDate = _sisAgeController.SisAgeData.Data[0]
                    .Date[_t0 - 1].AddMonths(1);
            }
            else
            {
                _t0 = _sisAgeController.SisAgeData.Data[0].Date.Count;
                _forecastStartDate = _sisAgeController.SisAgeData.Data[0]
                    .Date[_t0 - 1].AddMonths(1);
            }
        }

        private void SisCalculateButton_Click(object sender, EventArgs e)
        {
            _forecastMode = ForecastMode.Realistic;
            if (radioButtonRealistic.Checked) _forecastMode = ForecastMode.Realistic;
            if (radioButtonOptimistic.Checked) _forecastMode = ForecastMode.Optimistic;
            if (radioButtonPessimistic.Checked) _forecastMode = ForecastMode.Pessimistic;

            _birthMode = radioButtonConstBirth.Checked ? BirthMode.Const : BirthMode.Dynamic;
            _deathMode = radioButtonDeathConst.Checked ? DeathMode.Const : DeathMode.Dynamic;
            
            _forecastEndDate = dateTimePickerEnd.Value;
            _forecastEndDate = new DateTime(_forecastEndDate.Year, _forecastEndDate.Month, 1);

            _sisAgeController.RestoreSourceData();
            _sisAgeController.DefineAll();
            try
            {
                InputSisParams();
            }
            catch(Exception exception)
            {
                MessageBox.Show(exception.Message);
                return;
            }
            SetForecastStartDate();
            _sisAgeController.BirthMode = _birthMode;
            _sisAgeController.DeathMode = _deathMode;
            if (_birthMode == BirthMode.Dynamic)
                _sisAgeController.BirthPercent = _birthPercent;
            else _sisAgeController.Birth = _birth;
            if (_deathMode == DeathMode.Dynamic)
                _sisAgeController.DeathPercent = _deathPercent;
            else _sisAgeController.Death = _death;

            _sisAgeController.AddForecast(_forecastEndDate, _forecastMode);
            _sisResults.Add(_sisAgeController.SisAgeData);
            PagesHide(false);
            CalculateSis();
            CalculateClinic();
            InitializeSisv();
        }

        private void InputSisParams()
        {
            double birthPercent = 0;
            double deathPercent = 0;
            int birth = 0;
            int death = 0;
            if (_birthMode == BirthMode.Dynamic)
            {
                birthPercent = Valid.CheckBirthPercent(textBoxBirthPercent.Text);
                CheckParams(birthPercent, birthPercent);
                _birthPercent = birthPercent;
            }
            else
            {
                birth = Valid.CheckBirth(textBoxBirth.Text);
                var sis = _sisAgeController.SisAgeData.Data[0];
                birthPercent = (double) birth / sis.N[sis.N.Count - 1];
                CheckParams(birthPercent, birth);
                _birth = birth;
            }
            if (_deathMode == DeathMode.Dynamic)
            {
                deathPercent = Valid.CheckDeathPercent(textBoxDeathPercent.Text);
                CheckParams(deathPercent, deathPercent);
                _deathPercent = deathPercent;
            }
            else
            {
                death = Valid.CheckDeath(textBoxDeath.Text);
                var sis = _sisAgeController.SisAgeData.Data[0];
                deathPercent = (double)death / sis.N[sis.N.Count - 1];
                CheckParams(deathPercent, death);
                _death = death;
            }
            if(Math.Abs(birthPercent - deathPercent) > 0.002)
                throw new Exception("Відношення параметрів народжуваності і смертності недопустимо велике");
        }

        private static void CheckPercent(double percent)
        {
            if (percent > 0.005)
                throw new Exception("Значення параметру недопустимо велике: ");
            if (percent < 0.0005)
                throw new Exception("Значення параметру недопустимо мале: ");
        }

        private static void CheckParams(double percent, double total)
        {
            try
            {
                CheckPercent(percent);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message + total);
            }
        }

        private void SisInitialize()
        {
            PagesHide(true);
            CalculateSis();
        }

        private void CalculateSis()
        {
            _sisAgeData = IncomingData.Instance.CloneSisAgeData();
            _sisAgeData.Add(_sisAgeController.AddSisTable());
            _ageGroups = IncomingData.Instance.AgeGroups.Select(g => g + " років").ToList();
            _ageGroups.Add("всі");
            ageListBox.DataSource = _ageGroups;
            ageListBox.SelectedIndex = 0;
            _sisAgeController.BuildCommonSisAgeInfoTables();
        }
        
        private void DisplaySis(DataGridView dataGridView, DataTable data)
        {
            dataGridView.DataSource = data;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridViewController.Format(dataGridView, 4);

            //chartIFromB.Series[0].ChartType = SeriesChartType.Line;
            //chartIFromB.Series[0].Name = "I(B)";
            //chartIFromB.Series[0].Points.Clear();
            //chartIFromB.ChartAreas[0].Axes[0].Title = Titles.Birth;
            //chartIFromB.ChartAreas[0].Axes[1].Title = Titles.Inf;
            //chartIFromB.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            //chartIFromB.ChartAreas[0].AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;
            //chartIFromB.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gainsboro;
            //chartIFromB.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gainsboro;

            SisAgeData sis = _sisAgeController.SisAgeData;
            int a = ageListBox.SelectedIndex;
            int columnId = 2;
            _forecastStartDate = _forecastStartDate.AddMonths(-1);
            int tDate = _sisAgeController.GetTbyDate(_forecastStartDate);

            chartS.Series.Clear();
            ChartController.ChartConfig(chartS, "S", data, columnId, Titles.Date, Titles.Sus);
            ChartController.DisplayMarker(chartS, _forecastStartDate, Convert.ToDouble(data.Rows[tDate][columnId++]));
            chartS.Series[0].IsVisibleInLegend = false;
            if (ageListBox.SelectedIndex < _ageGroups.Count - 1)
            {
                columnId++;
            }
            //    for (int t = 0; t < sis.Data[a].Date.Count; t++)
            //        chartIFromB.Series[0].Points.AddXY(sis.Birth[t], sis.Data[a].I[t]);
            //}
            //else
            //{
            //    for (int t = 0; t < sis.Data[0].Date.Count; t++)
            //        chartIFromB.Series[0].Points.AddXY(sis.Birth[t], sis.Data[0].Itotal[t]);
            //}
            //chartIFromB.Series[0].Sort(PointSortOrder.Ascending, "X");

            chartI.Series.Clear();
            ChartController.ChartConfig(chartI, "I", data, columnId, Titles.Date, Titles.Inf);
            ChartController.DisplayMarker(chartI, _forecastStartDate,
                Convert.ToDouble(data.Rows[tDate][columnId++]));
            chartI.Series[0].IsVisibleInLegend = false;
            chartBeta.Series.Clear();
            ChartController.ChartConfig(chartBeta, "beta", data, columnId, Titles.Date, Titles.Beta);
            ChartController.DisplayMarker(chartBeta, _forecastStartDate,
                Convert.ToDouble(data.Rows[tDate][columnId++]));
            chartBeta.Series[0].IsVisibleInLegend = false;
            chartGamma.Series.Clear();
            ChartController.ChartConfig(chartGamma, "gamma", data, columnId, Titles.Date, Titles.Gamma);
            ChartController.DisplayMarker(chartGamma, _forecastStartDate,
                Convert.ToDouble(data.Rows[tDate][columnId]));
            chartGamma.Series[0].IsVisibleInLegend = false;

            SensitiveSis();
            _forecastStartDate = _forecastStartDate.AddMonths(1);
        }

        private void SensitiveSis()
        {
            if (_sisAgeController == null) return;
            int a = ageListBox.SelectedIndex;
            sensitiveSisChart.Series.Clear();
            sensitiveSisChart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
            sensitiveSisChart.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            sensitiveSisChart.ChartAreas[0].AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;
            sensitiveSisChart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gainsboro;
            sensitiveSisChart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gainsboro;
            sensitiveSisChart.ChartAreas[0].Axes[0].Title = Titles.Date;
            sensitiveSisChart.ChartAreas[0].Axes[1].Title = Titles.Inf;

            for (int k = 0; k < _sisResults.Count; k++)
            {
                SisAgeData sis = _sisResults[k];
                ConfigSensitiveSisChart(k, _sisResults[k]);
                for (int t = 0; t < sis.Birth.Count; t++)
                {
                    if (a < _ageGroups.Count - 1)
                        sensitiveSisChart.Series[k].Points.AddXY(sis.Data[a].Date[t], sis.Data[a].I[t]);
                    else sensitiveSisChart.Series[k].Points.AddXY(sis.Data[0].Date[t], sis.Data[0].Itotal[t]);
                }
            }
        }

        private void ConfigSensitiveSisChart(int k, SisAgeData sis)
        {
            string name = ""; //Titles.Inf + ", при ";
            if (sis.BirthMode == BirthMode.Const)
                name += $"Народжуваність ({sis.BirthConst} осіб);";
            else name += $" Коефіцієнт народжуваності ({sis.BirthPercent});";
            if (sis.DeathMode == DeathMode.Const)
                name += $" смертність ({sis.DeathConst} осіб).";
            else name += $" коефіцієнт смертності ({sis.DeathPercent}).";
            //if (sis.ForecastMode == ForecastMode.Optimistic)
            //    name += " прогноз оптимістичний.";
            //else if (sis.ForecastMode == ForecastMode.Realistic)
            //    name += " прогноз реалістичний.";
            //else name += " прогноз песимістичний.";

            while (true)
            {
                if (sensitiveSisChart.Series.FindByName(name) == null) break;
                name += "*";
            }
            sensitiveSisChart.Series.Add(new Series());
            sensitiveSisChart.Series[k].XValueType = ChartValueType.DateTime;
            sensitiveSisChart.Series[k].ChartType = SeriesChartType.Line;
            sensitiveSisChart.Series[k].Name = name;
        }

        private void ClearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _sisResults.Clear();
            sensitiveSisChart.Series.Clear();
        }

        private void ClearAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SensitiveClearAll();
        }

        private void SensitiveClearAll()
        {
            _sisResults.Clear();
            sensitiveSisChart.Series.Clear();

            _sisClinicResults.Clear();
            sensitiveClinicSisChart.Series.Clear();

            _sisClinicCounterResults.Clear();
            sensitiveClinicCounterSisChart.Series.Clear();

            _sisvResults.Clear();
            sensitiveSisvChart.Series.Clear();

            _sisvClinicResults.Clear();
            sensitiveClinicSisvChart.Series.Clear();

            _sisvClinicCounterResults.Clear();
            sensitiveClinicCounterSisvChart.Series.Clear();
        }

        private void SaveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveImage(sender);
        }

        private void AgeListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = ageListBox.SelectedIndex;
            DisplaySis(dataGridViewSis, _sisAgeData[index]);
        }
        #endregion

        #region Clinic model methods

        private void CalculateClinic()
        {
            _clinicController = new ClinicController(_sisAgeController.SisAgeData);
            _clinicController.BuildClinicSisAgeTables();
            List<string> items = _clinicController.BuildClinicSisAgeTables();
            dataListBox.DataSource = items;
            dataListBox.SelectedIndex = 0;
            _sisClinicResults.Add(_clinicController.Clinic);
            var ageGroups = _ageGroups.ToList();
            //ageGroups.RemoveAt(ageGroups.Count - 1);
            ageSisClinicListBox1.DataSource = ageGroups;
            ageSisClinicListBox1.SelectedIndex = 0;
            _clinicTables = IncomingData.Instance.ClinicSisAgeData.ToList();
            DisplayClinic(_clinicTables[0]);
            SensitiveClinicSis();
            CalculateClinicCounter();
        }

        private void DisplayClinic(DataTable data)
        {
            dataGridViewClinic.DataSource = data;
            dataGridViewClinic.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridViewController.Format(dataGridViewClinic, 6);
            List<List<double>> p1Table = _clinicController.Clinic.P1;

            chartClinicP1.Series.Clear();
            for (int a = 0; a < p1Table[0].Count; a++)
            {
                List<double> p1List = new List<double>();
                for (int t = 0; t < p1Table.Count; t++)
                    p1List.Add(p1Table[t][a]);
                string name = $"{_clinicTables[2].Columns[a + 1].ColumnName} років";
                ChartController.ChartConfig(chartClinicP1,  name, 
                    _clinicController.Clinic.Date, p1List, Titles.P1Clinic, a);
            }
            ChartController.ChartConfig(chartClinicP1, "Всі",
                _clinicController.Clinic.Date, _clinicController.Clinic.CommonP1, Titles.P1Clinic, p1Table[0].Count);
            chartClinicP1.Series["Всі"].Color = Color.Black;
        }

        private void DataListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = dataListBox.SelectedIndex;
            DisplayClinic(_clinicTables[index]);
        }

        private void AgeSisClinicListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SensitiveClinicSis();
        }

        private void SensitiveClinicSis()
        {
            if (_clinicController == null) return;
            int a = ageSisClinicListBox1.SelectedIndex;
            sensitiveClinicSisChart.Series.Clear();
            for (int k = 0; k < _sisClinicResults.Count; k++)
            {
                ClinicModel clinic = _sisClinicResults[k];
                List<List<double>> p1Table = clinic.P1;
                List<double> p1List = new List<double>();
                string name;
                if (a < ageSisClinicListBox1.Items.Count - 1)
                {
                    for (int t = 0; t < p1Table.Count; t++)
                        p1List.Add(p1Table[t][a]);
                    name = $"Вік: {_clinicTables[2].Columns[a + 1].ColumnName} років; {clinic.Info}";
                }
                else
                {
                    p1List.AddRange(clinic.CommonP1);
                    name = $"Всі; {clinic.Info}";
                }
                while (true)
                {
                    if (sensitiveClinicSisChart.Series.FindByName(name) == null) break;
                    name += "*";
                }
                ChartController.ChartConfig(sensitiveClinicSisChart, name,
                    _clinicController.Clinic.Date, p1List, Titles.P1Clinic, a);
            }
        }

        private void ClearToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            _sisClinicResults.Clear();
            _sisClinicCounterResults.Clear();
            sensitiveClinicSisChart.Series.Clear();
            sensitiveClinicCounterSisChart.Series.Clear();
        }

        private void ClearAllToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            SensitiveClearAll();
        }

        private void SaveToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            SaveImage(sender);
        }
        
        #endregion

        #region Clinic counter model methods

        private void CalculateClinicCounter()
        {
            _clinicCounterController = new ClinicCounterController(_clinicController.Clinic, _t0);
            _clinicCounterController.DefineAll();
            //_detailClinicCounterTable = IncomingData.Instance.ClinicCounterTable;
            GetListBoxClinicCounter();
            var results = new List<List<DataTable>>();
            for (int t = _t0; t < _sisAgeController.SisAgeData.Data[0].Date.Count; t++)
            {
                results.Add(new List<DataTable>());
                for (int a = 0; a <= _sisAgeController.SisAgeData.Data.Count; a++)
                {
                    _commonClinicCounterTable = _clinicCounterController.CreateCommonInfoTable(t, a);
                    results[t - _t0].Add(_commonClinicCounterTable);
                }
            }
            _sisClinicCounterResults.Add(results);
            //DisplayClinicCounter(_detailClinicCounterTable, dataGridViewClinicCounterDetail);
            listBoxClinicCounterAges.SelectedIndex = 0;
            listBoxClinicCounterAges.SelectedIndexChanged += ListBoxClinicCounterAges_SelectedIndexChanged;
            //listBoxClinicCounterPeriods.SelectedIndexChanged += ListBoxClinicCounterPeriods_SelectedIndexChanged;
            listBoxClinicCounterPeriods.SelectedIndex = 0;
            //DisplayClinicCounter(_commonClinicCounterTable, dataGridViewClinicCounterCommon);
            //DisplaySensitiveClinicCounterSis();
        }

        private void GetListBoxClinicCounter()
        {
            var ageGroups = _ageGroups.ToList();
            listBoxClinicCounterAges.DataSource = ageGroups;
            var dates = _sisAgeController.SisAgeData.Data[0].Date;
            listBoxClinicCounterPeriods.Items.Clear();
            for (int t = _t0; t < dates.Count; t++)
            {
                listBoxClinicCounterPeriods.Items.Add($"{dates[t].ToString("dd.MM.yyyy")}-{dates[dates.Count - 1].ToString("dd.MM.yyyy")}");
            }
        }

        private void ListBoxClinicCounterPeriods_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayClinicCounter(dataGridViewClinicCounterCommon);
            DisplaySensitiveClinicCounterSis();
        }

        private void ListBoxClinicCounterAges_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayClinicCounter(dataGridViewClinicCounterCommon);
            DisplaySensitiveClinicCounterSis();
        }

        private void DisplayClinicCounter(DataGridView dataGridView)
        {
            int t = listBoxClinicCounterPeriods.SelectedIndex;
            int a = listBoxClinicCounterAges.SelectedIndex;
            var data = _clinicCounterController.CreateCommonInfoTable(t + _t0, a);
            _commonClinicCounterTable = data;
            dataGridView.DataSource = data;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            chartClinicCounter.Series.Clear();
            ChartController.ChartConfig(chartClinicCounter, 
                "Частота захворюваності", data, 1, "Випадок захворювання", 
                "Кількість хворих осіб на 100 тисяч осіб одного віку");
            chartClinicCounter.Series[0].XValueType = ChartValueType.Int32;
            chartClinicCounter.Series[0].ChartType = SeriesChartType.Column;
            chartClinicCounter.Series[0].IsVisibleInLegend = false;
        }

        private void DisplaySensitiveClinicCounterSis()
        {
            sensitiveClinicCounterSisChart.Series.Clear();
            int t = listBoxClinicCounterPeriods.SelectedIndex;
            int a = listBoxClinicCounterAges.SelectedIndex;
            for (int k = 0; k < _sisClinicCounterResults.Count; k++)
            {
                if(_sisClinicCounterResults[k].Count <= t) continue;
                DataTable dt = _sisClinicCounterResults[k][t][a];
                string name = $"Частота захворюваності при {_sisClinicResults[k].Info}";
                while (true)
                {
                    if (sensitiveClinicCounterSisChart.Series.FindByName(name) == null) break;
                    name += "*";
                }
                ChartController.ChartConfig(sensitiveClinicCounterSisChart, name, dt, 1, "Частота", "Кількість осіб", k);
                sensitiveClinicCounterSisChart.Series[name].XValueType = ChartValueType.Int32;
                sensitiveClinicCounterSisChart.Series[name].ChartType = SeriesChartType.Column;
            }
        }

        #endregion

        #region Sisv calculate methods

        private void FirstInitializeSisv()
        {
            analyzeSisvListBox.Items.Add("I(B)");
            analyzeSisvListBox.Items.Add("I(Cv1)");
            analyzeSisvListBox.Items.Add("I(Cv2)");
            analyzeSisvListBox.Items.Add("I(Ci)");
            analyzeSisvListBox.Items.Add("I(Cv1/Ci)");
            analyzeSisvListBox.Items.Add("I(Cv2/Ci)");
            analyzeSisvListBox.Items.Add("I(p1)");
            analyzeSisvListBox.Items.Add("I(p2)");
            analyzeSisvListBox.Items.Add("p1(Cv1)");
            analyzeSisvListBox.Items.Add("p1(Cv2)");
            analyzeSisvListBox.Items.Add("p1(Ci)");
            analyzeSisvListBox.Items.Add("p2(Cv1)");
            analyzeSisvListBox.Items.Add("p2(Cv2)");
            analyzeSisvListBox.Items.Add("p2(Ci)");

            sensitiveSisvListBox.Items.Add("Інфіковані особи");
            sensitiveSisvListBox.Items.Add("Охоплення осіб однією дозою вакцини");
            sensitiveSisvListBox.Items.Add("Охоплення осіб двома дозами вакцини");
            sensitiveSisvListBox.DrawMode = DrawMode.OwnerDrawVariable;

            _sisvResults = new List<SisvAge>();
            _sisvClinicResults = new List<ClinicModel>();
            _sisvClinicCounterResults = new List<List<List<DataTable>>>();

            _vaccinationMode = VaccinationMode.Static;
            textBoxP1.Enabled = false;
            textBoxP2.Enabled = false;
            radioButtonStatic.Checked = true;
            //radioButtonDynamic.Checked = true;
        }

        private void InitializeSisv()
        {
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
            _p1 = 0.1;
            _p2 = 0.2;
            
            analyzeSisvListBox.SelectedIndex = 0;
            sensitiveSisvListBox.SelectedIndex = 0;

            InitializeParameters();

            chartSisvS.Series.Clear();
            chartSisvInf.Series.Clear();
            chartSisvBeta.Series.Clear();
            chartSisvGamma.Series.Clear();
            chartSisvAnalyze.Series.Clear();
            chartSisvIv1.Series.Clear();
            chartSisvIv2.Series.Clear();
            chartSisvP1.Series.Clear();
            chartSisvP2.Series.Clear();
            chartSisvV1.Series.Clear();
            chartSisvV2.Series.Clear();
            //sensitiveSisvChart.Series.Clear();

            CalculateSisv();
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

        private void SisvCalculateButton_Click(object sender, EventArgs e)
        {
            CalculateSisv();
        }

        private bool InputData()
        {
            try
            {
                _ef = Valid.CheckEf(efTextBox.Text);
                _a1 = Valid.CheckA1(textBoxA1.Text);
                _a2 = Valid.CheckA2(textBoxA2.Text);
                _a3 = Valid.CheckA3(textBoxA3.Text);
                _a4 = Valid.CheckA4(textBoxA4.Text);
                Valid.CheckCv(cv1TextBox.Text, cv2TextBox.Text);
                _cv1 = Valid.CheckCv1(cv1TextBox.Text);
                _cv2 = Valid.CheckCv2(cv2TextBox.Text);
                _ci = Valid.CheckCi(ciTextBox.Text);
                _inflation = Valid.CheckInflation(inflationTextBox.Text);
                if (checkBoxVaccMode.Checked == true)
                {
                    Valid.CheckP(textBoxP1.Text, textBoxP2.Text);
                    _p1 = Valid.CheckP1(textBoxP1.Text);
                    _p2 = Valid.CheckP2(textBoxP2.Text);
                }
            }
            catch(Exception exception)
            {
                MessageBox.Show(exception.Message);
                return false;
            }
            return true;
        }

        private void CalculateSisv()
        {
            if (InputData() == false) return;

            // Calculates clinic model
            _clinicController = new ClinicController(_sisAgeController.SisAgeData);
            _clinicCounterSisvController = _clinicCounterController; 

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
            _sisvAgeController.DefineAll(_sisAgeController.SisAgeData, 
                _clinicCounterSisvController.ChanceOfInfection, _forecastStartDate);
            _sisvResults.Add(_sisvAgeController.Sisv);
            _sisvAgeData = IncomingData.Instance.CloneSisvAgeData();
            _sisvAgeData.Add(_sisvAgeController.AddSisTable());

            // Recalculate clinic model.
            CalculateSisvClinic();

            ageListBox2.DataSource = _ageGroups;
            ageListBox2.SelectedIndex = 0;
            DisplaySisv(dataGridViewSisv, _sisvAgeData[0]);
            //optimizationReportToolStripMenuItem.Enabled = true;

            OptimizationCostCalculate();
        }

        private void AgeListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = ageListBox2.SelectedIndex;
            DisplaySisv(dataGridViewSisv, _sisvAgeData[index]);
        }

        #region DisplaySisv Methods

        private void DisplaySisv(DataGridView dataGridView, DataTable data)
        {
            dataGridView.DataSource = data;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridViewController.Format(dataGridView, 4);
            dataGridView.Columns[0].Width = 70;
            dataGridView.Columns[1].Width = 80;

            int columnId = 2;
            int tDate = _t0;
            chartSisvS.Series.Clear();
            ChartController.ChartConfig(chartSisvS, "S", data, columnId, Titles.Date, Titles.Sus);
            ChartController.DisplayMarker(chartSisvS, _forecastStartDate, Convert.ToDouble(data.Rows[tDate][columnId++]));
            chartSisvS.Series[0].IsVisibleInLegend = false;
            if (ageListBox2.SelectedIndex < _ageGroups.Count - 1) columnId++;

            chartSisvInf.Series.Clear();
            ChartController.ChartConfig(chartSisvInf, "I", data, columnId, Titles.Date, Titles.Inf);
            ChartController.DisplayMarker(chartSisvInf, _forecastStartDate,
                Convert.ToDouble(data.Rows[tDate][columnId++]));
            chartSisvInf.Series[0].IsVisibleInLegend = false;

            chartSisvBeta.Series.Clear();
            ChartController.ChartConfig(chartSisvBeta, "beta", data, columnId, Titles.Date, Titles.Beta);
            ChartController.DisplayMarker(chartSisvBeta, _forecastStartDate,
                Convert.ToDouble(data.Rows[tDate][columnId++]));
            chartSisvBeta.Series[0].IsVisibleInLegend = false;

            chartSisvGamma.Series.Clear();
            ChartController.ChartConfig(chartSisvGamma, "gamma", data, columnId, Titles.Date, Titles.Gamma);
            ChartController.DisplayMarker(chartSisvGamma, _forecastStartDate,
                Convert.ToDouble(data.Rows[tDate][columnId++]));
            chartSisvGamma.Series[0].IsVisibleInLegend = false;

            chartSisvP1.Series.Clear();
            ChartController.ChartConfig(chartSisvP1, "p1", data, columnId, Titles.Date, Titles.P1);
            ChartController.DisplayMarker(chartSisvP1, _forecastStartDate,
                Convert.ToDouble(data.Rows[tDate][columnId++]));
            chartSisvP1.Series[0].IsVisibleInLegend = false;

            chartSisvP2.Series.Clear();
            ChartController.ChartConfig(chartSisvP2, "p2", data, columnId, Titles.Date, Titles.P2);
            ChartController.DisplayMarker(chartSisvP2, _forecastStartDate,
                Convert.ToDouble(data.Rows[tDate][columnId++]));
            chartSisvP2.Series[0].IsVisibleInLegend = false;

            chartSisvV1.Series.Clear();
            ChartController.ChartConfig(chartSisvV1, "V1", data, columnId, Titles.Date, Titles.V1);
            ChartController.DisplayMarker(chartSisvV1, _forecastStartDate,
                Convert.ToDouble(data.Rows[tDate][columnId++]));
            chartSisvV1.Series[0].IsVisibleInLegend = false;

            chartSisvV2.Series.Clear();
            ChartController.ChartConfig(chartSisvV2, "V2", data, columnId, Titles.Date, Titles.V2);
            ChartController.DisplayMarker(chartSisvV2, _forecastStartDate,
                Convert.ToDouble(data.Rows[tDate][columnId++]));
            chartSisvV2.Series[0].IsVisibleInLegend = false;

            chartSisvIv1.Series.Clear();
            ChartController.ChartConfig(chartSisvIv1, "Iv1", data, columnId, Titles.Date, Titles.Iv1);
            ChartController.DisplayMarker(chartSisvIv1, _forecastStartDate,
                Convert.ToDouble(data.Rows[tDate][columnId++]));
            chartSisvIv1.Series[0].IsVisibleInLegend = false;

            chartSisvIv2.Series.Clear();
            ChartController.ChartConfig(chartSisvIv2, "Iv2", data, columnId, Titles.Date, Titles.Iv2);
            ChartController.DisplayMarker(chartSisvIv2, _forecastStartDate,
                Convert.ToDouble(data.Rows[tDate][columnId++]));
            chartSisvIv2.Series[0].IsVisibleInLegend = false;
            //Analyze();
            DisplayEfficientAnalyze(_sisvAgeController.Sisv);
            SensitiveSisv();
        }

        private void SensitiveSisv()
        {
            if (_sisvAgeController == null) return;
            int a = ageListBox2.SelectedIndex;
            SisvAge sisv = _sisvAgeController.Sisv;
            sensitiveSisvChart.Series.Clear();
            sensitiveSisvChart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
            sensitiveSisvChart.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            sensitiveSisvChart.ChartAreas[0].AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;
            sensitiveSisvChart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gainsboro;
            sensitiveSisvChart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gainsboro;

            switch (sensitiveSisvListBox.SelectedIndex)
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
                ConfigSensitiveSisvChart(k, sisv);
                for (int t = 0; t < sisv.Date.Count; t++)
                {
                    if (a < _ageGroups.Count - 1)
                        sensitiveSisvChart.Series[k].Points.AddXY(sisv.Date[t], sisv.I[t][a]);
                    else sensitiveSisvChart.Series[k].Points.AddXY(sisv.Date[t], sisv.Itotal[t]);
                }
            }
            sensitiveSisvChart.ChartAreas[0].Axes[0].Title = Titles.Date;
            sensitiveSisvChart.ChartAreas[0].Axes[1].Title = Titles.Inf;
        }

        private void DisplayP1FromT()
        {
            for (int k = 0; k < _sisvResults.Count; k++)
            {
                SisvAge sisv = _sisvResults[k];
                ConfigSensitiveSisvChart(k, sisv);
                for (int t = 0; t < sisv.Date.Count; t++)
                {
                    sensitiveSisvChart.Series[k].Points.AddXY(sisv.Date[t], sisv.P1[t]);
                }
            }
            sensitiveSisvChart.ChartAreas[0].Axes[0].Title = Titles.Date;
            sensitiveSisvChart.ChartAreas[0].Axes[1].Title = Titles.P1;
        }

        private void DisplayP2FromT()
        {
            for (int k = 0; k < _sisvResults.Count; k++)
            {
                SisvAge sisv = _sisvResults[k];
                ConfigSensitiveSisvChart(k, sisv);
                for (int t = 0; t < sisv.Date.Count; t++)
                {
                    sensitiveSisvChart.Series[k].Points.AddXY(sisv.Date[t], sisv.P2[t]);
                }
            }
            sensitiveSisvChart.ChartAreas[0].Axes[0].Title = Titles.Date;
            sensitiveSisvChart.ChartAreas[0].Axes[1].Title = Titles.P2;
        }

        private void ConfigSensitiveSisvChart(int k, SisvAge sisv)
        {
            string name = //sensitiveSisvListBox.SelectedItem +
                          $"Вартість однієї дози вакцини ({sisv.Cv1} грн);" +
                          $" вартість двох доз вакцини ({sisv.Cv2} грн);" +
                          $" витрати при захворюванні ({sisv.Ci} грн);";
            if (sisv.BirthMode == BirthMode.Const)
                name += $" народжуваність ({sisv.BirthConst} осіб);";
            else name += $" коефіцієнт народжуваності ({sisv.BirthPercent});";
            if (sisv.DeathMode == DeathMode.Const)
                name += $" смертність ({sisv.DeathConst} осіб).";
            else name += $" коефіцієнт смертності ({sisv.DeathPercent}).";
            //if (sisv.ForecastMode == ForecastMode.Optimistic)
            //    name += " прогноз оптимістичний.";
            //else if (sisv.ForecastMode == ForecastMode.Realistic)
            //    name += " прогноз реалістичний.";
            //else name += " прогноз песимістичний.";

            while (true)
            {
                if (sensitiveSisvChart.Series.FindByName(name) == null) break;
                name += "*";
            }
            sensitiveSisvChart.Series.Add(new Series());
            sensitiveSisvChart.Series[k].XValueType = ChartValueType.DateTime;
            sensitiveSisvChart.Series[k].ChartType = SeriesChartType.Line;
            sensitiveSisvChart.Series[k].Name = name;
        }

        private void DisplayEfficientAnalyze(SisvAge sisv)
        {
            int a = ageListBox2.SelectedIndex;
            efficientSisvChart.Series.Clear();
            ChartController.Config(efficientSisvChart, Titles.PredictedInfetions);
            efficientSisvChart.ChartAreas[0].Axes[0].Title = Titles.Date;
            efficientSisvChart.ChartAreas[0].Axes[1].Title = Titles.PredictedInfetions;
            for (int t = _t0; t < sisv.Date.Count; t++)
            {
                efficientSisvChart.Series[0].Points.AddXY(sisv.Date[t], sisv.PredictedInfections[t][a]);
            }
            efficientSisvChart.Series[0].IsVisibleInLegend = false;
        }

        #region Analyze

        private void Analyze()
        {
            if (_sisvAgeController == null) return;
            int a = ageListBox2.SelectedIndex;
            SisvAge sisv = _sisvAgeController.Sisv;
            _t0 = sisv.Date.IndexOf(_forecastStartDate);
            if (chartSisvAnalyze.Series.Count == 0)
                chartSisvAnalyze.Series.Add(analyzeSisvListBox.SelectedItem.ToString());
            chartSisvAnalyze.Series[0].Name = analyzeSisvListBox.SelectedItem.ToString();
            chartSisvAnalyze.Series[0].Points.Clear();
            chartSisvAnalyze.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            chartSisvAnalyze.ChartAreas[0].AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;
            chartSisvAnalyze.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gainsboro;
            chartSisvAnalyze.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gainsboro;
            chartSisvAnalyze.Series[0].ChartType = SeriesChartType.Line;
            switch (analyzeSisvListBox.SelectedIndex)
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
            chartSisvAnalyze.Series[0].Sort(PointSortOrder.Ascending, "X");
        }

        private void DisplayCiFromP2(SisvAge sisv)
        {
            chartSisvAnalyze.ChartAreas[0].Axes[0].Title = Titles.Ci;
            chartSisvAnalyze.ChartAreas[0].Axes[1].Title = Titles.P2;
            for (int t = _t0; t < sisv.Date.Count; t++)
            {
                chartSisvAnalyze.Series[0].Points.AddXY(Math.Round(sisv.CiList[t], 2), sisv.P2[t]);
            }
        }

        private void DisplayCv2FromP2(SisvAge sisv)
        {
            chartSisvAnalyze.ChartAreas[0].Axes[0].Title = Titles.Cv2;
            chartSisvAnalyze.ChartAreas[0].Axes[1].Title = Titles.P2;
            for (int t = _t0; t < sisv.Date.Count; t++)
            {
                chartSisvAnalyze.Series[0].Points.AddXY(Math.Round(sisv.Cv2List[t], 2), sisv.P2[t]);
            }
        }

        private void DisplayCv1FromP2(SisvAge sisv)
        {
            chartSisvAnalyze.ChartAreas[0].Axes[0].Title = Titles.Cv1;
            chartSisvAnalyze.ChartAreas[0].Axes[1].Title = Titles.P2;
            for (int t = _t0; t < sisv.Date.Count; t++)
            {
                chartSisvAnalyze.Series[0].Points.AddXY(Math.Round(sisv.Cv1List[t], 2), sisv.P2[t]);
            }
        }

        private void DisplayCiFromP1(SisvAge sisv)
        {
            chartSisvAnalyze.ChartAreas[0].Axes[0].Title = Titles.Ci;
            chartSisvAnalyze.ChartAreas[0].Axes[1].Title = Titles.P1;
            for (int t = _t0; t < sisv.Date.Count; t++)
            {
                chartSisvAnalyze.Series[0].Points.AddXY(Math.Round(sisv.CiList[t], 2), sisv.P1[t]);
            }
        }

        private void DisplayCv2FromP1(SisvAge sisv)
        {
            chartSisvAnalyze.ChartAreas[0].Axes[0].Title = Titles.Cv2;
            chartSisvAnalyze.ChartAreas[0].Axes[1].Title = Titles.P1;
            for (int t = _t0; t < sisv.Date.Count; t++)
            {
                chartSisvAnalyze.Series[0].Points.AddXY(Math.Round(sisv.Cv2List[t], 2), sisv.P1[t]);
            }
        }

        private void DisplayCv1FromP1(SisvAge sisv)
        {
            chartSisvAnalyze.ChartAreas[0].Axes[0].Title = Titles.Cv1;
            chartSisvAnalyze.ChartAreas[0].Axes[1].Title = Titles.P1;
            for (int t = _t0; t < sisv.Date.Count; t++)
            {
                chartSisvAnalyze.Series[0].Points.AddXY(Math.Round(sisv.Cv1List[t], 2), sisv.P1[t]);
            }
        }

        private void DisplayInfectedFromP2(SisvAge sisv, int a)
        {
            chartSisvAnalyze.ChartAreas[0].Axes[0].Title = Titles.P2;
            chartSisvAnalyze.ChartAreas[0].Axes[1].Title = Titles.Inf;
            //chartSisvAnalyze.Series[0].ChartType = SeriesChartType.Point;
            for (int t = _t0; t < sisv.Date.Count; t++)
            {
                if (a < _ageGroups.Count - 1)
                    chartSisvAnalyze.Series[0].Points.AddXY(Math.Round(sisv.P2[t], 2), sisv.I[t][a]);
                else chartSisvAnalyze.Series[0].Points.AddXY(Math.Round(sisv.P2[t], 2), sisv.Itotal[t]);
            }
        }

        private void DisplayInfectedFromP1(SisvAge sisv, int a)
        {
            chartSisvAnalyze.ChartAreas[0].Axes[0].Title = Titles.P1;
            chartSisvAnalyze.ChartAreas[0].Axes[1].Title = Titles.Inf;
            //chartSisvAnalyze.Series[0].ChartType = SeriesChartType.Point;
            for (int t = _t0; t < sisv.Date.Count; t++)
            {
                if (a < _ageGroups.Count - 1)
                    chartSisvAnalyze.Series[0].Points.AddXY(Math.Round(sisv.P1[t], 2), sisv.I[t][a]);
                else chartSisvAnalyze.Series[0].Points.AddXY(Math.Round(sisv.P1[t], 2), sisv.Itotal[t]);
            }
        }

        private void DisplayInfectedFromCv2Ci(SisvAge sisv, int a)
        {
            chartSisvAnalyze.ChartAreas[0].Axes[0].Title = Titles.Cv2DivCi;
            chartSisvAnalyze.ChartAreas[0].Axes[1].Title = Titles.Inf;
            for (int t = _t0; t < sisv.Date.Count; t++)
            {
                if (a < _ageGroups.Count - 1)
                    chartSisvAnalyze.Series[0].Points.AddXY(Math.Round(sisv.Cv2List[t] / sisv.CiList[t], 2), sisv.I[t][a]);
                else chartSisvAnalyze.Series[0].Points.AddXY(Math.Round(sisv.Cv2List[t] / sisv.CiList[t], 2), sisv.Itotal[t]);
            }
        }

        private void DisplayInfectedFromCv1Ci(SisvAge sisv, int a)
        {
            chartSisvAnalyze.ChartAreas[0].Axes[0].Title = Titles.Cv1DivCi;
            chartSisvAnalyze.ChartAreas[0].Axes[1].Title = Titles.Inf;
            for (int t = _t0; t < sisv.Date.Count; t++)
            {
                if (a < _ageGroups.Count - 1)
                    chartSisvAnalyze.Series[0].Points.AddXY(Math.Round(sisv.Cv1List[t] / sisv.CiList[t], 2), sisv.I[t][a]);
                else chartSisvAnalyze.Series[0].Points.AddXY(Math.Round(sisv.Cv1List[t] / sisv.CiList[t], 2), sisv.Itotal[t]);
            }
        }

        private void DisplayInfectedFromCi(SisvAge sisv, int a)
        {
            chartSisvAnalyze.ChartAreas[0].Axes[0].Title = Titles.Ci;
            chartSisvAnalyze.ChartAreas[0].Axes[1].Title = Titles.Inf;
            for (int t = _t0; t < sisv.Date.Count; t++)
            {
                if (a < _ageGroups.Count - 1)
                    chartSisvAnalyze.Series[0].Points.AddXY(Math.Round(sisv.CiList[t], 2), sisv.I[t][a]);
                else chartSisvAnalyze.Series[0].Points.AddXY(Math.Round(sisv.CiList[t], 2), sisv.Itotal[t]);
            }
        }

        private void DisplayInfectedFromCv2(SisvAge sisv, int a)
        {
            chartSisvAnalyze.ChartAreas[0].Axes[0].Title = Titles.Cv2;
            chartSisvAnalyze.ChartAreas[0].Axes[1].Title = Titles.Inf;
            for (int t = _t0; t < sisv.Date.Count; t++)
            {
                if (a < _ageGroups.Count - 1)
                    chartSisvAnalyze.Series[0].Points.AddXY(Math.Round(sisv.Cv2List[t], 2), sisv.I[t][a]);
                else chartSisvAnalyze.Series[0].Points.AddXY(Math.Round(sisv.Cv2List[t], 2), sisv.Itotal[t]);
            }
        }

        private void DisplayInfectedFromCv1(SisvAge sisv, int a)
        {
            chartSisvAnalyze.ChartAreas[0].Axes[0].Title = Titles.Cv1;
            chartSisvAnalyze.ChartAreas[0].Axes[1].Title = Titles.Inf;
            for (int t = _t0; t < sisv.Date.Count; t++)
            {
                if (a < _ageGroups.Count - 1)
                    chartSisvAnalyze.Series[0].Points.AddXY(Math.Round(sisv.Cv1List[t], 2), sisv.I[t][a]);
                else chartSisvAnalyze.Series[0].Points.AddXY(Math.Round(sisv.Cv1List[t], 2), sisv.Itotal[t]);
            }
        }

        private void DisplayInfectedFromBirth(SisvAge sisv, int a)
        {
            chartSisvAnalyze.ChartAreas[0].Axes[0].Title = Titles.Birth;
            chartSisvAnalyze.ChartAreas[0].Axes[1].Title = Titles.Inf;
            //chartSisvAnalyze.Series[0].ChartType = SeriesChartType.Point;
            for (int t = 0; t < sisv.Date.Count; t++)
            {
                if (a < _ageGroups.Count - 1)
                    chartSisvAnalyze.Series[0].Points.AddXY(sisv.Birth[t], sisv.I[t][a]);
                else chartSisvAnalyze.Series[0].Points.AddXY(sisv.Birth[t], sisv.Itotal[t]);
            }
        }
        #endregion
        #endregion

        private void AnalyzeSisvListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Analyze();
        }

        private void SensitiveSisvListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SensitiveSisv();
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

        private void ToolStripMenuClear_Click(object sender, EventArgs e)
        {
            _sisvResults.Clear();
            sensitiveSisvChart.Series.Clear();
        }

        private void ClearAllToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SensitiveClearAll();
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

        private void SensitiveSisvListBox_MeasureItem(object sender, MeasureItemEventArgs e)
        {
            double width = splitContainerSisv5.SplitterDistance;
            double fontSize = sensitiveSisvListBox.Font.Size;
            double count = sensitiveSisvListBox.Items[e.Index].ToString().Length;
            int height = (int) Math.Round(count / width * fontSize * fontSize * 1.5);
            //if (e.Index == 0) e.ItemHeight = 25;
            //else e.ItemHeight = 70;
            e.ItemHeight = height;
        }

        private void SensitiveSisvListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.Graphics.DrawString(sensitiveSisvListBox.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds);
        }
        #endregion

        #region Clinic model for Sisv methods

        private void CalculateSisvClinic()
        {
            _clinicController = new ClinicController(_sisvAgeController.Sisv);
            _clinicController.BuildClinicSisAgeTables();
            List<string> items = _clinicController.BuildClinicSisAgeTables();
            dataSisvListBox.DataSource = items;
            dataSisvListBox.SelectedIndex = 0;
            _sisvClinicResults.Add(_clinicController.Clinic);
            var ageGroups = _ageGroups.ToList();
            //ageGroups.RemoveAt(ageGroups.Count - 1);
            ageSisvClinicListBox.DataSource = ageGroups;
            ageSisvClinicListBox.SelectedIndex = 0;
            _clinicSisvTables = IncomingData.Instance.ClinicSisAgeData.ToList();
            DisplaySisvClinic(_clinicSisvTables[0]);
            SensitiveClinicSisv();
            CalculateClinicSisvCounter();
        }

        private void DisplaySisvClinic(DataTable data)
        {
            dataGridViewSisvClinic.DataSource = data;
            dataGridViewSisvClinic.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridViewController.Format(dataGridViewSisvClinic, 6);
            List<List<double>> p1Table = _clinicController.Clinic.P1;

            chartSisvClinicP1.Series.Clear();
            for (int a = 0; a < p1Table[0].Count; a++)
            {
                List<double> p1List = new List<double>();
                for (int t = 0; t < p1Table.Count; t++)
                    p1List.Add(p1Table[t][a]);
                string name = $"{_clinicSisvTables[2].Columns[a + 1].ColumnName} років";
                ChartController.ChartConfig(chartSisvClinicP1, name,
                    _clinicController.Clinic.Date, p1List, Titles.P1Clinic, a);
            }
            ChartController.ChartConfig(chartSisvClinicP1, "Всі",
                _clinicController.Clinic.Date, _clinicController.Clinic.CommonP1, Titles.P1Clinic, p1Table[0].Count);
            chartSisvClinicP1.Series["Всі"].Color = Color.Black;
        }

        private void DataSisvListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = dataSisvListBox.SelectedIndex;
            DisplaySisvClinic(_clinicSisvTables[index]);
        }

        private void AgeSisvClinicListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SensitiveClinicSisv();
        }

        private void SensitiveClinicSisv()
        {
            if (_clinicController == null) return;
            int a = ageSisvClinicListBox.SelectedIndex;
            sensitiveClinicSisvChart.Series.Clear();
            for (int k = 0; k < _sisvClinicResults.Count; k++)
            {
                ClinicModel clinic = _sisvClinicResults[k];
                List<List<double>> p1Table = clinic.P1;
                List<double> p1List = new List<double>();
                string name;
                if (a < ageSisvClinicListBox.Items.Count - 1)
                {
                    for (int t = 0; t < p1Table.Count; t++)
                        p1List.Add(p1Table[t][a]);
                    name = $"Вік: {_clinicSisvTables[2].Columns[a + 1].ColumnName} років; {clinic.Info}";
                }
                else
                {
                    p1List.AddRange(clinic.CommonP1);
                    name = $"Всі; {clinic.Info}";
                }
                while (true)
                {
                    if (sensitiveClinicSisvChart.Series.FindByName(name) == null) break;
                    name += "*";
                }
                ChartController.ChartConfig(sensitiveClinicSisvChart, name,
                    _clinicController.Clinic.Date, p1List, Titles.P1Clinic, a);
            }
        }

        private void ClearToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            _sisvClinicResults.Clear();
            _sisvClinicCounterResults.Clear();
            sensitiveClinicSisvChart.Series.Clear();
            sensitiveClinicCounterSisvChart.Series.Clear();
        }

        private void SaveToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            SaveImage(sender);
        }

        private void ClearAllToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            SensitiveClearAll();
        }
        #endregion

        #region Clinic with Sisv counter model methods

        private void CalculateClinicSisvCounter()
        {
            _clinicCounterSisvController = new ClinicCounterController(_clinicController.Clinic, _t0);
            _clinicCounterSisvController.DefineAll();
            GetListBoxSisvClinicCounter();
            var results = new List<List<DataTable>>();
            for (int t = _t0; t < _sisvAgeController.Sisv.Date.Count; t++)
            {
                results.Add(new List<DataTable>());
                for (int a = 0; a <= _sisvAgeController.Sisv.Beta[t].Count; a++)
                {
                    _commonClinicSisvCounterTable = _clinicCounterSisvController.CreateCommonInfoTable(t, a);
                    results[t - _t0].Add(_commonClinicSisvCounterTable);
                }
            }
            _sisvClinicCounterResults.Add(results);
            //DisplayClinicSisvCounter(_detailClinicSisvCounterTable, dataGridViewSisvClinicDetail);
            listBoxSisvClinicCounterAges.SelectedIndex = 0;
            listBoxSisvClinicCounterAges.SelectedIndexChanged += ListBoxSisvClinicCounterAges_SelectedIndexChanged;
            listBoxSisvClinicCounterPeriods.SelectedIndex = 0;
            //DisplayClinicCounter(_commonClinicCounterTable, dataGridViewClinicCounterCommon);
            //DisplaySensitiveClinicCounterSisv();
        }

        private void GetListBoxSisvClinicCounter()
        {
            var ageGroups = _ageGroups.ToList();
            listBoxSisvClinicCounterAges.DataSource = ageGroups;
            var dates = _sisAgeController.SisAgeData.Data[0].Date;
            listBoxSisvClinicCounterPeriods.Items.Clear();
            for (int t = _t0; t < dates.Count; t++)
            {
                listBoxSisvClinicCounterPeriods.Items.Add($"{dates[t].ToString("dd.MM.yyyy")}-{dates[dates.Count - 1].ToString("dd.MM.yyyy")}");
            }
        }

        private void ListBoxSisvClinicCounterPeriods_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayClinicSisvCounter(dataGridViewSisvClinicCommon);
            DisplaySensitiveClinicCounterSisv();
        }

        private void ListBoxSisvClinicCounterAges_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayClinicSisvCounter(dataGridViewSisvClinicCommon);
            DisplaySensitiveClinicCounterSisv();
        }

        private void DisplayClinicSisvCounter(DataGridView dataGridView)
        {
            int t = listBoxSisvClinicCounterPeriods.SelectedIndex;
            int a = listBoxSisvClinicCounterAges.SelectedIndex;
            var data = _clinicCounterSisvController.CreateCommonInfoTable(t + _t0, a);
            _commonClinicSisvCounterTable = data;
            dataGridView.DataSource = data;
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            chartSisvClinicCounter.Series.Clear();
            ChartController.ChartConfig(chartSisvClinicCounter,
                "Частота захворюваності", data, 1, "Випадок захворювання",
                "Кількість хворих осіб на 100 тисяч осіб одного віку");
            chartSisvClinicCounter.Series[0].XValueType = ChartValueType.Int32;
            chartSisvClinicCounter.Series[0].ChartType = SeriesChartType.Column;
            chartSisvClinicCounter.Series[0].IsVisibleInLegend = false;
        }
        
        private void DisplaySensitiveClinicCounterSisv()
        {
            sensitiveClinicCounterSisvChart.Series.Clear();
            int t = listBoxSisvClinicCounterPeriods.SelectedIndex;
            int a = listBoxSisvClinicCounterAges.SelectedIndex;
            for (int k = 0; k < _sisvClinicCounterResults.Count; k++)
            {
                if (_sisvClinicCounterResults[k].Count <= t) continue;
                DataTable dt = _sisvClinicCounterResults[k][t][a];
                string name = $"Частота захворюваності при {_sisvClinicResults[k].Info}";
                while (true)
                {
                    if (sensitiveClinicCounterSisvChart.Series.FindByName(name) == null) break;
                    name += "*";
                }
                ChartController.ChartConfig(sensitiveClinicCounterSisvChart, name, dt, 1, "Частота", "Кількість осіб", k);
                sensitiveClinicCounterSisvChart.Series[name].XValueType = ChartValueType.Int32;
                sensitiveClinicCounterSisvChart.Series[name].ChartType = SeriesChartType.Column;
            }
        }

        #endregion

        #region Optimization report

        private void CreateCommon()
        {
            richTextBoxOpt1.Text = $"Оптимальні суспільні витрати на вакцинацію однією дозою вакцини: {Math.Round(_sisv.CostV1Total, 2)} грн. \n" +
                                $"Оптимальні суспільні витрати на вакцинацію двома дозами вакцини: {Math.Round(_sisv.CostV2Total, 2)} грн. \n" +
                                $"Оптимальні суспільні витрати на лікування: {Math.Round(_sisv.CostInfectedTotal, 2)} грн. \n" +
                                $"Загальні суспільні витрати: {Math.Round(_sisv.CostTotal, 2)} грн.";
        }

        private void CreateDetail()
        {
            _dtOpt = new DataTable();
            _dtOpt.Columns.Add(new DataColumn("Дата", typeof(DateTime)));
            _dtOpt.Columns.Add(new DataColumn("Охоплення осіб однією дозою вакцини", typeof(double)));
            _dtOpt.Columns.Add(new DataColumn("Індивідуальні витрати на вакцинацію однією дозою вакцини, грн", typeof(double)));
            _dtOpt.Columns.Add(new DataColumn("Компенсація витрат на вакцинацію однією дозою вакцини, грн", typeof(double)));
            _dtOpt.Columns.Add(new DataColumn("Охоплення осіб двома дозами вакцини", typeof(double)));
            _dtOpt.Columns.Add(new DataColumn("Індивідуальні витрати на вакцинацію двома дозами вакцини, грн", typeof(double)));
            _dtOpt.Columns.Add(new DataColumn("Компенсація витрат на вакцинацію двома дозами вакцини, грн", typeof(double)));
            _dtOpt.Columns.Add(new DataColumn("Суспільні витрати на вакцинацію однією дозою вакцини, грн", typeof(double)));
            _dtOpt.Columns.Add(new DataColumn("Суспільні витрати на вакцинацію двома дозами вакцини, грн", typeof(double)));
            _dtOpt.Columns.Add(new DataColumn("Суспільні витрати при захворюванні, грн", typeof(double)));
            _dtOpt.Columns.Add(new DataColumn("Загальні суспільні витрати, грн", typeof(double)));

            for (int t = 0; t < _sisv.Date.Count; t++)
            {
                _dtOpt.Rows.Add();
                _dtOpt.Rows[t][0] = _sisv.Date[t];
                _dtOpt.Rows[t][1] = _sisv.P1[t];
                _dtOpt.Rows[t][2] = Math.Round(_sisv.IndividualCostV1[t], 2);
                _dtOpt.Rows[t][3] = Math.Round(_sisv.GetCompensationCostV1(t), 2);
                _dtOpt.Rows[t][4] = _sisv.P2[t];
                _dtOpt.Rows[t][5] = Math.Round(_sisv.IndividualCostV2[t], 2);
                _dtOpt.Rows[t][6] = Math.Round(_sisv.GetCompensationCostV2(t), 2);
                double cv1 = Math.Round(_sisv.Birth[t] * _sisv.P1[t] * _sisv.Cv1List[t], 2);
                _dtOpt.Rows[t][7] = cv1;
                double cv2 = Math.Round(_sisv.Birth[t] * _sisv.P2[t] * _sisv.Cv2List[t], 2);
                _dtOpt.Rows[t][8] = cv2;
                double cost = Math.Round(_sisv.Cost[t], 2);
                _dtOpt.Rows[t][9] = Math.Round(cost - cv1 - cv2, 2);
                _dtOpt.Rows[t][10] = cost;
            }
            dataGridViewOpt1.DataSource = _dtOpt;
            dataGridViewOpt1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        private void CreateIndividualStrategy()
        {
            _dtOpt = new DataTable();
            _dtOpt.Columns.Add(new DataColumn("Дата", typeof(DateTime)));
            _dtOpt.Columns.Add(new DataColumn("Тип вакцинації", typeof(string)));
            _dtOpt.Columns.Add(new DataColumn("Індивідуальні витрати, грн", typeof(double)));
            _dtOpt.Columns.Add(new DataColumn("Компенасція, грн", typeof(double)));

            _sisv.GetIndividualStrategy(ref _firstStrategy, ref _secondStrategy, ref _withoutVaccination);
            for (int t = 0; t < _sisv.Date.Count; t++)
            {
                _dtOpt.Rows.Add();
                _dtOpt.Rows[t][0] = _sisv.Date[t];
                if (_firstStrategy[t] == 1)
                {
                    _dtOpt.Rows[t][1] = "Однією дозою вакцини";
                    _dtOpt.Rows[t][2] = Math.Round(_sisv.IndividualCostV1[t], 2);
                    _dtOpt.Rows[t][3] = Math.Round(_sisv.GetCompensationCostV1(t), 2);
                    continue;
                }
                if (_secondStrategy[t] == 1)
                {
                    _dtOpt.Rows[t][1] = "Двома дозами вакцини";
                    _dtOpt.Rows[t][2] = Math.Round(_sisv.IndividualCostV2[t], 2);
                    _dtOpt.Rows[t][3] = Math.Round(_sisv.GetCompensationCostV2(t), 2);
                    continue;
                }
                if (_withoutVaccination[t] == 1)
                {
                    _dtOpt.Rows[t][1] = "Без вакцинації";
                    _dtOpt.Rows[t][2] = Math.Round(_sisv.GetChanceOfInfection(t, 0) * _sisv.CiList[t], 2);
                    _dtOpt.Rows[t][3] = 0;
                }
                else
                {
                    _dtOpt.Rows[t][1] = "Без вакцинації";
                    _dtOpt.Rows[t][2] = 0;
                    _dtOpt.Rows[t][3] = 0;
                }
            }
            dataGridViewIndStrategy.DataSource = _dtOpt;
            dataGridViewIndStrategy.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DisplayIndStrategy();
        }

        private void DisplayIndStrategy()
        {
            chartIndStrategy.Series.Clear();
            int counter = 0;
            if (checkedListBoxIndStrategy.GetItemChecked(0) == true)
            {
                ChartController.ChartConfig(chartIndStrategy, "Однією дозою вакцини", _sisv.Date,
                    _firstStrategy.Select(f => (double)f).ToList(), "Вибір стратегії", counter++);
            }
            if (checkedListBoxIndStrategy.GetItemChecked(1) == true)
            {
                ChartController.ChartConfig(chartIndStrategy, "Двома дозами вакцини", _sisv.Date,
                    _secondStrategy.Select(f => (double)f).ToList(), "Вибір стратегії", counter++);
            }
            if (checkedListBoxIndStrategy.GetItemChecked(2) == true)
            {
                ChartController.ChartConfig(chartIndStrategy, "Без вакцинації", _sisv.Date,
                    _withoutVaccination.Select(f => (double)f).ToList(), "Вибір стратегії", counter++);
            }
        }

        private void OptimizationCostCalculate()
        {
            _sisv = _sisvAgeController.Sisv;
            splitContainerOpt1.SplitterDistance = (int)(0.3 * Width);
            splitContainerOpt2.SplitterDistance = 150;
            CreateDetail();
            CreateCommon();
            for (int i = 0; i < checkedListBoxIndStrategy.Items.Count; i++)
            {
                checkedListBoxIndStrategy.SetItemChecked(i, true);
            }
            CreateIndividualStrategy();
        }

        private void CheckedListBoxIndStrategy_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayIndStrategy();
        }
        
        
        #endregion
        
    }
}
