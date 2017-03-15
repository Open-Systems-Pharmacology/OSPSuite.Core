using System.Collections.Generic;

namespace OSPSuite.Core.Domain.ParameterIdentifications
{
   public static class ParameterConstraintExtensions
   {
      public static IReadOnlyList<OptimizedParameterValue> ToParameterValues(this IReadOnlyList<OptimizedParameterConstraint> parameterConstraints, double[] values)
      {
         var parameterValues = new OptimizedParameterValue[parameterConstraints.Count];
         for (int i = 0; i < parameterConstraints.Count; i++)
         {
            parameterValues[i] = new OptimizedParameterValue(parameterConstraints[i].Name, values[i], parameterConstraints[i].StartValue);
         }
         return parameterValues;
      }
   }
}