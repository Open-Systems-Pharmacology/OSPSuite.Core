using System;
using System.Data;

namespace OSPSuite.Core.Maths.RootFinding
{
    /// <summary>
    /// find the root of a function func known to lie between x1 and x2. The
    /// root, returned as zbrent, will be refined until its accuracy is tol.
    /// Numerical recipe page 361
    /// </summary>
    public static class Zbrent
    {
        private static int ITMAX = 100;
        private static double EPS = 3.0e-8;

        public static double Compute(Func<double,double> func,double x1, double x2, double tol)
        {
            int iter;
            double a = x1, b = x2, c = x2, d = 0.0, e = 0.0, min1, min2;
            double fa = func(a), fb = func(b), fc, p, q, r, s, tol1, xm;

            if ((fa > 0.0 && fb > 0.0) || (fa < 0.0 && fb < 0.0))
                throw new InvalidConstraintException("Root must be bracketed in zbrent");

            fc = fb;
            for (iter = 1; iter <= ITMAX; iter++)
            {
                if ((fb > 0.0 && fc > 0.0) || (fb < 0.0 && fc < 0.0))
                {
                    c = a;
                    fc = fa;
                    e = d = b - a;
                }
                if (System.Math.Abs(fc) < System.Math.Abs(fb))
                {
                    a = b;
                    b = c;
                    c = a;
                    fa = fb;
                    fb = fc;
                    fc = fa;
                }
                tol1 = 2.0 * EPS * System.Math.Abs(b) + 0.5 * tol;
                xm = 0.5 * (c - b);
                if (System.Math.Abs(xm) <= tol1 || fb == 0.0) return b;
                if (System.Math.Abs(e) >= tol1 && System.Math.Abs(fa) > System.Math.Abs(fb))
                {
                    s = fb / fa;
                    if (a == c)
                    {
                        p = 2.0 * xm * s;
                        q = 1.0 - s;
                    }
                    else
                    {
                        q = fa / fc;
                        r = fb / fc;
                        p = s * (2.0 * xm * q * (q - r) - (b - a) * (r - 1.0));
                        q = (q - 1.0) * (r - 1.0) * (s - 1.0);
                    }
                    if (p > 0.0) q = -q;
                    p = System.Math.Abs(p);
                    min1 = 3.0 * xm * q - System.Math.Abs(tol1 * q);
                    min2 = System.Math.Abs(e * q);
                    if (2.0 * p < (min1 < min2 ? min1 : min2))
                    {
                        e = d;
                        d = p / q;
                    }
                    else
                    {
                        d = xm;
                        e = d;
                    }
                }
                else
                {
                    d = xm;
                    e = d;
                }
                a = b;
                fa = fb;
                if (System.Math.Abs(d) > tol1)
                    b += d;
                else
                    b += System.Math.Sign(tol1) * xm;
                fb = func(b);
            }
            throw new ConstraintException("Maximum number of iterations exceeded in zbrent");
        }
    }
}