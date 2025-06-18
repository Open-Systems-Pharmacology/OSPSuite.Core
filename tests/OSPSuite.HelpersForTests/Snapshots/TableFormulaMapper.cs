using OSPSuite.Core.Domain.Formulas;
using OSPSuite.Core.Domain.ParameterIdentifications;
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

   public class ParameterIdentificationRunModeMapper : OSPSuite.Core.Snapshots.Mappers.ParameterIdentificationRunModeMapper
   {
      protected override ParameterIdentificationRunMode RunModeFrom(Core.Snapshots.ParameterIdentificationRunMode snapshot)
      {
         return new StandardParameterIdentificationRunMode();
      }
   }
}