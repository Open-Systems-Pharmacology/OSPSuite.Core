using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;

namespace OSPSuite.Starter.Services
{
   public class ParameterAnalysableParameterSelector : AbstractParameterAnalysableParameterSelector, IParameterAnalysableParameterSelector
   {
      public override bool CanUseParameter(IParameter parameter)
      {
         return true;
      }

      public override ParameterGroupingMode DefaultParameterSelectionMode =>ParameterGroupingModes.Simple;
   }
}