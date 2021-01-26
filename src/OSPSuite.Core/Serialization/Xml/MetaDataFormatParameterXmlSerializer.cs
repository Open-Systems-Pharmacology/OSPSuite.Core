using OSPSuite.Core.Import;

namespace OSPSuite.Core.Serialization.Xml
{
   public class MetaDataFormatParameterXmlSerializer : OSPSuiteXmlSerializer<MetaDataFormatParameter>
   {
      public override void PerformMapping()
      {
         Map(x => x.ColumnName);
         Map(x => x.MetaDataId);
      }
   }
}
