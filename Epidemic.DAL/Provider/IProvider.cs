namespace Epidemic.DAL.Provider
{
    public interface IProvider
    {
        string[,] Read(string fileName, int pageIndex);
        void Write(string[,] data, string fileName, int pageIndex);
    }
}
