using OSPSuite.Core.Import;

namespace OSPSuite.Core.Serialization.Xml
{
   public class AddGroupByFormatParameterXmlSerializer : OSPSuiteXmlSerializer<AddGroupByFormatParameter>
   {
      public override void PerformMapping()
      {
         Map(x => x.ColumnName);
      }
   }
}
