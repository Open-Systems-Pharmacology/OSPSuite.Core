using System.Collections.Generic;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Maths.Random;
using OSPSuite.Core.Maths.Statistics;

namespace OSPSuite.Core.Domain.Formulas
{
   public class NormalDistributionFormula : DistributionFormula
   {
      protected override double CalculateFor(IEnumerable<IObjectReference> usedObjects, IUsingFormula dependentObject)
      {
         var locDependentObject = dependentObject.ConvertedTo<IDistributedParameter>();
         var percentile = locDependentObject.Percentile;
         return CalculateValueFromPercentile(percentile, locDependentObject);
      }

      public override double CalculatePercentileForValue(double value, IUsingFormula dependentObject)
      {
         return normalDistributionFor(dependentObject).CalculatePercentileForValue(value);
      }

      public override double CalculateValueFromPercentile(double percentile, IUsingFormula dependentObject)
      {
         return normalDistributionFor(dependentObject).CalculateValueFromPercentile(percentile);
      }

      public override double ProbabilityDensityFor(double value, IUsingFormula dependentObject)
      {
         return normalDistributionFor(dependentObject).ProbabilityDensityFor(value);
      }

      public override double RandomDeviate(RandomGenerator randomGenerator, IUsingFormula refObject, double min, double max)
      {
         return normalDistributionFor(refObject).RandomDeviate(randomGenerator, min, max);
      }

      public override double RandomDeviate(RandomGenerator randomGenerator, IUsingFormula refObject)
      {
         return normalDistributionFor(refObject).RandomDeviate(randomGenerator);
      }

      private NormalDistribution normalDistributionFor(IUsingFormula dependentObject)
      {
         return new NormalDistribution(Mean(dependentObject), Deviation(dependentObject));
      }

   }
}