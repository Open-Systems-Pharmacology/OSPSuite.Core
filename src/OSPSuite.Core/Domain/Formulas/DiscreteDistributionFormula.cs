using System.Collections.Generic;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Maths.Random;

namespace OSPSuite.Core.Domain.Formulas
{
   public class DiscreteDistributionFormula : DistributionFormula
   {
      protected override double CalculateFor(IEnumerable<IObjectReference> usedObjects, IUsingFormula dependentObject)
      {
         var distributedParameter = dependentObject.ConvertedTo<IDistributedParameter>();
         return CalculateValueFromPercentile(distributedParameter.Percentile, distributedParameter);
      }

      public override double CalculatePercentileForValue(double value, IUsingFormula refObject)
      {
         return value < Mean(refObject) ? 0 : 0.5; 
      }

      public override double CalculateValueFromPercentile(double percentile, IUsingFormula refObject)
      {
         return percentile >= 0.5 ? Mean(refObject) : 0; 
      }

      public override double ProbabilityDensityFor(double value, IUsingFormula refObject)
      {
         return value == Mean(refObject) ? 1 : 0;
      }

      public override double RandomDeviate(RandomGenerator randomGenerator, IUsingFormula refObject, double min, double max)
      {
         return Mean(refObject);
      }

      public override double RandomDeviate(RandomGenerator randomGenerator, IUsingFormula refObject)
      {
         return RandomDeviate(randomGenerator, refObject, Mean(refObject), Mean(refObject));
      }

   }
}