using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using OSPSuite.CLI.Core.RunOptions;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Core.Qualification;
using OSPSuite.Core.Services;
using OSPSuite.Core.Snapshots;
using OSPSuite.Core.Snapshots.Services;
using OSPSuite.Utility;
using OSPSuite.Utility.Collections;
using OSPSuite.Utility.Extensions;
using OSPSuite.Utility.Validation;
using static OSPSuite.Assets.Error;
using ModelDataRepository = OSPSuite.Core.Domain.Data.DataRepository;

namespace OSPSuite.CLI.Core.Services
{
   public abstract class QualificationRunner<TSnapshotProject, TModelProject, TRunOptions> : IBatchRunner<TRunOptions> 
      where TSnapshotProject : SnapshotBase, IWithName 
      where TModelProject : Project
      where TRunOptions : QualificationRunOptions
   {
      protected readonly Cache<string, TSnapshotProject> _snapshotProjectCache = new Cache<string, TSnapshotProject>();
      protected readonly IDataRepositoryExportTask _dataRepositoryExportTask;
      protected readonly IOSPSuiteLogger _logger;
      protected readonly IJsonSerializer _jsonSerializer;
      protected readonly ISnapshotTask<TModelProject, TSnapshotProject> _snapshotTask;

      protected QualificationRunner(IOSPSuiteLogger logger, IDataRepositoryExportTask dataRepositoryExportTask, IJsonSerializer jsonSerializer, ISnapshotTask<TModelProject, TSnapshotProject> snapshotTask)
      {
         _logger = logger;
         _dataRepositoryExportTask = dataRepositoryExportTask;
         _jsonSerializer = jsonSerializer;
         _snapshotTask = snapshotTask;
      }

      protected async Task<TSnapshotProject> SnapshotProjectFromFile(string snapshotPath)
      {
         if (!_snapshotProjectCache.Contains(snapshotPath))
         {
            var snapshot = await _snapshotTask.LoadSnapshotFromFileAsync<TSnapshotProject>(snapshotPath);
            _snapshotProjectCache[snapshotPath] = snapshot ?? throw new QualificationRunException(CannotLoadSnapshotFromFile(snapshotPath));
         }

         return _snapshotProjectCache[snapshotPath];
      }

      private Task<QualificationConfiguration> readConfigurationFrom(TRunOptions runOptions)
      {
         if (!FileHelper.FileExists(runOptions.ConfigurationFile))
            throw new QualificationRunException(FileDoesNotExist(runOptions.ConfigurationFile));

         _logger.AddDebug($"Reading configuration from file '{runOptions.ConfigurationFile}'");
         return _jsonSerializer.Deserialize<QualificationConfiguration>(runOptions.ConfigurationFile);
      }

      private string createProjectOutputFolder(string outputPath, string projectName)
      {
         var projectOutputFolder = Path.Combine(outputPath, projectName);

         if (DirectoryHelper.DirectoryExists(projectOutputFolder))
            DirectoryHelper.DeleteDirectory(projectOutputFolder, true);

         DirectoryHelper.CreateDirectory(projectOutputFolder);

         return projectOutputFolder;
      }

      private string relativePath(string path, string relativeTo) => FileHelper.CreateRelativePath(path, relativeTo, useUnixPathSeparator: true);

      public virtual async Task RunBatchAsync(TRunOptions runOptions)
      {
         _snapshotProjectCache.Clear();
         var (snapshot, config) = await readSnapshot(runOptions);

         await performBuildingBlockSwap(snapshot, config.BuildingBlocks);

         await performSimulationParameterSwap(snapshot, config.SimulationParameters);

         //Retrieve charts and validate inputs before exiting validation to ensure that we can throw error messages if an element is not available
         var plots = retrievePlotDefinitionsFrom(snapshot, config);

         ValidateInputs(snapshot, config);

         if (runOptions.Validate)
         {
            _logger.AddInfo($"Validation run terminated for {snapshot.Name}", categoryName: snapshot.Name);
            return;
         }

         var begin = DateTime.UtcNow;
         var (project, inputMappings) = await LoadProjectAndExportInputs(runOptions, snapshot, config);
         LoadProjectContext(project);

         var projectOutputFolder = createProjectOutputFolder(config.OutputFolder, project.Name);

         _logger.AddDebug($"Exporting project {project.Name} to '{projectOutputFolder}'", project.Name);

         var exportRunOptions = new ExportRunOptions
         {
            OutputFolder = projectOutputFolder,
            //We run the output, this is for the old matlab implementation where we need xml. Otherwise, we only need pkml export
            ExportMode = ExportMode(runOptions),

            Simulations = config.Simulations,

            //We only want to export what is required in this case
            ExportAllSimulationsIfListIsEmpty = false
         };

         //Using absolute path for simulation folder. We need them to be relative
         var simulationMappings = await ExportSimulationsIn(project, exportRunOptions);
         simulationMappings.Each(x => x.Path = relativePath(x.Path, config.OutputFolder));

         var observedDataMappings = await exportAllObservedData(project, config);

         var mapping = new QualificationMapping
         {
            SimulationMappings = simulationMappings,
            ObservedDataMappings = observedDataMappings,
            Plots = plots,
            Inputs = inputMappings
         };

         await _jsonSerializer.Serialize(mapping, config.MappingFile);
         _logger.AddDebug($"Project mapping for '{project.Name}' exported to '{config.MappingFile}'", project.Name);

         if (runOptions.ExportProjectFiles)
         {
            var projectFile = Path.Combine(config.TempFolder, $"{project.Name}{ProjectExtension}");
            SaveProjectContext(projectFile);
            _logger.AddDebug($"Project saved to '{projectFile}'", project.Name);

            var snapshotFile = Path.Combine(config.TempFolder, $"{project.Name}{Constants.Filter.JSON_EXTENSION}");
            await _snapshotTask.ExportModelToSnapshotAsync(project, snapshotFile);
            _logger.AddDebug($"Project snapshot saved to '{snapshotFile}'", project.Name);
         }

         var end = DateTime.UtcNow;
         var timeSpent = end - begin;
         _logger.AddInfo($"Project '{project.Name}' exported for qualification in {timeSpent.ToDisplay()}", project.Name);
      }

      private Task<ObservedDataMapping[]> exportAllObservedData(TModelProject project, QualificationConfiguration configuration)
      {
         var allObservedData = project.AllObservedData;
         if (!allObservedData.Any())
            return Task.FromResult(Array.Empty<ObservedDataMapping>());

         var observedDataOutputFolder = configuration.ObservedDataFolder;
         DirectoryHelper.CreateDirectory(observedDataOutputFolder);

         return Task.WhenAll(allObservedData.Select(x => exportObservedData(x, configuration, project)));
      }

      private async Task<ObservedDataMapping> exportObservedData(ModelDataRepository observedData, QualificationConfiguration configuration, TModelProject project)
      {
         var observedDataOutputFolder = configuration.ObservedDataFolder;
         var removeIllegalCharactersFrom = FileHelper.RemoveIllegalCharactersFrom(observedData.Name);
         var csvFullPath = Path.Combine(observedDataOutputFolder, $"{removeIllegalCharactersFrom}{Constants.Filter.CSV_EXTENSION}");
         var xlsFullPath = Path.Combine(observedDataOutputFolder, $"{removeIllegalCharactersFrom}{Constants.Filter.XLSX_EXTENSION}");
         _logger.AddDebug($"Observed data '{observedData.Name}' exported to '{csvFullPath}'", project.Name);
         await _dataRepositoryExportTask.ExportToCsvAsync(observedData, csvFullPath);

         _logger.AddDebug($"Observed data '{observedData.Name}' exported to '{xlsFullPath}'", project.Name);
         await _dataRepositoryExportTask.ExportToExcelAsync(observedData, xlsFullPath, launchExcel: false);

         return new ObservedDataMapping
         {
            Id = observedData.Name,
            Path = relativePath(csvFullPath, configuration.OutputFolder)
         };
      }

      private async Task<(TSnapshotProject, QualificationConfiguration)> readSnapshot(TRunOptions runOptions)
      {
         _logger.AddInfo(runOptions.Validate ? "Starting validation run..." : "Starting qualification run...");

         var config = await readConfigurationFrom(runOptions);
         if (config == null)
            throw new QualificationRunException(UnableToLoadQualificationConfigurationFromFile(runOptions.ConfigurationFile));

         var errorMessage = config.Validate().Message;
         if (!string.IsNullOrEmpty(errorMessage))
            throw new QualificationRunException(errorMessage);

         _logger.AddDebug($"Loading project from snapshot file '{config.SnapshotFile}'...", categoryName: config.Project);

         var snapshot = await SnapshotProjectFromFile(config.SnapshotFile);

         //Ensures that the name of the snapshot is also the name of the project as defined in the configuration
         snapshot.Name = config.Project;
         _logger.AddDebug($"Project {snapshot.Name} loaded from snapshot file '{config.SnapshotFile}'.", categoryName: snapshot.Name);

         return (snapshot, config);
      }

      private Task performBuildingBlockSwap(TSnapshotProject projectSnapshot, BuildingBlockSwap[] buildingBlockSwaps)
      {
         if (buildingBlockSwaps == null)
            return Task.CompletedTask;

         return Task.WhenAll(buildingBlockSwaps.Select(x => SwapBuildingBlockIn(projectSnapshot, x)));
      }

      private Task performSimulationParameterSwap(TSnapshotProject projectSnapshot, SimulationParameterSwap[] simulationParameters)
      {
         if (simulationParameters == null)
            return Task.CompletedTask;

         return Task.WhenAll(simulationParameters.Select(x => SwapSimulationParametersIn(projectSnapshot, x)));
      }

      private PlotMapping[] retrievePlotDefinitionsFrom(TSnapshotProject snapshotProject, QualificationConfiguration configuration)
      {
         var plotMappings = configuration.SimulationPlots?.SelectMany(x => RetrievePlotDefinitionsForSimulation(x, snapshotProject));
         var plotMappingsArray = plotMappings?.ToArray() ?? Array.Empty<PlotMapping>();
         var exportedSimulations = configuration.Simulations?.ToArray() ?? Array.Empty<string>();
         var unmappedSimulations = plotMappingsArray.Select(x => x.Simulation).Distinct().Where(x => !exportedSimulations.Contains(x)).ToList();

         //All simulations referenced in the plot mapping are also exported. We are good
         if (!unmappedSimulations.Any())
            return plotMappingsArray;

         throw new QualificationRunException(SimulationUsedInPlotsAreNotExported(unmappedSimulations, snapshotProject.Name));
      }

      protected abstract Task<(TModelProject, InputMapping[])> LoadProjectAndExportInputs(TRunOptions runOptions, TSnapshotProject snapshot, QualificationConfiguration config);

      protected abstract SimulationExportMode ExportMode(TRunOptions runOptions);

      protected abstract Task<SimulationMapping[]> ExportSimulationsIn(TModelProject project, ExportRunOptions exportRunOptions);

      protected abstract object BuildingBlockBy(TModelProject project, Input input);

      protected abstract string ProjectExtension { get; }

      protected abstract void SaveProjectContext(string projectFile);

      protected abstract void LoadProjectContext(TModelProject project);

      protected abstract IEnumerable<PlotMapping> RetrievePlotDefinitionsForSimulation(SimulationPlot simulationPlot, TSnapshotProject snapshotProject);

      protected abstract Task SwapSimulationParametersIn(TSnapshotProject projectSnapshot, SimulationParameterSwap simulationParameterSwap);

      protected abstract Task SwapBuildingBlockIn(TSnapshotProject projectSnapshot, BuildingBlockSwap buildingBlockSwap);

      protected abstract void ValidateInputs(TSnapshotProject snapshotProject, QualificationConfiguration configuration);
   }
}