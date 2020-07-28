using NPOI.OpenXml4Net.OPC;

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

      public DataFormatParameterType Type { get; }

      public ParameterConfiguration(DataFormatParameterType type)
      {
         Type = type;
      }

      public override string ToString()
      {
         return $"{Type}";
      }
   }

   public class ParameterConfiguration<T> : ParameterConfiguration where T : class
   {
      public T Data { get; }

      public ParameterConfiguration(DataFormatParameterType type, T data = null) : base(type)
      {
         Data = data;
      }

      public override string ToString()
      {
         return $"{Type}<{Data}>";
      }
   }


   public interface  IDataFormatParameter
   {
      string ColumnName { get; }
      ParameterConfiguration.DataFormatParameterType Type { get; }
   }


   public abstract class DataFormatParameter<T> : IDataFormatParameter where T : class
   {
      public string ColumnName { get; }

      public ParameterConfiguration.DataFormatParameterType Type => Configuration.Type;
      
      //CONSTRUCTOR???
      public ParameterConfiguration<T> Configuration { get; set; }

      protected DataFormatParameter(string columnName)
      {
         ColumnName = columnName;
      }
   }

   public class MetaDataFormatParameter : DataFormatParameter<string>
   {
      public MetaDataFormatParameter(string columnName, string metaDataId) : base(columnName)
      {
         Configuration = new ParameterConfiguration<string>(ParameterConfiguration.DataFormatParameterType.MetaData, metaDataId);
      }

      public string MetaDataId => Configuration.Data;
   }

   public class GroupByDataFormatParameter : DataFormatParameter<string>
   {
      public GroupByDataFormatParameter(string columnName) : base(columnName)
      {
         Configuration = new ParameterConfiguration<string>(ParameterConfiguration.DataFormatParameterType.GroupBy);
      }
   }

   public class MappingDataFormatParameter : DataFormatParameter<Column>
   {
      public MappingDataFormatParameter(string columnName, Column mappedColumn) : base(columnName)
      {
         Configuration = new ParameterConfiguration<Column>(ParameterConfiguration.DataFormatParameterType.Mapping, mappedColumn);
      }

      public Column MappedColumn => Configuration.Data;
   }
}