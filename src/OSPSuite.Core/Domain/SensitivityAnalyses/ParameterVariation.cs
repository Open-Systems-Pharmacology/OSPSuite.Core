using System.Collections.Generic;

namespace OSPSuite.Core.Domain.SensitivityAnalyses
{
   public class ParameterVariation
   {
      public string ParameterName { get; set; }
      public int VariationId { get; set; }
      public IReadOnlyList<double> Variation { get; set; }
   }
}