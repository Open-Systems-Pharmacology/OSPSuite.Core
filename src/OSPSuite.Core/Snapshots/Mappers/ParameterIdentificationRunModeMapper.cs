using System.Threading.Tasks;
using OSPSuite.Core.Domain.ParameterIdentifications;
using OSPSuite.Utility.Extensions;
using ModelParameterIdentificationRunMode = OSPSuite.Core.Domain.ParameterIdentifications.ParameterIdentificationRunMode;
using SnapshotParameterIdentificationRunMode = OSPSuite.Core.Snapshots.ParameterIdentificationRunMode;

namespace OSPSuite.Core.Snapshots.Mappers
{
   public abstract class ParameterIdentificationRunModeMapper : SnapshotMapperBase<ModelParameterIdentificationRunMode, SnapshotParameterIdentificationRunMode, SnapshotContext>
   {
      public override async Task<SnapshotParameterIdentificationRunMode> MapToSnapshot(ModelParameterIdentificationRunMode runMode)
      {
         if (runMode == null || runMode.IsAnImplementationOf<StandardParameterIdentificationRunMode>())
            return null;

         var snapshot = await SnapshotFrom(runMode);
         await MapRunModeParameters(snapshot, runMode);
         return snapshot;
      }

      protected virtual async Task MapRunModeParameters(SnapshotParameterIdentificationRunMode snapshot, ModelParameterIdentificationRunMode runMode)
      {
         switch (runMode)
         {
            case MultipleParameterIdentificationRunMode multipleParameterIdentificationRunMode:
               snapshot.NumberOfRuns = multipleParameterIdentificationRunMode.NumberOfRuns;
               break;
         }
      }

      public override Task<ModelParameterIdentificationRunMode> MapToModel(SnapshotParameterIdentificationRunMode snapshot, SnapshotContext snapshotContext)
      {
         return Task.FromResult(mapRunModeFrom(snapshot));
      }

      private ModelParameterIdentificationRunMode mapRunModeFrom(SnapshotParameterIdentificationRunMode snapshot)
      {
         if (snapshot == null)
            return new StandardParameterIdentificationRunMode();

         if (snapshot.NumberOfRuns.HasValue)
            return new MultipleParameterIdentificationRunMode { NumberOfRuns = snapshot.NumberOfRuns.Value };

         return RunModeFrom(snapshot);
      }

      protected abstract ModelParameterIdentificationRunMode RunModeFrom(SnapshotParameterIdentificationRunMode snapshot);
   }
}