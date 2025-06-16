using System.Threading.Tasks;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Extensions;
using ModelQuantityInfo = OSPSuite.Core.Domain.Data.QuantityInfo;
using SnapshotQuantityInfo = OSPSuite.Core.Snapshots.QuantityInfo;

namespace OSPSuite.Core.Snapshots.Mappers
{
   public abstract class QuantityInfoMapper<TProject> : SnapshotMapperBase<ModelQuantityInfo, SnapshotQuantityInfo, SnapshotContext<TProject>> where TProject : Project
   {
      public override Task<SnapshotQuantityInfo> MapToSnapshot(ModelQuantityInfo quantityInfo)
      {
         return SnapshotFrom(quantityInfo, snapshot =>
         {
            snapshot.OrderIndex = SnapshotValueFor(quantityInfo.OrderIndex);
            snapshot.Path = SnapshotValueFor(quantityInfo.PathAsString);
            snapshot.Type = SnapshotValueFor(quantityInfo.Type, QuantityType.Undefined);
         });
      }

      public override Task<ModelQuantityInfo> MapToModel(SnapshotQuantityInfo snapshot, SnapshotContext<TProject> snapshotContext)
      {
         var type = ModelValueFor(snapshot.Type, QuantityType.Undefined);
         var path = ModelValueFor(snapshot.Path);

         var quantityInfo = new ModelQuantityInfo(path.ToPathArray(), type)
         {
            OrderIndex = ModelValueFor(snapshot.OrderIndex)
         };

         return Task.FromResult(quantityInfo);
      }
   }
}