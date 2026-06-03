using System.Collections.Generic;

namespace OSPSuite.Core.Serialization.SimModel.DTO
{
   public abstract class FormulaExport
   {
      public int Id { get; set; }
   }

   public class ExplicitFormulaExport : FormulaExport
   {
      public ExplicitFormulaExport()
      {
         ReferenceList = new Dictionary<string, int>();
      }

      public string Equation { get; set; }
      public IDictionary<string, int> ReferenceList { get; private set; }
   }
}