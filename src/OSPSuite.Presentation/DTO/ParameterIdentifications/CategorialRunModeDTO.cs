using System.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Presentation.DTO.ParameterIdentifications
{
   public class CategorialRunModeDTO
   {
      public CategorialParameterIdentificationRunMode CategorialRunMode { get; }

      public CategorialRunModeDTO(CategorialParameterIdentificationRunMode categorialRunMode)
      {
         CategorialRunMode = categorialRunMode;
      }

      public DataTable CalculationMethodSelectionTable { get; set; }

      public bool ShouldShowSelection => CalculationMethodSelectionTable.Rows.Count > 0;

      public bool AllTheSame
      {
         get { return CategorialRunMode.AllTheSame; }
         set { CategorialRunMode.AllTheSame = value; }
      }
   }
}