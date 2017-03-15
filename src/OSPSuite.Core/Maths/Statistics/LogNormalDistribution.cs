using System;
using OSPSuite.Core.Maths.Random;
using OSPSuite.Core.Maths.RootFinding;

namespace OSPSuite.Core.Maths.Statistics
{
   public class LogNormalDistribution : IDistribution
   {
      public double Mean { get; private set; }
      public double Deviation { get; private set; }

      private readonly NormalDistribution _standardNormalDistribution;

      public LogNormalDistribution(double mean, double deviation)
      {
         Mean = mean;
         Deviation = deviation;
         _standardNormalDistribution = new NormalDistribution();
      }

      public LogNormalDistribution() : this(0, 1)
      {
      }

      public double CalculatePercentileForValue(double value)
      {
         var compValue = (Math.Log(value) - Mean)/Deviation;
         return _standardNormalDistribution.CalculatePercentileForValue(compValue);
      }

      public double CalculateValueFromPercentile(double percentile)
      {
         return Zbrent.Compute(x => CalculatePercentileForValue(x) - percentile,
                               Math.Exp(Mean)/Math.Pow(Math.Exp(Deviation), 10),
                               Math.Exp(Mean)*Math.Pow(Math.Exp(Deviation), 10),
                               0.0000000000001);
      }

      public double ProbabilityDensityFor(double value)
      {
         if (value < 0) return 0;

         double alpha = 1/(Deviation*value*Math.Sqrt(2*Math.PI));
         double beta = 2*Deviation*Deviation;

         return alpha*Math.Exp(-Math.Pow(Math.Log(value) - Mean, 2)/beta);
      }

      public double RandomDeviate(RandomGenerator randomGenerator)
      {
         return RandomDeviate(randomGenerator, double.NegativeInfinity, double.PositiveInfinity);
      }

      public double RandomDeviate(RandomGenerator randomGenerator, double min, double max)
      {
         if (Deviation <= 0)
            return Math.Exp(Mean);

         if (min > max ||  Math.Exp(Mean - 3 * Deviation) > max || Math.Exp(Mean + 3 * Deviation) < min)
            throw new LimitsNotInLogNormalDistributionFunctionRangeException(min, max, Math.Exp(Mean), Deviation);

         if (min == max)
            return min;

         double result;
         do
         {
            result = Math.Exp(Mean + Deviation*randomGenerator.NormalDeviate());
         } while (result < min || result > max);
         return result;
      }
   }
}