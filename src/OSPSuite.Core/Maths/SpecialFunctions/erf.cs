namespace OSPSuite.Core.Maths.SpecialFunctions
{
    public static class Erf
    {
        //Numerical recipe page 221 with erfc(x) = 1 - erf(x)
        public static double Compute(double x)
        {
            double z = System.Math.Abs(x);
            double t = 1.0 / (1.0 + 0.5 * z);
            double ans = t * System.Math.Exp(-z * z - 1.26551223 + t * (1.00002368 + t * (0.37409196 + t * (0.09678418 +
                                                                                                            t * (-0.18628806 + t * (0.27886807 + t * (-1.13520398 + t * (1.48851587 +
                                                                                                                                                                         t * (-0.82215223 + t * 0.17087277)))))))));
            ans= x >= 0.0 ? ans : 2.0 - ans;
            return 1 - ans;
        }
    }
}