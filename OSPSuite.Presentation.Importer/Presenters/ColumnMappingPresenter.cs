using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Importer.Core.DataFormat;
using OSPSuite.Presentation.Importer.Views;
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


      public ColumnMappingPresenter(IColumnMappingControl view, IImporterTask importerTask) : base(view)
      {
         _importerTask = importerTask;
      }

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

      public void SetDataFormat(IDataFormat format)
      {
         _format = format;
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
      }

      public void ClearActiveRow()
      {
         _activeRow.Source = ColumnMappingFormatter.Parse(_activeRow.Source.ColumnName, _activeRow.Description);
      }

      public void ResetMapping()
      {
         SetDataFormat(_format);
      }

      public void ClearMapping()
      {
         _mappings.Each(
            m =>
            {
               m.Description = ColumnMappingFormatter.Ignored();
               m.Source = ColumnMappingFormatter.Parse(m.Source.ColumnName, m.Description);
            }
         );
         View.SetMappingSource(_mappings);
      }
   }
}