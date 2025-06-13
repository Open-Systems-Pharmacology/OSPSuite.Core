using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Extensions;
using OSPSuite.Utility.Extensions;
using SnapshotDataRepository = OSPSuite.Core.Snapshots.DataRepository;

namespace OSPSuite.Core.Snapshots.Mappers
{
   public abstract class DataRepositoryMapper<TProject, TSnapshotContext> : ObjectBaseSnapshotMapperBase<Domain.Data.DataRepository, SnapshotDataRepository, TSnapshotContext> where TProject : Project where TSnapshotContext : SnapshotContext<TProject>
   {
      private readonly ExtendedPropertyMapper<TProject> _extendedPropertyMapper;
      private readonly DataColumnMapper<TProject> _dataColumnMapper;

      public DataRepositoryMapper(ExtendedPropertyMapper<TProject> extendedPropertyMapper, DataColumnMapper<TProject> dataColumnMapper)
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

      public override async Task<Domain.Data.DataRepository> MapToModel(SnapshotDataRepository snapshot, TSnapshotContext snapshotContext)
      {
         var dataRepository = new Domain.Data.DataRepository();
         var contextWithDataRepository = ContextFor(snapshotContext, dataRepository);
         MapSnapshotPropertiesToModel(snapshot, dataRepository);

         var mapToModel = await _dataColumnMapper.MapToModel(snapshot.BaseGrid, contextWithDataRepository);
         dataRepository.Add(mapToModel);

         dataRepository.AddColumns(await _dataColumnMapper.MapToModels(snapshot.Columns, contextWithDataRepository));

         var extendedProperties = await _extendedPropertyMapper.MapToModels(snapshot.ExtendedProperties, snapshotContext);
         extendedProperties?.Each(dataRepository.ExtendedProperties.Add);

         return dataRepository;
      }

      protected abstract SnapshotContextWithDataRepository<TProject> ContextFor(TSnapshotContext snapshotContext, Domain.Data.DataRepository dataRepository);
   }
}