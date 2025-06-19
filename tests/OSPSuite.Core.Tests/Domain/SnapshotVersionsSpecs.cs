using OSPSuite.BDDHelper;
using OSPSuite.BDDHelper.Extensions;
using OSPSuite.Core.Snapshots;

namespace OSPSuite.Core.Domain
{
   internal class SnapshotVersionsSpecs : StaticContextSpecification
   {
      [Observation]
      public void versions_below_current_can_be_loaded_and_versions_should_be_default_for_older_snapshots()
      {
         for (int i = 0; i <= SnapshotVersions.Current; i++)
         {
            SnapshotVersions.CanLoadVersion(i).ShouldBeTrue();
            if(i <= SnapshotVersions.V9)
               SnapshotVersions.FindBy(i).ShouldBeEqualTo(SnapshotVersions.V9);
         }
      }

      [Observation]
      public void versions_newer_than_current_cannot_be_loaded()
      {
         SnapshotVersions.CanLoadVersion(SnapshotVersions.Current+1).ShouldBeFalse();
      }
   }
}
