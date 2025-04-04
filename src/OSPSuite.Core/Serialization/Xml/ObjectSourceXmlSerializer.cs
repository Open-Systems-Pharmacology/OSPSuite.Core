using OSPSuite.Core.Domain;

namespace OSPSuite.Core.Serialization.Xml
{
   public class ObjectSourceXmlSerializer : OSPSuiteXmlSerializer<ObjectSource>
   {
      public override void PerformMapping()
      {
         Map(x => x.ObjectId);
         Map(x => x.ModuleId);
         Map(x => x.BuildingBlockId);
         Map(x => x.SourceId);
         Map(x => x.SourceType);
      }
   }
}