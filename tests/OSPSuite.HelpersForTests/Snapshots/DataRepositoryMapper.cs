using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Snapshots.Mappers;

namespace OSPSuite.Helpers.Snapshots
{
   public class DataRepositoryMapper : DataRepositoryMapper<TestProject, SnapshotContext>
   {
      public DataRepositoryMapper(ExtendedPropertyMapper<TestProject> extendedPropertyMapper, DataColumnMapper<TestProject> dataColumnMapper) : base(extendedPropertyMapper, dataColumnMapper)
      {
      }

      protected override SnapshotContextWithDataRepository<TestProject> ContextFor(SnapshotContext snapshotContext, DataRepository dataRepository)
      {
         return new SnapshotContextWithDataRepository(dataRepository, snapshotContext);
      }
   }
}
