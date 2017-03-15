using System.Collections.Generic;
using System.Linq;
using OSPSuite.Utility.Extensions;
using OSPSuite.Core.Commands;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Core.Domain.Services.ParameterIdentifications
{
   public interface IParameterIdentificationSimulationPathUpdater
   {
      /// <summary>
      /// Updates paths in all parameter identifications using <paramref name="simulation"/> to use the <paramref name="newName"/> in place of <paramref name="oldName"/>
      /// </summary>
      void UpdatePathsForRenamedSimulation(ISimulation simulation, string oldName, string newName);
   }

   public class ParameterIdentificationSimulationPathUpdater : IParameterIdentificationSimulationPathUpdater
   {
      private readonly IOSPSuiteExecutionContext _executionContext;

      public ParameterIdentificationSimulationPathUpdater(IOSPSuiteExecutionContext executionContext)
      {
         _executionContext = executionContext;
      }

      public void UpdatePathsForRenamedSimulation(ISimulation simulation, string oldName, string newName)
      {
         var allIdentifications = _executionContext.Project.AllParameterIdentifications;

         allIdentifications.Each(identification => updatePaths(identification, simulation, oldName, newName));
      }

      private void updatePaths(ParameterIdentification identification, ISimulation simulation, string oldName, string newName)
      {
         if (!identification.UsesSimulation(simulation))
            return;

         _executionContext.Load(identification);

         updateMappedObservationColumnPaths(identification, simulation, oldName, newName);
         updateSimulationResultColumnPaths(identification, oldName, newName);
         updateResidualOutputsPaths(identification, oldName, newName);
      }

      private void updateResidualOutputsPaths(ParameterIdentification identification, string oldName, string newName)
      {
         if (identification.HasResults)
            identification.Results.SelectMany(result => result.BestResult.AllOutputResiduals).Each(residual => updateOutputResidualPath(residual, oldName, newName));
      }

      private void updateOutputResidualPath(OutputResiduals residual, string oldName, string newName)
      {
         residual.UpdateFullOutputPath(oldName, newName);
      }

      private void updateSimulationResultColumnPaths(ParameterIdentification identification, string oldName, string newName)
      {
         var columnsToModify = identification.Results.SelectMany(allSimulationCalculationColumnsFrom).Where(column => columnUsesOldSimulationName(oldName, column));
         updateColumnPaths(columnsToModify, oldName, newName);
      }

      private static IEnumerable<DataColumn> allSimulationCalculationColumnsFrom(ParameterIdentificationRunResult result)
      {
         return result.BestResult.SimulationResults.SelectMany(x => x.AllButBaseGrid());
      }

      private static bool columnUsesOldSimulationName(string oldName, DataColumn x)
      {
         return Equals(x.QuantityInfo.Path.First(), oldName);
      }

      private void updateMappedObservationColumnPaths(ParameterIdentification identification, ISimulation simulation, string oldName, string newName)
      {
         var columnsToModify = identification.AllOutputMappings.
            Where(mapping => mapping.UsesSimulation(simulation)).
            SelectMany(x => x.WeightedObservedData.ObservedData.AllButBaseGrid());

         updateColumnPaths(columnsToModify, oldName, newName);
      }

      private static void updateColumnPaths(IEnumerable<DataColumn> columnsToModify, string oldName, string newName)
      {
         columnsToModify.Each(column => column.QuantityInfo.Path = pathWithNewSimulation(column, oldName, newName));
      }

      private static IEnumerable<string> pathWithNewSimulation(DataColumn column, string oldName, string newName)
      {
         var withNewSimulation = new ObjectPath(column.QuantityInfo.Path);
         withNewSimulation.Replace(oldName, newName);
         return withNewSimulation;
      }
   }
}