using System;
using OSPSuite.Core.Maths.Random;

namespace OSPSuite.Core.Maths.Statistics
{
   public class UniformDistribution : IDistribution
   {
      public double Min { get; private set; }
      public double Max { get; private set; }

      public UniformDistribution(double min, double max)
      {
         Min = min;
         Max = max;
      }

      public double CalculatePercentileForValue(double value)
      {
         return (value - Min) / (Max - Min);
      }

      public double CalculateValueFromPercentile(double percentile)
      {
         return Min + percentile * (Max - Min);
      }

      public double ProbabilityDensityFor(double value)
      {
         throw new NotSupportedException();
      }

      public double RandomDeviate(RandomGenerator randomGenerator)
      {
         return randomGenerator.UniformDeviate(Min, Max);
      }

      public double RandomDeviate(RandomGenerator randomGenerator, double min, double max)
      {
         //Interval does not make sense
         if (min > max || min > Max || max < Min || Min > Max)
            throw new LimitsNotInUniformDistributionFunctionRangeException(min, max, Min, Max);

         if (min == max)
            return min;

         return randomGenerator.UniformDeviate(Math.Max(min,Min), Math.Min(max, Max));
      }
   }
}