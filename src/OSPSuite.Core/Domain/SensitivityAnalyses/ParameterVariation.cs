using System.Collections.Generic;

namespace OSPSuite.Core.Domain.SensitivityAnalyses
{
   public class ParameterVariation
   {
      /// <summary>
      ///    Name of the parameter being varied
      /// </summary>
      public string ParameterName { get; }

      /// <summary>
      ///    Id of the variation in the variation table (0-based)
      /// </summary>
      public int VariationId { get; }

      /// <summary>
      ///    List of all parameters that will be set in the simulation (one entire row in the variation table)
      /// </summary>
      public IReadOnlyList<double> Variation { get; }

      /// <summary>
      ///    Index of parameter value for the variation (to be found at Variation[ValueIndex])
      /// </summary>
      public int ParameterIndex { get; }

      /// <summary>
      ///    Returns the actual parameter value for this variation
      /// </summary>
      public double ParameterValue => Variation[ParameterIndex];

      public ParameterVariation(string parameterName, int parameterIndex, int variationId, IReadOnlyList<double> variation)
      {
         ParameterName = parameterName;
         VariationId = variationId;
         Variation = variation;
         ParameterIndex = parameterIndex;
      }
   }
}