using System.Data;

namespace Epidemic.DAL.Repository
{
    public interface IEpidemicRepository
    {
        string[,] Read(string fileName, int pageIndex);
        void Write(DataTable dataTable, string fileName, int pageIndex);
        void PrepareData(string sisAgeFileName, string populationFileName);
    }
}
