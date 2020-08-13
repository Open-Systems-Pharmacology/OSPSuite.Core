using System;
using System.Collections.Generic;
using DevExpress.XtraRichEdit.Layout;
using OSPSuite.Assets;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Importer.Core.DataFormat;
using OSPSuite.Presentation.Importer.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility;

namespace OSPSuite.Presentation.Importer.Views
{
   public class ColumnMappingViewModel
   {
      public string ColumnName { get; private set; }
      private string _description;
      public string Description 
      {
         get => _description;
         set
         {
            _description = value;
         }
      }
      public DataFormatParameter Source { get; set; }
      public ColumnMappingViewModel(string columnName, string description, DataFormatParameter source)
      {
         ColumnName = columnName;
         Description = description;
         Source = source;
      }

      public override bool Equals(object obj)
      {
         var other = obj as ColumnMappingViewModel;
         return ColumnName == other.ColumnName && Description == other.Description && Source.Equals(other.Source);
      }

      public override int GetHashCode()
      {
         return base.GetHashCode();
      }
   }

   public static class ColumnMappingFormatter
   {
      static private readonly string _ignored = $"{ColumnMappingOption.DescriptionType.Ignored}";
      public static string Ignored()
      {
         return _ignored;
      }

      public static string GroupBy()
      {
         return $"{ColumnMappingOption.DescriptionType.GroupBy}";
      }

      public static string Mapping(string mappingId, string unit)
      {
         return $"{ColumnMappingOption.DescriptionType.Mapping},{mappingId},{unit}";
      }

      public static string MetaData(string metaDataId)
      {
         return $"{ColumnMappingOption.DescriptionType.MetaData},{metaDataId}";
      }

      public static string Stringify(DataFormatParameter model)
      {
         switch (model)
         {
            case IgnoredDataFormatParameter _:
               return Ignored();
            case GroupByDataFormatParameter _:
               return GroupBy();
            case MappingDataFormatParameter mp:
               return Mapping(mp.MappedColumn.Name.ToString(), mp.MappedColumn.Unit);
            case MetaDataFormatParameter mp:
               return MetaData(mp.MetaDataId);
            default:
               throw new Exception(Error.TypeNotSupported(model.GetType()));
         }
      }

      public static DataFormatParameter Parse(string columnName, string description)
      {
         var parsed = description.Split(',');
         if (parsed[0] == ColumnMappingOption.DescriptionType.Ignored.ToString())
         {
            return new IgnoredDataFormatParameter(columnName);
         }
         else if (parsed[0] == ColumnMappingOption.DescriptionType.GroupBy.ToString())
         {
            return new GroupByDataFormatParameter(columnName);
         }
         else if (parsed[0] == ColumnMappingOption.DescriptionType.Mapping.ToString())
         {
            var column = EnumHelper.ParseValue<Core.Column.ColumnNames>(parsed[1]);
            return new MappingDataFormatParameter(columnName, new Core.Column() { Name = column, Unit = parsed[2] });
         }
         else if (parsed[0] == ColumnMappingOption.DescriptionType.MetaData.ToString())
         {
            return new MetaDataFormatParameter(columnName, parsed[1]);
         }
         throw new Exception(Error.TypeNotSupported(parsed[0]));
      }
   }

   public interface IColumnMappingControl : IView<IColumnMappingPresenter>
   {
      void SetMappingSource(IReadOnlyList<ColumnMappingViewModel> mappings);

      void SetSettings(IReadOnlyList<ColumnInfo> columnInfos, IDataFormat format);
   }
}