using DevExpress.ClipboardSource.SpreadsheetML;
using OSPSuite.Core.Domain.ParameterIdentifications;

namespace OSPSuite.Presentation.Importer.Core.DataFormat
{
   public class ParameterConfiguration
   {
      public enum DataFormatParameterType
      {
         Mapping,
         GroupBy,
         MetaData
      }

      public ParameterConfiguration(DataFormatParameterType type, object data = null)
      {
         Type = type;
         Data = data;
      }

      public DataFormatParameterType Type { get; private set; }

      public object Data { get; private set; }

      public override string ToString()
      {
         return $"{Type}<{Data}>";
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
         Configuration = new ParameterConfiguration(ParameterConfiguration.DataFormatParameterType.MetaData, metaDataId);
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
         Configuration = new ParameterConfiguration(ParameterConfiguration.DataFormatParameterType.GroupBy);
      }
   }

   public class MappingDataFormatParameter : DataFormatParameter
   {
      public MappingDataFormatParameter(string columnName, Column mappedColumn) : base(columnName)
      {
         Configuration = new ParameterConfiguration(ParameterConfiguration.DataFormatParameterType.Mapping, mappedColumn);
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