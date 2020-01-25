using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using Epidemic.DAL.Entity;

namespace Vaccination.Controllers
{
    public class ChartController
    {
        public static void SirChartConfig(Chart chart, string name, DataTable data,
            int columnId, int seriesId = 0)
        {
            Config(chart, name, seriesId);
            for (int i = 0; i < data.Rows.Count; i++)
            {
                chart.Series[name].Points.AddXY(i, data.Rows[i][columnId]);
            }
        }

        public static void ChartConfig(Chart chart, string name, DataTable data, 
            int columnId, string xName, string yName, int seriesId = 0)
        {
            Config(chart, name);
            chart.ChartAreas[0].Axes[0].Title = xName;
            chart.ChartAreas[0].Axes[1].Title = yName;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                chart.Series[name].Points.AddXY(data.Rows[i][0], data.Rows[i][columnId]);
            }
        }

        public static void ChartConfig(Chart chart, string name, List<DateTime> dates, 
            List<double> values, string yName, int seriesId = 0)
        {
            Config(chart, name);
            chart.ChartAreas[0].Axes[0].Title = Titles.Date;
            chart.ChartAreas[0].Axes[1].Title = yName;
            int count = values.Count < dates.Count ? values.Count : dates.Count;
            for (int i = 0; i < count; i++)
            {
                chart.Series[name].Points.AddXY(dates[i], values[i]);
            }
        }

        public static void ChartConfig(Chart chart, string name, List<DateTime> dates,
            DataTable data, int columnId, string yName, int seriesId = 0)
        {
            Config(chart, name, seriesId);
            chart.ChartAreas[0].Axes[0].Title = Titles.Date;
            chart.ChartAreas[0].Axes[1].Title = yName;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                chart.Series[name].Points.AddXY(dates[i], data.Rows[i][columnId]);
            }
        }

        public static void Config(Chart chart, string name, int seriesId = 0)
        {
            if(chart.Series.IsUniqueName(name) == true)
                chart.Series.Add(name);
            else chart.Series[seriesId].Name = name;
            chart.Series[name].Points.Clear();
            //chart.Series[seriesId].Name = name;
            chart.Series[name].ChartType = SeriesChartType.Line;
            chart.Dock = DockStyle.Fill;
            chart.Series[name].XValueType = ChartValueType.DateTime;
            chart.ChartAreas[0].AxisX.IntervalAutoMode = IntervalAutoMode.VariableCount;
            chart.ChartAreas[0].AxisY.IntervalAutoMode = IntervalAutoMode.VariableCount;
            chart.ChartAreas[0].AxisX.LabelStyle.Angle = -90;
            chart.ChartAreas[0].AxisX.MajorGrid.LineColor = Color.Gainsboro;
            chart.ChartAreas[0].AxisY.MajorGrid.LineColor = Color.Gainsboro;
            chart.Legends[0].MaximumAutoSize = 30;
        }

        private static void ConfigMarker(Chart chart, string name, int seriesId = 0)
        {
            if (seriesId == chart.Series.Count)
                chart.Series.Add(name);
            else chart.Series[seriesId].Name = name;
            chart.Series[name].Points.Clear();
            //chart.Series[seriesId].Name = name;
            chart.Series[name].ChartType = SeriesChartType.Point;
            chart.Series[name].MarkerStyle = MarkerStyle.Circle;
            chart.Series[name].Color = Color.Red;
            chart.Series[name].XValueType = ChartValueType.DateTime;
        }

        public static void DisplayMarker(Chart chart, DateTime date, double value, int seriesId = 1)
        {
            ConfigMarker(chart, "Прогноз", seriesId);
            chart.Series[1].Points.AddXY(date, value);
        }

        public static void Display(Chart chart, string name, DataTable data,
            int columnId, int seriesId = 0)
        {
            if (seriesId == chart.Series.Count)
            {
                chart.Series.Add(name);
                chart.Series[name].ChartType = SeriesChartType.Line;
            }
            for (int i = 0; i < data.Rows.Count; i++)
            {
                chart.Series[name].Points.AddXY(data.Rows[i][0], data.Rows[i][columnId]);
            }
        }
    }
}
