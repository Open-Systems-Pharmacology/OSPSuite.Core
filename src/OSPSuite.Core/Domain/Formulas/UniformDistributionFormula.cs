using System.Collections.Generic;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Maths.Random;
using OSPSuite.Core.Maths.Statistics;

namespace OSPSuite.Core.Domain.Formulas
{
   public class UniformDistributionFormula : DistributionFormula
   {
      protected override double CalculateFor(IEnumerable<IObjectReference> usedObjects, IUsingFormula refObject)
      {
         var distributedParameter = refObject.ConvertedTo<IDistributedParameter>();
         var percentile = distributedParameter.Percentile;
         return CalculateValueFromPercentile(percentile, refObject);
      }

      public override double CalculatePercentileForValue(double value, IUsingFormula refObject)
      {
         return uniformDistributionFor(refObject).CalculatePercentileForValue(value);
      }

      public override double CalculateValueFromPercentile(double percentile, IUsingFormula refObject)
      {
         return uniformDistributionFor(refObject).CalculateValueFromPercentile(percentile);
      }

      public override double ProbabilityDensityFor(double value, IUsingFormula refObject)
      {
         return uniformDistributionFor(refObject).ProbabilityDensityFor(value);
      }

      public override double RandomDeviate(RandomGenerator randomGenerator, IUsingFormula refObject, double min, double max)
      {
         return uniformDistributionFor(refObject).RandomDeviate(randomGenerator, min, max);
      }

      public override double RandomDeviate(RandomGenerator randomGenerator, IUsingFormula refObject)
      {
         return uniformDistributionFor(refObject).RandomDeviate(randomGenerator);
      }

      private UniformDistribution uniformDistributionFor(IUsingFormula dependentObject)
      {
         return new UniformDistribution(Minimum(dependentObject), Maximum(dependentObject));
      }

      protected virtual double Minimum(IUsingFormula refObject)
      {
         return GetReferencedEntityByAlias(Constants.Distribution.MINIMUM, refObject).Value;
      }

      protected virtual double Maximum(IUsingFormula refObject)
      {
         return GetReferencedEntityByAlias(Constants.Distribution.MAXIMUM, refObject).Value;
      }
   }
}