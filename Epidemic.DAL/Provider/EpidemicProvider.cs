using System;
using System.IO;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;

namespace Epidemic.DAL.Provider
{
    public class EpidemicProvider : IProvider
    {
        public string[,] Read(string fileName, int pageIndex)
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook = xlApp.Workbooks.Open(System.IO.Path.GetFullPath(fileName));
            Excel._Worksheet xlWorksheet = (Excel._Worksheet) xlWorkbook.Sheets[pageIndex];
            Excel.Range xlRange = xlWorksheet.UsedRange;
            var lastCell = xlWorksheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);

            // Saves information from excel file
            int columnsCount = lastCell.Column;
            int rowsCount = ((Excel.Range)xlRange.Cells[xlRange.Rows.Count, 1]).get_End(Excel.XlDirection.xlUp).Row;//xlRange.Cells[xlRange.Rows.Count, 1].End(Excel.XlDirection.xlUp).Row;
           
            if (rowsCount == 1) rowsCount = xlRange.Rows.Count;
            string[,] data = new string[rowsCount, columnsCount];

            for (int i = 1; i <= rowsCount; i++)
            {
                for (int j = 1; j <= columnsCount; j++)
                {
                    data[i - 1, j - 1] = ((Excel.Range)xlRange.Cells[i, j]).Text.ToString();
                }
            }

            //cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //release com objects to fully kill excel process from running in the background
            Marshal.ReleaseComObject(xlRange);
            Marshal.ReleaseComObject(xlWorksheet);

            //close and release
            xlWorkbook.Close();
            Marshal.ReleaseComObject(xlWorkbook);

            //quit and release
            xlApp.Quit();
            Marshal.ReleaseComObject(xlApp);

            return data;
        }

        public void Write(string[,] data, string fileName, int pageIndex)
        {
            Excel.Application xlApp = new Excel.Application();
            Excel.Workbook xlWorkbook;
            if (File.Exists(Path.GetFullPath(fileName)))
                xlWorkbook = xlApp.Workbooks.Open(Path.GetFullPath(fileName));
            else
            {
                xlWorkbook = xlApp.Workbooks.Add(Excel.XlWBATemplate.xlWBATWorksheet);
                xlWorkbook.SaveAs(Path.GetFullPath(fileName));
            }
            if (xlWorkbook.Sheets.Count < pageIndex)
            {
                xlWorkbook.Sheets.Add(After: xlWorkbook.Sheets[xlWorkbook.Sheets.Count]);
            }
            Excel._Worksheet xlWorksheet = (Excel._Worksheet) xlWorkbook.Sheets[pageIndex];
            Excel.Range xlRange = xlWorksheet.UsedRange;
            //var lastCell = xlWorksheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);

            int columnsCount = data.GetLength(1);
            int rowsCount = data.GetLength(0);

            for (int i = 1; i <= rowsCount; i++)
            {
                for (int j = 1; j <= columnsCount; j++)
                {
                    double val = 0;
                    if (Double.TryParse(data[i - 1, j - 1], out val) == true)
                    {
                        ((Excel.Range) xlRange.Cells[i, j]).EntireColumn.NumberFormat = "@";
                        if (i > 1 && j > 1)
                            ((Excel.Range) xlRange.Cells[i, j]).Value = Convert.ToDouble(data[i - 1, j - 1]);
                    }
                    else
                    {
                        ((Excel.Range) xlRange.Cells[i, j]).Value = data[i - 1, j - 1];
                    }

                }
            }

            //cleanup
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //release com objects to fully kill excel process from running in the background
            Marshal.ReleaseComObject(xlRange);
            Marshal.ReleaseComObject(xlWorksheet);

            //close and release
            xlWorkbook.Save();
            xlWorkbook.Close();
            Marshal.ReleaseComObject(xlWorkbook);

            //quit and release
            xlApp.Quit();
            Marshal.ReleaseComObject(xlApp);
        }
    }
}
