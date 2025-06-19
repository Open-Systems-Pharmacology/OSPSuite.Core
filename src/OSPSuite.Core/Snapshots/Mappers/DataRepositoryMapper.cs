using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Extensions;
using SnapshotDataRepository = OSPSuite.Core.Snapshots.DataRepository;

namespace OSPSuite.Core.Snapshots.Mappers
{
   public class DataRepositoryMapper : ObjectBaseSnapshotMapperBase<Domain.Data.DataRepository, SnapshotDataRepository, SnapshotContext>
   {
      private readonly ExtendedPropertyMapper _extendedPropertyMapper;
      private readonly DataColumnMapper _dataColumnMapper;

      public DataRepositoryMapper(ExtendedPropertyMapper extendedPropertyMapper, DataColumnMapper dataColumnMapper)
      {
         _extendedPropertyMapper = extendedPropertyMapper;
         _dataColumnMapper = dataColumnMapper;
      }

      public override async Task<SnapshotDataRepository> MapToSnapshot(Domain.Data.DataRepository dataRepository)
      {
         var snapshot = await SnapshotFrom(dataRepository, x => { x.Name = SnapshotValueFor(dataRepository.Name); });

         snapshot.ExtendedProperties = await _extendedPropertyMapper.MapToSnapshots(dataRepository.ExtendedProperties);
         snapshot.Columns = await mapColumns(dataRepository.AllButBaseGrid().Where(column => !dataRepository.ColumnIsInRelatedColumns(column)));
         snapshot.BaseGrid = await _dataColumnMapper.MapToSnapshot(dataRepository.BaseGrid);
         return snapshot;
      }

      private Task<DataColumn[]> mapColumns(IEnumerable<Domain.Data.DataColumn> dataRepositoryColumns) => _dataColumnMapper.MapToSnapshots(dataRepositoryColumns);

      public override async Task<Domain.Data.DataRepository> MapToModel(SnapshotDataRepository snapshot, SnapshotContext snapshotContext)
      {
         var dataRepository = new Domain.Data.DataRepository();
         var contextWithDataRepository = contextFor(snapshotContext, dataRepository);
         MapSnapshotPropertiesToModel(snapshot, dataRepository);

         var mapToModel = await _dataColumnMapper.MapToModel(snapshot.BaseGrid, contextWithDataRepository);
         dataRepository.Add(mapToModel);

         dataRepository.AddColumns(await _dataColumnMapper.MapToModels(snapshot.Columns, contextWithDataRepository));

         var extendedProperties = await _extendedPropertyMapper.MapToModels(snapshot.ExtendedProperties, snapshotContext);
         extendedProperties?.Each(dataRepository.ExtendedProperties.Add);

         return dataRepository;
      }

      private SnapshotContextWithDataRepository contextFor(SnapshotContext snapshotContext, Domain.Data.DataRepository dataRepository)
      {
         return new SnapshotContextWithDataRepository(dataRepository, snapshotContext);
      }
   }
}