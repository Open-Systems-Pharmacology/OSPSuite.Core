namespace OSPSuite.Core.Maths.Interpolations
{
   public class Sample<T>
   {
      public double X { get; }
      public T Y { get; }

      public Sample(double x, T y)
      {
         X = x;
         Y = y;
      }
   }

   public class Sample : Sample<double>
   {
      public Sample(double x, double y) : base(x, y)
      {
      }
   }
}