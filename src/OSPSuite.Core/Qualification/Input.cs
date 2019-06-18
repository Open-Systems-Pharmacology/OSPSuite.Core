using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Qualification
{
   public class Input : IWithName, IReferencingProject
   {
      public PKSimBuildingBlockType Type { get; set; }
      public string Name { get; set; }
      public string Project { get; set; }
      public int SectionId { get; set; }
   }
}