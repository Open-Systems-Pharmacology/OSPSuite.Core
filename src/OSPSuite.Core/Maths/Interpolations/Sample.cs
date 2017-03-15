namespace OSPSuite.Core.Maths.Interpolations
{
    public class Sample
    {
        public Sample(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double X { get; private set; }
        public double Y { get; private set; }
    }
}