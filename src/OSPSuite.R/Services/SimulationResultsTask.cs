using System.Threading;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.R.Extensions;
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

      public SimulationResultsTask(ISimulationResultsToDataTableConverter simulationResultsToDataTableConverter, ISimulationResultsImportTask simulationResultsImportTask)
      {
         _simulationResultsToDataTableConverter = simulationResultsToDataTableConverter;
         _simulationResultsImportTask = simulationResultsImportTask;
      }

      public void ExportResultsToCSV(SimulationResults simulationResults, IModelCoreSimulation simulation, string csvFile)
      {
         var dataTable = _simulationResultsToDataTableConverter.ResultsToDataTable(simulationResults, simulation);
         dataTable.ExportToCSV(csvFile);
      }

      public SimulationResults ImportResultsFromCSV(IModelCoreSimulation simulation, params string[] csvFiles)
      {
         var simulationResultsImport = _simulationResultsImportTask.ImportResults(simulation, csvFiles, CancellationToken.None, showImportProgress: false).Result;
         simulationResultsImport.LogToR();
         return simulationResultsImport.SimulationResults;
      }
   }
}