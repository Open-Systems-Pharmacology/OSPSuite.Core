using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Core;
using OSPSuite.Presentation.Importer.Core;
using OSPSuite.Presentation.Importer.Core.DataFormat;
using OSPSuite.Presentation.Importer.Services;
using OSPSuite.Presentation.Importer.Views;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Importer.Presenters
{
   public class ColumnMappingPresenter : AbstractPresenter<IColumnMappingControl, IColumnMappingPresenter>, IColumnMappingPresenter
   {
      private IDataFormat _format;
      private List<ColumnMappingViewModel> _mappings;
      private IReadOnlyList<ColumnInfo> _columnInfos;
      private IReadOnlyList<MetaDataCategory> _metaDataCategories;
      private readonly IImporterTask _importerTask;
      private readonly IApplicationController _applicationController;
      private IList<DataFormatParameter> _originalFormat;
      public ColumnMappingPresenter
      (
         IColumnMappingControl view,
         IImporterTask importerTask,
         IApplicationController applicationController
      ) : base(view)
      {
         _importerTask = importerTask;
         _applicationController = applicationController;
      }

      public void SetSettings(
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         IReadOnlyList<ColumnInfo> columnInfos
      )
      {
         _columnInfos = columnInfos;
         _metaDataCategories = metaDataCategories;
      }

      private void setDataFormat(IList<DataFormatParameter> formatParameters)
      {
         _mappings = _columnInfos.Select(c =>
         {
            var target = formatParameters.OfType<MappingDataFormatParameter>().FirstOrDefault(m => m.MappedColumn.Name.ToString() == c.Name);
            return new ColumnMappingViewModel
            (
               c.Name,
               ColumnMappingFormatter.Stringify(target),
               target,
               _importerTask.GetImageIndex(new MappingDataFormatParameter("", null))
            );
         }).Union
         (
            _metaDataCategories.Select(md =>
            {
               var target = formatParameters.OfType<MetaDataFormatParameter>().FirstOrDefault(m => m.MetaDataId == md.Name);
               return new ColumnMappingViewModel
               (
                  md.Name,
                  ColumnMappingFormatter.Stringify(target),
                  target,
                  _importerTask.GetImageIndex(new MetaDataFormatParameter("", ""))
               );
            })
         ).Union
         (
            formatParameters.OfType<GroupByDataFormatParameter>().Select(
               gb =>
               {
                  return new ColumnMappingViewModel(
                     Captions.GroupByTitle,
                     ColumnMappingFormatter.Stringify(gb),
                     gb,
                     _importerTask.GetImageIndex(gb)
                  );
               }
            )
         ).Append
         (
            new ColumnMappingViewModel(
               Captions.AddGroupByTitle,
               null,
               new IgnoredDataFormatParameter(""),
               _importerTask.GetImageIndex(new GroupByDataFormatParameter(""))
            )
         ).ToList();
         View.SetMappingSource(_mappings);
         ValidateMapping();
      }

      public void SetDataFormat(IDataFormat format)
      {
         _format = format;
         _originalFormat = _format.Parameters.ToList();
         setDataFormat(format.Parameters);
      }

      private ColumnMappingOption generateIgnoredColumnMappingOption(string description)
      {
         return new ColumnMappingOption()
         {
            Label = Captions.Importer.NoneEditorNullText,
            Description = description
         };
      }

      public void ChangeUnitsOnRow(ColumnMappingViewModel model)
      {
         using (var unitsEditorPresenter = _applicationController.Start<IUnitsEditorPresenter>())
         {
            var activeRow = modelByMappingName(model.MappingName);
            var column = ((MappingDataFormatParameter)activeRow.Source).MappedColumn;
            unitsEditorPresenter.ShowFor
            (
               column,
               _columnInfos
                  .First(i => i.DisplayName == column.Name.ToString())
                  .DimensionInfos
                  .Select(d => d.Dimension)
            );
            if (!unitsEditorPresenter.Canceled)
            {
               column.Unit = unitsEditorPresenter.SelectedUnit;
               model.Description = ColumnMappingFormatter.Stringify(model.Source);
               _view.Rebind();
            }
         }
      }

      private ColumnMappingOption generateGroupByColumnMappingOption(string description)
      {
         return new ColumnMappingOption()
         {
            Label = Captions.GroupByTitle,
            Description = description
         };
      }

      private ColumnMappingOption generateMappingColumnMappingOption(string description, string columnName, string mappingId, string unit = "?")
      {
         return new ColumnMappingOption()
         {
            Label = Captions.MappingDescription(mappingId, unit),
            Description = description
         };
      }

      private ColumnMappingOption generateMetaDataColumnMappingOption(string description, string columnName, string metaDataId)
      {
         return new ColumnMappingOption()
         {
            Label = Captions.MetaDataDescription(metaDataId),
            Description = description
         };
      }

      public IEnumerable<ColumnMappingOption> GetAvailableOptionsFor(ColumnMappingViewModel model)
      {
         var activeRow = modelByMappingName(model.MappingName);
         var options = new List<ColumnMappingOption>();
         if (activeRow == null)
            return new List<ColumnMappingOption>();
         if (activeRow != null && activeRow.Source != null)
         {
            switch (activeRow.Source)
            {
               case IgnoredDataFormatParameter _:
                  options.Add(generateIgnoredColumnMappingOption(activeRow.Description));
                  break;
               case GroupByDataFormatParameter _:
                  options.Add(generateGroupByColumnMappingOption(activeRow.Description));
                  break;
               case MappingDataFormatParameter tm:
                  options.Add(generateMappingColumnMappingOption(activeRow.Description, tm.ColumnName, tm.MappedColumn.Name.ToString(),
                     tm.MappedColumn.Unit));
                  break;
               case MetaDataFormatParameter tm:
                  options.Add(generateMetaDataColumnMappingOption(activeRow.Description, tm.ColumnName, tm.MetaDataId));
                  break;
               default:
                  throw new Exception(Error.TypeNotSupported(activeRow.Source.GetType()));
            }

            //Ignored
            if (!(activeRow.Source is IgnoredDataFormatParameter))
            {
               options.Add(generateIgnoredColumnMappingOption(ColumnMappingFormatter.Ignored()));
            }

            //GroupBy
            if (!(activeRow.Source is GroupByDataFormatParameter))
            {
               options.Add(generateGroupByColumnMappingOption(ColumnMappingFormatter.GroupBy()));
            }
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
                     activeRow.MappingName,
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
                     activeRow.MappingName,
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
         if (element == null)
            return new ToolTipDescription()
            {
               Title = Captions.NotConfiguredField
            };
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

      public void SetDescriptionForRow(ColumnMappingViewModel model)
      {
         var activeRow = modelByMappingName(model.MappingName);
         activeRow.Source = ColumnMappingFormatter.Parse(model.MappingName, model.Description);
         OnParameterChanged.Invoke(model.MappingName, model.Source);
      }

      public event ParameterChangedHandler OnParameterChanged = delegate { };

      public void ClearRow(ColumnMappingViewModel model)
      {
         var activeRow = modelByColumnName(model.Source.ColumnName);
         if (activeRow.Source is GroupByDataFormatParameter)
         {
            _mappings.Remove(activeRow);
         }
         else
         {
            activeRow.Source = ColumnMappingFormatter.Parse(activeRow.Source.ColumnName, ColumnMappingFormatter.Ignored());
         }
         OnParameterChanged.Invoke(model.Source.ColumnName, model.Source);
         View.Rebind();
      }

      private ColumnMappingViewModel modelByMappingName(string mappingName)
      {
         return _mappings.FirstOrDefault(m => m.MappingName == mappingName);
      }

      private ColumnMappingViewModel modelByColumnName(string columnName)
      {
         return _mappings.FirstOrDefault(m => m.Source?.ColumnName == columnName);
      }

      public void ResetMapping()
      {
         setDataFormat(_originalFormat);
         OnFormatPropertiesChanged.Invoke(_originalFormat);
      }

      public void ClearMapping()
      {
         var format = _format.Parameters.Select(p => new IgnoredDataFormatParameter(p.ColumnName) as DataFormatParameter).ToList();
         setDataFormat(format);
         OnFormatPropertiesChanged.Invoke(format);
      }

      public void ValidateMapping()
      {
         var missingColumn = _importerTask.CheckWhetherAllDataColumnsAreMapped(_columnInfos, _mappings.Select(m => m.Source));
         if (missingColumn != null)
         {
            OnMissingMapping(this, new MissingMappingEventArgs {Message = missingColumn});
         }
         else
         {
            OnMappingCompleted(this);
         }
      }

      public event MappingCompletedHandler OnMappingCompleted = delegate { };

      public event MissingMappingHandler OnMissingMapping = delegate { };

      public event FormatPropertiesChangedHandler OnFormatPropertiesChanged = delegate { };
   }
}