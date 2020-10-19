using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Importer;
using OSPSuite.Presentation.Core;
using OSPSuite.Core.Importer.DataFormat;
using OSPSuite.Core.Importer.Services;
using OSPSuite.UI.Views.Importer;
using OSPSuite.Presentation.Presenters;

namespace OSPSuite.Presentation.Importer
{
   public class ColumnMappingPresenter : AbstractPresenter<IColumnMappingControl, IColumnMappingPresenter>, IColumnMappingPresenter
   {
      private IDataFormat _format;
      private List<ColumnMappingDTO> _mappings;
      private IReadOnlyList<ColumnInfo> _columnInfos;
      private IReadOnlyList<MetaDataCategory> _metaDataCategories;
      private readonly IImporterTask _importerTask;
      private readonly IApplicationController _applicationController;
      private IList<DataFormatParameter> _originalFormat;
      private IList<string> _extraColumns;
      private UnformattedData _rawData;
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
            var target = formatParameters.OfType<MappingDataFormatParameter>().FirstOrDefault(m => m.MappedColumn.Name == c.Name);
            return new ColumnMappingDTO
            (
               ColumnMappingDTO.ColumnType.Mapping,
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
               return new ColumnMappingDTO
               (
                  ColumnMappingDTO.ColumnType.MetaData,
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
                  return new ColumnMappingDTO(
                     ColumnMappingDTO.ColumnType.GroupBy,
                     Captions.GroupByTitle,
                     ColumnMappingFormatter.Stringify(gb),
                     gb,
                     _importerTask.GetImageIndex(gb)
                  );
               }
            )
         ).Append
         (
            new ColumnMappingDTO(
               ColumnMappingDTO.ColumnType.AddGroupBy,
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
         _extraColumns = _originalFormat
               .OfType<MappingDataFormatParameter>()
               .Where(m => m.MappedColumn?.Unit?.ColumnName != null)
               .Select(m => m.MappedColumn.Unit.ColumnName)
               .Union(
                  _originalFormat
                  .OfType<MappingDataFormatParameter>()
                  .Where(m => m.MappedColumn?.LloqColumn != null)
                  .Select(m => m.MappedColumn.LloqColumn)
               )
               .ToList();
         setDataFormat(format.Parameters);
      }

      public void SetRawData(UnformattedData rawData)
      {
         _rawData = rawData;
      }

      private ColumnMappingOption generateIgnoredColumnMappingOption(string description)
      {
         return new ColumnMappingOption()
         {
            Label = Captions.Importer.NoneEditorNullText,
            Description = description
         };
      }

      public void ChangeLloqOnRow(ColumnMappingDTO model)
      {
         using (var lloqEditorPresenter = _applicationController.Start<ILloqEditorPresenter>())
         {
            var column = ((MappingDataFormatParameter)model.Source).MappedColumn;
            var columns = new List<string>() { column.LloqColumn };
            if (column.LloqColumn != "")
            {
               columns.Add("");
            }
            columns.AddRange(availableColumns());
            lloqEditorPresenter.ShowFor
            (
               columns,
               column.LloqColumn
            );
            if (!lloqEditorPresenter.Canceled)
            {
               column.LloqColumn = lloqEditorPresenter.LloqColumn;
               _view.Rebind();
            }
         }
      }

      public void ChangeUnitsOnRow(ColumnMappingDTO model)
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
                  .Select(d => d.Dimension),
               new List<string>() { column.Unit.ColumnName }.Union(availableColumns())
            );
            if (!unitsEditorPresenter.Canceled)
            {
               column.Unit = unitsEditorPresenter.Unit;
               if (column.Unit.ColumnName != null)
               {
                  column.Unit.AttachUnitFunction(_rawData.GetColumn(column.Unit.ColumnName));
               }
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

      public IEnumerable<ColumnMappingOption> GetAvailableOptionsFor(ColumnMappingDTO model)
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
                     tm.MappedColumn.Unit.SelectedUnit));
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
         var availableColumns = this.availableColumns();
         switch (model.CurrentColumnType)
         {
            case ColumnMappingDTO.ColumnType.Mapping:
               foreach (var column in availableColumns)
               {
                  options.Add(
                     generateMappingColumnMappingOption(
                        ColumnMappingFormatter.Mapping(new MappingDataFormatParameter(column, new Column() { Name = model.MappingName, Unit = new UnitDescription("?") })),
                        column
                     )
                  );
               }
               break;
            case ColumnMappingDTO.ColumnType.MetaData:
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
            case ColumnMappingDTO.ColumnType.GroupBy:
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
            case ColumnMappingDTO.ColumnType.AddGroupBy:
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

      private IEnumerable<string> availableColumns()
      {
         return _originalFormat
            .Select(f => f.ColumnName)
            .Union(_extraColumns)
            .Where
            (
               cn =>
                  _format.Parameters.OfType<MappingDataFormatParameter>().All(p => p.ColumnName != cn && p.MappedColumn?.Unit?.ColumnName != cn && p.MappedColumn?.LloqColumn != cn) &&
                  _format.Parameters.OfType<MetaDataFormatParameter>().All(p => p.ColumnName != cn) &&
                  _format.Parameters.OfType<GroupByDataFormatParameter>().All(p => p.ColumnName != cn)
            );
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
                  Description = Captions.MappingHint(mp.ColumnName, mp?.MappedColumn.Name, mp.MappedColumn.Unit.SelectedUnit)
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

      public void SetDescriptionForRow(ColumnMappingDTO model)
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

      public void ClearRow(ColumnMappingDTO model)
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
            OnMappingCompleted(this, new EventArgs());
         }
      }

      public event EventHandler OnMappingCompleted = delegate { };

      public event EventHandler<MissingMappingEventArgs> OnMissingMapping = delegate { };
   }
}