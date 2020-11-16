using System;
using System.Collections.Generic;
using DevExpress.XtraEditors.DXErrorProvider;
using OSPSuite.Assets;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Core.DataFormat;
using OSPSuite.Presentation.Presenters.Importer;
using OSPSuite.Utility.Reflection;

namespace OSPSuite.Presentation.Views.Importer
{
   public class ColumnMappingDTO : Notifier, IDXDataErrorInfo
   {
      public string MappingName { get; private set; }
      private string _description;
      public ColumnInfo ColumnInfo { get; private set; }

      public string ErrorMessage { get; set; }

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
      public ColumnMappingDTO(ColumnType columnType, string columnName, string description, DataFormatParameter source, int icon, ColumnInfo columnInfo = null)
      {
         ColumnInfo = columnInfo;
         CurrentColumnType = columnType;
         MappingName = columnName;
         Description = description;
         Source = source;
         Icon = icon;
      }
      public enum MappingStatus
      {
         NotSet,
         Valid,
         Invalid,
         InvalidUnit
      }
      public MappingStatus Status { get; set; }

      public void GetPropertyError(string propertyName, ErrorInfo info)
      {
         switch (Status)
         {
            case MappingStatus.Invalid:
               info.ErrorText = Captions.MissingMandatoryMapping;
               info.ErrorType = ErrorType.Critical;
               break;
            case MappingStatus.InvalidUnit:
               info.ErrorText = Captions.MissingUnit;
               info.ErrorType = ErrorType.Critical;
               break;
            default:
               info.ErrorText = "";
               info.ErrorType = ErrorType.None;
               break;
         }
      }

      public void GetError(ErrorInfo info)
      {
         switch (Status)
         {
            case MappingStatus.Invalid:
               info.ErrorText = Captions.MissingMandatoryMapping;
               info.ErrorType = ErrorType.Critical;
               break;
            case MappingStatus.InvalidUnit:
               info.ErrorText = Captions.MissingUnit;
               info.ErrorType = ErrorType.Critical;
               break;
            default:
               info.ErrorText = "";
               info.ErrorType = ErrorType.None;
               break;
         }
      }
   }

   public static class ColumnMappingFormatter
   {
      private static readonly string _ignored = $"{ColumnMappingOption.DescriptionType.Ignored}";
      public static string Ignored(IgnoredDataFormatParameter model = null)
      {
         if (model == null)
            return _ignored;
         return $"{_ignored},{model.ColumnName}";
      }

      public static string GroupBy(GroupByDataFormatParameter model)
      {
         return $"{ColumnMappingOption.DescriptionType.GroupBy},{model.ColumnName}";
      }

      public static string AddGroupBy(AddGroupByFormatParameter model)
      {
         return $"{ColumnMappingOption.DescriptionType.AddGroupBy},{model.ColumnName}";
      }

      public static string Mapping(MappingDataFormatParameter model)
      {
         return $"{ColumnMappingOption.DescriptionType.Mapping},{model.ColumnName},{model.MappedColumn.Name},{model.MappedColumn.Unit.SelectedUnit}";
      }

      public static string MetaData(MetaDataFormatParameter model)
      {
         return $"{ColumnMappingOption.DescriptionType.MetaData},{model.ColumnName},{model.MetaDataId}";
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
               return Captions.MappingDescription(mp.MappedColumn.Name, mp.MappedColumn.Unit.SelectedUnit);
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
            case IgnoredDataFormatParameter m:
               return Ignored(m);
            case GroupByDataFormatParameter gb:
               return GroupBy(gb);
            case MappingDataFormatParameter mp:
               return Mapping(mp);
            case MetaDataFormatParameter mp:
               return MetaData(mp);
            case AddGroupByFormatParameter ag:
               return AddGroupBy(ag);
            default:
               return Ignored();
         }
      }

      public static DataFormatParameter Parse(string description)
      {
         var parsed = description.Split(',');
         if (parsed[0] == ColumnMappingOption.DescriptionType.Ignored.ToString())
         {
            return new IgnoredDataFormatParameter(parsed.Length == 2 ? parsed[1] : "");
         }
         else if (parsed[0] == ColumnMappingOption.DescriptionType.GroupBy.ToString())
         {
            return new GroupByDataFormatParameter(parsed[1]);
         }
         else if (parsed[0] == ColumnMappingOption.DescriptionType.Mapping.ToString())
         {
            return new MappingDataFormatParameter(parsed[1], new Column() { Name = parsed[2], Unit = new UnitDescription(parsed[3]) });
         }
         else if (parsed[0] == ColumnMappingOption.DescriptionType.MetaData.ToString())
         {
            return new MetaDataFormatParameter(parsed[1], parsed[2]);
         }
         else if (parsed[0] == ColumnMappingOption.DescriptionType.AddGroupBy.ToString())
         {
            return new AddGroupByFormatParameter(parsed[1]);
         }
         throw new Exception(Error.TypeNotSupported(parsed[0]));
      }
   }

   public interface IColumnMappingControl : IView<IColumnMappingPresenter>
   {
      void SetMappingSource(IList<ColumnMappingDTO> mappings);
      void RefreshData();
      void FillSubView(IView view);
      void CloseEditor();
   }
}