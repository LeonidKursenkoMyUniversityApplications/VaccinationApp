using Epidemic.DAL.Entity;

namespace Epidemic.BLL.Models
{
    public class SisData : Sis
    {
        #region Attributes

        private static SisData instance;

        #endregion
        
        #region Properties

        public static SisData Instance
        {
            get
            {
                if (instance == null)
                    instance = new SisData();
                return instance;
            }
        }
        
        #endregion

        #region Constructors
        
        private SisData() : base(IncomingData.Instance.SisData)
        {
        }

        #endregion
        
    }
}
