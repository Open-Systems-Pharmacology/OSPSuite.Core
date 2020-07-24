namespace OSPSuite.Presentation.Importer.Core.DataFormat
{
   public enum DataFormatParameterType
   {
      Mapping,
      GroupBy,
      MetaData
   }

   public abstract class DataFormatParameter
   {
      public string ColumnName { get; protected set; }

      public DataFormatParameterType Type { get; protected set; }
   }

   public class MetaDataFormatParameter : DataFormatParameter
   {
      public MetaDataFormatParameter(string columnName)
      {
         Type = DataFormatParameterType.MetaData;
         ColumnName = columnName;
      }
   }

   public class GroupByDataFormatParameter : DataFormatParameter
   {
      public GroupByDataFormatParameter(string columnName)
      {
         Type = DataFormatParameterType.GroupBy;
         ColumnName = columnName;
      }
   }

   public class MappingDataFormatParameter : DataFormatParameter
   {
      public Column MappedColumn { get; private set; }

      public MappingDataFormatParameter(string columnName, Column mappedColumn)
      {
         MappedColumn = mappedColumn;
         Type = DataFormatParameterType.Mapping;
         ColumnName = columnName;
      }
   }
}