using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Vaccination.Controllers
{
    public class DataGridViewController
    {
        public static void Format(DataGridView dataGridView, int precision)
        {
            string format = "#,0.";
            for (int i = 0; i < precision; i++) format += "#";
            for (int i = 1; i < dataGridView.Columns.Count; i++)
            {
                dataGridView.Columns[i].DefaultCellStyle.Format = format;
            }
        }
    }
}
