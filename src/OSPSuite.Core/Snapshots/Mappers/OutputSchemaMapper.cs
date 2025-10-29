using System.Threading.Tasks;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Extensions;
using ModelOutputSchema = OSPSuite.Core.Domain.OutputSchema;
using SnapshotOutputSchema = OSPSuite.Core.Snapshots.OutputSchema;

namespace OSPSuite.Core.Snapshots.Mappers
{
   public abstract class OutputSchemaMapper : SnapshotMapperBase<ModelOutputSchema, SnapshotOutputSchema, SnapshotContext>
   {
      private readonly OutputIntervalMapper _outputIntervalMapper;
      private readonly IContainerTask _containerTask;

      protected OutputSchemaMapper(OutputIntervalMapper outputIntervalMapper, IContainerTask containerTask)
      {
         _outputIntervalMapper = outputIntervalMapper;
         _containerTask = containerTask;
      }

      public override async Task<SnapshotOutputSchema> MapToSnapshot(ModelOutputSchema outputSchema)
      {
         var snapshot = await SnapshotFrom(outputSchema);
         var intervals = await _outputIntervalMapper.MapToSnapshots(outputSchema.Intervals);
         intervals?.Each(snapshot.Add);
         return snapshot;
      }

      public override async Task<ModelOutputSchema> MapToModel(SnapshotOutputSchema snapshot, SnapshotContext snapshotContext)
      {
         var outputSchema = CreateEmpty();
         var intervals = await _outputIntervalMapper.MapToModels(snapshot, snapshotContext);
         intervals?.Each(interval =>
         {
            interval.Name = _containerTask.CreateUniqueName(outputSchema, interval.Name);
            outputSchema.AddInterval(interval);
         });
         return outputSchema;
      }

      protected abstract ModelOutputSchema CreateEmpty();
   }
}