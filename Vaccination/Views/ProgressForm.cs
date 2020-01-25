using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Epidemic.DAL.Repository;

namespace Vaccination.Views
{
    public partial class ProgressForm : Form
    {
        private List<DataTable> _dts;
        private string _fileName;

        public ProgressForm(List<DataTable> dts, string fileName)
        {
            InitializeComponent();
            _dts = dts;
            _fileName = fileName;
            progressBar1.Maximum = 100;
            progressBar1.Step = 1;
            progressBar1.Value = 0;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
            backgroundWorker1.RunWorkerAsync();
        }

        private void BackgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        private void ProgressForm_Activated(object sender, EventArgs e)
        {
            
        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            //var backgroundWorker1 = sender as BackgroundWorker;
            var repos = new EpidemicRepository();
            for (int i = 0; i < _dts.Count; i++)
            {
                try
                {
                    repos.Write(_dts[i], _fileName, i + 1);
                }
                catch (Exception exception)
                {
                    MessageBox.Show("Виникла помилка при збереженні даних. " + exception.Message);
                    Close();
                }

                backgroundWorker1.ReportProgress((i + 1) * 100 / _dts.Count);
            }
        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //MessageBox.Show("ok");
            Close();
        }
    }
}
