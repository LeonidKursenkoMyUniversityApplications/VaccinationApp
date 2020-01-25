using System.Collections.Generic;
using DataTable = System.Data.DataTable;

namespace Epidemic.DAL.Entity
{
    public class IncomingData
    {
        #region Attributes
        private static IncomingData instance;
        #endregion

        #region Properties
        public static IncomingData Instance
        {
            get
            {
                if (instance == null)
                    instance = new IncomingData();
                return instance;
            }
        }

        public DataTable Data { set; get; }
        public DataTable SirData { set; get; }
        public DataTable SisData { set; get; }
        public List<DataTable> SisAgeData { set; get; }
        public DataTable SDataForSisAge { set; get; }
        public List<DataTable> CommonSisAgeInfoData { set; get; }
        public List<string> AgeGroups { set; get; }
        public List<DataTable> ClinicSisAgeData { get; set; }
        //public DataTable ClinicCounterTable { set; get; }
        //public DataTable CommonClinicCounterTable { set; get; }
        public List<DataTable> SisvAgeData { set; get; }
        
        #endregion

        #region Constructors
        private IncomingData()
        {
        }
        #endregion

        public List<DataTable> CloneSisAgeData()
        {
            var dt = new List<DataTable>();
            for (int a = 0; a < SisAgeData.Count; a++)
            {
                dt.Add(SisAgeData[a].Copy());
            }
            return dt;
        }

        public List<DataTable> CloneSisvAgeData()
        {
            var dt = new List<DataTable>();
            for (int a = 0; a < SisvAgeData.Count; a++)
            {
                dt.Add(SisvAgeData[a].Copy());
            }
            return dt;
        }
    }
}
