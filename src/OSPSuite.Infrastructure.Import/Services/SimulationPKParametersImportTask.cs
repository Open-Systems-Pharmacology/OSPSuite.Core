using System.Threading;
using System.Threading.Tasks;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;

namespace OSPSuite.Infrastructure.Import.Services
{
   public interface ISimulationPKParametersImportTask
   {
      Task<SimulationPKParametersImport> ImportPKParameters(string fileFullPath, IModelCoreSimulation simulation, CancellationToken cancellationToken);
   }

   public class SimulationPKParametersImportTask : ISimulationPKParametersImportTask
   {
      private readonly ISimulationPKAnalysesImporter _pkAnalysesImporter;
      private readonly IEntitiesInSimulationRetriever _entitiesInSimulationRetriever;

      public SimulationPKParametersImportTask(ISimulationPKAnalysesImporter pkAnalysesImporter, IEntitiesInSimulationRetriever entitiesInSimulationRetriever)
      {
         _pkAnalysesImporter = pkAnalysesImporter;
         _entitiesInSimulationRetriever = entitiesInSimulationRetriever;
      }

      public async Task<SimulationPKParametersImport> ImportPKParameters(string fileFullPath, IModelCoreSimulation simulation, CancellationToken cancellationToken)
      {
         var importedPKAnalysis = await importPKAnalysesFromFile(fileFullPath, simulation, cancellationToken);
         validateConsistencyWithSimulation(simulation, importedPKAnalysis);
         addImportedPKToLogForSuccessfulImport(importedPKAnalysis);
         return importedPKAnalysis;
      }

      private void validateConsistencyWithSimulation(IModelCoreSimulation simulation, SimulationPKParametersImport importedPKParameter)
      {
         var allQuantities = _entitiesInSimulationRetriever.OutputsFrom(simulation);
         foreach (var pkParameter in importedPKParameter.PKParameters)
         {
            verifyThatQuantityExistsInSimulation(allQuantities, pkParameter, importedPKParameter);
         }
      }

      private void verifyThatQuantityExistsInSimulation(PathCache<IQuantity> allQuantities, QuantityPKParameter pkParameter, SimulationPKParametersImport importedPKParameter)
      {
         if (allQuantities.Contains(pkParameter.QuantityPath))
            return;

         importedPKParameter.AddError(Error.CouldNotFindQuantityWithPath(pkParameter.QuantityPath));
      }

      private void addImportedPKToLogForSuccessfulImport(SimulationPKParametersImport pkParameterImport)
      {
         if (pkParameterImport.Status.Is(NotificationType.Error))
            return;

         pkParameterImport.AddInfo(Messages.FollowingPKParametersWereSuccessfullyImported);
         foreach (var quantityPKParameter in pkParameterImport.PKParameters)
         {
            pkParameterImport.AddInfo(quantityPKParameter.ToString());
         }
      }

      private Task<SimulationPKParametersImport> importPKAnalysesFromFile(string fileFullPath, IModelCoreSimulation modelCoreSimulation, CancellationToken cancellationToken)
      {
         return Task.Run(() =>
         {
            var importLogger = new SimulationPKParametersImport {FilePath = fileFullPath};
            importLogger.PKParameters = _pkAnalysesImporter.ImportPKParameters(fileFullPath, modelCoreSimulation, importLogger);
            return importLogger;
         }, cancellationToken);
      }
   }
}