using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using OSPSuite.Core.Domain;
using OSPSuite.Core.Domain.Data;
using OSPSuite.Infrastructure.Import.Core.DataFormat;

namespace OSPSuite.Infrastructure.Import.Core.Mappers
{
   public interface IDataSetToDataRepositoryMapper
   {
      DataRepository ConvertImportDataSet(IDataSource dataSource, int dataSetIndex, string dataSetName);
   }

   class DataSetToDataRepositoryMapper : IDataSetToDataRepositoryMapper
   {
      public DataRepository ConvertImportDataSet(IDataSource dataSource, int dataSetIndex, string dataSetName)
      {
         //var dataSet = dataSource.DataSets.ElementAt(dataSetIndex);
         var ParsedDataSet = GetDataSet(dataSource, dataSetIndex, out var sheetIndex);
         
         var dataSetPair = dataSource.DataSets.KeyValues.ElementAt(sheetIndex);
         var sheetName = dataSetPair.Key;
         var dataSet = dataSetPair.Value;
         var dataRepository = new DataRepository { Name = dataSetName };
         var configuration = dataSource.GetImporterConfiguration();


         //var colInfos = columnInfos as IList<ColumnInfo> ?? columnInfos.ToList();

         //do we actually need to sort our IDataSet somehow?
         //importDataTable.DefaultView.Sort = getSortString(colInfos);

         addExtendedPropertyForSource(configuration.FileName, sheetName, dataRepository);

         addExtendedPropertiesForMetaData(configuration.Format, dataSet, dataRepository);


         /*


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
         */
         return dataRepository;
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

         foreach (MetaDataFormatParameter parameter in format.Parameters)
         {
            var name = parameter.MetaDataId;
            /*
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
         }
      }
   }
}
