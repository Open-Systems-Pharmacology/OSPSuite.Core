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
         return Data?.ToString() ?? string.Empty;
      }
   }

   public abstract class DataFormatParameter
   {
      public string ColumnName { get; private set; }

      protected DataFormatParameter(string columnName)
      {
         ColumnName = columnName;
      }
   }

   public class IgnoredDataFormatParameter : DataFormatParameter
   {
      public IgnoredDataFormatParameter(string columnName) : base(columnName)
      { }
   }

   public class MetaDataFormatParameter : DataFormatParameter
   {
      public MetaDataFormatParameter(string columnName, string metaDataId) : base(columnName)
      {
         MetaDataId = metaDataId;
      }

      public string MetaDataId { get; private set; }
   }

   public class GroupByDataFormatParameter : DataFormatParameter
   {
      public GroupByDataFormatParameter(string columnName) : base(columnName)
      { }
   }

   public class MappingDataFormatParameter : DataFormatParameter
   {
      public MappingDataFormatParameter(string columnName, Column mappedColumn) : base(columnName)
      {
         MappedColumn = mappedColumn;
      }

      public Column MappedColumn { get; private set; }
   }
}