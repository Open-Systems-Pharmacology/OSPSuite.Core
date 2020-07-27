using DevExpress.ClipboardSource.SpreadsheetML;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Presentation.Importer.Core.DataFormat
{
   public class ParameterConfiguration
   {
      public ParameterConfiguration(object data = null)
      {
         Data = data;
      }

      public object Data { get; private set; }

      public override string ToString()
      {
         return Data?.ToString();
      }
   }

   public abstract class DataFormatParameter
   {
      public string ColumnName { get; private set; }

      public ParameterConfiguration Configuration { get; set; }

      protected DataFormatParameter(string columnName)
      {
         ColumnName = columnName;
      }
   }

   public class MetaDataFormatParameter : DataFormatParameter
   {
      public MetaDataFormatParameter(string columnName, string metaDataId) : base(columnName)
      {
         Configuration = new ParameterConfiguration(metaDataId);
      }

      public string MetaDataId
      {
         get
         {
            return Configuration.Data as string;
         }
      }
   }

   public class GroupByDataFormatParameter : DataFormatParameter
   {
      public GroupByDataFormatParameter(string columnName) : base(columnName)
      {
         Configuration = new ParameterConfiguration();
      }
   }

   public class MappingDataFormatParameter : DataFormatParameter
   {
      public MappingDataFormatParameter(string columnName, Column mappedColumn) : base(columnName)
      {
         Configuration = new ParameterConfiguration(mappedColumn);
      }

      public Column MappedColumn 
      { 
         get
         {
            return Configuration.Data as Column;
         }
      }
   }
}