using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Epidemic.BLL.Models;
using Epidemic.DAL.Entity;

namespace Epidemic.BLL.Controllers
{
    public class EquilibriumController
    {
        //    public SisAgeData SisAgeData { set; get; }
        //    public List<DataTable> EquilibriumTables { set; get; }

        //    public void CreateEqTables()
        //    {
        //        List<DataTable> dts = new List<DataTable>();
        //        for (int a = 0; a < SisAgeData.Data.Count; a++)
        //        {
        //            dts.Add(FillEqTable(a));
        //        }
        //        dts.Add(FillEqTotalTable());
        //        EquilibriumTables = dts;
        //    }

        //    private DataTable FillEqTable(int a)
        //    {
        //        var dt = CreateEqTable();
        //        var sis = SisAgeData.Data[a];
        //        for (int t = 0; t < sis.Date.Count; t++)
        //        {
        //            dt.Rows.Add();
        //            dt.Rows[t]["Дата"] = sis.Date[t];
        //            dt.Rows[t]["S"] = sis.S[t];
        //            dt.Rows[t]["I"] = sis.I[t];
        //            dt.Rows[t]["S*"] = sis.SuEq[t];
        //            dt.Rows[t]["I*"] = sis.InfEq[t];
        //        }
        //        return dt;
        //    }

        //    private DataTable FillEqTotalTable()
        //    {
        //        var dt = CreateEqTable();
        //        var sis = SisAgeData.Data[0];
        //        for (int t = 0; t < sis.Date.Count; t++)
        //        {
        //            dt.Rows.Add();
        //            dt.Rows[t]["Дата"] = sis.Date[t];
        //            dt.Rows[t]["S"] = sis.N[t] - sis.Itotal[t];
        //            dt.Rows[t]["I"] = sis.Itotal[t];
        //            dt.Rows[t]["S*"] = SisAgeData.SuEqTotal[t];
        //            dt.Rows[t]["I*"] = SisAgeData.InfEqTotal[t];
        //        }
        //        return dt;
        //    }

        //    private static DataTable CreateEqTable()
        //    {
        //        DataTable dt = new DataTable();
        //        dt.Columns.Add(new DataColumn("Дата", typeof(DateTime)));
        //        dt.Columns.Add(new DataColumn("S", typeof(int)));
        //        dt.Columns.Add(new DataColumn("I", typeof(int)));
        //        dt.Columns.Add(new DataColumn("S*", typeof(int)));
        //        dt.Columns.Add(new DataColumn("I*", typeof(int)));
        //        return dt;
        //    }
    }
}
