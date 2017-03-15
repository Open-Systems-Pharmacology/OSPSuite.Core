using OSPSuite.Core.Domain.Descriptors;

namespace OSPSuite.Core.Serialization.Xml
{
   public class TagSerializer : OSPSuiteXmlSerializer<Tag>
   {
      public override void PerformMapping()
      {
         Map(x => x.Value);
      }
   }
}