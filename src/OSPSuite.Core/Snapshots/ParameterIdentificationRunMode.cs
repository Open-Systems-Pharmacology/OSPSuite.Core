using System.Collections.Generic;

namespace OSPSuite.Core.Snapshots
{
   public class ParameterIdentificationRunMode
   {
      public int? NumberOfRuns { get; set; }
      public CalculationMethodCache AllTheSameSelection { get; set; }
      public Dictionary<string, CalculationMethodCache> CalculationMethods { get; set; }
   }
}