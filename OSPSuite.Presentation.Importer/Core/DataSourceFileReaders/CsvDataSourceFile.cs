using System;
using System.Collections.Generic;
using System.Linq;
using OSPSuite.Core.Services;
using OSPSuite.Infrastructure.Import.Services;


namespace OSPSuite.Presentation.Importer.Core.DataSourceFileReaders
{
   public interface ICsvDataSourceFile : IDataSourceFile { }

   public class CsvDataSourceFile : DataSourceFile, ICsvDataSourceFile
   {
      public CsvDataSourceFile(IImportLogger logger) : base(logger) { }
      protected override Dictionary<string, IDataSheet> LoadFromFile(string path)
      {
         try
         {
            using (var reader = new CsvReaderDisposer(path))
            {
               var csv = reader.Csv;
               var headers = csv.GetFieldHeaders();
               var rows = new List<List<string>>(headers.Length);

               IDataSheet dataSheet = new DataSheet();
               dataSheet.RawData = new UnformattedData();

               for (var i = 0; i < headers.Length; i++)
                  dataSheet.RawData.AddColumn(headers[i], i);
               var currentRow = new string[csv.FieldCount];

               while (csv.ReadNextRecord())
               {
                  csv.CopyCurrentRecordTo(currentRow);
                  var rowList = currentRow.ToList();
                  var levels = getMeasurementLevels(rowList);
                  dataSheet.RawData.CalculateColumnDescription(levels);
                  dataSheet.RawData.AddRow(rowList);
               }

               var loadedData = new Dictionary<string, IDataSheet>
               {
                  { "", dataSheet }
               };
               return loadedData;
            }
         }
         catch (Exception e)
         {
            _logger.AddError(e.ToString());
            return null;
         }
      }

      private List<ColumnDescription.MeasurementLevel> getMeasurementLevels(List<string> dataList)
      {
         var resultList = new List<ColumnDescription.MeasurementLevel>();

         foreach (var item in dataList)
         {
            //IMPORTANT
            /*
             * Providing NumberStyles.Any tells double.TryParse() to allow any format, except AllowHexSpecifier. This includes the AllowThousands option.
               Providing the InvariantCulture causes parsing to use the ',' character as the thousands separator.

             */
            /*
            if (double.TryParse(item, System.Globalization.NumberStyles.Float,
                  System.Globalization.CultureInfo.InvariantCulture, out var doubleValueInvariant))
               resultList.Add(ColumnDescription.MeasurementLevel.NUMERIC);
            else */if (Double.TryParse(item, out var doubleValue)) //TODO Resharper
               resultList.Add(ColumnDescription.MeasurementLevel.Numeric);
            else
               resultList.Add(ColumnDescription.MeasurementLevel.Discrete);

         }
         return resultList;
      }
   }
}
