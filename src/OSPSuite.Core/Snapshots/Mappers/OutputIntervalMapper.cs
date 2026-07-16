using System.Threading.Tasks;
using OSPSuite.Core.Domain;
using ModelOutputInterval = OSPSuite.Core.Domain.OutputInterval;
using SnapshotOutputInterval = OSPSuite.Core.Snapshots.OutputInterval;

namespace OSPSuite.Core.Snapshots.Mappers
{
   public abstract class OutputIntervalMapper : ParameterContainerSnapshotMapperBase<ModelOutputInterval, SnapshotOutputInterval, SnapshotContext>
   {
      protected OutputIntervalMapper(ParameterMapper parameterMapper) : base(parameterMapper)
      {
      }

      public override Task<SnapshotOutputInterval> MapToSnapshot(ModelOutputInterval outputInterval)
      {
         return SnapshotFrom(outputInterval, x =>
         {
            //name will be generated on the fly when creating the intervals
            x.Name = null;
         });
      }

      public override async Task<ModelOutputInterval> MapToModel(SnapshotOutputInterval snapshot, SnapshotContext snapshotContext)
      {
         var outputInterval = CreateDefault();
         await UpdateParametersFromSnapshot(snapshot, outputInterval, snapshotContext, Constants.OUTPUT_INTERVAL);
         return outputInterval;
      }

      protected override bool ShouldExportToSnapshot(IParameter parameter)
      {
         //we want to ensure that start time and end time are always exported
         return parameter.NameIsOneOf(Constants.Parameters.START_TIME, Constants.Parameters.END_TIME, Constants.Parameters.RESOLUTION);
      }

      protected abstract ModelOutputInterval CreateDefault();
   }
}