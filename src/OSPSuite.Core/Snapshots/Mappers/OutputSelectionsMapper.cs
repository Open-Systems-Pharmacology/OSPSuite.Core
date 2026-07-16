using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Services;
using OSPSuite.Utility.Extensions;
using System.Linq;
using System.Threading.Tasks;
using ModelOutputSelections = OSPSuite.Core.Domain.OutputSelections;
using SnapshotOutputSelections = OSPSuite.Core.Snapshots.OutputSelections;

namespace OSPSuite.Core.Snapshots.Mappers
{
   public class OutputSelectionsMapper : SnapshotMapperBase<ModelOutputSelections, SnapshotOutputSelections, SnapshotContextWithSimulation>
   {
      private readonly IOSPSuiteLogger _logger;
      private readonly IEntitiesInSimulationRetriever _entitiesInSimulationRetriever;

      public OutputSelectionsMapper(IOSPSuiteLogger logger, IEntitiesInSimulationRetriever entitiesInSimulationRetriever)
      {
         _logger = logger;
         _entitiesInSimulationRetriever = entitiesInSimulationRetriever;
      }

      public override async Task<SnapshotOutputSelections> MapToSnapshot(ModelOutputSelections outputSelections)
      {
         if (!outputSelections.HasSelection)
            return null;

         var snapshot = await SnapshotFrom(outputSelections);
         snapshot.AddRange(outputSelections.AllOutputs.Select(x => x.Path));
         return snapshot;
      }

      public override Task<ModelOutputSelections> MapToModel(SnapshotOutputSelections snapshot, SnapshotContextWithSimulation snapshotContext)
      {
         var outputSelections = new ModelOutputSelections();
         if (snapshot == null)
            return Task.FromResult(outputSelections);

         var allQuantities = _entitiesInSimulationRetriever.QuantitiesFrom(snapshotContext.Simulation);

         snapshot.Each(path =>
         {
            var quantity = allQuantities[path];
            if (quantity == null)
               _logger.AddWarning(Error.CouldNotFindQuantityWithPath(path));
            else
               outputSelections.AddOutput(new QuantitySelection(path, quantity.QuantityType));
         });

         return Task.FromResult(outputSelections);
      }
   }
}