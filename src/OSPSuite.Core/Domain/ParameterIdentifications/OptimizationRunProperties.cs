using System;

namespace OSPSuite.Core.Domain.ParameterIdentifications
{
   public class OptimizationRunProperties
   {
      public virtual int NumberOfEvaluations { get; }

      [Obsolete("For serialization")]
      public OptimizationRunProperties()
      {
      }

      public OptimizationRunProperties(int numberOfEvaluations)
      {
         NumberOfEvaluations = numberOfEvaluations;
      }
   }
}