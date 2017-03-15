using System;

namespace OSPSuite.Core.Maths.Interpolations
{
   /// <summary>
   /// Given arrays xa[0..n-1] and ya[0..n-1], and given a value x, this routine returns a value y, and
   /// an error estimate dy. If P(x) is the polynomial of degree N - 1 such that P(xai) = yai, i =
   /// 0, . . . , n-1, then the returned value y = P(x).
   /// </summary>
   public class PolInt
   {
      private double y;
      private double dy;

      public double Y // output
      {
         get { return y; }
      }

      public double Error // output
      {
         get { return dy; }
      }

      public void Polint(double[] xa, double[] ya, int n, double x) // input 
      {
         int i, m, ns = 0;
         double den, dif, dift, ho, hp, w;
         double[] c = new double[n];
         double[] d = new double[n];
         dif = Math.Abs(x - xa[0]);

         for (i = 0; i < n; i++) // Here we find the index of the closest table entry,
         {
            if ((dift = Math.Abs(x - xa[i])) < dif)
            {
               ns = i;
               dif = dift;
            }
            c[i] = ya[i]; // and initialize the tableau of c¡¯s and d¡¯s.
            d[i] = ya[i];
         }
         y = ya[ns--]; // This is the initial approximation to y.
         for (m = 0; m < n - 1; m++) // For each column of the tableau,
         {
            for (i = 0; i < n - m - 1; i++) // we loop over the current c¡¯s and d¡¯s and update them
            {
               ho = xa[i] - x;
               hp = xa[i + m + 1] - x;
               w = c[i + 1] - d[i];
               if ((den = ho - hp) == 0.0)
                  throw new Exception("Error in PolCof.Polcof Invalid inputs");

               // This error can occur only if two input xa¡¯s are (to within roundoff) identical.
               den = w / den;
               d[i] = hp * den; // Here the c¡¯s and d¡¯s are updated.
               c[i] = ho * den;
            }
            y += (dy = (2 * (ns + 1) < (n - m - 1) ? c[ns + 1] : d[ns--]));
            /*After each column in the tableau is completed, we decide which correction, c or d,
               we want to add to our accumulating value of y, i.e., which path to take through the
               tableau-forking up or down. We do this in such a way as to take the most "straight
               line" route through the tableau to its apex, updating ns accordingly to keep track of
               where we are. This route keeps the partial approximations centered (insofar as possible)
               on the target x. The last dy added is thus the error indication. */
         }
      }
   }
}