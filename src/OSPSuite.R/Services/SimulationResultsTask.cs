using System.Threading;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.R.Domain;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.R.Services
{
   public interface ISimulationResultsTask
   {
      /// <summary>
      ///    Exports simulation results to the csv file with path <paramref name="csvFile" />
      /// </summary>
      void ExportResultsToCSV(SimulationResults simulationResults, IModelCoreSimulation simulation, string csvFile);

      /// <summary>
      ///    Import simulation results from one or more csv files defined in <paramref name="csvFiles" />
      /// </summary>
      SimulationResults ImportResultsFromCSV(IModelCoreSimulation simulation, params string[] csvFiles);
   }

   public class SimulationResultsTask : ISimulationResultsTask
   {
      private readonly ISimulationResultsToDataTableConverter _simulationResultsToDataTableConverter;
      private readonly ISimulationResultsImportTask _simulationResultsImportTask;
      private readonly RLogger _logger;

      public SimulationResultsTask(
         ISimulationResultsToDataTableConverter simulationResultsToDataTableConverter,
         ISimulationResultsImportTask simulationResultsImportTask, RLogger logger)
      {
         _simulationResultsToDataTableConverter = simulationResultsToDataTableConverter;
         _simulationResultsImportTask = simulationResultsImportTask;
         _logger = logger;
      }

      public void ExportResultsToCSV(SimulationResults simulationResults, IModelCoreSimulation simulation, string csvFile)
      {
         if (simulationResults.IsNull() || simulationResults.Count == 0)
         {
            _logger.AddWarning(Error.NoResultsAvailableForExportToCSV);
            return;
         }

         var dataTable = _simulationResultsToDataTableConverter.ResultsToDataTable(simulationResults, simulation);
         dataTable.ExportToCSV(csvFile);
      }

      public SimulationResults ImportResultsFromCSV(IModelCoreSimulation simulation, params string[] csvFiles)
      {
         var simulationResultsImport = _simulationResultsImportTask.ImportResults(simulation, csvFiles, CancellationToken.None, showImportProgress: false).Result;
         simulationResultsImport.ThrowOnError();

         _logger.Log(simulationResultsImport);
         return simulationResultsImport.SimulationResults;
      }
   }
}