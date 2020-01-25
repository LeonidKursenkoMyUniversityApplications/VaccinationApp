using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Epidemic.BLL.Controllers;
using Epidemic.BLL.Controllers.Input;
using Epidemic.BLL.Models;
using Epidemic.BLL.Services;
using Epidemic.DAL.Entity;

namespace Vaccination.Views
{
    public partial class ForecastDateForm : Form
    {
        private List<DataTable> _sisTables;
        private SisAgeController _sisAgeController;
        private DateTime _forecastStartDate;
        private BirthMode _birthMode;
        private DeathMode _deathMode;
        private double _birth;
        private double _death;

        public ForecastDateForm()
        {
            InitializeComponent();
        }

        private void ForecastDateForm_Load(object sender, EventArgs e)
        {
            _sisAgeController = new SisAgeController();
            _sisAgeController.DefineAll();
            _forecastStartDate = _sisAgeController.SisAgeData.Data[0]
                .Date[_sisAgeController.SisAgeData.Data[0].Date.Count - 1].AddMonths(1);

            dateTimePickerFirst.Value = _forecastStartDate;
            dateTimePickerEnd.MinDate = _forecastStartDate;
            dateTimePickerEnd.Value = new DateTime(2019, 5, 1);

            radioButtonRealistic.Checked = true;
            radioButtonConstBirth.Checked = true;
            radioButtonDeathConst.Checked = true;
            _birthMode = BirthMode.Const;
            _deathMode = DeathMode.Const;
            _birth = 0.00954;
            _death = 0.0144;
            textBoxBirth.Text = _birth.ToString();
            textBoxDeath.Text = _death.ToString();
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ButtonOk_Click(object sender, EventArgs e)
        {
            ForecastMode forecastMode = ForecastMode.Realistic;
            if (radioButtonRealistic.Checked) forecastMode = ForecastMode.Realistic;
            if (radioButtonOptimistic.Checked) forecastMode = ForecastMode.Optimistic;
            if (radioButtonPessimistic.Checked) forecastMode = ForecastMode.Pessimistic;

            _birthMode = radioButtonConstBirth.Checked ? BirthMode.Const : BirthMode.Dynamic;
            _deathMode = radioButtonDeathConst.Checked ? DeathMode.Const : DeathMode.Dynamic;

            var endDate = dateTimePickerEnd.Value;
            endDate = new DateTime(endDate.Year, endDate.Month, endDate.Day);

            try
            {
                _birth = Transform.ToDouble(textBoxBirth.Text);
                _death = Transform.ToDouble(textBoxDeath.Text);
            }
            catch
            {
                MessageBox.Show("Недопустимий формат вводу");
                return;
            }

            _sisAgeController.BirthMode = _birthMode;
            _sisAgeController.DeathMode = _deathMode;
            _sisAgeController.BirthPercent = _birth;
            _sisAgeController.DeathPercent = _death;
            new SisAgeForm(_sisAgeController, endDate, forecastMode).Show();
            Close();
        }

        private void RadioButtonDeathDynamic_CheckedChanged(object sender, EventArgs e)
        {
            //textBoxDeath.Enabled = radioButtonDeathDynamic.Checked;
        }

        private void RadioButtonDynamicBirth_CheckedChanged(object sender, EventArgs e)
        {
            //textBoxBirth.Enabled = radioButtonDynamicBirth.Checked;
        }

        
    }
}
