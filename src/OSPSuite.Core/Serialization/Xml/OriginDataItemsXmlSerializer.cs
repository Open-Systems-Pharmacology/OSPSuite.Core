using OSPSuite.Core.Domain.Builder;

namespace OSPSuite.Core.Serialization.Xml
{
   public class OriginDataItemsXmlSerializer : OSPSuiteXmlSerializer<OriginDataItems>
   {
      protected OriginDataItemsXmlSerializer(string name)
         : base(name)
      {
      }

      protected OriginDataItemsXmlSerializer() 
      {
      }

      public override void PerformMapping()
      {
         Map(x => x.ValueOrigin);
         MapEnumerable(x => x.AllDataItems, x=> x.AddOriginDataItem);
      }
   }
}