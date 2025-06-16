using OSPSuite.Core.Snapshots.Mappers;

namespace OSPSuite.Helpers.Snapshots
{
   public class ClassificationSnapshotTask : ClassificationSnapshotTask<TestProject>
   {
      public ClassificationSnapshotTask(ClassificationMapper<TestProject> classificationMapper) : base(classificationMapper)
      {
      }

      protected override ClassificationSnapshotContext<TestProject> ContextFor<TClassifiable, TSubject>(SnapshotContext<TestProject> snapshotContext)
      {
         return new ClassificationSnapshotContext(ClassificationTypeFor<TClassifiable>(), snapshotContext);
      }
   }
}
