using System.Threading;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.R.Domain;
using OSPSuite.R.Mapper;
using OSPSuite.Utility.Extensions;
using SensitivityAnalysis = OSPSuite.R.Domain.SensitivityAnalysis;

namespace OSPSuite.R.Services
{
   public interface ISensitivityAnalysisTask
   {
      /// <summary>
      ///    Returns an array of parameters that are potential candidate to perturb for a sensitivity analysis performed for
      ///    <paramref name="simulation" />
      /// </summary>
      /// <param name="simulation">Simulation used</param>
      /// <returns>An array of parameter path</returns>
      string[] PotentialVariableParameterPathsFor(ISimulation simulation);

      /// <summary>
      ///    Exports sensitivity analysis run results to the csv file with path <paramref name="csvFile" />
      /// </summary>
      /// <param name="sensitivityAnalysisRunResult">Sensitivity analysis run result to export</param>
      /// <param name="simulation">Simulation used</param>
      /// <param name="csvFile">Full path of csv file where results should be exported</param>
      void ExportResultsToCSV(SensitivityAnalysisRunResult sensitivityAnalysisRunResult, ISimulation simulation, string csvFile);

      /// <summary>
      ///    Imports  sensitivity analysis run results from one or more csv files defined in <paramref name="csvFiles" />
      /// </summary>
      SensitivityAnalysisRunResult ImportResultsFromCSV(IModelCoreSimulation simulation, params string[] csvFiles);

      string SensitivityParameterNameForParameter(IParameter parameter);
   }

   public class SensitivityAnalysisTask : ISensitivityAnalysisTask
   {
      private readonly ISensitivityAnalysisToCoreSensitivityAnalysisMapper _sensitivityAnalysisMapper;
      private readonly ISimulationResultsToDataTableConverter _simulationResultsToDataTableConverter;
      private readonly ISensitivityAnalysisRunResultsImportTask _sensitivityAnalysisRunResultsImportTask;
      private readonly IFullPathDisplayResolver _fullPathDisplayResolver;
      private readonly RLogger _logger;

      public SensitivityAnalysisTask(
         ISensitivityAnalysisToCoreSensitivityAnalysisMapper sensitivityAnalysisMapper,
         ISimulationResultsToDataTableConverter simulationResultsToDataTableConverter,
         ISensitivityAnalysisRunResultsImportTask sensitivityAnalysisRunResultsImportTask,
         IFullPathDisplayResolver fullPathDisplayResolver,
         RLogger logger)
      {
         _sensitivityAnalysisMapper = sensitivityAnalysisMapper;
         _simulationResultsToDataTableConverter = simulationResultsToDataTableConverter;
         _sensitivityAnalysisRunResultsImportTask = sensitivityAnalysisRunResultsImportTask;
         _fullPathDisplayResolver = fullPathDisplayResolver;
         _logger = logger;
      }

      public string[] PotentialVariableParameterPathsFor(ISimulation simulation)
      {
         var sensitivityAnalysis = new SensitivityAnalysis(simulation);
         var coreSensitivityAnalysis = _sensitivityAnalysisMapper.MapFrom(sensitivityAnalysis);
         return coreSensitivityAnalysis.AllSensitivityParameterPaths;
      }

      public void ExportResultsToCSV(SensitivityAnalysisRunResult simulationResults, ISimulation simulation, string csvFile)
      {
         var dataTable = _simulationResultsToDataTableConverter.SensitivityAnalysisResultsToDataTable(simulationResults, simulation);
         dataTable.ExportToCSV(csvFile);
      }

      public SensitivityAnalysisRunResult ImportResultsFromCSV(IModelCoreSimulation simulation, params string[] csvFiles)
      {
         var sensitivityAnalysisImportResult = _sensitivityAnalysisRunResultsImportTask.ImportResults(simulation, csvFiles, CancellationToken.None, showImportProgress: false).Result;
         sensitivityAnalysisImportResult.ThrowOnError();
         _logger.Log(sensitivityAnalysisImportResult);
         return sensitivityAnalysisImportResult.SensitivityAnalysisRunResult;
      }

      public string SensitivityParameterNameForParameter(IParameter parameter) => _fullPathDisplayResolver.FullPathFor(parameter);
   }
}