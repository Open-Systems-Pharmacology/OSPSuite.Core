using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Infrastructure.Import.Core.DataFormat;
using OSPSuite.Infrastructure.Import.Extensions;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Infrastructure.Import.Core.Mappers
{
   public interface IDataSetToDataRepositoryMapper
   {
      DataRepository ConvertImportDataSet(IDataSource dataSource, int dataSetIndex, string dataSetName);
   }

   class DataSetToDataRepositoryMapper : IDataSetToDataRepositoryMapper
   {
      private readonly IDimensionFactory _dimensionFactory;

      public DataSetToDataRepositoryMapper(IDimensionFactory dimensionFactory)
      {
         _dimensionFactory = dimensionFactory;
      }
      public DataRepository ConvertImportDataSet(IDataSource dataSource, int dataSetIndex, string dataSetName)
      {
         var parsedDataSet = getDataSet(dataSource, dataSetIndex, out var sheetIndex);
         var dataSetPair = dataSource.DataSets.KeyValues.ElementAt(sheetIndex);
         var sheetName = dataSetPair.Key;
         var configuration = dataSource.GetImporterConfiguration();
         var dataRepository = new DataRepository { Name = dataSetName };

         addExtendedPropertyForSource(configuration.FileName, sheetName, dataRepository);

         foreach (var column in parsedDataSet.Data)
         {
            convertParsedDataColumn(dataRepository, column, configuration.FileName);
         }

         //associate columns
         foreach (var column in parsedDataSet.Data)
         {
            if (string.IsNullOrEmpty(column.Key.ColumnInfo.RelatedColumnOf))
               continue;

            if (!containsColumnByName(dataRepository.Columns, column.Key.ColumnInfo.RelatedColumnOf))
               continue;

            var col = findColumnByName(dataRepository.Columns, column.Key.ColumnInfo.Name);

            var relatedCol = findColumnByName(dataRepository.Columns, column.Key.ColumnInfo.RelatedColumnOf);
            relatedCol.AddRelatedColumn(col);
         }

         return dataRepository;
      }

      private void convertParsedDataColumn(DataRepository dataRepository, KeyValuePair<ExtendedColumn, IList<SimulationPoint>> column, string fileName)
      {
         DataColumn dataColumn;
         //so the dimension is calculated correctly but with the wrong function
         //for now it stays like this, cause otherwise we have to have the association
         //var dimension_test = _dimensionFactory.Dimension(column.Key.Name);
         //otherwise we can also just get the defaultDimension from the columnInfos when we
         //create the extended column
         var dimension = _dimensionFactory.DimensionForUnit(column.Key.Column.Unit.SelectedUnit);

         if (column.Key.ColumnInfo.IsBase())
            dataColumn = new BaseGrid(column.Key.ColumnInfo.Name, dimension);
         else
         {
            
            if (!containsColumnByName(dataRepository.Columns, column.Key.ColumnInfo.BaseGridName))
            {
               //convertParsedDataColumn(dataRepository, importDataTable.Columns.ItemByName(colInfo.BaseGridName));
               throw new Exception();
            }
            var baseGrid = findColumnByName(dataRepository.Columns, column.Key.ColumnInfo.BaseGridName) as BaseGrid;
            dataColumn = new DataColumn(column.Key.ColumnInfo.Name, dimension, baseGrid);
         }

         var dataInfo = new DataInfo(ColumnOrigins.Undefined);
         dataColumn.DataInfo = dataInfo;

         if (!string.IsNullOrEmpty(fileName))
            dataInfo.Source = fileName;

         var unit = dimension.Unit(column.Key.Column.Unit.SelectedUnit);
         var values = new float[column.Value.Count];
         var i = 0;

         //loop over view rows to get the sorted values.
         foreach (var value in column.Value)
         {
            if (value == null || double.IsNaN(value.Value)) //but we actually should not be allowing this at all right?
               values[i++] = float.NaN;
            else
               values[i++] = (float)dataColumn.Dimension.UnitValueToBaseUnitValue(dimension.Unit(value.Unit), (double)value.Value);
         }

         dataInfo.DisplayUnitName = unit.Name;
         dataColumn.Values = values;

         var propInfo = dataInfo.GetType().GetProperty(Constants.AUXILIARY_TYPE);
         var errorType = AuxiliaryType.Undefined;

         if (propInfo != null)
         {
            if (column.Key.ColumnInfo.IsAuxiliary())
            {
               switch (column.Key.ErrorDeviation)
               {
                  case Constants.STD_DEV_ARITHMETIC:
                     errorType = AuxiliaryType.ArithmeticStdDev;
                     for (i = 0; i < dataColumn.Values.Count; i++)
                        if (dataColumn.Values[i] < 0F)
                        {
                           dataColumn[i] = float.NaN;
                        }
                     break;
                  case Constants.STD_DEV_GEOMETRIC:
                     errorType = AuxiliaryType.GeometricStdDev;
                     for (i = 0; i < dataColumn.Values.Count; i++)
                        if (dataColumn.Values[i] < 1F)
                        {
                           dataColumn[i] = float.NaN;
                        }
                     break;
               }
            }
            propInfo.SetValue(dataColumn.DataInfo, errorType, null);
         }

         //special case: Origin
         //if AuxiliaryType is set, set Origin to ObservationAuxiliary else to Observation for standard columns
         //type is set to BaseGrid for baseGrid column
         if (dataColumn.IsBaseGrid())
            dataInfo.Origin = ColumnOrigins.BaseGrid;
         else
            dataInfo.Origin = (dataColumn.DataInfo.AuxiliaryType == AuxiliaryType.Undefined)
               ? ColumnOrigins.Observation
               : ColumnOrigins.ObservationAuxiliary;

         //meta data information and input parameters currently not handled
         dataRepository.Add(dataColumn);
      }

      private static DataColumn findColumnByName(IEnumerable<DataColumn> columns, string name)
      {
         var column = columns.FirstOrDefault(col => col.Name == name);
         if (column == null)
            throw new Exception(name);//this should actually be named
         return column;
      }
      private static void addExtendedPropertyForSource(string fileName, string sheetName,  DataRepository dataRepository)
      {
         if (!string.IsNullOrEmpty(fileName))
         {
            var sourceProperty =
               Activator.CreateInstance(typeof(ExtendedProperty<>).MakeGenericType(fileName.GetType()))
                  as IExtendedProperty;
            if (sourceProperty != null)
            {
               sourceProperty.Name = "Source";
               sourceProperty.ValueAsObject = fileName;
               dataRepository.ExtendedProperties.Add(sourceProperty);
            }

            dataRepository.ExtendedProperties.Add(new ExtendedProperty<string> { Name = Constants.FILE, Value = fileName });
            dataRepository.ExtendedProperties.Add(new ExtendedProperty<string> { Name = Constants.SHEET, Value = sheetName });
         }
      }

      private static bool containsColumnByName(IEnumerable<DataColumn> columns, string name)
      {
         return columns.Any(col => col.Name == name);
      }

      private ParsedDataSet getDataSet(IDataSource dataSource, int dataSetIndex, out int sheetIndex)
      {
         sheetIndex = 0;
         var sheet = dataSource.DataSets.GetEnumerator();
         while (sheet.MoveNext() && dataSetIndex >= 0)
         {
            if (sheet.Current.Data.Count() > dataSetIndex)
               return sheet.Current.Data.ElementAt(dataSetIndex);
            else
            {
               dataSetIndex -= sheet.Current.Data.Count();
               sheetIndex++;
            }
         }

         return null;
      }
   }
}
