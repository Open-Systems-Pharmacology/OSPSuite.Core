using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.SensitivityAnalyses;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Infrastructure.Import.Services
{
   public interface ISensitivityAnalysisRunResultsImportTask
   {
      Task<SensitivityAnalysisRunResultsImport> ImportResults(IModelCoreSimulation simulation, IReadOnlyCollection<string> files, CancellationToken cancellationToken, bool showImportProgress = true);
   }

   public class SensitivityAnalysisRunResultsImportTask : ISensitivityAnalysisRunResultsImportTask
   {
      private readonly IProgressManager _progressManager;
      private readonly IPKParameterSensitivitiesImporter _pkParameterSensitivitiesImporter;
      private readonly IEntitiesInSimulationRetriever _quantitiesRetriever;

      public SensitivityAnalysisRunResultsImportTask(IEntitiesInSimulationRetriever quantitiesRetriever, IPKParameterSensitivitiesImporter pkParameterSensitivitiesImporter, IProgressManager progressManager)
      {
         _progressManager = progressManager;
         _pkParameterSensitivitiesImporter = pkParameterSensitivitiesImporter;
         _quantitiesRetriever = quantitiesRetriever;
      }

      public async Task<SensitivityAnalysisRunResultsImport> ImportResults(IModelCoreSimulation simulation, IReadOnlyCollection<string> files, CancellationToken cancellationToken, bool showImportProgress = true)
      {
         using (var progressUpdater = showImportProgress ? _progressManager.Create() : new NoneProgressUpdater())
         {
            progressUpdater.Initialize(files.Count, Messages.ImportingResults);

            // Use ToList to execute the query and start the import task.
            var tasks = files.Select(f => importFiles(f, simulation, cancellationToken)).ToList();
            var allImportedResults = new List<PKParameterSensitivitiesImport>();
            // Await the completion of all the running tasks. 
            // Add a loop to process the tasks one at a time until none remain. 
            while (tasks.Count > 0)
            {
               cancellationToken.ThrowIfCancellationRequested();

               // Identify the first task that completes.
               var firstFinishedTask = await Task.WhenAny(tasks);

               // Remove the selected task from the list so that you don't 
               // process it more than once.
               tasks.Remove(firstFinishedTask);

               // Await the completed task. 
               allImportedResults.Add(await firstFinishedTask);
               progressUpdater.IncrementProgress();
            }

            //once all results have been imported, it is time to ensure that they are consistent
            var results = createSensitivityAnalysisRunResultsFrom(allImportedResults, simulation);

            addImportedQuantityToLogForSuccessfulImport(results);
            return results;
         }
      }

      private void addImportedQuantityToLogForSuccessfulImport(SensitivityAnalysisRunResultsImport sensitivityAnalysisRunResultsImport)
      {
         if (sensitivityAnalysisRunResultsImport.Status.Is(NotificationType.Error))
            return;

         sensitivityAnalysisRunResultsImport.AddInfo(Messages.FollowingPKParameterSensitivityWereSuccessfullyImported);
         foreach (var pkParameterSensitivity in sensitivityAnalysisRunResultsImport.SensitivityAnalysisRunResult.AllPKParameterSensitivities)
         {
            sensitivityAnalysisRunResultsImport.AddInfo(pkParameterSensitivity.ToString());
         }
      }

      private SensitivityAnalysisRunResultsImport createSensitivityAnalysisRunResultsFrom(IReadOnlyList<PKParameterSensitivitiesImport> allImportedResults, IModelCoreSimulation simulation)
      {
         var sensitivityAnalysisRunResultsImport = new SensitivityAnalysisRunResultsImport();

         //First add all available results
         allImportedResults.Each(import => addPKParameterSensitivitiesFromSingleFile(sensitivityAnalysisRunResultsImport, import));

         //now check that the defined outputs are actually available in the population simulation
         validateImportedQuantities(sensitivityAnalysisRunResultsImport, simulation);

         return sensitivityAnalysisRunResultsImport;
      }

      private void addPKParameterSensitivitiesFromSingleFile(SensitivityAnalysisRunResultsImport sensitivityAnalysisRunResultsImport, PKParameterSensitivitiesImport pkParameterSensitivitiesImport)
      {
         var sensitivityAnalysisRunResult = sensitivityAnalysisRunResultsImport.SensitivityAnalysisRunResult;
         sensitivityAnalysisRunResultsImport.PKParameterSensitivitiesImportFiles.Add(pkParameterSensitivitiesImport.PKParameterSensitivitiesImportFile);

         foreach (var pkParameterSensitivity in pkParameterSensitivitiesImport.PKParameterSensitivities)
         {
            if (sensitivityAnalysisRunResult.HasPKParameterSensitivityWithId(pkParameterSensitivity.Id))
               sensitivityAnalysisRunResultsImport.AddError(Error.DuplicatedPKParameterSensitivityFor(pkParameterSensitivity.Id));
            else
               sensitivityAnalysisRunResult.AddPKParameterSensitivity(pkParameterSensitivity);
         }
      }

      private void validateImportedQuantities(SensitivityAnalysisRunResultsImport sensitivityAnalysisRunResultsImport, IModelCoreSimulation modelCoreSimulation)
      {
         var allOutputs = _quantitiesRetriever.OutputsFrom(modelCoreSimulation);
         foreach (var quantityPath in sensitivityAnalysisRunResultsImport.SensitivityAnalysisRunResult.AllPKParameterSensitivities.Select(x => x.QuantityPath).Distinct())
         {
            var quantity = allOutputs[quantityPath];
            if (quantity != null) continue;

            sensitivityAnalysisRunResultsImport.AddError(Error.CouldNotFindQuantityWithPath(quantityPath));
         }
      }

      private Task<PKParameterSensitivitiesImport> importFiles(string fileFullPath, IModelCoreSimulation simulation, CancellationToken cancellationToken)
      {
         return Task.Run(() =>
         {
            var sensitivityAnalysisRunResultsImportFile = new PKParameterSensitivitiesImportFile {FilePath = fileFullPath};
            var importResult = new PKParameterSensitivitiesImport
            {
               PKParameterSensitivities = _pkParameterSensitivitiesImporter.ImportFrom(fileFullPath, simulation, sensitivityAnalysisRunResultsImportFile),
               PKParameterSensitivitiesImportFile = sensitivityAnalysisRunResultsImportFile
            };


            return importResult;
         }, cancellationToken);
      }

      private class PKParameterSensitivitiesImport
      {
         public IReadOnlyList<PKParameterSensitivity> PKParameterSensitivities { get; set; }
         public PKParameterSensitivitiesImportFile PKParameterSensitivitiesImportFile { get; set; }
      }
   }
}