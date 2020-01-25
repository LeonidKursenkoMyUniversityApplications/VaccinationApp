using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Epidemic.BLL.Controllers;
using Epidemic.DAL.Entity;
using Epidemic.DAL.Repository;
using Vaccination.Controllers;

namespace Vaccination.Views
{
    public partial class CommonSisAgeInfoForm : Form
    {
        private List<DataTable> _tables;
        public CommonSisAgeInfoForm()
        {
            InitializeComponent();
            splitContainer1.SplitterDistance = 40;

            saveFileDialog1.FileName = "sis.xslx";
            saveFileDialog1.Title = "Зберегти дані";
            saveFileDialog1.InitialDirectory = "звіти\\";
            saveFileDialog1.Filter = "excel files (*.xlsx)|*.xlsx|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;
        }

        private void CommonSisAgeInfoForm_Load(object sender, EventArgs e)
        {
            SisAgeController sisAgeController = new SisAgeController();
            List<string> items = sisAgeController.BuildCommonSisAgeInfoTables();
            dataListBox.DataSource = items;
            dataListBox.SelectedIndex = 0;
            _tables = IncomingData.Instance.CommonSisAgeInfoData;
            Display(_tables[0]);
        }

        private void DataListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = dataListBox.SelectedIndex;
            Display(_tables[index]);
        }

        private void Display(DataTable data)
        {
            dataGridView1.DataSource = data;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            DataGridViewController.Format(dataGridView1, 4);
        }

        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "sis.xlsx";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveFileDialog1.FileName;
                new ProgressForm(new List<DataTable> { _tables[dataListBox.SelectedIndex] }, fileName).Show();
            }
        }

        private void SaveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            saveFileDialog1.FileName = "sis.xlsx";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string fileName = saveFileDialog1.FileName;
                new ProgressForm(_tables, fileName).Show();
            }
        }

        private void CloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
