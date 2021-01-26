using OSPSuite.Core.Import;

namespace OSPSuite.Core.Serialization.Xml
{
   public class DataFormatParameterXmlSerializer : OSPSuiteXmlSerializer<DataFormatParameter>
   {
      public override void PerformMapping()
      {
         Map(x => x.ColumnName);
      }
   }
}
