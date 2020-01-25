using Epidemic.BLL.Controllers;
using Epidemic.DAL.Entity;
using Epidemic.DAL.Repository;

namespace Epidemic.BLL.Services
{
    public class EpidemicService
    {
        public static void PrepareEpidemicData(string sisAgeFileName, string populationFileName)
        {
            EpidemicRepository repos = new EpidemicRepository();
            repos.PrepareData(sisAgeFileName, populationFileName);
            IncomingData inData = IncomingData.Instance;
            inData.Data = repos.EpidemicData;
            inData.SisData = repos.SisData;
            inData.SisAgeData = repos.SisAgeData;
            inData.SDataForSisAge = repos.AgeStructureDataForSisAge;
            //inData.SirData = repos.SirData;

            // Transform for new age groups
            AgeGroupController ageGroupController = new AgeGroupController();
            inData.SisAgeData = ageGroupController.GetNewSisAgeTables(inData.SisAgeData);
        }
    }
}
