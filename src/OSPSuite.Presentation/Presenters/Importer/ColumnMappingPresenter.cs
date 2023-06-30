using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Core.Import;
using OSPSuite.Infrastructure.Import.Core;
using OSPSuite.Infrastructure.Import.Services;
using OSPSuite.Presentation.Views.Importer;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Presentation.Presenters.Importer
{
   public class ColumnMappingPresenter : AbstractPresenter<IColumnMappingView, IColumnMappingPresenter>, IColumnMappingPresenter
   {
      private IDataFormat _format;
      private List<ColumnMappingDTO> _mappings;
      private ColumnInfoCache _columnInfos;
      private IReadOnlyList<MetaDataCategory> _metaDataCategories;
      private readonly IImporter _importer;
      private DataSheet _rawData;
      private MappingProblem _mappingProblem = new MappingProblem() { MissingMapping = new List<string>(), MissingUnit = new List<string>() };
      private readonly IMappingParameterEditorPresenter _mappingParameterEditorPresenter;
      private readonly IMetaDataParameterEditorPresenter _metaDataParameterEditorPresenter;
      private readonly IDimensionFactory _dimensionFactory;

      public ColumnMappingPresenter
      (
         IColumnMappingView view,
         IImporter importer,
         IMappingParameterEditorPresenter mappingParameterEditorPresenter,
         IMetaDataParameterEditorPresenter metaDataParameterEditorPresenter,
         IDimensionFactory dimensionFactory
      ) : base(view)
      {
         _importer = importer;
         _mappingParameterEditorPresenter = mappingParameterEditorPresenter;
         _metaDataParameterEditorPresenter = metaDataParameterEditorPresenter;
         _dimensionFactory = dimensionFactory;
         View.FillMappingView(_mappingParameterEditorPresenter.BaseView);
         View.FillMetaDataView(_metaDataParameterEditorPresenter.BaseView);
      }

      public void SetSettings(
         IReadOnlyList<MetaDataCategory> metaDataCategories,
         ColumnInfoCache columnInfos
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
               new AddGroupByFormatParameter(Captions.Importer.SelectToAdd),
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
         var measurementColumn = ((MappingDataFormatParameter)measurementColumnDTO?.Source)?.MappedColumn;

         if (measurementColumn != null && unitNotSet(errorColumn))
            errorColumn.Unit = measurementColumn.Unit;
      }

      private bool unitNotSet(Column column)
      {
         //when unit is not set and also columnName is not set
         return (column.Unit.SelectedUnit == null || column.Unit.SelectedUnit == "?")
                && string.IsNullOrEmpty(column.Unit.ColumnName);
      }

      public void SetDataFormat(IDataFormat format)
      {
         if (format == null)
            return;
         _format = format;
         setDataFormat(format.Parameters);
      }

      public void SetRawData(DataSheet rawData)
      {
         _rawData = rawData;
      }

      public void SetDescriptionForRow(ColumnMappingDTO model)
      {
         var values = _metaDataCategories.FirstOrDefault(md => md.Name == model.MappingName)?.ListOfValues.Keys;
         setDescriptionForRow(model, values != null && values.All(v => v != model.ExcelColumn));
      }

      public void UpdateMetaDataForModel(MetaDataFormatParameter mappingSource)
      {
         if (mappingSource == null)
            return;

         _mappings.First(m => m.MappingName == mappingSource.MetaDataId).ExcelColumn = _metaDataParameterEditorPresenter.Input;
         mappingSource.ColumnName = _metaDataParameterEditorPresenter.Input;
         mappingSource.IsColumn = false;
         ValidateMapping();
         _view.CloseEditor();
      }

      private void updateErrorAfterMeasurementChanges(ColumnMappingDTO model, Column column, Action<MappingDataFormatParameter> updateAction)
      {
         if (!model.ColumnInfo.IsMeasurement)
            return;
         foreach (var relatedColumn in _columnInfos.RelatedColumnsFrom(column.Name))
         {
            var relatedParameter = _mappings.Select(x => x.Source).OfType<MappingDataFormatParameter>()
               .FirstOrDefault(x => x.MappedColumn.Name == relatedColumn.Name);
            if (relatedParameter == null)
               continue;

            updateAction(relatedParameter);
         }
      }

      private void updateErrorDescriptionAfterMeasurementDimensionChanged(ColumnMappingDTO model, Column column)
      {
         updateErrorAfterMeasurementChanges(model, column, relatedParameter =>
         {
            relatedParameter.MappedColumn.Dimension = column.Dimension;
            relatedParameter.MappedColumn.Unit = new UnitDescription(column.Unit.SelectedUnit);
         });
      }

      private void updateErrorDescriptionAfterMeasurementUnitIsSetFromColumn(ColumnMappingDTO model, Column column)
      {
         updateErrorAfterMeasurementChanges(model, column, relatedParameter =>
         {
            if (!relatedParameter.MappedColumn.Unit.ColumnName.IsNullOrEmpty())
               //already a column, nothing to do here
               return;
            relatedParameter.MappedColumn.Dimension = null;
            relatedParameter.MappedColumn.Unit = new UnitDescription(column.Unit.SelectedUnit, column.Unit.ColumnName);
         });
      }

      public void UpdateDescriptionForModel(MappingDataFormatParameter mappingSource)
      {
         if (mappingSource == null)
            return;

         var model = _mappings.FirstOrDefault(x => x.Source == mappingSource);

         if (model == null)
            return;

         var column = ((MappingDataFormatParameter)model.Source).MappedColumn;
         if (!string.IsNullOrEmpty(_mappingParameterEditorPresenter.Unit.ColumnName))
         {
            column.Unit = new UnitDescription(_rawData.GetColumn(_mappingParameterEditorPresenter.Unit.ColumnName).FirstOrDefault(),
               _mappingParameterEditorPresenter.Unit.ColumnName);
            column.Dimension = null;
            updateErrorDescriptionAfterMeasurementUnitIsSetFromColumn(model, column);
         }
         else
         {
            //When unit is set, the dimension has to be inferred from it.
            //The dimension is the first from the supported dimension which
            //has the selected unit.
            column.Unit = _mappingParameterEditorPresenter.Unit;
            if (column.Dimension == null || !column.Dimension.HasUnit(column.Unit.SelectedUnit))
            {
               column.Dimension = _columnInfos[model.MappingName]
                  .SupportedDimensions
                  .FirstOrDefault(x => x.HasUnit(column.Unit.SelectedUnit));
               updateErrorDescriptionAfterMeasurementDimensionChanged(model, column);
            }
         }

         if (model.ColumnInfo.IsBase)
         {
            ValidateMapping();
            _view.CloseEditor();
            return;
         }

         if (model.ColumnInfo.IsAuxiliary)
         {
            if (_mappingParameterEditorPresenter.SelectedErrorType == 0)
               column.ErrorStdDev = Constants.STD_DEV_ARITHMETIC;
            else
            {
               column.ErrorStdDev = Constants.STD_DEV_GEOMETRIC;
               column.Dimension = Constants.Dimension.NO_DIMENSION;
               column.Unit = new UnitDescription(column.Dimension.BaseUnit.Name);
            }
         }
         else //in this case the column is a measurement column
         {
            column.LloqColumn = _mappingParameterEditorPresenter.LloqFromColumn() ? _mappingParameterEditorPresenter.LloqColumn : null;
         }

         ValidateMapping();
         _view.CloseEditor();
      }

      public bool ShouldManualInputOnMetaDataBeEnabled(ColumnMappingDTO model)
      {
         var metaDataCategory = _metaDataCategories.FindByName(model.MappingName);
         return metaDataCategory != null && metaDataCategory.AllowsManualInput;
      }

      public bool ShouldManualInputOnMetaDataBeEnabled(DataFormatParameter parameter)
      {
         var model = _mappings.FirstOrDefault(m => m.Source == parameter);
         if (model == null)
            return false;

         return ShouldManualInputOnMetaDataBeEnabled(model);
      }

      public void SetSubEditorSettingsForMapping(ColumnMappingDTO model)
      {
         _mappingParameterEditorPresenter.HideAll();
         if (!(model.Source is MappingDataFormatParameter))
            return;

         var source = (MappingDataFormatParameter)model.Source;
         var column = source.MappedColumn;

         _mappingParameterEditorPresenter.InitView();

         var columns = new List<string>() { column.Unit.ColumnName };
         var dimensions = new List<IDimension>();

         string measurementUnit = null;
         if (model.ColumnInfo.RelatedColumnOf != null) //if there is a measurement column
         {
            var relatedColumnDTO = _mappings.FirstOrDefault(c => c.MappingName == model.ColumnInfo.RelatedColumnOf);
            var relatedColumn = ((MappingDataFormatParameter)relatedColumnDTO?.Source)?.MappedColumn;
            measurementUnit = relatedColumn?.Unit?.SelectedUnit;

            if (relatedColumn != null && !relatedColumn.Unit.ColumnName.IsNullOrEmpty())
            {
               _mappingParameterEditorPresenter.SetUnitColumnSelection();
               columns.Add(relatedColumn.Unit.ColumnName);
            }
            else
               _mappingParameterEditorPresenter.SetUnitsManualSelection();

            if (relatedColumn != null)
               dimensions.Add(relatedColumn.Dimension);
         }
         else
         {
            var errorColumnDTO = _mappings.FirstOrDefault(c => c.ColumnInfo?.RelatedColumnOf == model.MappingName);
            var errorColumn = ((MappingDataFormatParameter)errorColumnDTO?.Source)?.MappedColumn;

            if (errorColumn?.Unit != null && !errorColumn.Unit.ColumnName.IsNullOrEmpty())
               columns.Add(errorColumn.Unit.ColumnName);

            dimensions.AddRange(_columnInfos
               .First(i => i.DisplayName == model.MappingName)
               .SupportedDimensions);
         }

         _mappingParameterEditorPresenter.SetUnitOptions(column, dimensions, columns.Union(availableColumns()));

         if (model.ColumnInfo.IsBase)
            return;

         if (model.ColumnInfo.IsAuxiliary)
         {
            _mappingParameterEditorPresenter.SetErrorTypeOptions
            (
               new List<string>() { Constants.STD_DEV_ARITHMETIC, Constants.STD_DEV_GEOMETRIC },
               source.MappedColumn.ErrorStdDev,
               type => type == Constants.STD_DEV_ARITHMETIC ? measurementUnit : null
            );
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

      private bool columnNameHasManualInput(ColumnMappingDTO model, MetaDataCategory metaDataCategory)
      {
         if (model.Source == null)
            return false;

         var source = model.Source as MetaDataFormatParameter;

         if (source.ColumnName == null)
            return false;

         if (source.IsColumn)
            return false;

         return !metaDataCategory.ListOfValues.Keys.Union(availableColumns()).Contains(model.ExcelColumn);
      }

      public IEnumerable<RowOptionDTO> GetAvailableRowsFor(ColumnMappingDTO model)
      {
         var options = new List<RowOptionDTO>();
         if (model == null)
            return options;
         var excelColumns = availableColumns();
         var topNames = new List<string>();
         if (model.CurrentColumnType == ColumnMappingDTO.ColumnType.MetaData)
         {
            var metaDataCategory = _metaDataCategories.FirstOrDefault(md => md.Name == model.MappingName);
            if (columnNameHasManualInput(model, metaDataCategory))
               options.Add(new RowOptionDTO()
                  { Description = model.ExcelColumn, ImageIndex = ApplicationIcons.IconIndex(ApplicationIcons.MetaData) });
            if (metaDataCategory != null && metaDataCategory.ShouldListOfValuesBeIncluded)
            {
               options.AddRange(metaDataCategory.ListOfValues.Keys.Select(v =>
               {
                  metaDataCategory.ListOfImages.TryGetValue(v, out var value);
                  if (value != null)
                     return new RowOptionDTO() { Description = v, ImageIndex = ApplicationIcons.IconIndex(value) };

                  var iconIndex = ApplicationIcons.IconIndex(v);
                  if (iconIndex == -1)
                     iconIndex = ApplicationIcons.IconIndex(ApplicationIcons.MetaData);
                  return new RowOptionDTO() { Description = v, ImageIndex = iconIndex };
               }));
            }

            topNames = metaDataCategory.TopNames;
         }

         if (model.Source != null && (model.CurrentColumnType == ColumnMappingDTO.ColumnType.MetaData &&
                                      (model.Source as MetaDataFormatParameter).IsColumn))
         {
            options.Add(new RowOptionDTO()
               { Description = model.Source.ColumnName, ImageIndex = ApplicationIcons.IconIndex(ApplicationIcons.ObservedDataForMolecule) });
         }
         else if (model.Source != null && !(model.Source is AddGroupByFormatParameter) && !(model.Source is MetaDataFormatParameter))
         {
            options.Add(new RowOptionDTO()
               { Description = model.Source.ColumnName, ImageIndex = ApplicationIcons.IconIndex(ApplicationIcons.ObservedDataForMolecule) });
         }

         if (model.CurrentColumnType == ColumnMappingDTO.ColumnType.AddGroupBy)
            options.Add(new RowOptionDTO() { Description = model.ExcelColumn, ImageIndex = ApplicationIcons.IconIndex(ApplicationIcons.Add) });

         options.AddRange(excelColumns.Select(c => new RowOptionDTO()
            { Description = c, ImageIndex = ApplicationIcons.IconIndex(ApplicationIcons.ObservedDataForMolecule) }));
         var metaDataIconIndex = ApplicationIcons.IconIndex(ApplicationIcons.ObservedDataForMolecule);
         return options.OrderByDescending(o => topNames.Contains(o.Description)).ThenBy(o => o.ImageIndex == metaDataIconIndex)
            .ThenBy(o => o.Description);
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

      private IEnumerable<string>
         availableColumns()
      {
         //ToDo: this should be refactor to be more readable
         return _format.ExcelColumnNames
            .Where
            (
               cn =>
                  _format.GetParameters<MappingDataFormatParameter>().All(p =>
                     p.ColumnName != cn && p.MappedColumn?.Unit?.ColumnName != cn && p.MappedColumn?.LloqColumn != cn) &&
                  _format.GetParameters<MetaDataFormatParameter>().All(p => p.ColumnName != cn) &&
                  _format.GetParameters<GroupByDataFormatParameter>().All(p => p.ColumnName != cn)
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
         if ((element is MappingDataFormatParameter) &&
             _mappingProblem.MissingUnit.Contains((element as MappingDataFormatParameter).MappedColumn.Name))
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

      private void setUnitAndDimension(ColumnMappingDTO model)
      {
         var unit = _format.ExtractUnitDescriptions(model.ExcelColumn, _columnInfos[model.MappingName]);
         if (unit.SelectedUnit == UnitDescription.InvalidUnit)
            return;

         var mappingDataFormatParameter = (model.Source as MappingDataFormatParameter);
         mappingDataFormatParameter.MappedColumn.Unit = unit;

         mappingDataFormatParameter.MappedColumn.Dimension = _columnInfos[model.MappingName].DimensionForUnit(unit.SelectedUnit);
      }

      //ToDo: this big switch statement here underneath should be refactored, probably broken to more than one functions.
      //ToDo: also the part of the switch(model.Source) that also then in the cases changes the model.Source is kind of murky. 
      private void setDescriptionForRow(ColumnMappingDTO model, bool isColumn)
      {
         switch (model.Source)
         {
            //this is the case for the first setting of Mapping or MetaData. GroupBy and AddGroupBy are never null.
            case null:
               switch (model.CurrentColumnType)
               {
                  case ColumnMappingDTO.ColumnType.MetaData:
                     model.Source = new MetaDataFormatParameter(model.ExcelColumn, model.MappingName, isColumn);
                     break;
                  case ColumnMappingDTO.ColumnType.Mapping:
                     model.Source = new MappingDataFormatParameter(model.ExcelColumn,
                        new Column() { Name = model.MappingName, Unit = new UnitDescription(UnitDescription.InvalidUnit) });
                     setUnitAndDimension(model);
                     break;
                  default:
                     throw new NotImplementedException($"Setting description for unhandled column type: {model.CurrentColumnType}");
               }

               _format.Parameters.Add(model.Source);
               break;
            //this is the case for setting a new GroupBy parameter. The AddGroupByFormatParameter corresponds to the line in the ColumnMapping Grid
            //that has the "+" button for adding the GroupBy, that's why in this case we are introducing a new Parameter, instead of changing the existing.
            //The AddGroupByFormatParameter, should never be changed, as the "+" button line should always present in the Grid
            case AddGroupByFormatParameter _:
               model.Source = new GroupByDataFormatParameter(model.ExcelColumn);
               _format.Parameters.Add(model.Source);
               setDataFormat(_format.Parameters);
               break;
            //this is the case that corresponds to the changing of the mapping for a Parameter.
            //The Parameter in this case can be Mapping, MetaData or GroupBy
            default:
            {
               model.Source.ColumnName = model.ExcelColumn;
               switch (model.CurrentColumnType)
               {
                  case ColumnMappingDTO.ColumnType.MetaData:
                     (model.Source as MetaDataFormatParameter).IsColumn = isColumn;
                     break;
                  case ColumnMappingDTO.ColumnType.Mapping:
                     setUnitAndDimension(model);
                     break;
                  case ColumnMappingDTO.ColumnType.GroupBy:
                     break;
                  default:
                     throw new NotImplementedException($"Setting description for unhandled column type: {model.CurrentColumnType}");
               }

               break;
            }
         }
      }

      public void ClearRow(ColumnMappingDTO model)
      {
         if (model.Source == null)
            return;

         if (model.Source is GroupByDataFormatParameter)
         {
            _mappings.Remove(model);
            _format.Parameters.Remove(model.Source);
         }
         else
         {
            if (ShouldManualInputOnMetaDataBeEnabled(model))
            {
               var source = model.Source as MetaDataFormatParameter;
               _mappings.First(m => m.MappingName == source.MetaDataId).ExcelColumn = null;
               source.ColumnName = null;
               source.IsColumn = false;
            }
            else
            {
               var index = _format.Parameters.IndexOf(model.Source);
               model.ExcelColumn = Captions.Importer.NoneEditorNullText;
               model.Source = null;
               _format.Parameters.RemoveAt(index);
            }
         }

         ValidateMapping();
      }

      public void AddGroupBy(AddGroupByFormatParameter source)
      {
         var parameter = new GroupByDataFormatParameter(source.ColumnName);
         _format.Parameters.Insert(_format.Parameters.Count - 2, parameter);
         new GroupByDataFormatParameter(source.ColumnName);
         setDataFormat(_mappings
            .Where(f => !(f.Source is AddGroupByFormatParameter))
            .Select(f => f.Source)
            .Append(parameter)
            .ToList());
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

      private void validateErrorUnit()
      {
         var errorColumnDTO = _mappings?.FirstOrDefault(c => (c?.ColumnInfo != null) && !c.ColumnInfo.RelatedColumnOf.IsNullOrEmpty());

         var errorColumn = ((MappingDataFormatParameter)errorColumnDTO?.Source)?.MappedColumn;
         if (errorColumn == null) return;
         var measurementColumnDTO = _mappings.FirstOrDefault(c => c.MappingName == errorColumnDTO.ColumnInfo.RelatedColumnOf);
         var measurementColumn = ((MappingDataFormatParameter)measurementColumnDTO?.Source)?.MappedColumn;
         if (measurementColumn == null) return;

         //either both measurement and error units should be coming from excel columns, or they should have the same dimension
         checkErrorUnitAgainstMeasurementUnit(errorColumn, measurementColumn);

         //if the unit is already automatically set but the Standard Deviation not,
         //Then set the STD to Arithmetic
         setDeviationWhenUnitNotNull(errorColumn);
      }

      private static void setDeviationWhenUnitNotNull(Column errorColumn)
      {
         if (errorColumn.Unit.SelectedUnit != null && errorColumn.Unit.SelectedUnit != UnitDescription.InvalidUnit
                                                   && errorColumn.ErrorStdDev == null)
            errorColumn.ErrorStdDev = Constants.STD_DEV_ARITHMETIC;
      }

      private static void checkErrorUnitAgainstMeasurementUnit(Column errorColumn, Column measurementColumn)
      {
         if (errorColumn.ErrorStdDev != Constants.STD_DEV_GEOMETRIC &&
             ((errorColumn.Unit?.ColumnName.IsNullOrEmpty() != measurementColumn.Unit?.ColumnName.IsNullOrEmpty()) ||
              (measurementColumn.Unit?.ColumnName == null && measurementColumn.Dimension != errorColumn.Dimension)))
         {
            errorColumn.Unit = new UnitDescription();
            errorColumn.Dimension = measurementColumn.Dimension;
         }
      }

      public void ValidateMapping()
      {
         validateErrorUnit();

         _mappingProblem = _importer.CheckWhetherAllDataColumnsAreMapped(_columnInfos, _mappings.Select(m => m.Source));
         setStatuses();
         if (_mappingProblem.MissingMapping.Count != 0 || _mappingProblem.MissingUnit.Count != 0)
         {
            OnMissingMapping(this,
               new MissingMappingEventArgs
                  { Message = _mappingProblem.MissingMapping.FirstOrDefault() ?? _mappingProblem.MissingUnit.FirstOrDefault() });
         }
         else
         {
            OnMappingCompleted(this, new EventArgs());
         }

         //at the end refresh the data in the columnMappingView grid, to ensure consistency 
         View.RefreshData();
      }

      public event EventHandler OnMappingCompleted = delegate { };

      public event EventHandler<MissingMappingEventArgs> OnMissingMapping = delegate { };

      public event EventHandler OnResetMappingBasedOnCurrentSheet = delegate { };

      public IEnumerable<string> GetAllAvailableExcelColumns()
      {
         return _format.ExcelColumnNames;
      }
   }
}