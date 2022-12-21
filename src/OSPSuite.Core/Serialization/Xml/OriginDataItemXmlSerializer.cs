using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Serialization.Xml
{
   public class OriginDataItemXmlSerializer : OSPSuiteXmlSerializer<OriginDataItem>
   {
      public override void PerformMapping()
      {
         Map(x => x.Value);
         Map(x => x.Description);
         Map(x => x.Name);
         Map(x => x.DisplayName);
         Map(x => x.Icon);
      }
   }
}