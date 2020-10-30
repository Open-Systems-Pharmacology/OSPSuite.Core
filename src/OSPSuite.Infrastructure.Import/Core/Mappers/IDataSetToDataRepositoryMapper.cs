using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Infrastructure.Import.Core.DataFormat;
using OSPSuite.Utility.Exceptions;

namespace OSPSuite.Infrastructure.Import.Core.Mappers
{
   public interface IDataSetToDataRepositoryMapper
   {
      DataRepository ConvertImportDataSet(IDataSource dataSource, int dataSetIndex, string dataSetName);
   }

   class DataSetToDataRepositoryMapper : IDataSetToDataRepositoryMapper
   {
      private readonly IDimensionFactory _dimensionFactory; //TO BE INITIALIZED IN THE CONSTRUCTOR

      public DataSetToDataRepositoryMapper(IDimensionFactory dimensionFactory)
      {
         _dimensionFactory = dimensionFactory;
      }
      public DataRepository ConvertImportDataSet(IDataSource dataSource, int dataSetIndex, string dataSetName)
      {
         //var dataSet = dataSource.DataSets.ElementAt(dataSetIndex);
         var ParsedDataSet = GetDataSet(dataSource, dataSetIndex, out var sheetIndex);
         
         var dataSetPair = dataSource.DataSets.KeyValues.ElementAt(sheetIndex);
         var sheetName = dataSetPair.Key;
         var dataSet = dataSetPair.Value;
         var configuration = dataSource.GetImporterConfiguration();

         var dataRepository = new DataRepository { Name = dataSetName };


         //a) - we ommit the sorting of the view that is being done in the old importer

         //var colInfos = columnInfos as IList<ColumnInfo> ?? columnInfos.ToList();

         //do we actually need to sort our IDataSet somehow?
         //importDataTable.DefaultView.Sort = getSortString(colInfos);

         addExtendedPropertyForSource(configuration.FileName, sheetName, dataRepository);

         //addExtendedPropertiesForMetaData(configuration.Format, dataSet, dataRepository);

         foreach (var column in ParsedDataSet.Data)
         {
            convertParsedDataColumn(dataRepository, column, configuration.FileName);
         }

         /*


                  addExtendedPropertiesForGroupBy(importDataTable, dataRepository);

                  //convert columns
                  foreach (ImportDataColumn importDataColumn in importDataTable.Columns)
                  {
                     bool columnErrorNaN;
                     convertParsedDataColumn(dataRepository, importDataColumn, colInfos, out columnErrorNaN);
                     errorNaN |= columnErrorNaN;
                  }

                  // make associations of columns.
                  associateColumns(colInfos, dataRepository);
         */
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

         if (string.IsNullOrEmpty(column.Key.BaseGridName) || (column.Key.BaseGridName == column.Key.Name))
            dataColumn = new BaseGrid(column.Key.Name, dimension);
         else
         {
            
            if (!containsColumnByName(dataRepository.Columns, column.Key.BaseGridName))
            {
               //convertParsedDataColumn(dataRepository, importDataTable.Columns.ItemByName(colInfo.BaseGridName));
               throw new Exception();
            }
            var baseGrid = findColumnByName(dataRepository.Columns, column.Key.BaseGridName) as BaseGrid;
            dataColumn = new DataColumn(column.Key.Name, dimension, baseGrid);
         }


         //var baseGrid = findColumnByName(dataRepository.Columns, colInfo.BaseGridName) as BaseGrid;
         //dataColumn = new DataColumn(column.Key.Unit.ColumnName, dimension, baseGrid);

         //maybe colInfo.Name...!!!!
         //dataColumn = new BaseGrid(column.Key.Column.Name, dimension); //it is not good that we have column 

         var dataInfo = new DataInfo(ColumnOrigins.Undefined);
         dataColumn.DataInfo = dataInfo;

         //this should actually be the name of the file or the sheet
         if (!string.IsNullOrEmpty(fileName))
            dataInfo.Source = fileName;

         
         //not 100% sure this is correct
         var unit = dimension.Unit(column.Key.Column.Unit.SelectedUnit);
         
         
         //we have this, so we can actually populate the error list for the mapping
         //column.Key.Column.ErrorColumn
         
         
         
         var values = new float[column.Value.Count];
         var i = 0;
         //loop over view rows to get the sorted values.
         foreach (var value in column.Value)
         {
            if (value == null) //but we actually should not be allowing this at all right?
               values[i++] = float.NaN;
            else
               values[i++] = (float)dataColumn.Dimension.UnitValueToBaseUnitValue(dimension.Unit(value.Unit), (double)value.Value);
         }

         dataInfo.DisplayUnitName = unit.Name; //or column.Key.Unit.Name?
         dataColumn.Values = values;

         //special case: Origin
         //if AuxiliaryType is set, set Origin to ObservationAuxiliary else to Observation for standard columns
         //type is set to BaseGrid for baseGrid column
         if (dataColumn.IsBaseGrid())
            dataInfo.Origin = ColumnOrigins.BaseGrid;
         else
            dataInfo.Origin = (dataColumn.DataInfo.AuxiliaryType == AuxiliaryType.Undefined)
               ? ColumnOrigins.Observation
               : ColumnOrigins.ObservationAuxiliary;
        // if (dataInfo.Origin == ColumnOrigins.Observation)
          //  addLowerLimitOfQuantification(importDataColumn, dataColumn);

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

      //should be UNIT TESTED
      private ParsedDataSet GetDataSet(IDataSource dataSource, int dataSetIndex, out int sheetIndex)
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

      private static void addExtendedPropertiesForMetaData(IDataFormat format,IDataSet dataSet, DataRepository dataRepository)
      {
         //null check?
         /*
                  foreach (MetaDataFormatParameter parameter in format.Parameters)
                  {
                     var name = parameter.MetaDataId;

                     var value = importDataTable.MetaData.Rows.ItemByIndex(0)[metaData];
                     parameter.
                     if (value == DBNull.Value && !metaData.Required) continue;

                     // add extended property
                     var extendedProperty = Activator.CreateInstance(typeof(ExtendedProperty<>).MakeGenericType(metaData.DataType))
                        as IExtendedProperty;
                     if (extendedProperty == null) continue;
                     extendedProperty.Name = parameter.ColumnName;
                     extendedProperty.ValueAsObject = value;
                     dataRepository.ExtendedProperties.Add(extendedProperty);
         */
      //}
   }
   }
}
