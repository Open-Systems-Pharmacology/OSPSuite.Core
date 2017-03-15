using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public interface IParameterIdentificationSimulationSwapCorrector
   {
      void CorrectParameterIdentification(ParameterIdentification parameterIdentification, ISimulation oldSimulation, ISimulation newSimulation);
   }

   public class ParameterIdentificationSimulationSwapCorrector : ParameterIdentificationSimulationSwap, IParameterIdentificationSimulationSwapCorrector
   {
      public ParameterIdentificationSimulationSwapCorrector(ISimulationQuantitySelectionFinder simulationQuantitySelectionFinder) : base(simulationQuantitySelectionFinder)
      {
      }

      public void CorrectParameterIdentification(ParameterIdentification parameterIdentification, ISimulation oldSimulation, ISimulation newSimulation)
      {
         FindProblems(parameterIdentification, oldSimulation, newSimulation);
      }

      protected override void ActionForLinkedParameterIsNotInSimulation(ParameterIdentification parameterIdentification, ISimulation newSimulation, ParameterSelection linkedParameter, IdentificationParameter identificationParameter)
      {
         identificationParameter.RemovedLinkedParameter(linkedParameter);
      }

      protected override void ActionForSimulationDoesNotUseObservedData(ParameterIdentification parameterIdentification, ISimulation newSimulation, OutputMapping outputMapping)
      {
         parameterIdentification.RemoveOutputMapping(outputMapping);
      }

      protected override void ActionForSimulationDoesNotHaveOutputPath(ParameterIdentification parameterIdentification, ISimulation newSimulation, OutputMapping outputMapping)
      {
         parameterIdentification.RemoveOutputMapping(outputMapping);
      }
   }
}