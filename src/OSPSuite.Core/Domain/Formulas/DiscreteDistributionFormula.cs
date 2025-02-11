using System.Collections.Generic;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Maths.Random;

namespace OSPSuite.Core.Domain.Formulas
{
   public class DiscreteDistributionFormula : DistributionFormula
   {
      public DiscreteDistributionFormula() : base(DistributionType.Discrete)
      {
      }

      protected override double CalculateFor(IReadOnlyList<IObjectReference> usedObjects, IUsingFormula dependentObject)
      {
         var distributedParameter = dependentObject.ConvertedTo<IDistributedParameter>();
         //percentile not used in this formula
         return CalculateValueFromPercentile(Constants.DEFAULT_PERCENTILE, distributedParameter);
      }

      public override double CalculatePercentileForValue(double value, IUsingFormula refObject)
      {
         return 0.5; 
      }

      public override double CalculateValueFromPercentile(double percentile, IUsingFormula refObject)
      {
         return Mean(refObject); 
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