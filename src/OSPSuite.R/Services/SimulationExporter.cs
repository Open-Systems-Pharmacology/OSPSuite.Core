using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.R.Services
{
   public interface ISimulationExporter
   {
      /// <summary>
      ///    Exports simulation results to the csv file with path <paramref name="fileName" />
      /// </summary>
      void ExportResultsToCSV(SimulationResults simulationResults, IModelCoreSimulation simulation, string fileName);

      void ExportPKAnalysesToCSV(PopulationSimulationPKAnalyses pkAnalyses, IModelCoreSimulation simulation, string fileName);
   }

   public class SimulationExporter : ISimulationExporter
   {
      private readonly ISimulationResultsToDataTableConverter _simulationResultsToDataTableConverter;

      public SimulationExporter(ISimulationResultsToDataTableConverter simulationResultsToDataTableConverter)
      {
         _simulationResultsToDataTableConverter = simulationResultsToDataTableConverter;
      }

      public void ExportResultsToCSV(SimulationResults simulationResults, IModelCoreSimulation simulation, string fileName)
      {
         var dataTable = _simulationResultsToDataTableConverter.ResultsToDataTable(simulationResults, simulation).Result;
         dataTable.ExportToCSV(fileName);
      }

      public void ExportPKAnalysesToCSV(PopulationSimulationPKAnalyses pkAnalyses, IModelCoreSimulation simulation, string fileName)
      {
         var dataTable = _simulationResultsToDataTableConverter.PKAnalysesToDataTable(pkAnalyses, simulation).Result;
         dataTable.ExportToCSV(fileName);
      }
   }
}