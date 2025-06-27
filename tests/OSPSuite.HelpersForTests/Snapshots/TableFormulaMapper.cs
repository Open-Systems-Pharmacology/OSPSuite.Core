using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.UnitSystem;

namespace OSPSuite.Helpers.Snapshots
{
   public class TableFormulaMapper : Core.Snapshots.Mappers.TableFormulaMapper
   {
      protected override TableFormula CreateNewTableFormula()
      {
         return new TableFormula();
      }

      protected override IDimension DimensionByName(string dimensionName)
      {
         return DimensionFactoryForSpecs.Factory.Dimension(dimensionName);
      }
   }
}