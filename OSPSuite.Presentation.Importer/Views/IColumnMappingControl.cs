using System;
using System.Collections.Generic;
using OSPSuite.Assets;
using OSPSuite.Presentation.Importer.Core.DataFormat;
using OSPSuite.Presentation.Importer.Presenters;
using OSPSuite.Presentation.Views;
using OSPSuite.Utility.Reflection;

namespace OSPSuite.Presentation.Importer.Views
{
   public class ColumnMappingViewModel : Notifier
   {
      public string MappingName { get; private set; }
      private string _description;

      public string Description 
      {
         get => _description;
         set => SetProperty(ref _description, value);
      }
      private DataFormatParameter _source;
      public DataFormatParameter Source 
      {
         get => _source;
         set
         {
            _source = value;
            Description = ColumnMappingFormatter.Stringify(value);
         }
      }
      public int Icon { get; set; }
      public enum ColumnType
      {
         Mapping,
         MetaData,
         GroupBy,
         AddGroupBy
      }
      public ColumnType CurrentColumnType { get; set; }
      public ColumnMappingViewModel(ColumnType columnType, string columnName, string description, DataFormatParameter source, int icon)
      {
         CurrentColumnType = columnType;
         MappingName = columnName;
         Description = description;
         Source = source;
         Icon = icon;
      }
   }

   public static class ColumnMappingFormatter
   {
      private static readonly string _ignored = $"{ColumnMappingOption.DescriptionType.Ignored}";
      public static string Ignored()
      {
         return _ignored;
      }

      public static string GroupBy(string columnName)
      {
         return $"{ColumnMappingOption.DescriptionType.GroupBy},{columnName}";
      }

      public static string AddGroupBy(string columnName)
      {
         return $"{ColumnMappingOption.DescriptionType.AddGroupBy},{columnName}";
      }

      public static string Mapping(string mappingId, string unit)
      {
         return $"{ColumnMappingOption.DescriptionType.Mapping},{mappingId},{unit}";
      }

      public static string MetaData(string metaDataId)
      {
         return $"{ColumnMappingOption.DescriptionType.MetaData},{metaDataId}";
      }

      public static string MappingName(DataFormatParameter model)
      {
         switch (model)
         {
            case IgnoredDataFormatParameter _:
               return Ignored();
            case GroupByDataFormatParameter _:
               return Captions.GroupByDescription;
            case MappingDataFormatParameter mp:
               return Captions.MappingDescription(mp.MappedColumn.Name.ToString(), mp.MappedColumn.Unit);
            case MetaDataFormatParameter mp:
               return Captions.MetaDataDescription(mp.MetaDataId);
            default:
               return Ignored();
         }
      }

      public static string Stringify(DataFormatParameter model)
      {
         switch (model)
         {
            case IgnoredDataFormatParameter _:
               return Ignored();
            case GroupByDataFormatParameter gb:
               return GroupBy(gb.ColumnName);
            case MappingDataFormatParameter mp:
               return Mapping(mp.MappedColumn.Name.ToString(), mp.MappedColumn.Unit);
            case MetaDataFormatParameter mp:
               return MetaData(mp.MetaDataId);
            case AddGroupByFormatParameter ag:
               return AddGroupBy(ag.GroupingByColumn);
            default:
               return Ignored();
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
            return new MappingDataFormatParameter(columnName, new Core.Column() { Name = parsed[1], Unit = parsed[2] });
         }
         else if (parsed[0] == ColumnMappingOption.DescriptionType.MetaData.ToString())
         {
            return new MetaDataFormatParameter(columnName, parsed[1]);
         }
         else if (parsed[0] == ColumnMappingOption.DescriptionType.AddGroupBy.ToString())
         {
            return new AddGroupByFormatParameter(columnName, parsed[1]);
         }
         throw new Exception(Error.TypeNotSupported(parsed[0]));
      }
   }

   public interface IColumnMappingControl : IView<IColumnMappingPresenter>
   {
      void SetMappingSource(IList<ColumnMappingViewModel> mappings);

      void Rebind();
   }
}