using System.Threading.Tasks;
using ModelSolverSettings = OSPSuite.Core.Domain.SolverSettings;
using SnapshotSolverSettings = OSPSuite.Core.Snapshots.SolverSettings;

namespace OSPSuite.Core.Snapshots.Mappers
{
   public abstract class SolverSettingsMapper : SnapshotMapperBase<ModelSolverSettings, SnapshotSolverSettings, SnapshotContext>
   {
      private readonly ModelSolverSettings _defaultSolverSettings;

      protected SolverSettingsMapper(ModelSolverSettings defaultSolverSettings)
      {
         _defaultSolverSettings = defaultSolverSettings;
      }

      public override Task<SnapshotSolverSettings> MapToSnapshot(ModelSolverSettings solverSettings)
      {
         return SnapshotFrom(solverSettings, snapshot =>
         {
            snapshot.AbsTol = SnapshotValueFor(solverSettings.AbsTol, _defaultSolverSettings.AbsTol);
            snapshot.RelTol = SnapshotValueFor(solverSettings.RelTol, _defaultSolverSettings.RelTol);
            snapshot.UseJacobian = SnapshotValueFor(solverSettings.UseJacobian, _defaultSolverSettings.UseJacobian);
            snapshot.H0 = SnapshotValueFor(solverSettings.H0, _defaultSolverSettings.H0);
            snapshot.HMin = SnapshotValueFor(solverSettings.HMin, _defaultSolverSettings.HMin);
            snapshot.HMax = SnapshotValueFor(solverSettings.HMax, _defaultSolverSettings.HMax);
            snapshot.MxStep = SnapshotValueFor(solverSettings.MxStep, _defaultSolverSettings.MxStep);
         });
      }

      public override Task<ModelSolverSettings> MapToModel(SnapshotSolverSettings snapshot, SnapshotContext snapshotContext)
      {
         var solverSettings = CreateDefault();
         if (snapshot == null)
            return Task.FromResult(solverSettings);

         solverSettings.AbsTol = ModelValueFor(snapshot.AbsTol, _defaultSolverSettings.AbsTol);
         solverSettings.RelTol = ModelValueFor(snapshot.RelTol, _defaultSolverSettings.RelTol);
         solverSettings.UseJacobian = ModelValueFor(snapshot.UseJacobian, _defaultSolverSettings.UseJacobian);
         solverSettings.H0 = ModelValueFor(snapshot.H0, _defaultSolverSettings.H0);
         solverSettings.HMin = ModelValueFor(snapshot.HMin, _defaultSolverSettings.HMin);
         solverSettings.HMax = ModelValueFor(snapshot.HMax, _defaultSolverSettings.HMax);
         solverSettings.MxStep = ModelValueFor(snapshot.MxStep, _defaultSolverSettings.MxStep);

         return Task.FromResult(solverSettings);
      }

      protected abstract ModelSolverSettings CreateDefault();
   }
}