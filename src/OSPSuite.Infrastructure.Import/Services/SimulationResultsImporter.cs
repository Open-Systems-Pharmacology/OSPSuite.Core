using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Events;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Infrastructure.Import.Services
{
   public interface ISimulationResultsImportTask
   {
      Task<SimulationResultsImport> ImportResults(IModelCoreSimulation simulation, IReadOnlyCollection<string> files, CancellationToken cancellationToken, bool showImportProgress = true);
   }

   public class SimulationResultsImportTask : ISimulationResultsImportTask
   {
      private readonly IEntitiesInSimulationRetriever _quantitiesRetriever;
      private readonly IIndividualResultsImporter _individualResultsImporter;
      private readonly IProgressManager _progressManager;

      public SimulationResultsImportTask(IEntitiesInSimulationRetriever quantitiesRetriever, IIndividualResultsImporter individualResultsImporter, IProgressManager progressManager)
      {
         _quantitiesRetriever = quantitiesRetriever;
         _individualResultsImporter = individualResultsImporter;
         _progressManager = progressManager;
      }

      public async Task<SimulationResultsImport> ImportResults(IModelCoreSimulation simulation, IReadOnlyCollection<string> files, CancellationToken cancellationToken, bool showImportProgress = true)
      {
         using (var progressUpdater = showImportProgress ? _progressManager.Create() : new NoneProgressUpdater())
         {
            progressUpdater.Initialize(files.Count, Messages.ImportingResults);

            // Use ToList to execute the query and start the import task.
            var tasks = files.Select(f => importFiles(f, simulation, cancellationToken)).ToList();
            var allImportedResults = new List<IndividualResultsImport>();
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
            var results = createSimulationResultsFrom(allImportedResults, simulation);

            addImportedQuantityToLogForSuccessfulImport(results);
            return results;
         }
      }

      private void addImportedQuantityToLogForSuccessfulImport(SimulationResultsImport simulationResultsImport)
      {
         if (simulationResultsImport.Status.Is(NotificationType.Error))
            return;

         simulationResultsImport.AddInfo(Messages.FollowingOutputsWereSuccessfullyImported(simulationResultsImport.SimulationResults.Count));
         foreach (var quantityPath in simulationResultsImport.SimulationResults.AllQuantityPaths())
         {
            simulationResultsImport.AddInfo(quantityPath);
         }
      }

      private Task<IndividualResultsImport> importFiles(string fileFullPath, IModelCoreSimulation simulation, CancellationToken cancellationToken)
      {
         return Task.Run(() =>
         {
            var importResult = new IndividualResultsImport();
            var simulationResultsFile = new SimulationResultsImportFile {FilePath = fileFullPath};
            importResult.IndividualResults = _individualResultsImporter.ImportFrom(fileFullPath, simulation, simulationResultsFile).ToList();
            simulationResultsFile.NumberOfIndividuals = importResult.IndividualResults.Count;
            importResult.SimulationResultsFile = simulationResultsFile;

            if (importResult.IndividualResults.Any())
               importResult.SimulationResultsFile.NumberOfQuantities = importResult.IndividualResults.First().Count();

            return importResult;
         }, cancellationToken);
      }

      private SimulationResultsImport createSimulationResultsFrom(IEnumerable<IndividualResultsImport> importedResults, IModelCoreSimulation simulation)
      {
         var simulationResultsImport = new SimulationResultsImport();

         //First add all available results
         importedResults.Each(import => addIndividualResultsFromSingleFile(simulationResultsImport, import));

         //now check that the defined outputs are actually available in the population simulation
         validateImportedQuantities(simulationResultsImport, simulation);

         return simulationResultsImport;
      }

      private void addIndividualResultsFromSingleFile(SimulationResultsImport simulationResultsImport, IndividualResultsImport individualResultsImport)
      {
         var simulationResults = simulationResultsImport.SimulationResults;
         simulationResultsImport.SimulationResultsFiles.Add(individualResultsImport.SimulationResultsFile);

         foreach (var individualResult in individualResultsImport.IndividualResults)
         {
            validateResults(simulationResultsImport, individualResult);

            if (simulationResults.HasResultsFor(individualResult.IndividualId))
               simulationResultsImport.AddError(Error.DuplicatedIndividualResultsForId(individualResult.IndividualId));
            else
               simulationResults.Add(individualResult);
         }
      }

      private void validateImportedQuantities(SimulationResultsImport simulationResultsImport, IModelCoreSimulation simulation)
      {
         var allQuantities = _quantitiesRetriever.QuantitiesFrom(simulation);

         foreach (var quantityPath in simulationResultsImport.SimulationResults.AllQuantityPaths())
         {
            var quantity = allQuantities[quantityPath];
            if (quantity != null) continue;

            simulationResultsImport.AddError(Error.CouldNotFindQuantityWithPath(quantityPath));
         }
      }

      private class IndividualResultsImport
      {
         public IList<IndividualResults> IndividualResults { get; set; }
         public SimulationResultsImportFile SimulationResultsFile { get; set; }
      }

      private static void validateResults(SimulationResultsImport simulationResultsImport, IndividualResults individualResults)
      {
         var simulationResults = simulationResultsImport.SimulationResults;

         //No entry yet? Set this individual results as base for the import
         if (!simulationResults.Any())
         {
            simulationResults.Time = individualResults.Time;
            individualResults.UpdateQuantityTimeReference();
            return;
         }

         validateTimeResults(simulationResultsImport, individualResults);
         var availableQuantityPaths = simulationResults.AllQuantityPaths();
         var currentQuantityPaths = individualResults.Select(x => x.QuantityPath).ToList();

         if (availableQuantityPaths.Count != currentQuantityPaths.Count)
         {
            simulationResultsImport.AddError(Error.IndividualResultsDoesNotHaveTheExpectedQuantity(individualResults.IndividualId, availableQuantityPaths, currentQuantityPaths));
            return;
         }

         for (int i = 0; i < availableQuantityPaths.Count; i++)
         {
            if (!string.Equals(availableQuantityPaths[i], currentQuantityPaths[i]))
            {
               simulationResultsImport.AddError(Error.IndividualResultsDoesNotHaveTheExpectedQuantity(individualResults.IndividualId, availableQuantityPaths, currentQuantityPaths));
               return;
            }
         }
      }

      private static void validateTimeResults(SimulationResultsImport simulationResultsImport, IndividualResults individualResults)
      {
         var time = simulationResultsImport.SimulationResults.Time;
         int expectedLength = time.Values.Length;
         int currentLength = individualResults.Time.Values.Length;

         if (time.Values.Length != individualResults.Time.Values.Length)
         {
            simulationResultsImport.AddError(Error.TimeArrayLengthDoesNotMatchFirstIndividual(individualResults.IndividualId, expectedLength, currentLength));
            return;
         }

         for (int i = 0; i < currentLength; i++)
         {
            if (!ValueComparer.AreValuesEqual(time[i], individualResults.Time[i]))
               simulationResultsImport.AddError(Error.TimeArrayValuesDoesNotMatchFirstIndividual(individualResults.IndividualId, i, time[i], individualResults.Time[i]));
         }

         //update reference time to ensure that all results are using the same time
         individualResults.Time = time;
         individualResults.UpdateQuantityTimeReference();
      }
   }
}