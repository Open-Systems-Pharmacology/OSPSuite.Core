using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Snapshots.Mappers;

namespace OSPSuite.Helpers.Snapshots
{
   public class SnapshotContext : SnapshotContext<TestProject>
   {
      public SnapshotContext(TestProject project, int version) : base(project, version)
      {
      }

      public SnapshotContext() : base(new TestProject(), 9)
      {

      }
   }

   public class SnapshotContextWithDataRepository : SnapshotContextWithDataRepository<TestProject>
   {
      public SnapshotContextWithDataRepository(DataRepository dataRepository, SnapshotContext snapshotContext) : base(dataRepository, snapshotContext, 9)
      {
         
      }
   }

   public class ClassificationSnapshotContext : ClassificationSnapshotContext<TestProject>
   {
      public ClassificationSnapshotContext(ClassificationType classificationType, SnapshotContext<TestProject> baseContext, int version) : base(classificationType, baseContext, version)
      {
      }

      public ClassificationSnapshotContext(ClassificationType classificationTypeFor, SnapshotContext<TestProject> snapshotContext) : base(classificationTypeFor, snapshotContext, 9)
      {
         
      }
   }

   public class ParameterSnapshotContext : SnapshotContext
   {
      public IParameter Parameter { get; }

      public ParameterSnapshotContext(IParameter parameter, SnapshotContext baseContext) : base(baseContext.Project, baseContext.Version)
      {
         Parameter = parameter;
      }
   }
}