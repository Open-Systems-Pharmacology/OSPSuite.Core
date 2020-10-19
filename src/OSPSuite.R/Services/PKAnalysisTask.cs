using System.Data;
using System.Threading;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Importer.Services;
using OSPSuite.R.Domain;
using OSPSuite.Utility.Extensions;
using ICorePKAnalysisTask = OSPSuite.Core.Domain.Services.IPKAnalysesTask;

namespace OSPSuite.R.Services
{
   /// <summary>
   ///    Required because of optional DynamicParameters dependencies
   /// </summary>
   public class CalculatePKAnalysisArgs
   {
      public IModelCoreSimulation Simulation { get; set; }
      public int NumberOfIndividuals { get; set; }
      public SimulationResults SimulationResults { get; set; }
   }

   public interface IPKAnalysisTask
   {
      void ExportPKAnalysesToCSV(PopulationSimulationPKAnalyses pkAnalyses, IModelCoreSimulation simulation, string fileName);
      DataTable ConvertToDataTable(PopulationSimulationPKAnalyses pkAnalyses, IModelCoreSimulation simulation);
      PopulationSimulationPKAnalyses ImportPKAnalysesFromCSV(string fileName, IModelCoreSimulation simulation);
      PopulationSimulationPKAnalyses CalculateFor(CalculatePKAnalysisArgs calculatePKAnalysisArgs);
   }

   public class PKAnalysisTask : IPKAnalysisTask
   {
      private readonly IPopulationSimulationPKAnalysesToDataTableConverter _populationSimulationPKAnalysesToDataTableConverter;
      private readonly ICorePKAnalysisTask _corePKAnalysesTask;
      private readonly ISimulationPKParametersImportTask _simulationPKParametersImportTask;
      private readonly RLogger _logger;

      public PKAnalysisTask(
         IPopulationSimulationPKAnalysesToDataTableConverter populationSimulationPKAnalysesToDataTableConverter,
         ICorePKAnalysisTask corePKAnalysesTask,
         ISimulationPKParametersImportTask simulationPKParametersImportTask,
         RLogger logger)
      {
         _populationSimulationPKAnalysesToDataTableConverter = populationSimulationPKAnalysesToDataTableConverter;
         _corePKAnalysesTask = corePKAnalysesTask;
         _simulationPKParametersImportTask = simulationPKParametersImportTask;
         _logger = logger;
      }

      public void ExportPKAnalysesToCSV(PopulationSimulationPKAnalyses pkAnalyses, IModelCoreSimulation simulation, string fileName)
      {
         var dataTable = ConvertToDataTable(pkAnalyses, simulation);
         dataTable.ExportToCSV(fileName);
      }

      public PopulationSimulationPKAnalyses ImportPKAnalysesFromCSV(string fileName, IModelCoreSimulation simulation)
      {
         var pkSimulationImport = _simulationPKParametersImportTask.ImportPKParameters(fileName, simulation, CancellationToken.None).Result;
         pkSimulationImport.ThrowOnError();
         _logger.Log(pkSimulationImport);

         var simulationPKAnalyses = new PopulationSimulationPKAnalyses();
         pkSimulationImport.PKParameters.Each(x => simulationPKAnalyses.AddPKAnalysis(x));
         return simulationPKAnalyses;
      }

      public PopulationSimulationPKAnalyses CalculateFor(CalculatePKAnalysisArgs args)
      {
         return _corePKAnalysesTask.CalculateFor(args.Simulation, args.NumberOfIndividuals, args.SimulationResults);
      }

      public DataTable ConvertToDataTable(PopulationSimulationPKAnalyses pkAnalyses, IModelCoreSimulation simulation)
      {
         return _populationSimulationPKAnalysesToDataTableConverter.PKAnalysesToDataTable(pkAnalyses, simulation);
      }
   }
}