using System.Collections.Generic;
using System.Data;
using System.Threading;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.PKAnalyses;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.R.Extensions;
using OSPSuite.Utility.Extensions;
using ICorePKAnalysisTask = OSPSuite.Core.Domain.Services.IPKAnalysesTask;

namespace OSPSuite.R.Services
{
   /// <summary>
   /// Required because of optional DynamicParameters dependencies
   /// </summary>
   public class CalculatePKAnalysisArgs
   {
      private readonly List<DynamicPKParameter> _allDynamicParameters= new List<DynamicPKParameter>();
      public IModelCoreSimulation Simulation { get; set; }
      public int NumberOfIndividuals { get; set; }
      public SimulationResults SimulationResults { get; set; }
      public IReadOnlyList<DynamicPKParameter> DynamicParameters => _allDynamicParameters;

      public void AddDynamicParameter(DynamicPKParameter dynamicPKParameter) => _allDynamicParameters.Add(dynamicPKParameter);
   }

   public interface IPKAnalysesTask
   {
      void ExportPKAnalysesToCSV(PopulationSimulationPKAnalyses pkAnalyses, IModelCoreSimulation simulation, string fileName);
      DataTable ConvertToDataTable(PopulationSimulationPKAnalyses pkAnalyses, IModelCoreSimulation simulation);
      PopulationSimulationPKAnalyses ImportPKAnalysesFromCSV(string fileName, IModelCoreSimulation simulation);
      PopulationSimulationPKAnalyses CalculateFor(CalculatePKAnalysisArgs calculatePKAnalysisArgs);
   }

   public class PKAnalysesTask : IPKAnalysesTask
   {
      private readonly ISimulationResultsToDataTableConverter _simulationResultsToDataTableConverter;
      private readonly ICorePKAnalysisTask _corePKAnalysesTask;
      private readonly ISimulationPKParametersImportTask _simulationPKParametersImportTask;

      public PKAnalysesTask(
         ISimulationResultsToDataTableConverter simulationResultsToDataTableConverter,
         ICorePKAnalysisTask corePKAnalysesTask,
         ISimulationPKParametersImportTask simulationPKParametersImportTask)
      {
         _simulationResultsToDataTableConverter = simulationResultsToDataTableConverter;
         _corePKAnalysesTask = corePKAnalysesTask;
         _simulationPKParametersImportTask = simulationPKParametersImportTask;
      }

      public void ExportPKAnalysesToCSV(PopulationSimulationPKAnalyses pkAnalyses, IModelCoreSimulation simulation, string fileName)
      {
         var dataTable = ConvertToDataTable(pkAnalyses, simulation);
         dataTable.ExportToCSV(fileName);
      }

      public PopulationSimulationPKAnalyses ImportPKAnalysesFromCSV(string fileName, IModelCoreSimulation simulation)
      {
         var pkSimulationImport = _simulationPKParametersImportTask.ImportPKParameters(fileName, simulation, CancellationToken.None).Result;
         pkSimulationImport.LogToR();
         var simulationPKAnalyses = new PopulationSimulationPKAnalyses();
         pkSimulationImport.PKParameters.Each(x => simulationPKAnalyses.AddPKAnalysis(x));
         return simulationPKAnalyses;
      }

      public PopulationSimulationPKAnalyses CalculateFor(CalculatePKAnalysisArgs args)
      {
         return _corePKAnalysesTask.CalculateFor(args.Simulation, args.NumberOfIndividuals, args.SimulationResults, args.DynamicParameters);
      }

      public DataTable ConvertToDataTable(PopulationSimulationPKAnalyses pkAnalyses, IModelCoreSimulation simulation)
      {
         return _simulationResultsToDataTableConverter.PKAnalysesToDataTable(pkAnalyses, simulation);
         ;
      }
   }
}