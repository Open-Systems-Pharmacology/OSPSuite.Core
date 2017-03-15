using OSPSuite.Core.Maths.Random;

namespace OSPSuite.Core.Maths.Statistics
{
   public interface IDistribution
   {
      /// <summary>
      /// returns the percentile for the given value 
      /// </summary>
      double CalculatePercentileForValue(double value);

      /// <summary>
      /// returns the value for the given percentile
      /// </summary>
      double CalculateValueFromPercentile(double percentile);

      /// <summary>
      /// returns the value of the probability density function for the given value
      /// </summary>
      double ProbabilityDensityFor(double value);

      /// <summary>
      /// Calculate a random deviate for the distribution in the default interval as defined in the distribution
      /// using the randomGenerator to create random values
      /// </summary>
      double RandomDeviate(RandomGenerator randomGenerator);

      /// <summary>
      /// Calculate a random deviate for the distribution between in the interval [min;max]
      /// using the randomGenerator to create random values;
      /// </summary>
      double RandomDeviate(RandomGenerator randomGenerator, double min, double max);
   }
}