using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public abstract class ParameterIdentificationSimulationSwap
   {
      private readonly ISimulationQuantitySelectionFinder _simulationQuantitySelectionFinder;
      protected abstract void ActionForLinkedParameterIsNotInSimulation(ParameterIdentification parameterIdentification, ISimulation newSimulation, ParameterSelection linkedParameter, IdentificationParameter identificationParameter);
      protected abstract void ActionForSimulationDoesNotUseObservedData(ParameterIdentification parameterIdentification, ISimulation newSimulation, OutputMapping outputMapping);
      protected abstract void ActionForSimulationDoesNotHaveOutputPath(ParameterIdentification parameterIdentification, ISimulation newSimulation, OutputMapping outputMapping);

      protected ParameterIdentificationSimulationSwap(ISimulationQuantitySelectionFinder simulationQuantitySelectionFinder)
      {
         _simulationQuantitySelectionFinder = simulationQuantitySelectionFinder;
      }

      protected void FindMissingLinkedParameterPaths(ParameterIdentification parameterIdentification, ISimulation oldSimulation, ISimulation newSimulation)
      {
         parameterIdentification.AllIdentificationParameters.Each(x => findMissingLinkedParameterPaths(x, parameterIdentification, oldSimulation, newSimulation));
      }

      private void findMissingLinkedParameterPaths(IdentificationParameter identificationParameter, ParameterIdentification parameterIdentification, ISimulation oldSimulation, ISimulation newSimulation)
      {
         var linkedParametersNotInSimulation = identificationParameter.LinkedParametersFor(oldSimulation).Where(linkedParameter => !_simulationQuantitySelectionFinder.SimulationHasSelection(linkedParameter, newSimulation));
         linkedParametersNotInSimulation.ToList().Each(parameter =>
            ActionForLinkedParameterIsNotInSimulation(parameterIdentification, newSimulation, parameter, identificationParameter)
            );
      }

      protected void FindMissingObservedData(ParameterIdentification parameterIdentification, ISimulation oldSimulation, ISimulation newSimulation)
      {
         var mappingUsingSimulation = parameterIdentification.AllOutputMappingsFor(oldSimulation);
         mappingUsingSimulation.Where(mapping => !newSimulation.UsesObservedData(mapping.WeightedObservedData.ObservedData)).ToList().Each(mapping =>
            ActionForSimulationDoesNotUseObservedData(parameterIdentification, newSimulation, mapping));
      }

      protected void FindMissingOutputPaths(ParameterIdentification parameterIdentification, ISimulation oldSimulation, ISimulation newSimulation)
      {
         var outputMappings = parameterIdentification.AllOutputMappingsFor(oldSimulation);
         outputMappings.Where(outputMapping => !simulationHasEquivalentPath(outputMapping.OutputPath, newSimulation)).ToList().Each(outputPath => 
            ActionForSimulationDoesNotHaveOutputPath(parameterIdentification, newSimulation, outputPath));
      }

      private static bool simulationHasEquivalentPath(string outputPath, ISimulation newSimulation)
      {
         return newSimulation.OutputSelections.Any(selection => Equals(outputPath, selection.Path));
      }

      protected void FindProblems(ParameterIdentification parameterIdentification, ISimulation oldSimulation, ISimulation newSimulation)
      {
         FindMissingOutputPaths(parameterIdentification, oldSimulation, newSimulation);
         FindMissingObservedData(parameterIdentification, oldSimulation, newSimulation);
         FindMissingLinkedParameterPaths(parameterIdentification, oldSimulation, newSimulation);
      }
   }
}