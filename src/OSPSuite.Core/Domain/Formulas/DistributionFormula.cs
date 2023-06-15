using OSPSuite.Core.Maths.Random;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Formulas
{
   public abstract class DistributionFormula : Formula
   {
      public DistributionType DistributionType { get; }

      protected DistributionFormula(DistributionType distributionType)
      {
         DistributionType = distributionType;
      }

      /// <summary>
      ///    returns the percentile for the current value of the quantity
      /// </summary>
      public double CalculatePercentile(IQuantity refObject)
      {
         var quantity = refObject.ConvertedTo<Quantity>();
         return CalculatePercentileForValue(quantity.Value, refObject);
      }

      /// <summary>
      ///    returns the percentile for the given value using the parameters defined in the reference object
      /// </summary>
      /// <param name="value">value used to calculate the percentile</param>
      /// <param name="refObject">reference object used to retrieve distribution parameters</param>
      public abstract double CalculatePercentileForValue(double value, IUsingFormula refObject);

      /// <summary>
      ///    returns the value for the given percentile using the parameters defined in the reference object
      /// </summary>
      /// <param name="percentile">percentile used to calculate the value</param>
      /// <param name="refObject">reference object used to retrieve distribution parameters</param>
      public abstract double CalculateValueFromPercentile(double percentile, IUsingFormula refObject);

      /// <summary>
      ///    returns the value of the probability density function for the given value using the parameters defined in the
      ///    reference object
      /// </summary>
      /// <param name="value">value used to calculate the probability density</param>
      /// <param name="refObject">reference object used to retrieve distribution parameters</param>
      public abstract double ProbabilityDensityFor(double value, IUsingFormula refObject);

      /// <summary>
      ///    Calculate a random deviate for the distribution between in the interval [min;max]
      ///    using the randomGenerator to create random values if necessary
      /// </summary>
      /// <param name="randomGenerator">Used to generate uniform or gaussian deviates</param>
      /// <param name="refObject">reference object used to retrieve distribution parameters</param>
      /// <param name="min">The generated value should be bigger than min</param>
      /// <param name="max">The generated value should be smaller than max</param>
      public abstract double RandomDeviate(RandomGenerator randomGenerator, IUsingFormula refObject, double min, double max);

      /// <summary>
      ///    Calculate a random deviate for the distribution in the default interval as defined in the distribution
      ///    using the randomGenerator to create random values if necessary
      /// </summary>
      /// <param name="randomGenerator">Used to generate uniform or gaussian deviates</param>
      /// <param name="refObject">reference object used to retrieve distribution parameters</param>
      public abstract double RandomDeviate(RandomGenerator randomGenerator, IUsingFormula refObject);

      protected virtual double Deviation(IUsingFormula refObject)
      {
         return GetReferencedEntityByAlias(Constants.Distribution.DEVIATION, refObject).Value;
      }

      protected virtual double Mean(IUsingFormula refObject)
      {
         return GetReferencedEntityByAlias(Constants.Distribution.MEAN, refObject).Value;
      }
   }
}