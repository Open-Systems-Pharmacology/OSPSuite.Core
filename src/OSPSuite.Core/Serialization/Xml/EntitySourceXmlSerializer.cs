using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class EntitySourceXmlSerializer : OSPSuiteXmlSerializer<EntitySource>
   {
      public override void PerformMapping()
      {
         Map(x => x.EntityId);
         Map(x => x.BuildingBlockId);
         Map(x => x.SourceId);
         Map(x => x.SourceType);
      }
   }
}