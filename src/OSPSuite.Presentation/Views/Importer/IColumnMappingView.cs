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
      public string MappingName { get; }

      public string ExcelColumn
      {
         get;
         set;
      }

      public ColumnInfo ColumnInfo { get; }

      public string ErrorMessage { get; set; }

      private DataFormatParameter _source;
      public DataFormatParameter Source 
      {
         get => _source;
         set
         {
            _source = value;
            if (value != null)
               ExcelColumn = value.ColumnName;
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
      public ColumnMappingDTO(ColumnType columnType, string columnName, DataFormatParameter source, int icon, ColumnInfo columnInfo = null)
      {
         ColumnInfo = columnInfo;
         CurrentColumnType = columnType;
         MappingName = columnName;
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

   public interface IColumnMappingView : IView<IColumnMappingPresenter>
   {
      void SetMappingSource(IList<ColumnMappingDTO> mappings);
      void RefreshData();
      void FillSubView(IView view);
      void CloseEditor();
   }
}