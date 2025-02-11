using System.Linq;
using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Utility.Collections;

namespace OSPSuite.Presentation.DTO
{
   public class TableFormulaDTO
   {
      private readonly TableFormula _tableFormula;

      public TableFormulaDTO(TableFormula tableFormula)
      {
         _tableFormula = tableFormula;
         AllPoints = new NotifyList<ValuePointDTO>(_tableFormula.AllPoints.Select(x => new ValuePointDTO(_tableFormula, x)));
      }

      public bool UseDerivedValues => _tableFormula.UseDerivedValues;
      public INotifyList<ValuePointDTO> AllPoints { get; set; }
   }
}