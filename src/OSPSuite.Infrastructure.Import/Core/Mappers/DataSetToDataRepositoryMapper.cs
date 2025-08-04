using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Assets;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Core.Domain.Services;
using OSPSuite.Core.Import;
using OSPSuite.Utility.Extensions;

namespace OSPSuite.Infrastructure.Import.Core.Mappers
{
   public class DataSetToDataRepositoryMappingResult
   {
      public DataSetToDataRepositoryMappingResult(DataRepository dataRepository, string warningMessage = "")
      {
         WarningMessage = warningMessage;
         DataRepository = dataRepository;
      }

      public string WarningMessage { get; private set; }

      public DataRepository DataRepository { get; private set; }
   }

   public interface IDataSetToDataRepositoryMapper
   {
      DataSetToDataRepositoryMappingResult ConvertImportDataSet(ImportedDataSet dataSet);
   }

   public class DataSetToDataRepositoryMapper : IDataSetToDataRepositoryMapper
   {
      public DataSetToDataRepositoryMappingResult ConvertImportDataSet(ImportedDataSet dataSet)
      {
         var sheetName = dataSet.SheetName;
         var dataRepository = new DataRepository { Name = dataSet.Name };

         addExtendedPropertyForSource(dataSet.FileName, sheetName, dataRepository);

         foreach (var metaDataDescription in dataSet.MetaDataDescription)
         {
            if (!metaDataDescription.Value.IsNullOrEmpty() && !dataRepository.ExtendedProperties.Contains(metaDataDescription.Name))
               dataRepository.ExtendedProperties.Add(new ExtendedProperty<string>()
                  { Name = metaDataDescription.Name, Value = metaDataDescription.Value });
         }

         var warningFlag = false;
         foreach (var column in dataSet.ParsedDataSet.Data)
         {
            warningFlag |= convertParsedDataColumnAndReturnWarningFlag(dataRepository, column, dataSet.FileName);
         }

         //associate columns
         foreach (var column in dataSet.ParsedDataSet.Data)
         {
            if (string.IsNullOrEmpty(column.Key.ColumnInfo.RelatedColumnOf))
               continue;

            if (!containsColumnByName(dataRepository.Columns, column.Key.ColumnInfo.RelatedColumnOf))
               continue;

            var col = findColumnByName(dataRepository.Columns, column.Key.ColumnInfo.Name);

            var relatedCol = findColumnByName(dataRepository.Columns, column.Key.ColumnInfo.RelatedColumnOf);
            relatedCol.AddRelatedColumn(col);
         }

         return new DataSetToDataRepositoryMappingResult(dataRepository, warningFlag ? Captions.Importer.LLOQInconsistentValuesAt(dataSet.Name) : "");
      }

      private bool convertParsedDataColumnAndReturnWarningFlag(DataRepository dataRepository,
         KeyValuePair<ExtendedColumn, IList<SimulationPoint>> columnAndData, string fileName)
      {
         DataColumn dataColumn;
         var unitName = columnAndData.Value.FirstOrDefault(x => unitIsValid(x.Unit))?.Unit ??
                        Constants.Dimension.NO_DIMENSION.DefaultUnitName;
         var warningFlag = false;

         var dimension = columnAndData.Key.Column.Dimension ?? columnAndData.Key.ColumnInfo.DimensionForUnit(unitName);

         if (columnAndData.Key.ColumnInfo.IsBase)
            dataColumn = new BaseGrid(columnAndData.Key.ColumnInfo.Name, dimension);
         else
         {
            if (!containsColumnByName(dataRepository.Columns, columnAndData.Key.ColumnInfo.BaseGridName))
            {
               throw new BaseGridColumnNotFoundException(columnAndData.Key.ColumnInfo.BaseGridName);
            }

            var baseGrid = findColumnByName(dataRepository.Columns, columnAndData.Key.ColumnInfo.BaseGridName) as BaseGrid;
            dataColumn = new DataColumn(columnAndData.Key.ColumnInfo.Name, dimension, baseGrid);
         }

         var dataInfo = new DataInfo(ColumnOrigins.Undefined);
         dataColumn.DataInfo = dataInfo;
         dataInfo.DisplayUnitName = unitName;

         var values = new float[columnAndData.Value.Count];
         var i = 0;

         SimulationPoint lloqValue = null;
         //loop over view rows to get the sorted values.
         foreach (var value in columnAndData.Value)
         {
            if (!double.IsNaN(value.Lloq))
            {
               if (lloqValue != null && !ValueComparer.AreValuesEqual(lloqValue.Lloq, value.Lloq))
               {
                  warningFlag = true;
                  if (lloqValue.Lloq > value.Lloq)
                     value.Lloq = lloqValue.Lloq;
               }

               if (lloqValue == null || lloqValue.Lloq < value.Lloq)
                  lloqValue = value;
            }

            var adjustedValue = truncateUsingLLOQ(value);
            if (double.IsNaN(adjustedValue))
               values[i++] = float.NaN;
            else if (unitIsValid(unitName) && !string.IsNullOrEmpty(value.Unit))
               values[i++] = (float)dataColumn.Dimension.UnitValueToBaseUnitValue(dimension?.FindUnit(value.Unit, ignoreCase: true), adjustedValue);
            else
               values[i++] = (float)adjustedValue;
         }

         if (lloqValue != null)
            dataInfo.LLOQ =
               Convert.ToSingle(dimension?.UnitValueToBaseUnitValue(dimension.FindUnit(lloqValue.Unit, ignoreCase: true), lloqValue.Lloq));

         dataColumn.Values = values;

         var propInfo = dataInfo.GetType().GetProperty(Constants.AUXILIARY_TYPE);
         var errorType = AuxiliaryType.Undefined;

         if (propInfo != null)
         {
            if (columnAndData.Key.ColumnInfo.IsAuxiliary)
            {
               switch (columnAndData.Key.ErrorDeviation)
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
         //type is set to BaseGrid for baseGrid columnAndData
         if (dataColumn.IsBaseGrid())
            dataInfo.Origin = ColumnOrigins.BaseGrid;
         else
            dataInfo.Origin = (dataColumn.DataInfo.AuxiliaryType == AuxiliaryType.Undefined)
               ? ColumnOrigins.Observation
               : ColumnOrigins.ObservationAuxiliary;

         //meta data information and input parameters currently not handled
         dataRepository.Add(dataColumn);
         return warningFlag;
      }

      private static bool unitIsValid(string unit) => !string.IsNullOrEmpty(unit) && unit != UnitDescription.InvalidUnit;

      private double truncateUsingLLOQ(SimulationPoint value)
      {
         if (value == null)
            return double.NaN;

         if (double.IsNaN(value.Lloq))
            return value.Measurement;

         if (double.IsNaN(value.Measurement) || value.Measurement < value.Lloq)
            return value.Lloq / 2;

         return value.Measurement;
      }

      private static DataColumn findColumnByName(IEnumerable<DataColumn> columns, string name)
      {
         var column = columns.FirstOrDefault(col => col.Name == name);
         if (column == null)
            throw new ColumnNotFoundException(name);
         return column;
      }

      private static void addExtendedPropertyForSource(string fileName, string sheetName, DataRepository dataRepository)
      {
         // dataRepository.ExtendedProperties.Add(new ExtendedProperty<string> {Name = Constants.FILE, Value = fileName});
         dataRepository.ExtendedProperties.Add(new ExtendedProperty<string> { Name = Constants.SHEET, Value = sheetName });
      }

      private static bool containsColumnByName(IEnumerable<DataColumn> columns, string name)
      {
         return columns.Any(col => col.Name == name);
      }
   }
}