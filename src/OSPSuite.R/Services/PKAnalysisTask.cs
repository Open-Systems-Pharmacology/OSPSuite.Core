using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.R.Domain;
using OSPSuite.R.Extensions;
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

   /// <summary>
   ///    Outputs and PK parameters to keep when converting PK-Analyses.
   ///    Singular properties exist because R passes a vector of length one as a single value
   /// </summary>
   public class PKAnalysesSelection
   {
      //Potentially null. All outputs are kept when neither QuantityPaths nor QuantityPath is set
      public string[] QuantityPaths { get; set; }

      //Potentially null. This will be used when one output only is kept
      public string QuantityPath { get; set; }

      //Potentially null. All PK parameters are kept when neither PKParameterNames nor PKParameterName is set
      public string[] PKParameterNames { get; set; }

      //Potentially null. This will be used when one PK parameter only is kept
      public string PKParameterName { get; set; }

      public string[] SelectedQuantityPaths => QuantityPaths.ToNetArray(QuantityPath);

      public string[] SelectedPKParameterNames => PKParameterNames.ToNetArray(PKParameterName);
   }

   public interface IPKAnalysisTask
   {
      void ExportPKAnalysesToCSV(PopulationSimulationPKAnalyses pkAnalyses, IModelCoreSimulation simulation, string fileName);
      string PKAnalysesToCSV(PopulationSimulationPKAnalyses pkAnalyses, IModelCoreSimulation simulation);

      /// <summary>
      ///    Returns the CSV of the PK-Analyses selected by <paramref name="selection" />, in the order of
      ///    <see cref="PopulationSimulationPKAnalyses.All" />. Paths and names that are not in <paramref name="pkAnalyses" />
      ///    select nothing. A null <paramref name="selection" /> keeps all PK-Analyses
      /// </summary>
      string PKAnalysesToCSV(PopulationSimulationPKAnalyses pkAnalyses, IModelCoreSimulation simulation, PKAnalysesSelection selection);

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

      public string PKAnalysesToCSV(PopulationSimulationPKAnalyses pkAnalyses, IModelCoreSimulation simulation)
      {
         var dataTable = ConvertToDataTable(pkAnalyses, simulation);
         return dataTable.ToCSV();
      }

      public string PKAnalysesToCSV(PopulationSimulationPKAnalyses pkAnalyses, IModelCoreSimulation simulation, PKAnalysesSelection selection)
      {
         return PKAnalysesToCSV(selectedPKAnalysesFrom(pkAnalyses, selection), simulation);
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
         var selectedQuantityPaths = toHashSet(selection?.SelectedQuantityPaths);
         var selectedPKParameterNames = toHashSet(selection?.SelectedPKParameterNames);
         if (selectedQuantityPaths == null && selectedPKParameterNames == null)
            return pkAnalyses;

         var selectedPKAnalyses = new PopulationSimulationPKAnalyses();
         pkAnalyses.All()
            .Where(x => isSelected(x.QuantityPath, selectedQuantityPaths) && isSelected(x.Name, selectedPKParameterNames))
            .Each(selectedPKAnalyses.AddPKAnalysis);

         return selectedPKAnalyses;
      }

      //null when nothing is set, so that everything is selected
      private static HashSet<string> toHashSet(string[] entries) => entries == null ? null : new HashSet<string>(entries);

      private static bool isSelected(string entry, HashSet<string> selectedEntries) => selectedEntries == null || selectedEntries.Contains(entry);
   }
}