using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Microsoft.SqlServer.Server;
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

      public IDataFormat GetDataFormat()
      {
         return _format;
      }

      private void setDataFormat(IList<DataFormatParameter> formatParameters)
      {
         _mappings = _columnInfos.Select(c =>
         {
            var target = formatParameters.OfType<MappingDataFormatParameter>().FirstOrDefault(m => m.MappedColumn.Name.ToString() == c.Name);
            return new ColumnMappingViewModel
            (
               ColumnMappingViewModel.ColumnType.Mapping,
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
                  ColumnMappingViewModel.ColumnType.MetaData,
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
                     ColumnMappingViewModel.ColumnType.GroupBy,
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
               ColumnMappingViewModel.ColumnType.AddGroupBy,
               Captions.AddGroupByTitle,
               null,
               new AddGroupByFormatParameter(""),
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
            var column = ((MappingDataFormatParameter)model.Source).MappedColumn;
            unitsEditorPresenter.ShowFor
            (
               column,
               _columnInfos
                  .First(i => i.DisplayName == model.MappingName)
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

      private ColumnMappingOption generateGroupByColumnMappingOption(string description, string columnName)
      {
         return new ColumnMappingOption()
         {
            Label = columnName,
            Description = description
         };
      }

      private ColumnMappingOption generateAddGroupByColumnMappingOption(string description, string columnName)
      {
         return new ColumnMappingOption()
         {
            Label = columnName,
            Description = description
         };
      }

      private ColumnMappingOption generateMappingColumnMappingOption(string description, string mappingId, string unit = "?")
      {
         return new ColumnMappingOption()
         {
            Label = Captions.MappingDescription(mappingId, unit),
            Description = description
         };
      }

      private ColumnMappingOption generateMetaDataColumnMappingOption(string description, string metaDataId)
      {
         return new ColumnMappingOption()
         {
            Label = Captions.MetaDataDescription(metaDataId),
            Description = description
         };
      }

      public IEnumerable<ColumnMappingOption> GetAvailableOptionsFor(ColumnMappingViewModel model)
      {
         var options = new List<ColumnMappingOption>();
         if (model == null)
            return new List<ColumnMappingOption>();
         if (model.Source != null)
         {
            switch (model.Source)
            {
               case GroupByDataFormatParameter _:
                  options.Add(generateGroupByColumnMappingOption(model.Description, model.Source.ColumnName));
                  break;
               case MappingDataFormatParameter tm:
                  options.Add(generateMappingColumnMappingOption(model.Description, model.Source.ColumnName,
                     tm.MappedColumn.Unit));
                  break;
               case MetaDataFormatParameter tm:
                  options.Add(generateMetaDataColumnMappingOption(model.Description, model.Source.ColumnName));
                  break;
               case IgnoredDataFormatParameter _:
               case AddGroupByFormatParameter _:
                  break;
               default:
                  throw new Exception(Error.TypeNotSupported(model.Source.GetType()));
            }
         }
         options.Add(generateIgnoredColumnMappingOption(ColumnMappingFormatter.Ignored()));
         var availableColumns =
            _originalFormat
               .Select(f => f.ColumnName)
               .Where
               (
                  cn =>
                  _format.Parameters.OfType<MappingDataFormatParameter>().All(p => p.ColumnName != cn) &&
                  _format.Parameters.OfType<MetaDataFormatParameter>().All(p => p.ColumnName != cn) &&
                  _format.Parameters.OfType<GroupByDataFormatParameter>().All(p => p.ColumnName != cn)
               );
         switch (model.CurrentColumnType)
         {
            case ColumnMappingViewModel.ColumnType.Mapping:
               foreach (var column in availableColumns)
               {
                  options.Add(
                     generateMappingColumnMappingOption(
                        ColumnMappingFormatter.Mapping(new MappingDataFormatParameter(column, new Column() { Name = model.MappingName, Unit = "?" })),
                        column
                     )
                  );
               }
               break;
            case ColumnMappingViewModel.ColumnType.MetaData:
               foreach (var column in availableColumns)
               {
                  options.Add(
                     generateMetaDataColumnMappingOption(
                        ColumnMappingFormatter.MetaData(new MetaDataFormatParameter(column, model.MappingName)),
                        column
                     )
                  );
               }
               break;
            case ColumnMappingViewModel.ColumnType.GroupBy:
               //GroupBy only for missing columns
               foreach (var column in availableColumns)
               {
                  options.Add(
                     generateGroupByColumnMappingOption(
                        ColumnMappingFormatter.GroupBy(new GroupByDataFormatParameter(column)),
                        column
                     )
                  );
               }
               break;
            case ColumnMappingViewModel.ColumnType.AddGroupBy:
               //GroupBy only for missing columns
               foreach (var column in availableColumns)
               {
                  options.Add(
                     generateAddGroupByColumnMappingOption(
                        ColumnMappingFormatter.AddGroupBy(new AddGroupByFormatParameter(column)),
                        column
                     )
                  );
               }
               break;
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
            case AddGroupByFormatParameter _:
               return new ToolTipDescription()
               {
                  Title = Captions.AddGroupByTitle,
                  Description = Captions.AddGroupByHint
               };
            default:
               throw new Exception(Error.TypeNotSupported(element.GetType()));
         }
      }

      public void SetDescriptionForRow(ColumnMappingViewModel model)
      {
         if (model.Source is AddGroupByFormatParameter)
         {
            model.Source = ColumnMappingFormatter.Parse(model.Description);
            return;
         }
         var newParam = ColumnMappingFormatter.Parse(model.Description);
         if (newParam is IgnoredDataFormatParameter && model.Source != null)
         {
            newParam = new IgnoredDataFormatParameter(model.Source.ColumnName);
         }
         if (model.Source != null)
         {
            var oldModel = _mappings.FirstOrDefault(m => !(m.Source is IgnoredDataFormatParameter) && m.Source == model.Source);
            if (oldModel != null)
            {
               ClearRow(oldModel);
            }
         }
         var oldParam = _format.Parameters.FirstOrDefault(p => p.ColumnName == newParam.ColumnName);
         if (oldParam != null)
         {
            var index = _format.Parameters.IndexOf(oldParam);
            if (index >= 0)
            {
               _format.Parameters[index] = newParam;
               model.Source = newParam;
            }
         }
         else 
         {
            _format.Parameters.Add(newParam);
            setDataFormat(_format.Parameters);
         }
      }

      public void ClearRow(ColumnMappingViewModel model)
      {
         if (model.Source is GroupByDataFormatParameter)
         {
            _mappings.Remove(model);
            _format.Parameters.Remove(model.Source);
         }
         else
         {
            var newParam = ColumnMappingFormatter.Parse(ColumnMappingFormatter.Ignored(new IgnoredDataFormatParameter(model.Source.ColumnName)));
            var index = _format.Parameters.IndexOf(model.Source);
            if (index < 0)
            {
               index = _format.Parameters.IndexOf(_format.Parameters.First(p => p.ColumnName == newParam.ColumnName));
            }
            model.Source = newParam;
            _format.Parameters[index] = newParam;
         }
         View.Rebind();
      }

      public void AddGroupBy(AddGroupByFormatParameter source)
      {
         var parameter = new GroupByDataFormatParameter(source.ColumnName);
         _format.Parameters.Insert(_format.Parameters.Count-2, parameter);new GroupByDataFormatParameter(source.ColumnName);
         setDataFormat(_mappings
            .Where(f => !(f.Source is IgnoredDataFormatParameter || f.Source is AddGroupByFormatParameter))
            .Select(f => f.Source)
            .Append(parameter)
            .ToList());
      }

      public void ResetMapping()
      {
         setDataFormat(_originalFormat);
         if (_format != null)
         {
            _format.Parameters.Clear();
            foreach (var p in _originalFormat)
               _format.Parameters.Add(p);
         }
      }

      public void ClearMapping()
      {
         var format = _format.Parameters.Select(p => new IgnoredDataFormatParameter(p.ColumnName) as DataFormatParameter).ToList();
         setDataFormat(format);
         _format.Parameters.Clear();
         foreach (var p in format)
            _format.Parameters.Add(p);
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
   }
}