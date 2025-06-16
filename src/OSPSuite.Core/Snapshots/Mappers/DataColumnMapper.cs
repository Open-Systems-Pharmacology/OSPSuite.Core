using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Extensions;
using ModelDataRepository = OSPSuite.Core.Domain.Data.DataRepository;
using SnapshotDataColumn = OSPSuite.Core.Snapshots.DataColumn;
using ModelDataColumn = OSPSuite.Core.Domain.Data.DataColumn;

namespace OSPSuite.Core.Snapshots.Mappers
{
   public class SnapshotContextWithDataRepository<TProject> : SnapshotContext<TProject> where TProject : Project
   {
      public ModelDataRepository DataRepository { get; }

      public SnapshotContextWithDataRepository(ModelDataRepository dataRepository, SnapshotContext<TProject> baseContext, int version) : base(baseContext.Project, version)
      {
         DataRepository = dataRepository;
      }
   }

   public abstract class DataColumnMapper<TProject> : SnapshotMapperBase<ModelDataColumn, SnapshotDataColumn, SnapshotContextWithDataRepository<TProject>> where TProject : Project
   {
      private readonly DataInfoMapper<TProject> _dataInfoMapper;
      private readonly QuantityInfoMapper<TProject> _quantityInfoMapper;

      public DataColumnMapper(DataInfoMapper<TProject> dataInfoMapper, QuantityInfoMapper<TProject> quantityInfoMapper)
      {
         _dataInfoMapper = dataInfoMapper;
         _quantityInfoMapper = quantityInfoMapper;
      }

      public override async Task<SnapshotDataColumn> MapToSnapshot(ModelDataColumn dataColumn)
      {
         var snapshot = await SnapshotFrom(dataColumn, x =>
         {
            x.Name = dataColumn.Name;
            x.Values = valuesInDisplayUnits(dataColumn);
            x.Dimension = dataColumn.Dimension.Name;
            x.Unit = SnapshotValueFor(dataColumn.DisplayUnit.Name);
         });

         snapshot.RelatedColumns = await this.MapToSnapshots(dataColumn.RelatedColumns);
         snapshot.DataInfo = await _dataInfoMapper.MapToSnapshot(dataColumn.DataInfo);
         snapshot.QuantityInfo = await _quantityInfoMapper.MapToSnapshot(dataColumn.QuantityInfo);
         return snapshot;
      }

      private IReadOnlyList<float> valuesInBaseUnits(ModelDataColumn dataColumn, IEnumerable<float> valuesInDisplayUnits)
      {
         return dataColumn.ConvertToBaseValues(valuesInDisplayUnits);
      }

      private List<float> valuesInDisplayUnits(ModelDataColumn dataColumn)
      {
         return dataColumn.ConvertToDisplayValues(dataColumn.Values).ToList();
      }

      public override async Task<ModelDataColumn> MapToModel(SnapshotDataColumn snapshot, SnapshotContextWithDataRepository<TProject> snapshotContext)
      {
         var dataRepository = snapshotContext.DataRepository;
         var dataInfo = await _dataInfoMapper.MapToModel(snapshot.DataInfo, snapshotContext);
         var dimension = DimensionFrom(snapshot);
         var dataColumn = dataInfo.Origin == ColumnOrigins.BaseGrid ? new BaseGrid(snapshot.Name, dimension) : new ModelDataColumn(snapshot.Name, dimension, dataRepository.BaseGrid);
         //this needs to be set after DATA Info to be sure that we are using the value from the snapshot
         dataColumn.DataInfo = dataInfo;
         dataColumn.DisplayUnit = displayUnitFor(dimension, snapshot.Unit);
         dataColumn.Values = valuesInBaseUnits(dataColumn, snapshot.Values);
         dataColumn.QuantityInfo = await _quantityInfoMapper.MapToModel(snapshot.QuantityInfo, snapshotContext);

         var relatedColumns = await this.MapToModels(snapshot.RelatedColumns, snapshotContext);
         relatedColumns?.Each(dataColumn.AddRelatedColumn);

         return dataColumn;
      }

      protected abstract IDimension DimensionFrom(SnapshotDataColumn snapshot);

      private Unit displayUnitFor(IDimension dimension, string snapshotUnit) => dimension.UnitOrDefault(snapshotUnit);
   }
}