using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Qualification
{
   public class BuildingBlockSwap : IWithName
   {
      public PKSimBuildingBlockType Type { get; set; }
      public string Name { get; set; }

      /// <summary>
      /// Absolute full path of snapshot file
      /// </summary>
      public string SnapshotFile { get; set; }

      public void Deconstruct(out PKSimBuildingBlockType type, out string name, out string snapshotFile)
      {
         type = Type;
         name = Name;
         snapshotFile = SnapshotFile;
      }
   }
}