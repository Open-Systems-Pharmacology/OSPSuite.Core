using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.UnitSystem;
using OSPSuite.Utility.Exceptions;
using DataColumn = OSPSuite.Core.Domain.Data.DataColumn;

namespace OSPSuite.Core.Importer.Mappers
{
   public interface IImportDataTableToDataRepositoryMapper
   {
      DataRepository ConvertImportDataTable(ImportDataTable importDataTable, IEnumerable<ColumnInfo> columnInfos, out bool tableErrorNaN);
   }

   class ImportDataTableToDataRepositoryMapper : IImportDataTableToDataRepositoryMapper
   {
      private readonly IDimensionFactory _dimensionFactory;
      private readonly IColumnCaptionHelper _columnCaptionHelper;

      /// <summary>
      ///    This function find a given column by name.
      /// </summary>
      /// <param name="columns">List of column to search in.</param>
      /// <param name="name">Name to search for.</param>
      /// <returns>Found column.</returns>
      /// <exception cref="ColumnNotFoundException">Thrown when column could not be found.</exception>
      private static DataColumn findColumnByName(IEnumerable<DataColumn> columns, string name)
      {
         var column = columns.FirstOrDefault(col => col.Name == name);
         if (column == null)
            throw new ColumnNotFoundException(name);
         return column;
      }

      public ImportDataTableToDataRepositoryMapper(IDimensionFactory dimensionFactory, IColumnCaptionHelper captionHelper)
      {
         _dimensionFactory = dimensionFactory;
         _columnCaptionHelper = captionHelper;
      }

      private static bool containsColumnByName(IEnumerable<DataColumn> columns, string name)
      {
         return columns.Any(col => col.Name == name);
      }

      /// <summary>
      ///    This method converts an <see cref="ImportDataColumn" /> object and adds it to the data repository.
      /// </summary>
      /// <param name="dataRepository"></param>
      /// <param name="importDataColumn">Import data column to be converted.</param>
      /// <param name="columnInfos">Specification of columns including specification of meta data.</param>
      /// <param name="errorNaN">
      ///    This is a flag informing the caller about whether error have been changed to NaN because they
      ///    are out of definition.
      /// </param>
      private void convertImportDataColumn(DataRepository dataRepository, ImportDataColumn importDataColumn, IEnumerable<ColumnInfo> columnInfos, out bool errorNaN)
      {
         errorNaN = false;
         if (containsColumnByName(dataRepository.Columns, importDataColumn.ColumnName)) return;
         if (string.IsNullOrEmpty(importDataColumn.Source)) return; //not mapped and this means not used.

         var importDataTable = importDataColumn.Table;

         var enumerable = columnInfos as IList<ColumnInfo> ?? columnInfos.ToList();
         var colInfo = enumerable.FirstOrDefault(col => col.Name == importDataColumn.ColumnName);
         if (colInfo == null) return;
         if (importDataColumn.Dimensions == null)
            throw new OSPSuiteException(string.Format("Column {0} has no dimension.", importDataColumn.ColumnName));

         var dimension = _dimensionFactory.Dimension(importDataColumn.ActiveDimension.Name);

         DataColumn dataColumn;
         if (string.IsNullOrEmpty(colInfo.BaseGridName) || (colInfo.BaseGridName == colInfo.Name))
            dataColumn = new BaseGrid(importDataColumn.ColumnName, dimension);
         else
         {
            if (!containsColumnByName(dataRepository.Columns, colInfo.BaseGridName))
            {
               bool columnErrorNaN;
               convertImportDataColumn(dataRepository, importDataTable.Columns.ItemByName(colInfo.BaseGridName),
                  enumerable, out columnErrorNaN);
               errorNaN |= columnErrorNaN;
            }
            var baseGrid = findColumnByName(dataRepository.Columns, colInfo.BaseGridName) as BaseGrid;
            dataColumn = new DataColumn(importDataColumn.ColumnName, dimension, baseGrid);
         }

         var dataInfo = new DataInfo(ColumnOrigins.Undefined);
         dataColumn.DataInfo = dataInfo;

         if (!string.IsNullOrEmpty(importDataColumn.Source))
            dataInfo.Source = importDataColumn.Source;

         var unit = dimension.Unit(importDataColumn.ActiveUnit.Name);
         var values = new float[importDataTable.Rows.Count];
         var i = 0;
         //loop over view rows to get the sorted values.
         foreach (DataRowView row in importDataTable.DefaultView)
         {
            var value = row[importDataColumn.ColumnName];
            if (value == null || value == DBNull.Value)
               values[i++] = float.NaN;
            else
               values[i++] = (float) dataColumn.Dimension.UnitValueToBaseUnitValue(unit, (double) value);
         }

         dataInfo.DisplayUnitName = unit.Name;
         dataColumn.Values = values;

         //transfer input parameters
         if (importDataColumn.ActiveDimension.InputParameters != null)
         {
            //transfer input parameters to extended properties
            foreach (var inputParameter in importDataColumn.ActiveDimension.InputParameters)
            {
               if (inputParameter.Value == null) continue;
               //create an extended property 
               var extendedProperty =
                  Activator.CreateInstance(typeof(ExtendedProperty<>).MakeGenericType(typeof(double)))
                     as IExtendedProperty;
               if (extendedProperty == null) continue;
               extendedProperty.Name = inputParameter.Name;
               extendedProperty.ValueAsObject = (double) inputParameter.Value;
               dataInfo.ExtendedProperties.Add(extendedProperty);
            }
         }

         //transfer meta data information to data info fields or extended properties
         if (importDataColumn.MetaData != null && importDataColumn.MetaData.Rows.Count > 0)
         {
            foreach (MetaDataColumn metaData in importDataColumn.MetaData.Columns)
            {
               var value = importDataColumn.MetaData.Rows.ItemByIndex(0)[metaData];
               if (value == DBNull.Value && !metaData.Required) continue;
               var propInfo = dataInfo.GetType().GetProperty(metaData.ColumnName);

               if (propInfo != null)
               {
                  if (metaData.ColumnName == Constants.AUXILIARY_TYPE)
                  {
                     switch (value.ToString())
                     {
                        case Constants.STD_DEV_ARITHMETIC:
                           value = AuxiliaryType.ArithmeticStdDev;
                           for (i = 0; i < dataColumn.Values.Count; i++)
                              if (dataColumn.Values[i] < 0F)
                              {
                                 dataColumn[i] = float.NaN;
                                 errorNaN = true;
                              }
                           break;
                        case Constants.STD_DEV_GEOMETRIC:
                           value = AuxiliaryType.GeometricStdDev;
                           for (i = 0; i < dataColumn.Values.Count; i++)
                              if (dataColumn.Values[i] < 1F)
                              {
                                 dataColumn[i] = float.NaN;
                                 errorNaN = true;
                              }
                           break;
                     }
                  }
                  else
                  {
                     // set explicitly defined property
                     if (propInfo.PropertyType.IsEnum)
                        value = Enum.Parse(propInfo.PropertyType, value.ToString(), true);
                  }
                  propInfo.SetValue(dataColumn.DataInfo, value, null);
               }
               else
               {
                  // add extended property
                  var extendedProperty =
                     Activator.CreateInstance(typeof(ExtendedProperty<>).MakeGenericType(metaData.DataType))
                        as IExtendedProperty;
                  if (extendedProperty == null) continue;
                  extendedProperty.Name = metaData.ColumnName;
                  extendedProperty.ValueAsObject = value;
                  dataInfo.ExtendedProperties.Add(extendedProperty);
               }
            }
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
         if (dataInfo.Origin == ColumnOrigins.Observation)
            addLowerLimitOfQuantification(importDataColumn, dataColumn);

         dataRepository.Add(dataColumn);
      }

      /// <summary>
      ///    This methods creates a sort string to sort a data view object by base grid columns specified
      ///    with column infos.
      /// </summary>
      /// <param name="columnInfos">Specification of columns including specification of meta data.</param>
      /// <returns></returns>
      private static string getSortString(IEnumerable<ColumnInfo> columnInfos)
      {
         var sortString = string.Empty;
         foreach (var column in columnInfos)
         {
            if (!string.IsNullOrEmpty(column.BaseGridName)) continue;
            if (sortString.Length > 0) sortString += ", ";
            sortString += column.Name;
         }
         if (sortString.Length > 0)
         {
            sortString += " ASC";
         }
         return sortString;
      }

      /// <summary>
      ///    This method converts an import data table to a DataRepository object.
      /// </summary>
      /// <param name="importDataTable">Import data table.</param>
      /// <param name="columnInfos">Specification of columns including specification of meta data.</param>
      /// <param name="errorNaN">
      ///    This is a flag informing the caller about whether error have been changed to NaN because they
      ///    are out of definition.
      /// </param>
      /// <returns>DataRepository object.</returns>
      public DataRepository ConvertImportDataTable(ImportDataTable importDataTable, IEnumerable<ColumnInfo> columnInfos, out bool errorNaN)
      {
         errorNaN = false;
         var dataRepository = new DataRepository {Name = importDataTable.TableName};

         //sort table in view ascending by base grid columns
         var colInfos = columnInfos as IList<ColumnInfo> ?? columnInfos.ToList();
         importDataTable.DefaultView.Sort = getSortString(colInfos);

         addExtendedPropertyForSource(importDataTable, dataRepository);

         addExtendedPropertiesForMetaData(importDataTable, dataRepository);

         addExtendedPropertiesForGroupBy(importDataTable, dataRepository);

         //convert columns
         foreach (ImportDataColumn importDataColumn in importDataTable.Columns)
         {
            bool columnErrorNaN;
            convertImportDataColumn(dataRepository, importDataColumn, colInfos, out columnErrorNaN);
            errorNaN |= columnErrorNaN;
         }

         // make associations of columns.
         associateColumns(colInfos, dataRepository);

         return dataRepository;
      }

      private void addLowerLimitOfQuantification(System.Data.DataColumn importDataTableColumn, DataColumn dataColumn)
      {
         if (!importDataTableColumn.ExtendedProperties.ContainsKey(Constants.LLOQ))
            return;

         var lowerLimitOfQuantification = (float) importDataTableColumn.ExtendedProperties[Constants.LLOQ];
         dataColumn.DataInfo.LLOQ = Convert.ToSingle(dataColumn.ConvertToBaseUnit(lowerLimitOfQuantification));
      }

      private static void associateColumns(IList<ColumnInfo> colInfos, DataRepository dataRepository)
      {
         foreach (var colInfo in colInfos)
         {
            if (!containsColumnByName(dataRepository.Columns, colInfo.Name))
               continue;

            var col = findColumnByName(dataRepository.Columns, colInfo.Name);

            // related column
            if (col.DataInfo.AuxiliaryType == AuxiliaryType.Undefined ||
                string.IsNullOrEmpty(colInfo.RelatedColumnOf))
               continue;

            if (!containsColumnByName(dataRepository.Columns, colInfo.RelatedColumnOf))
               continue;

            var relatedCol = findColumnByName(dataRepository.Columns, colInfo.RelatedColumnOf);
            relatedCol.AddRelatedColumn(col);
         }
      }

      private void addExtendedPropertiesForGroupBy(ImportDataTable importDataTable, DataRepository dataRepository)
      {
         foreach (DictionaryEntry property in importDataTable.ExtendedProperties)
         {
            if (dataRepository.ExtendedProperties.Contains(property.Key.ToString()))
               continue;
            if (string.IsNullOrEmpty(property.Value.ToString()))
               continue;
            dataRepository.ExtendedProperties.Add(new ExtendedProperty<string> {Name = _columnCaptionHelper.TrimUnits(property.Key.ToString()), Value = property.Value.ToString()});
         }
      }

      private static void addExtendedPropertiesForMetaData(ImportDataTable importDataTable, DataRepository dataRepository)
      {
         if (importDataTable.MetaData == null) return;
         if (importDataTable.MetaData.Rows.Count <= 0) return;

         foreach (MetaDataColumn metaData in importDataTable.MetaData.Columns)
         {
            var value = importDataTable.MetaData.Rows.ItemByIndex(0)[metaData];
            if (value == DBNull.Value && !metaData.Required) continue;

            // add extended property
            var extendedProperty = Activator.CreateInstance(typeof(ExtendedProperty<>).MakeGenericType(metaData.DataType))
               as IExtendedProperty;
            if (extendedProperty == null) continue;
            extendedProperty.Name = metaData.ColumnName;
            extendedProperty.ValueAsObject = value;
            dataRepository.ExtendedProperties.Add(extendedProperty);
         }
      }

      private static void addExtendedPropertyForSource(ImportDataTable importDataTable, DataRepository dataRepository)
      {
         if (!string.IsNullOrEmpty(importDataTable.Source))
         {
            var sourceProperty =
               Activator.CreateInstance(typeof(ExtendedProperty<>).MakeGenericType(importDataTable.Source.GetType()))
                  as IExtendedProperty;
            if (sourceProperty != null)
            {
               sourceProperty.Name = "Source";
               sourceProperty.ValueAsObject = importDataTable.Source;
               dataRepository.ExtendedProperties.Add(sourceProperty);
            }

            dataRepository.ExtendedProperties.Add(new ExtendedProperty<string> {Name = Constants.FILE, Value = Path.GetFileNameWithoutExtension(importDataTable.File)});
            dataRepository.ExtendedProperties.Add(new ExtendedProperty<string> {Name = Constants.SHEET, Value = importDataTable.Sheet});
         }
      }
   }
}