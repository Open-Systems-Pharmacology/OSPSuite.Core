using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class EntitySourceXmlSerializer : OSPSuiteXmlSerializer<EntitySource>
   {
      public override void PerformMapping()
      {
         Map(x => x.EntityPath);
         Map(x => x.BuildingBlockName);
         Map(x => x.BuildingBlockType);
         Map(x => x.ModuleName);
         Map(x => x.SourcePath);
      }
   }
}