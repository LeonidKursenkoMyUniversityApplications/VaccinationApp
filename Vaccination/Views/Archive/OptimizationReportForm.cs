using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Epidemic.BLL.Models;
using Vaccination.Controllers;

namespace Vaccination.Views
{
    public partial class OptimizationReportForm : Form
    {
        private SisvAge _sisv;
        private DataTable _dtOpt;
        private int _t0;

        private List<int> _firstStrategy = null;
        private List<int> _secondStrategy = null;
        private List<int> _withoutVaccination = null;

        public OptimizationReportForm(SisvAge sisvAge)
        {
            InitializeComponent();
            _sisv = sisvAge;
            _t0 = _sisv.StartForecast;
            
            saveFileDialog1.Title = "Зберегти дані";
            saveFileDialog1.InitialDirectory = "звіти\\";
            saveFileDialog1.Filter = "excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
        }

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
                    _dtOpt.Rows[t][2] = Math.Round(_sisv.GetChanceOfInfection(t - _t0, 0) * _sisv.CiList[t], 2);
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
                    _firstStrategy.Select(f => (double) f).ToList(), "Вибір стратегії", counter++);
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

        private void OptimizationReportForm_Load(object sender, EventArgs e)
        {
            splitContainerOpt1.SplitterDistance = (int) (0.3 * Width);
            splitContainerOpt2.SplitterDistance = 150;
            CreateDetail();
            CreateCommon();
            for (int i = 0; i < checkedListBoxIndStrategy.Items.Count; i++)
            {
                checkedListBoxIndStrategy.SetItemChecked(i, true);
            }
            CreateIndividualStrategy();
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "Звіт по витратах на вакцинацію.xlsx";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveFileDialog1.FileName;
                DataTable dt1 = (DataTable) dataGridViewOpt1.DataSource;
                DataTable dt2 = (DataTable)dataGridViewIndStrategy.DataSource;
                new ProgressForm(new List<DataTable> { dt1, dt2 }, fileName).Show();
            }
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void CheckedListBoxIndStrategy_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayIndStrategy();
        }
    }
}
