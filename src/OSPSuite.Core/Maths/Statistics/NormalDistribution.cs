using System;
using OSPSuite.Core.Maths.Random;
using OSPSuite.Core.Maths.RootFinding;
using OSPSuite.Core.Maths.SpecialFunctions;

namespace OSPSuite.Core.Maths.Statistics
{
   public class NormalDistribution : IDistribution
   {
      public double Mean { get; private set; }
      public double Deviation { get; private set; }

      public NormalDistribution(double mean, double deviation)
      {
         Mean = mean;
         Deviation = deviation;
      }

      public NormalDistribution() : this(0, 1)
      {
      }

      public double CalculatePercentileForValue(double value)
      {
         double alpha = (value - Mean)/(Deviation*Math.Sqrt(2));
         return 0.5*(1 + Erf.Compute(alpha));
      }

      public double CalculateValueFromPercentile(double percentile)
      {
         return Zbrent.Compute(x => CalculatePercentileForValue(x) - percentile,
                               Mean - 10*Deviation,
                               Mean + 10*Deviation,
                               0.0000000000001);
      }

      public double ProbabilityDensityFor(double value)
      {
         double alpha = 1/(Deviation*Math.Sqrt(2*Math.PI));
         double beta = 2*Deviation*Deviation;

         return alpha*Math.Exp(-Math.Pow(value - Mean, 2)/beta);
      }

      public double RandomDeviate(RandomGenerator randomGenerator)
      {
         return RandomDeviate(randomGenerator, double.NegativeInfinity, double.PositiveInfinity);
      }

      public double RandomDeviate(RandomGenerator randomGenerator, double min, double max)
      {
         if (min > max || Mean - 3 * Deviation > max || Mean + 3 * Deviation < min)
            throw new LimitsNotInNormalDistributionFunctionRangeException(min, max, Mean, Deviation);

         if (min == max)
            return min;
       
         double result;
         do
         {
            result = randomGenerator.NormalDeviate()*Deviation + Mean;
         } while (result < min || result > max);
         return result;
      }
   }
}