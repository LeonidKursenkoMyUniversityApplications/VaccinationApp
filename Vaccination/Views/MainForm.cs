using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows.Forms;
using Epidemic.BLL.Services;
using Epidemic.DAL.Entity;

namespace Vaccination.Views
{
    public partial class MainForm : Form
    {
        private List<DataTable> _sisTables;
        private string _sisFileName = @"data\Rotavirus_stat_Ukraine_3.xlsx";
        private string _populationFileName = @"data\Вікові групи.xlsx";

        public MainForm()
        {
            InitializeComponent();
            //this.Text = "Система вибору оптимальних схем вакцинації населення з використанням епідеміологічних даних";
            //this.Text = "Система вибору оптимальних схем вакцинації населення";
            this.Text = "Вибір даних";
            textBoxVaccFile.Text = _sisFileName;
            textBoxPopulation.Text = _populationFileName;

            //GetData();
        }

        private bool GetData()
        {
            try
            {
                //IncomingData.Instance.PrepareData();
                EpidemicService.PrepareEpidemicData(_sisFileName, _populationFileName);
                _sisTables = new List<DataTable>();
                for (int i = 0; i < IncomingData.Instance.SisAgeData.Count; i++)
                {
                    _sisTables.Add(IncomingData.Instance.SisAgeData[i].Copy());
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("При зчитуванні та обробці вхідних даних виникла помилка. " + e.Message);
                return false;
            }
            return true;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            _sisFileName = textBoxVaccFile.Text;
            if (File.Exists(_sisFileName) == false)
            {
                MessageBox.Show("Невдалося відкрити файл: \"" + _sisFileName + "\"");
                return;
            }
            _populationFileName = textBoxPopulation.Text;
            if (File.Exists(_populationFileName) == false)
            {
                MessageBox.Show("Невдалося відкрити файл: \"" + _populationFileName + "\"");
                return;
            }
            if (GetData() == false) return;
            IncomingData.Instance.SisAgeData = new List<DataTable>();
            for (int i = 0; i < _sisTables.Count; i++)
            {
                IncomingData.Instance.SisAgeData.Add(_sisTables[i].Copy());
            }
            new RviForm().Show();
            this.Hide();
        }

        private void ButtonVaccOpen_Click(object sender, EventArgs e)
        {
            openFileDialog1.FileName = "sis.xlsx";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _sisFileName = openFileDialog1.FileName;
                textBoxVaccFile.Text = _sisFileName;
            }
        }
        
        private void ButtonPopulationOpen_Click(object sender, EventArgs e)
        {

            openFileDialog1.FileName = "population.xlsx";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                _populationFileName = openFileDialog1.FileName;
                textBoxPopulation.Text = _populationFileName;
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Завантажити файл";
            openFileDialog1.InitialDirectory = "data\\";
            openFileDialog1.Filter = "excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
        }
    }
}
