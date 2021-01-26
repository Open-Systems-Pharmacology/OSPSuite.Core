using OSPSuite.Core.Import;

namespace OSPSuite.Core.Serialization.Xml
{
   public class MappingDataFormatParameterXmlSerializer : OSPSuiteXmlSerializer<MappingDataFormatParameter>
   {
      public override void PerformMapping()
      {
         Map(x => x.ColumnName);
         Map(x => x.MappedColumn);
      }
   }
}
