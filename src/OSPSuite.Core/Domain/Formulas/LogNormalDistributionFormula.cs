using System;
using System.Collections.Generic;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Maths.Random;
using OSPSuite.Core.Maths.Statistics;

namespace OSPSuite.Core.Domain.Formulas
{
   public class LogNormalDistributionFormula : DistributionFormula
   {
      protected override double CalculateFor(IEnumerable<IObjectReference> usedObjects, IUsingFormula refObject)
      {
         var distributedParameter = refObject.ConvertedTo<IDistributedParameter>();
         var percentile = distributedParameter.Percentile;
         return CalculateValueFromPercentile(percentile, distributedParameter);
      }

      public override double CalculatePercentileForValue(double value, IUsingFormula refObject)
      {
         return logNormalDistributionFor(refObject).CalculatePercentileForValue(value);
      }

      public override double CalculateValueFromPercentile(double percentile, IUsingFormula refObject)
      {
         return logNormalDistributionFor(refObject).CalculateValueFromPercentile(percentile);
      }

      public override double ProbabilityDensityFor(double value, IUsingFormula refObject)
      {
         return logNormalDistributionFor(refObject).ProbabilityDensityFor(value);
      }

      public override double RandomDeviate(RandomGenerator randomGenerator, IUsingFormula refObject, double min, double max)
      {
         return logNormalDistributionFor(refObject).RandomDeviate(randomGenerator, min, max);
      }

      public override double RandomDeviate(RandomGenerator randomGenerator, IUsingFormula refObject)
      {
         return logNormalDistributionFor(refObject).RandomDeviate(randomGenerator);
      }

      private LogNormalDistribution logNormalDistributionFor(IUsingFormula refObject)
      {
         return new LogNormalDistribution(Math.Log(Mean(refObject)), Math.Log(Deviation(refObject)));
      }

      protected override double Deviation(IUsingFormula refObject)
      {
         return GetReferencedEntityByAlias(Constants.Distribution.GEOMETRIC_DEVIATION, refObject).Value;
      }
   }
}