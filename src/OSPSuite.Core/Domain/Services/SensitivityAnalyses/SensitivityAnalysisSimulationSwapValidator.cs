using OSPSuite.Assets;
using OSPSuite.Core.Domain.SensitivityAnalyses;

namespace OSPSuite.Core.Domain.Services.SensitivityAnalyses
{
   public interface ISensitivityAnalysisSimulationSwapValidator
   {
      ValidationResult ValidateSwap(SensitivityAnalysis sensitivityAnalysis, ISimulation oldSimulation, ISimulation newSimulation);
   }

   public class SensitivityAnalysisSimulationSwapValidator : SensitivityAnalysisSimulationSwap, ISensitivityAnalysisSimulationSwapValidator
   {
      public SensitivityAnalysisSimulationSwapValidator(ISimulationQuantitySelectionFinder simulationQuantitySelectionFinder) : base(simulationQuantitySelectionFinder)
      {
      }

      private ValidationResult _validationResult;

      public ValidationResult ValidateSwap(SensitivityAnalysis sensitivityAnalysis, ISimulation oldSimulation, ISimulation newSimulation)
      {
         _validationResult = new ValidationResult();
         FindProblems(sensitivityAnalysis, oldSimulation, newSimulation);

         return _validationResult;
      }

      protected override void ActionForMissingSensitivityParameterPaths(SensitivityAnalysis sensitivityAnalysis, ISimulation newSimulation, SensitivityParameter sensitivityParameter)
      {
         _validationResult.AddMessage(
            NotificationType.Warning,
            sensitivityAnalysis,
            Error.SensitivityAnalysis.SimulationDoesNotHaveParameterPath(newSimulation.Name, sensitivityParameter.ParameterSelection.Path));
      }
   }
}