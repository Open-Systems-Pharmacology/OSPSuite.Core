using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Events;

namespace OSPSuite.Core.Domain.Services
{
   public interface ISimulationReferenceUpdater
   {
      /// <summary>
      /// Removes references to <paramref name="simulation"/> from all parameter identifications and sensitivity analyses in the project
      /// </summary>
      void RemoveSimulationFromParameterIdentificationsAndSensitivityAnalyses(ISimulation simulation);

      void SwapSimulationInParameterAnalysables(ISimulation oldsimulation, ISimulation newSimulation);
   }


   public class SimulationReferenceUpdater : ISimulationReferenceUpdater
   {
      private readonly IOSPSuiteExecutionContext _executionContext;

      public SimulationReferenceUpdater(IOSPSuiteExecutionContext executionContext)
      {
         _executionContext = executionContext;
      }

      public void RemoveSimulationFromParameterIdentificationsAndSensitivityAnalyses(ISimulation simulation)
      {
         removeSimulationFromParameterIdentification(simulation);
         removeSimulationFromSensitivityAnalyses(simulation);
      }

      private void removeSimulationFromParameterIdentification(ISimulation simulation)
      {
         var parameterIdentificationsUsingSimulation = _executionContext.Project.AllParameterIdentifications.Where(pi => pi.AnyOutputOfSimulationMapped(simulation));

         parameterIdentificationsUsingSimulation.Each(identification =>
         {
            _executionContext.Load(identification);
            removeSimulationFromIdentification(simulation, identification);
         });
      }

      private void removeSimulationFromSensitivityAnalyses(ISimulation simulation)
      {
         var allSensitivities = _executionContext.Project.AllSensitivityAnalyses.Where(sensitivityAnalysis => sensitivityAnalysis.IsLoaded);
         allSensitivities.Each(sensitivityAnalysis => removeSimulationFromAnalysis(simulation, sensitivityAnalysis));
      }

      private void removeSimulationFromAnalysis(ISimulation simulation, SensitivityAnalysis sensitivityAnalysis)
      {
         if (!Equals(sensitivityAnalysis.Simulation, simulation))
            return;

         removeSensitivityParameters(sensitivityAnalysis);
         sensitivityAnalysis.Simulation = null;
      }

      private void removeSensitivityParameters(SensitivityAnalysis sensitivityAnalysis)
      {
         sensitivityAnalysis.AllSensitivityParameters.ToList().Each(sensitivityAnalysis.RemoveSensitivityParameter);
      }

      private void removeSimulationFromIdentification(ISimulation simulation, ParameterIdentification identification)
      {
         if (!identification.UsesSimulation(simulation))
            return;

         removeOutputMappingsForSimulation(simulation, identification);
         removeLinkedParametersForSimulation(simulation, identification);
         identification.RemoveSimulation(simulation);
      }

      private static void removeLinkedParametersForSimulation(ISimulation simulation, ParameterIdentification identification)
      {
         identification.AllIdentificationParameters.Each(parameter => parameter.RemovedLinkedParametersForSimulation(simulation));
      }

      private static void removeOutputMappingsForSimulation(ISimulation simulation, ParameterIdentification identification)
      {
         identification.AllOutputMappings.Where(x => x.UsesSimulation(simulation)).ToList().Each(identification.RemoveOutputMapping);
      }

      public void SwapSimulationInParameterAnalysables(ISimulation oldsimulation, ISimulation newSimulation)
      {
         var parameterAnalysables = _executionContext.Project.AllParameterAnalysables.Where(x => x.UsesSimulation(oldsimulation));

         parameterAnalysables.Each(parameterAnalysable => swapSimulationInParameterAnalyzable(oldsimulation, newSimulation, parameterAnalysable));
      }

      private void swapSimulationInParameterAnalyzable(ISimulation oldsimulation, ISimulation newSimulation, IParameterAnalysable parameterAnalysable)
      {
         _executionContext.Load(parameterAnalysable);
         parameterAnalysable.SwapSimulations(oldsimulation, newSimulation);
         _executionContext.PublishEvent(new SimulationReplacedInParameterAnalyzableEvent(parameterAnalysable, oldsimulation, newSimulation));
      }
   }

}