using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Qualification
{
   public class BuildingBlockRef : IWithName, IReferencingProject
   {
      public PKSimBuildingBlockType Type { get; set; }
      public string Name { get; set; }
      public string Project { get; set; }
   }
}