using System.Collections.Generic;

namespace OSPSuite.Core.Serialization.SimModel.DTO
{
   public class VariableExport : QuantityExport
   {
      public VariableExport()
      {
         RHSIds = new List<int>();
         NegativeValuesAllowed = true;
      }

      public IList<int> RHSIds { get; set; }
      public double ScaleFactor { get; set; }

      public bool NegativeValuesAllowed { get; set; }
   }
}