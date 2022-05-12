using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Utility.Extensions;

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

      public abstract bool IsGroupingCriterion();
      public abstract bool ComesFromColumn();

      public abstract string TooltipTitle();

      public abstract string TooltipDescription();
   }
   
   public class AddGroupByFormatParameter : DataFormatParameter
   {
      public AddGroupByFormatParameter() { }

      public AddGroupByFormatParameter(string columnName) : base(columnName)
      {
      }

      public override string TooltipDescription()
      {
         return Captions.Importer.AddGroupByHint;
      }

      public override bool IsGroupingCriterion()
      {
         return false;
      }

      public override bool ComesFromColumn()
      {
         return false;
      }

      public override string TooltipTitle()
      {
         return Captions.Importer.AddGroupByTitle;
      }
   }

   public class MetaDataFormatParameter : DataFormatParameter
   {
      public MetaDataFormatParameter() { }

      public MetaDataFormatParameter(string columnName, string metaDataId, bool isColumn = true) : base(columnName)
      {
         MetaDataId = metaDataId;
         IsColumn = isColumn;
      }

      public string MetaDataId { get; }

      public bool IsColumn { get; set; }

      public override bool EquivalentTo(DataFormatParameter other)
      {
         return base.EquivalentTo(other) && MetaDataId == (other as MetaDataFormatParameter).MetaDataId;
      }

      public override bool IsGroupingCriterion()
      {
         return ColumnName != null;
      }

      public override bool ComesFromColumn()
      {
         return IsColumn && IsGroupingCriterion();
      }

      public override string TooltipDescription()
      {
         return Captions.Importer.MetaDataHint(ColumnName, MetaDataId);
      }

      public override string TooltipTitle()
      {
         return Captions.Importer.MetaDataTitle;
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

      public override bool IsGroupingCriterion()
      {
         return true;
      }

      public override bool ComesFromColumn()
      {
         return true;
      }

      public override string TooltipDescription()
      {
         return Captions.Importer.GroupByHint(ColumnName);
      }

      public override string TooltipTitle()
      {
         return Captions.Importer.GroupByTitle;
      }
   }

   public class MappingDataFormatParameter : DataFormatParameter
   {
      public MappingDataFormatParameter() 
      {
         MappedColumn = new Column();
      }

      public MappingDataFormatParameter(string columnName, Column mappedColumn) : base(columnName)
      {
         MappedColumn = mappedColumn;
      }

      public Column MappedColumn { get; set; }

      public override bool EquivalentTo(DataFormatParameter other)
      {
         return base.EquivalentTo(other) && MappedColumn.Name == (other as MappingDataFormatParameter).MappedColumn.Name;
      }

      public override bool IsGroupingCriterion()
      {
         return false;
      }

      public override bool ComesFromColumn()
      {
         return true;
      }

      public override string TooltipDescription()
      {
         if (MappedColumn.ErrorStdDev == Constants.STD_DEV_GEOMETRIC)
            return Captions.Importer.MappingHintGeometricError;

         if (!MappedColumn.Unit.ColumnName.IsNullOrEmpty())
            return Captions.Importer.MappingHintUnitColumn(ColumnName, MappedColumn.Name, MappedColumn.Unit.ColumnName);
         
         if (!MappedColumn.Unit.SelectedUnit.IsNullOrEmpty())
            return Captions.Importer.MappingHint(ColumnName, MappedColumn.Name, MappedColumn.Unit.SelectedUnit);

         return Captions.Importer.MappingHintNoUnit(ColumnName, MappedColumn.Name);
      }

      public override string TooltipTitle()
      {
         return Captions.Importer.MappingTitle;
      }
   }
}