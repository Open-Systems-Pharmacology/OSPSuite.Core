using System;

namespace OSPSuite.Core.Maths.Interpolations
{
   /// <summary>
   ///    Given arrays xa[0..n] and ya[0..n] containing a tabulated function yai = f(xai), this
   ///    routine returns an array of coefficients cof[0..n] such that y_{a} = \sum_{j}cof_{j}x_{a}^{j}.
   /// </summary>
   public class PolyFit
   {
      public double[] Polyfit(double[] xa, double[] ya, int n)
      {
         PolInt ob = new PolInt();
         int k, j, i;
         double xmin;
         double[] x = new double[n + 1];
         double[] y = new double[n + 1];
         double[] cof = new double[n + 1];
         for (j = 0; j <= n; j++)
         {
            x[j] = xa[j];
            y[j] = ya[j];
         }
         for (j = 0; j <= n; j++)
         {
            ob.Polint(x, y, n + 1 - j, 0.0);
            cof[j] = ob.Y;
            xmin = 1.0e38;
            k = -1;
            for (i = 0; i <= n - j; i++)
            {
               if (Math.Abs(x[i]) < xmin)
               {
                  xmin = Math.Abs(x[i]);
                  k = i;
               }
               if (x[i] != 0.0) y[i] = (y[i] - cof[j]) / x[i];
            }
            for (i = k + 1; i <= n - j; i++)
            {
               y[i - 1] = y[i];
               x[i - 1] = x[i];
            }
         }
         return cof;
      }
   }
}