using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.R.Services
{
   public interface ISimulationExporter
   {
      void ExportPKAnalysesToCSV(PopulationSimulationPKAnalyses pkAnalyses, IModelCoreSimulation simulation, string fileName);
   }

   public class SimulationExporter : ISimulationExporter
   {
      private readonly ISimulationResultsToDataTableConverter _simulationResultsToDataTableConverter;

      public SimulationExporter(ISimulationResultsToDataTableConverter simulationResultsToDataTableConverter)
      {
         _simulationResultsToDataTableConverter = simulationResultsToDataTableConverter;
      }

      public void ExportPKAnalysesToCSV(PopulationSimulationPKAnalyses pkAnalyses, IModelCoreSimulation simulation, string fileName)
      {
         var dataTable = _simulationResultsToDataTableConverter.PKAnalysesToDataTable(pkAnalyses, simulation);
         dataTable.ExportToCSV(fileName);
      }
   }
}