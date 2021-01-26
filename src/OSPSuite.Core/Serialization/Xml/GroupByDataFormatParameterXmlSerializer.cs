using OSPSuite.Core.Import;

namespace OSPSuite.Core.Serialization.Xml
{
   public class GroupByDataFormatParameterXmlSerializer : OSPSuiteXmlSerializer<GroupByDataFormatParameter>
   {
      public override void PerformMapping()
      {
         Map(x => x.ColumnName);
      }
   }
}
