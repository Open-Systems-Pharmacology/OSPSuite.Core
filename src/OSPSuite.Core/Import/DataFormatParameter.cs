namespace OSPSuite.Core.Import
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
      public string ColumnName { get; set; }

      protected DataFormatParameter(string columnName)
      {
         ColumnName = columnName;
      }

      protected DataFormatParameter() { }

      public virtual bool EquivalentTo(DataFormatParameter other)
      {
         return other.GetType() == GetType();
      }
   }
   
   public class AddGroupByFormatParameter : DataFormatParameter
   {
      public AddGroupByFormatParameter() { }

      public AddGroupByFormatParameter(string columnName) : base(columnName)
      {
      }
   }

   public class MetaDataFormatParameter : DataFormatParameter
   {
      public MetaDataFormatParameter() { }

      public MetaDataFormatParameter(string columnName, string metaDataId) : base(columnName)
      {
         MetaDataId = metaDataId;
      }

      public string MetaDataId { get; set; }

      public override bool EquivalentTo(DataFormatParameter other)
      {
         return base.EquivalentTo(other) && MetaDataId == (other as MetaDataFormatParameter).MetaDataId;
      }
   }

   public class GroupByDataFormatParameter : DataFormatParameter
   {
      public GroupByDataFormatParameter() { }

      public GroupByDataFormatParameter(string columnName) : base(columnName)
      { }

      public override bool EquivalentTo(DataFormatParameter other)
      {
         return base.EquivalentTo(other) && ColumnName == (other as GroupByDataFormatParameter).ColumnName;
      }
   }

   public class MappingDataFormatParameter : DataFormatParameter
   {
      public MappingDataFormatParameter() { }

      public MappingDataFormatParameter(string columnName, Column mappedColumn) : base(columnName)
      {
         MappedColumn = mappedColumn;
      }

      public Column MappedColumn { get; set; }

      public override bool EquivalentTo(DataFormatParameter other)
      {
         return base.EquivalentTo(other) && MappedColumn.Name == (other as MappingDataFormatParameter).MappedColumn.Name;
      }
   }
}