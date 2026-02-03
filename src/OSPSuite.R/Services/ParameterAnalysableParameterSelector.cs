using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.R.Services
{
   public class ParameterAnalysableParameterSelector : AbstractParameterAnalysableParameterSelector
   {
      public override bool CanUseParameter(IParameter parameter)
      {
         return parameter.CanBeVaried
                && !parameter.Info.ReadOnly
                && !ParameterIsTable(parameter)
                && !ParameterIsCategorial(parameter);
      }

      public override ParameterGroupingModeForParameterAnalyzable DefaultParameterSelectionMode { get; } = ParameterGroupingModesForParameterAnalyzable.Simple;
   }
}