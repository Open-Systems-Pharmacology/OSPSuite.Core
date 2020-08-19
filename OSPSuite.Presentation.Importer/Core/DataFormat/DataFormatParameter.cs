namespace OSPSuite.Presentation.Importer.Core.DataFormat
{
   public class ParameterConfiguration<T> where T : class
   {
      public ParameterConfiguration(T data = null)
      {
         Data = data;
      }

      public T Data { get; }

      public override string ToString() => Data?.ToString() ?? string.Empty;

      public override bool Equals(object obj)
      {
         var other = obj as ParameterConfiguration<T>;
         return other.Data.Equals(Data);
      }

      public override int GetHashCode()
      {
         return base.GetHashCode();
      }
   }

   public interface IDataFormatParameter
   {
      string ColumnName { get; }
      string DataAsString { get; }
   }

   public abstract class DataFormatParameter<T> : IDataFormatParameter where T : class
   {
      public string ColumnName { get; }

      public ParameterConfiguration<T> Configuration { get; set; }

      protected DataFormatParameter(string columnName)
      {
         ColumnName = columnName;
      }

      public override bool Equals(object obj)
      {
         var other = obj as DataFormatParameter<T>;
         return other.ColumnName == ColumnName &&
                ((Configuration == null && other.Configuration == null) || other.Configuration.Equals(Configuration));
      }

      public override int GetHashCode()
      {
         return base.GetHashCode();
      }

      public string DataAsString => Configuration?.ToString();
   }

   public class IgnoredDataFormatParameter : DataFormatParameter<string>
   {
      public IgnoredDataFormatParameter(string columnName) : base(columnName)
      {
      }
   }

   public class MetaDataFormatParameter : DataFormatParameter<string>
   {
      public MetaDataFormatParameter(string columnName, string metaDataId) : base(columnName)
      {
         Configuration = new ParameterConfiguration<string>(metaDataId);
      }

      public string MetaDataId => Configuration.Data;
   }

   public class GroupByDataFormatParameter : DataFormatParameter<string>
   {
      public GroupByDataFormatParameter(string columnName) : base(columnName)
      {
      }
   }

   public class MappingDataFormatParameter : DataFormatParameter<Column>
   {
      public MappingDataFormatParameter(string columnName, Column mappedColumn) : base(columnName)
      {
         Configuration = new ParameterConfiguration<Column>(mappedColumn);
      }

      public Column MappedColumn => Configuration.Data;
   }
}