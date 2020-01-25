using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Epidemic.BLL.Controllers;
using Epidemic.BLL.Models;
using Epidemic.DAL.Entity;
using Vaccination.Controllers;

namespace Vaccination.Views
{
    public partial class EquilibriumForm : Form
    {
        private EquilibriumController _equilibriumController;
        private SisAgeData _sisAgeData;
        private List<string> _ageGroups;
        private DateTime _forecastStartDate;

        public EquilibriumForm(SisAgeData sisAgeData, DateTime forecastStartDate)
        {
            InitializeComponent();
            _sisAgeData = sisAgeData;
            _forecastStartDate = forecastStartDate;
            Inititialize();
        }

        private void Inititialize()
        {
            splitContainer1.SplitterDistance = 40;
            splitContainer2.SplitterDistance = (int)(this.Width * 0.4);
        }

        private void EquilibriumForm_Load(object sender, EventArgs e)
        {
            //_equilibriumController = new EquilibriumController
            //{
            //    SisAgeData = _sisAgeData
            //};
            //_equilibriumController.CreateEqTables();
            //_ageGroups = IncomingData.Instance.AgeGroups.Select(s => s + " років").ToList();
            //_ageGroups.Add("Всі");

            //ageListBox.DataSource = _ageGroups;
            //ageListBox.SelectedIndex = 0;
            //Display(dataGridView1, _equilibriumController.EquilibriumTables[0]);
        }

        //private void Display(DataGridView dataGridView, DataTable data)
        //{
        //    dataGridView.DataSource = data;
        //    dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            
        //    int tDate = _equilibriumController.SisAgeData.Data[0].Date.IndexOf(_forecastStartDate);

        //    chartS.Series.Clear();
        //    chartI.Series.Clear();
        //    ChartController.ChartConfig(chartS, "S", data, 1, 0);
        //    ChartController.Display(chartS, "S*", data, 3, 1);
        //    ChartController.DisplayMarker(chartS, _forecastStartDate, 
        //        Convert.ToDouble(data.Rows[tDate][1]), 2);
            
        //    ChartController.ChartConfig(chartI, "I", data, 2, 0);
        //    ChartController.Display(chartI, "I*", data, 4, 1);
        //    ChartController.DisplayMarker(chartI, _forecastStartDate,
        //        Convert.ToDouble(data.Rows[tDate][2]), 2);
            
        //}

        private void ageListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = ageListBox.SelectedIndex;
            //Display(dataGridView1, _equilibriumController.EquilibriumTables[index]);
        }
    }
}
