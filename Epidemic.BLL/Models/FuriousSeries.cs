namespace Epidemic.BLL.Models
{
    public class FuriousSeries
    {
        public double[] A { set; get; }
        public double[] B { set; get; }

        public FuriousSeries(int n)
        {
            A = new double[n];
            B = new double[n];
        }
    }
}
