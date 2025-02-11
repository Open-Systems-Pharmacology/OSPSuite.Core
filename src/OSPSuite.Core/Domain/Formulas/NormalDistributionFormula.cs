using System.Collections.Generic;
using OSPSuite.Core.Maths.Random;
using OSPSuite.Core.Maths.Statistics;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Core.Domain.Formulas
{
   public class NormalDistributionFormula : DistributionFormula
   {
      public NormalDistributionFormula() : base(DistributionType.Normal)
      {
      }

      protected override double CalculateFor(IReadOnlyList<IObjectReference> usedObjects, IUsingFormula dependentObject)
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