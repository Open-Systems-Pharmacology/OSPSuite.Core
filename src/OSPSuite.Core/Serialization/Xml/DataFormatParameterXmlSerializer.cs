using OSPSuite.Core.Import;

namespace OSPSuite.Core.Serialization.Xml
{
   public class DataFormatParameterXmlSerializer<T> : OSPSuiteXmlSerializer<T>
      where T : DataFormatParameter
   {
      public override void PerformMapping()
      {
         Map(x => x.ColumnName);
      }
   }

   public class MetaDataFormatParameterXmlSerializer : DataFormatParameterXmlSerializer<MetaDataFormatParameter>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.MetaDataId);
         Map(x => x.IsColumn);
      }
   }

   public class MappingDataFormatParameterXmlSerializer : DataFormatParameterXmlSerializer<MappingDataFormatParameter>
   {
      public override void PerformMapping()
      {
         base.PerformMapping();
         Map(x => x.MappedColumn);
      }
   }

   public class GroupByDataFormatParameterXmlSerializer : DataFormatParameterXmlSerializer<GroupByDataFormatParameter>
   {
   }
}
