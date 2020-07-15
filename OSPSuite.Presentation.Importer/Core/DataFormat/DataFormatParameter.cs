using System;

namespace OSPSuite.Presentation.Importer.Core.DataFormat
{
   public enum DataFormatParameterType
   {
      MAPPING,
      GROUP_BY,
      META_DATA
   }

   public abstract class DataFormatParameter
   {
      public string ColumnName { get; set; }

      public DataFormatParameterType Type { get; set; }
   }

   public class MetaDataFormatParameter : DataFormatParameter
   {
      public MetaDataFormatParameter(string columnName)
      {
         Type = DataFormatParameterType.META_DATA;
         ColumnName = columnName;
      }
   }

   public class GroupByDataFormatParameter : DataFormatParameter
   {
      public GroupByDataFormatParameter(string columnName)
      {
         Type = DataFormatParameterType.GROUP_BY;
         ColumnName = columnName;
      }
   }

   public class MappingDataFormatParameter : DataFormatParameter
   {
      public IColumn MappedColumn { get; private set; }
      public MappingDataFormatParameter(string columnName, IColumn mappedColumn)
      {
         MappedColumn = mappedColumn;
         Type = DataFormatParameterType.MAPPING;
         ColumnName = columnName;
      }
   }
}
