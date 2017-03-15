using OSPSuite.Assets;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public interface IParameterIdentificationSimulationSwapValidator
   {
      ValidationResult ValidateSwap(ParameterIdentification parameterIdentification, ISimulation oldSimulation, ISimulation newSimulation);
   }

   public class ParameterIdentificationSimulationSwapValidator : ParameterIdentificationSimulationSwap, IParameterIdentificationSimulationSwapValidator
   {
      public ParameterIdentificationSimulationSwapValidator(ISimulationQuantitySelectionFinder simulationQuantitySelectionFinder) : base(simulationQuantitySelectionFinder)
      {
      }

      private ValidationResult _validationResult;

      public ValidationResult ValidateSwap(ParameterIdentification parameterIdentification, ISimulation oldSimulation, ISimulation newSimulation)
      {
         _validationResult = new ValidationResult();
         FindProblems(parameterIdentification, oldSimulation, newSimulation);

         return _validationResult;
      }

      protected override void ActionForLinkedParameterIsNotInSimulation(ParameterIdentification parameterIdentification, ISimulation newSimulation, ParameterSelection linkedParameter, IdentificationParameter identificationParameter)
      {
         _validationResult.AddMessage(
            NotificationType.Warning, 
            parameterIdentification, 
            Captions.ParameterIdentification.SimulationDoesNotContainParameterWithPath(newSimulation.Name, linkedParameter.Path));
      }

      protected override void ActionForSimulationDoesNotUseObservedData(ParameterIdentification parameterIdentification, ISimulation newSimulation, OutputMapping outputMapping)
      {
         _validationResult.AddMessage(
            NotificationType.Warning, 
            parameterIdentification, 
            Captions.ParameterIdentification.SimulationDoesNotUseObservedData(newSimulation.Name, outputMapping.WeightedObservedData.ObservedData.Name));
      }

      protected override void ActionForSimulationDoesNotHaveOutputPath(ParameterIdentification parameterIdentification, ISimulation newSimulation, OutputMapping outputMapping)
      {
         _validationResult.AddMessage(
            NotificationType.Warning, 
            parameterIdentification, 
            Captions.ParameterIdentification.SimulationDoesNotHaveOutputPath(newSimulation.Name, outputMapping.OutputPath));
      }
   }
}