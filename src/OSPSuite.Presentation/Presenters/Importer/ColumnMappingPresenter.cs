using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Import;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Extensions;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public class ColumnMappingPresenter : AbstractPresenter<IColumnMappingView, IColumnMappingPresenter>, IColumnMappingPresenter
   {
      private IDataFormat _format;
      private List<ColumnMappingDTO> _mappings;
      private IReadOnlyList<ColumnInfo> _columnInfos;
      private IReadOnlyList<MetaDataCategory> _metaDataCategories;
      private readonly IImporter _importer;
      private IList<DataFormatParameter> _originalFormat;
      private UnformattedData _rawData;
      private MappingProblem _mappingProblem = new MappingProblem() { MissingMapping = new List<string>(), MissingUnit = new List<string>() };
      private readonly IMappingParameterEditorPresenter _mappingParameterEditorPresenter;
      private readonly IMetaDataParameterEditorPresenter _metaDataParameterEditorPresenter;
      private ColumnMappingDTO _currentModel;
      public ColumnMappingPresenter
      (
         IColumnMappingView view,
         IImporter importer,
         IMappingParameterEditorPresenter mappingParameterEditorPresenter,
         IMetaDataParameterEditorPresenter metaDataParameterEditorPresenter
      ) : base(view)
      {
         _importer = importer;
         _mappingParameterEditorPresenter = mappingParameterEditorPresenter;
         _metaDataParameterEditorPresenter = metaDataParameterEditorPresenter;
         View.FillMappingView(_mappingParameterEditorPresenter.BaseView);
         View.FillMetaDataView(_metaDataParameterEditorPresenter.BaseView);
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
               target,
               _importer.GetImageIndex(new MappingDataFormatParameter("", null)),
               c
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
                  target,
                  _importer.GetImageIndex(new MetaDataFormatParameter("", ""))
               );
            })
         ).Union
         (
            formatParameters.OfType<GroupByDataFormatParameter>().Select(
               gb =>
               {
                  return new ColumnMappingDTO(
                     ColumnMappingDTO.ColumnType.GroupBy,
                     Captions.Importer.GroupByTitle,
                     gb,
                     _importer.GetImageIndex(gb)
                  );
               }
            )
         ).Append
         (
            new ColumnMappingDTO(
               ColumnMappingDTO.ColumnType.AddGroupBy,
               Captions.Importer.AddGroupByTitle,
               new AddGroupByFormatParameter(""),
               _importer.GetImageIndex(new GroupByDataFormatParameter(""))
            )
         ).ToList();
         View.SetMappingSource(_mappings);
         ValidateMapping();
         InitializeErrorUnit();
      }

      public void InitializeErrorUnit()
      {
         var errorColumnDTO = _mappings.FirstOrDefault(c => (c.ColumnInfo != null) && !c.ColumnInfo.RelatedColumnOf.IsNullOrEmpty());

         if (errorColumnDTO?.Source == null) return;

         var errorColumn = ((MappingDataFormatParameter)errorColumnDTO.Source).MappedColumn;

         if ((errorColumn.Unit.SelectedUnit != "?") && (!string.IsNullOrEmpty(errorColumn.Unit.ColumnName))) return;
         if (errorColumn.ErrorStdDev == Constants.STD_DEV_GEOMETRIC)
         {
            errorColumn.Unit = new UnitDescription("");
            return;
         }

         var measurementColumnDTO = _mappings.FirstOrDefault(c => c.MappingName == errorColumnDTO.ColumnInfo.RelatedColumnOf);
         var measurementColumn = ((MappingDataFormatParameter)measurementColumnDTO.Source).MappedColumn;
         errorColumn.Unit = measurementColumn.Unit;
      }

      public void SetDataFormat(IDataFormat format)
      {
         _format = format;
         _originalFormat = _format.Parameters.ToList();
         setDataFormat(format.Parameters);
      }

      public void SetRawData(UnformattedData rawData)
      {
         _rawData = rawData;
      }

      public void SetDescriptionForRow(ColumnMappingDTO model)
      {
         var values = _metaDataCategories.FirstOrDefault(md => md.Name == model.MappingName)?.ListOfValues.Keys;
         _setDescriptionForRow(model, values != null && values.All(v => v != model.ExcelColumn));
      }

      public void UpdateMetaDataForModel()
      {
         if (_currentModel == null) return;

         var metaData = _currentModel.Source as MetaDataFormatParameter;
         if (metaData == null) return;

         _mappings.First(m => m.MappingName == metaData.MetaDataId).ExcelColumn = _metaDataParameterEditorPresenter.Input;
         metaData.ColumnName = _metaDataParameterEditorPresenter.Input;
         ValidateMapping();
         _view.RefreshData();
         _view.CloseEditor();
      }

      public void UpdateDescriptrionForModel()
      {
         if (!(_currentModel.Source is MappingDataFormatParameter))
         {
            return;
         }

         var column = ((MappingDataFormatParameter)_currentModel.Source).MappedColumn;
         column.Unit = _mappingParameterEditorPresenter.Unit;
         if (column.Unit.ColumnName != null && !string.IsNullOrEmpty(column.Unit.ColumnName))
         {
            column.Unit = new UnitDescription(_rawData.GetColumn(column.Unit.ColumnName).First(u => !string.IsNullOrEmpty(u)), column.Unit.ColumnName);
         }

         if (_currentModel.ColumnInfo.IsBase())
         {
            ValidateMapping();
            _view.RefreshData();
            _view.CloseEditor();
            return;
         }

         if (_currentModel.ColumnInfo.IsAuxiliary())
         {
            if (_mappingParameterEditorPresenter.SelectedErrorType == 0)
               column.ErrorStdDev = Constants.STD_DEV_ARITHMETIC;
            else
            {
               column.ErrorStdDev = Constants.STD_DEV_GEOMETRIC;
               column.Unit = new UnitDescription(""); //geometric error has no unit
            }
         }
         else //in this case the column is a measurement column
         {
            var columns = new List<string>() { column.LloqColumn };
            if (column.LloqColumn != "")
            {
               columns.Add("");
            }
            columns.AddRange(availableColumns());
            column.LloqColumn = _mappingParameterEditorPresenter.LloqFromColumn() ? columns[_mappingParameterEditorPresenter.SelectedLloq] : null;
         }
         ValidateMapping();

         _view.RefreshData();
         _view.CloseEditor();
      }

      public bool ShouldManualInputOnMetaDataBeEnabled(ColumnMappingDTO model)
      {
         var metaDataCategory = _metaDataCategories.FindByName(model.MappingName);
         if (metaDataCategory != null && metaDataCategory.ShouldListOfValuesBeIncluded)
         {
            _currentModel = model;
            return true;
         }

         return false;
      }

      public void SetSubEditorSettingsForMapping(ColumnMappingDTO model)
      {
         _currentModel = model;
         _mappingParameterEditorPresenter.HideAll();
         if (!(model.Source is MappingDataFormatParameter))
            return;

         var source = (MappingDataFormatParameter)model.Source;
         var column = source.MappedColumn;

         _mappingParameterEditorPresenter.InitView();

         var columns = new List<string>() { column.Unit.ColumnName };

         if (model.ColumnInfo.RelatedColumnOf != null) //if there is a measurement column
         {
            var relatedColumnDTO = _mappings.FirstOrDefault(c => c.MappingName == model.ColumnInfo.RelatedColumnOf);
            var relatedColumn = ((MappingDataFormatParameter)relatedColumnDTO?.Source)?.MappedColumn;

            if (relatedColumn != null && !relatedColumn.Unit.ColumnName.IsNullOrEmpty())
            {
               _mappingParameterEditorPresenter.SetUnitColumnSelection();
               columns.Add(relatedColumn.Unit.ColumnName);
            }
            else
               _mappingParameterEditorPresenter.SetUnitsManualSelection();
         }

         _mappingParameterEditorPresenter.SetUnitOptions(
            column,
            _columnInfos
               .First(i => i.DisplayName == model.MappingName)
               .DimensionInfos
               .Select(d => d.Dimension),
               columns.Union(availableColumns())
         );


         if (model.ColumnInfo.IsBase())
            return;

         if (model.ColumnInfo.IsAuxiliary())
         {
            _mappingParameterEditorPresenter.SetErrorTypeOptions(new List<string>() { Constants.STD_DEV_ARITHMETIC, Constants.STD_DEV_GEOMETRIC }, source.MappedColumn.ErrorStdDev);
         }
         else
         {
            var lloqColumnSelection = column.LloqColumn != null;

            columns = new List<string>();

            if (column.LloqColumn != null)
               columns.Add(column.LloqColumn);

            if (column.LloqColumn != "")
            {
               columns.Add("");
            }
            columns.AddRange(availableColumns());
            _mappingParameterEditorPresenter.SetLloqOptions(columns, column.LloqColumn, lloqColumnSelection);
         }
      }

      private ColumnMappingOption generateGroupByColumnMappingOption(string columnName)
      {
         return new ColumnMappingOption()
         {
            Label = columnName
         };
      }

      private ColumnMappingOption generateAddGroupByColumnMappingOption(string columnName)
      {
         return new ColumnMappingOption()
         {
            Label = columnName
         };
      }

      private ColumnMappingOption generateMappingColumnMappingOption(string mappingId, string unit = "?")
      {
         return new ColumnMappingOption()
         {
            Label = Captions.Importer.MappingDescription(mappingId, unit),
         };
      }

      private ColumnMappingOption generateMetaDataColumnMappingOption(string metaDataId)
      {
         return new ColumnMappingOption()
         {
            Label = Captions.Importer.MetaDataDescription(metaDataId)
         };
      }

      public IEnumerable<RowOptionDTO> GetAvailableRowsFor(ColumnMappingDTO model)
      {
         var options = new List<RowOptionDTO>();
         if (model == null)
            return options;
         if (model.CurrentColumnType == ColumnMappingDTO.ColumnType.MetaData)
         {
            var metaDataCategory = _metaDataCategories.FirstOrDefault(md => md.Name == model.MappingName);
            if (metaDataCategory != null && metaDataCategory.ShouldListOfValuesBeIncluded)
            {
               if (model.ExcelColumn != null && !metaDataCategory.ListOfValues.Keys.Contains(model.ExcelColumn))
               {
                  options.Add(new RowOptionDTO() { Description = model.ExcelColumn, ImageIndex = ApplicationIcons.IconIndex(ApplicationIcons.ObservedDataForMolecule) });
               }
               options.AddRange(metaDataCategory.ListOfValues.Keys.Select(v =>
               {
                  var iconIndex = ApplicationIcons.IconIndex(v);
                  if (iconIndex == -1)
                     iconIndex = ApplicationIcons.IconIndex(ApplicationIcons.MetaData);
                  return new RowOptionDTO() { Description = v, ImageIndex = iconIndex };
               }));
            }
         }
         if (model.Source != null && (model.CurrentColumnType == ColumnMappingDTO.ColumnType.MetaData && (model.Source as MetaDataFormatParameter).IsColumn))
         {
            options.Add(new RowOptionDTO() { Description = model.Source.ColumnName, ImageIndex = ApplicationIcons.IconIndex(ApplicationIcons.ObservedDataForMolecule) });
         }
         else if (model.Source != null && !(model.Source is AddGroupByFormatParameter) && !(model.Source is MetaDataFormatParameter))
         {
            options.Add(new RowOptionDTO() { Description = model.Source.ColumnName, ImageIndex = ApplicationIcons.IconIndex(ApplicationIcons.ObservedDataForMolecule) });
         }
         options.AddRange(availableColumns().Select(c => new RowOptionDTO() { Description = c, ImageIndex = ApplicationIcons.IconIndex(ApplicationIcons.ObservedDataForMolecule) }));
         return options.OrderBy(o => o.ImageIndex).ThenBy(o => o.Description);
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
                  options.Add(generateGroupByColumnMappingOption(model.Source.ColumnName));
                  break;
               case MappingDataFormatParameter tm:
                  options.Add(generateMappingColumnMappingOption(model.Source.ColumnName,
                     tm.MappedColumn.Unit.SelectedUnit));
                  break;
               case MetaDataFormatParameter tm:
                  options.Add(generateMetaDataColumnMappingOption(model.Source.ColumnName));
                  break;
               case AddGroupByFormatParameter _:
                  break;
               default:
                  throw new Exception(Error.TypeNotSupported(model.Source.GetType().Name));
            }
         }
         var availableColumns = this.availableColumns();
         switch (model.CurrentColumnType)
         {
            case ColumnMappingDTO.ColumnType.Mapping:
               foreach (var column in availableColumns)
               {
                  options.Add(generateMappingColumnMappingOption(column));
               }
               break;
            case ColumnMappingDTO.ColumnType.MetaData:
               foreach (var column in availableColumns)
               {
                  options.Add(
                     generateMetaDataColumnMappingOption(
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
         return _format.ExcelColumnNames
            .Where
            (
               cn =>
                  _format.Parameters.OfType<MappingDataFormatParameter>().All(p => p.ColumnName != cn && p.MappedColumn?.Unit?.ColumnName != cn && p.MappedColumn?.LloqColumn != cn) &&
                  _format.Parameters.OfType<MetaDataFormatParameter>().All(p => p.ColumnName != cn) &&
                  _format.Parameters.OfType<GroupByDataFormatParameter>().All(p => p.ColumnName != cn)
            );
      }

      //TODO This method shpould take a mapping as parameter. The view must know if from the databinder
      public ToolTipDescription ToolTipDescriptionFor(int index)
      {
         var description = _mappings.ElementAt(index).MappingName;
         if (_mappingProblem.MissingMapping.Contains(description))
         {
            return new ToolTipDescription()
            {
               Title = Captions.Importer.MissingMandatoryMapping
            };
         }
         var element = _mappings.ElementAt(index).Source;
         if (element == null)
            return new ToolTipDescription()
            {
               Title = Captions.Importer.NotConfiguredField
            };
         if ((element is MappingDataFormatParameter) && _mappingProblem.MissingUnit.Contains((element as MappingDataFormatParameter).MappedColumn.Name))
            return new ToolTipDescription()
            {
               Title = Captions.Importer.MissingUnit
            };
         return new ToolTipDescription()
         {
            Title = element.TooltipTitle(),
            Description = element.TooltipDescription()
         };
      }

      private void _setDescriptionForRow(ColumnMappingDTO model, bool isColumn)
      {
         if (model.Source == null)
         {
            switch (model.CurrentColumnType)
            {
               case ColumnMappingDTO.ColumnType.MetaData:
                  model.Source = new MetaDataFormatParameter(model.ExcelColumn, model.MappingName, isColumn);
                  break;
               case ColumnMappingDTO.ColumnType.Mapping:
                  model.Source = new MappingDataFormatParameter(model.ExcelColumn, new Column() { Name = model.MappingName, Unit = new UnitDescription(UnitDescription.InvalidUnit) });
                  break;
               default:
                  throw new NotImplementedException($"Setting description for unhandled column type: {model.CurrentColumnType}");
            }
            _format.Parameters.Add(model.Source);
         }
         else if (model.Source is AddGroupByFormatParameter)
         {
            model.Source = new GroupByDataFormatParameter(model.ExcelColumn);
            _format.Parameters.Add(model.Source);
         }
         else
         {
            model.Source.ColumnName = model.ExcelColumn;
            if (model.CurrentColumnType == ColumnMappingDTO.ColumnType.MetaData)
            {
               (model.Source as MetaDataFormatParameter).IsColumn = isColumn;
            }
         }

         setDataFormat(_format.Parameters);
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
            var index = _format.Parameters.IndexOf(model.Source);
            model.ExcelColumn = Captions.Importer.NoneEditorNullText;
            model.Source = null;
            _format.Parameters.RemoveAt(index);
         }
         View.RefreshData();
      }

      public void AddGroupBy(AddGroupByFormatParameter source)
      {
         var parameter = new GroupByDataFormatParameter(source.ColumnName);
         _format.Parameters.Insert(_format.Parameters.Count - 2, parameter); new GroupByDataFormatParameter(source.ColumnName);
         setDataFormat(_mappings
            .Where(f => !(f.Source is AddGroupByFormatParameter))
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
         var format = new List<DataFormatParameter>();
         setDataFormat(format);
         _format.Parameters.Clear();
      }

      private void setStatuses()
      {
         foreach (var m in _mappings)
         {
            if (_mappingProblem.MissingMapping.Contains(m.MappingName))
            {
               m.Status = ColumnMappingDTO.MappingStatus.Invalid;
               continue;
            }
            if (m.Source == null)
            {
               m.Status = ColumnMappingDTO.MappingStatus.NotSet;
               continue;
            }
            if (_mappingProblem.MissingUnit.Contains((m.Source as MappingDataFormatParameter)?.MappedColumn.Name))
            {
               m.Status = ColumnMappingDTO.MappingStatus.InvalidUnit;
               continue;
            }

            m.Status = ColumnMappingDTO.MappingStatus.Valid;
         }
      }

      private void invalidateErrorUnit()
      {
         var errorColumnDTO = _mappings?.FirstOrDefault(c => (c?.ColumnInfo != null) && !c.ColumnInfo.RelatedColumnOf.IsNullOrEmpty());

         if (errorColumnDTO == null) return;

         var errorColumn = ((MappingDataFormatParameter)errorColumnDTO.Source)?.MappedColumn;
         if (errorColumn == null) return;
         var measurementColumnDTO = _mappings.FirstOrDefault(c => c.MappingName == errorColumnDTO.ColumnInfo.RelatedColumnOf);
         if (measurementColumnDTO == null) return;
         var measurementColumn = ((MappingDataFormatParameter)measurementColumnDTO.Source)?.MappedColumn;
         if (measurementColumn == null) return;

         if ((errorColumn.Unit?.ColumnName.IsNullOrEmpty() != measurementColumn.Unit?.ColumnName.IsNullOrEmpty()) && (errorColumn.ErrorStdDev == Constants.STD_DEV_GEOMETRIC))
            errorColumn.Unit = new UnitDescription();
      }

      public void ValidateMapping()
      {
         invalidateErrorUnit();

         _mappingProblem = _importer.CheckWhetherAllDataColumnsAreMapped(_columnInfos, _mappings.Select(m => m.Source));
         setStatuses();
         if (_mappingProblem.MissingMapping.Count != 0 || _mappingProblem.MissingUnit.Count != 0)
         {
            OnMissingMapping(this, new MissingMappingEventArgs { Message = _mappingProblem.MissingMapping.FirstOrDefault() ?? _mappingProblem.MissingUnit.FirstOrDefault() });
         }
         else
         {
            OnMappingCompleted(this, new EventArgs());
         }
      }

      public event EventHandler OnMappingCompleted = delegate { };

      public event EventHandler<MissingMappingEventArgs> OnMissingMapping = delegate { };
      public IEnumerable<string> GetAllAvailableExcelColumns()
      {
         return _format.ExcelColumnNames;
      }
   }
}