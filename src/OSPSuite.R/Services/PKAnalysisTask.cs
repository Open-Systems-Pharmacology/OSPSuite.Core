using System.Data;
using System.Linq;
using System.Threading;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Infrastructure.Import.Services;
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
      public SimulationResults SimulationResults { get; set; }
   }

   public interface IPKAnalysisTask
   {
      void ExportPKAnalysesToCSV(PopulationSimulationPKAnalyses pkAnalyses, IModelCoreSimulation simulation, string fileName);

      /// <summary>
      ///    Returns the CSV of the PK-Analyses kept by <paramref name="selection" />, in the order of
      ///    <see cref="PopulationSimulationPKAnalyses.All" />. Paths and names that are not in <paramref name="pkAnalyses" />
      ///    keep nothing. Without <paramref name="selection" />, all PK-Analyses are kept
      /// </summary>
      string PKAnalysesToCSV(PopulationSimulationPKAnalyses pkAnalyses, IModelCoreSimulation simulation, PKAnalysesSelection selection = null);

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

      public string PKAnalysesToCSV(PopulationSimulationPKAnalyses pkAnalyses, IModelCoreSimulation simulation, PKAnalysesSelection selection = null)
      {
         var dataTable = ConvertToDataTable(selectedPKAnalysesFrom(pkAnalyses, selection), simulation);
         return dataTable.ToCSV();
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
         return _corePKAnalysesTask.CalculateFor(args.Simulation, args.SimulationResults);
      }

      public DataTable ConvertToDataTable(PopulationSimulationPKAnalyses pkAnalyses, IModelCoreSimulation simulation)
      {
         return _populationSimulationPKAnalysesToDataTableConverter.PKAnalysesToDataTable(pkAnalyses, simulation);
      }

      private PopulationSimulationPKAnalyses selectedPKAnalysesFrom(PopulationSimulationPKAnalyses pkAnalyses, PKAnalysesSelection selection)
      {
         if (selection == null)
            return pkAnalyses;

         var selectedPKAnalyses = new PopulationSimulationPKAnalyses();
         pkAnalyses.All().Where(selection.Keeps).Each(selectedPKAnalyses.AddPKAnalysis);
         return selectedPKAnalyses;
      }
   }
}