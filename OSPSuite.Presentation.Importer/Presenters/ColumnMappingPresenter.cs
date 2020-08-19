using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Importer.Core.DataFormat;
using OSPSuite.Presentation.Importer.Services;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Importer.Views.Dialog;
using OSPSuite.Presentation.Presenters;
using OSPSuite.Utility;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public class ColumnMappingPresenter : AbstractPresenter<IColumnMappingControl, IColumnMappingPresenter>, IColumnMappingPresenter
   {
      private IDataFormat _format;
      private List<ColumnMappingViewModel> _mappings;
      private IReadOnlyList<ColumnInfo> _columnInfos; 
      private IReadOnlyList<MetaDataCategory> _metaDataCategories;
      private DataImporterSettings _dataImporterSettings;
      private readonly IImporterTask _importerTask;
      private ColumnMappingViewModel _activeRow;
      private string _sheetName;
      private IEmptyDialog _emptyDialog;
      private IEnumerable<IDataFormat> _availableFormats;

      public ColumnMappingPresenter
      (
         IColumnMappingControl view, 
         IImporterTask importerTask,
         IEmptyDialog emptyDialog
      ) : base(view)
      {
         _importerTask = importerTask;
         _emptyDialog = emptyDialog;
         View.OnFormatChange += (formatName) => this.DoWithinExceptionHandler(() =>
         {
            var format = _availableFormats.First(f => f.Name == formatName);
            SetDataFormat(format, _availableFormats, _sheetName);
            OnFormatChange?.Invoke(format);
         });
      }

      public event FormatChangedHandler OnFormatChange;

      public void SetSettings(
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         IReadOnlyList<ColumnInfo> columnInfos,
         DataImporterSettings dataImporterSettings
      )
      {
         _columnInfos = columnInfos;
         _metaDataCategories = metaDataCategories;
         _dataImporterSettings = dataImporterSettings;
      }

      public void SetDataFormat(IDataFormat format, IEnumerable<IDataFormat> availableFormats, string sheetName)
      {
         _format = format;
         _availableFormats = availableFormats;
         _sheetName = sheetName;
         _mappings = format.Parameters.Select(p =>
         {
            return new ColumnMappingViewModel
            (
               p.ColumnName,
               ColumnMappingFormatter.Stringify(p), 
               p
            );
         }).ToList();
         View.SetMappingSource(_mappings);
         View.SetFormats(availableFormats.Select(f => f.Name), format.Name);
         ValidateMapping();
      }

      private ColumnMappingOption generateIgnoredColumnMappingOption(string description)
      {
         return new ColumnMappingOption()
         {
            Label = Captions.Importer.NoneEditorNullText,
            Description = description,
            IconIndex = -1
         };
      }

      public void ChangeUnitsOnActiveRow()
      {
         var unitsEditorPresenter = _emptyDialog.Show<IUnitsEditorPresenter>(430, 180);
         var column = (_activeRow.Source as MappingDataFormatParameter).MappedColumn;
         unitsEditorPresenter.SetParams
         (
            column,
            _columnInfos
               .First(i => i.DisplayName == column.Name.ToString())
               .DimensionInfos
               .Select(d => d.Dimension)
         );
         unitsEditorPresenter.OnOK += (units) =>
         {
            View.BeginUpdate();
            column.Unit = units;
            View.EndUpdate();
            OnDataFormatParametersChanged?.Invoke(this, new DataFormatParametersChangedArgs() 
            { 
               Parameters = new List<DataFormatParameter>() { _activeRow.Source } 
            });
         };
      }

      private ColumnMappingOption generateGroupByColumnMappingOption(string description)
      {
         return new ColumnMappingOption()
         {
            Label = Captions.GroupByTitle,
            Description = description,
            IconIndex = _importerTask.GetImageIndex(new GroupByDataFormatParameter(""))
         };
      }

      private ColumnMappingOption generateMappingColumnMappingOption(string description, string columnName, string mappingId, string unit = "?")
      {
         var mappedColumnName = EnumHelper.ParseValue<Column.ColumnNames>(mappingId);
         return new ColumnMappingOption()
         {
            Label = Captions.MappingDescription(mappingId, unit),
            Description = description,
            IconIndex = _importerTask.GetImageIndex(new MappingDataFormatParameter(columnName, new Column() { Name = mappedColumnName }), _mappings.Select(m => m.Source))
         };
      }

      private ColumnMappingOption generateMetaDataColumnMappingOption(string description, string columnName, string metaDataId)
      {
         return new ColumnMappingOption()
         {
            Label = Captions.MetaDataDescription(metaDataId),
            Description = description,
            IconIndex = _importerTask.GetImageIndex(new MetaDataFormatParameter(columnName, metaDataId), _mappings.Select(m => m.Source))
         };
      }

      public IEnumerable<ColumnMappingOption> GetAvailableOptionsFor(int rowHandle)
      {
         _activeRow = _mappings.ElementAt(rowHandle);
         var options = new List<ColumnMappingOption>();
         switch (_activeRow.Source)
         {
            case IgnoredDataFormatParameter _:
               options.Add(generateIgnoredColumnMappingOption(_activeRow.Description));
               break;
            case GroupByDataFormatParameter _:
               options.Add(generateGroupByColumnMappingOption(_activeRow.Description));
               break;
            case MappingDataFormatParameter tm:
               options.Add(generateMappingColumnMappingOption(_activeRow.Description, tm.ColumnName, tm.MappedColumn.Name.ToString(), tm.MappedColumn.Unit));
               break;
            case MetaDataFormatParameter tm:
               options.Add(generateMetaDataColumnMappingOption(_activeRow.Description, tm.ColumnName, tm.MetaDataId));
               break;
            default:
               throw new Exception(Error.TypeNotSupported(_activeRow.Source.GetType()));
         }

         //Ignored
         if (!(_activeRow.Source is IgnoredDataFormatParameter))
         {
            options.Add(generateIgnoredColumnMappingOption(ColumnMappingFormatter.Ignored()));
         }

         //GroupBy
         if (!(_activeRow.Source is GroupByDataFormatParameter))
         {
            options.Add(generateGroupByColumnMappingOption(ColumnMappingFormatter.GroupBy()));
         }

         //Mappings only for missing columns
         foreach (var info in _columnInfos)
         {
            if (!_mappings.Any(m =>
               m.Source is MappingDataFormatParameter && (m.Source as MappingDataFormatParameter)?.MappedColumn.Name.ToString() == info.DisplayName))
            {
               options.Add(
                  generateMappingColumnMappingOption(
                     ColumnMappingFormatter.Mapping(info.DisplayName, "?"),
                     _activeRow.ColumnName, 
                     info.DisplayName
                  )
               );
            }
         }

         //MetaData only for missing data
         foreach (var category in _metaDataCategories)
         {
            if (!_mappings.Any(m => m.Source is MetaDataFormatParameter && (m.Source as MetaDataFormatParameter).MetaDataId == category.DisplayName))
            {
               options.Add(
                  generateMetaDataColumnMappingOption(
                     ColumnMappingFormatter.MetaData(category.DisplayName),
                     _activeRow.ColumnName,
                     category.DisplayName
                  )
               );
            }
         }

         return options;
      }

      public ToolTipDescription ToolTipDescriptionFor(int index)
      {
         var element = _mappings.ElementAt(index).Source;
         switch (element)
         {
            case MappingDataFormatParameter mp:
               return new ToolTipDescription()
               {
                  Title = Captions.MappingTitle,
                  Description = Captions.MappingHint(mp.ColumnName, mp?.MappedColumn.Name.ToString(), mp.MappedColumn.Unit)
               };
            case GroupByDataFormatParameter gp:
               return new ToolTipDescription()
               {
                  Title = Captions.GroupByTitle,
                  Description = Captions.GroupByHint(gp.ColumnName)
               };
            case MetaDataFormatParameter mp:
               return new ToolTipDescription()
               {
                  Title = Captions.MetaDataTitle,
                  Description = Captions.MetaDataHint(mp.ColumnName, mp.MetaDataId)
               };
            case IgnoredDataFormatParameter _:
               return new ToolTipDescription()
               {
                  Title = Captions.IgnoredParameterTitle,
                  Description = Captions.IgnoredParameterHint
               };
            default:
               throw new Exception(Error.TypeNotSupported(element.GetType()));
         }
      }

      public ButtonsConfiguration ButtonsConfigurationForActiveRow()
      {
         return new ButtonsConfiguration()
         {
            ShowButtons = !String.IsNullOrEmpty(_activeRow.ColumnName) && _activeRow.Description != ColumnMappingFormatter.Ignored(),
            UnitActive = _activeRow.Source is MappingDataFormatParameter
         };
      }

      public void SetDescriptionForActiveRow(string description)
      {
         _activeRow.Source = ColumnMappingFormatter.Parse(_activeRow.Source.ColumnName, description);
         OnDataFormatParametersChanged?.Invoke(this, new DataFormatParametersChangedArgs() 
         { 
            Parameters = new List<DataFormatParameter>() { _activeRow.Source } 
         });
      }

      public void ClearActiveRow()
      {
         _activeRow.Source = ColumnMappingFormatter.Parse(_activeRow.Source.ColumnName, _activeRow.Description);
         OnDataFormatParametersChanged?.Invoke(this, new DataFormatParametersChangedArgs() 
         { 
            Parameters = new List<DataFormatParameter>() { _activeRow.Source } 
         });
      }

      public void ResetMapping()
      {
         View.BeginUpdate();
         SetDataFormat(_format, _availableFormats, _sheetName);
         View.EndUpdate();
         OnDataFormatParametersChanged?.Invoke(this, new DataFormatParametersChangedArgs() 
         { 
            Parameters = _mappings.Select(m => m.Source)
         });
      }

      public void ClearMapping()
      {
         View.BeginUpdate();
         _mappings.Each(
            m =>
            {
               m.Description = ColumnMappingFormatter.Ignored();
               m.Source = ColumnMappingFormatter.Parse(m.Source.ColumnName, m.Description);
            }
         );
         View.SetMappingSource(_mappings);
         View.EndUpdate();
         OnDataFormatParametersChanged?.Invoke(this, new DataFormatParametersChangedArgs()
         {
            Parameters = _mappings.Select(m => m.Source)
         });
      }

      public void ValidateMapping()
      {
         var missingColumn = _importerTask.CheckWhetherAllDataColumnsAreMapped(_columnInfos, _mappings.Select(m => m.Source));
         if (missingColumn != null)
         {
            OnMissingMapping?.Invoke(this, new MissingMappingEventArgs { Message = missingColumn });
         }
         else
         {
            OnMappingCompleted?.Invoke(this, new MappingCompletedEventArgs { SheetName = _sheetName });
         }
      }

      public event MappingCompletedHandler OnMappingCompleted;      

      public event MissingMappingHandler OnMissingMapping;

      public event DataFormatParametersChangedHandler OnDataFormatParametersChanged;
   }
}